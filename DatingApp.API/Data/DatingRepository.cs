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

            if (userParams.Likers)
            {
                var userLikers =  await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likees)
            {
              var userLikees =  await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

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

        public async Task<Like> GetLike(int userId, int recipientId) 
        {
          return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
          var user = await _context.Users.Include(x => x.Likers)
            .Include(x => x.Likees)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (likers) 
            {
              return user.Likers.Where(x => x.LikeeId == id).Select(p => p.LikerId);
            }
            else 
            {
              return user.Likees.Where(x => x.LikerId == id).Select(p => p.LikeeId);
            }
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(u => u.Id == id);
        }      

        public Task<Message> GetMessageThread(int userId, int recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<Message>> GetMessagesForUsers(MessageParams messageParams)
        {
            var messages = _context.Messages
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            .AsQueryable();


            switch (messageParams.MessageContainer)
            {
              case "Inbox":
                messages = messages.Where(x => x.RecipientId == messageParams.UserId);
                break;

              case "Outbox":
                messages = messages.Where(x => x.SenderId ==  messageParams.UserId); 
                break;

                default:
                 messages = messages.Where(x => x.SenderId ==  messageParams.UserId && x.IsRead == false);  
                 break;
            }

            messages = messages.OrderByDescending(s => s.MeassgeSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }
    }
}