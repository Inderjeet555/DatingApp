using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DatingApp.API.Data
{
    public class DatingRepository : IdatingRepository
    {
        public readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }

      public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

      public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

      public async  Task<User> GetUser(int Id)
        {
          var user = await  _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == Id);
            return user;
        }

      public async  Task<PagedList <User>> GetUsers(UserParams userParams)
        {
             var users =  _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(x => x.Id != userParams.UserId);
            users = users.Where(x => x.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
              switch (userParams.OrderBy)
              {
                  case "created":
                  users = users.OrderByDescending(u => u.Created);
                  break;
                  default:
                  users = users.OrderByDescending(u => u.LastActive);
                  break;
              }
            }
             
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }    

       public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Photo> GetPhoto(int Id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == Id);
              return photo;
        }

        public async Task<Photo> GetMainPhoto(int userId)
        {
            return await _context.Photos.Where(x => x.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}