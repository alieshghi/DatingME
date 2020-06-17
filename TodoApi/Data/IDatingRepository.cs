using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.helper;
using TodoApi.Models;

namespace TodoApi.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetCurrentMainPhoto(int userId);
         Task<Like> GetLike(int userId, int resciveId);
        
    }
}