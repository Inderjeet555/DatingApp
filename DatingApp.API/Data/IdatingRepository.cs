using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using DatingApp.API.Helpers;

namespace DatingApp.API.Data
{
    public interface IdatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList <User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int Id); 
         Task<Photo> GetPhoto(int Id);        
         Task<Photo> GetMainPhoto(int userId);
         Task<Like> GetLike(int userId, int recipientId);
    }
}