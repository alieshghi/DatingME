using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TodoApi.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly ToDoContext _context;
        public DatingRepository(ToDoContext context)
        {
            _context=context;
        }
        public void Add<T>(T entity) where T : class
        {
           _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public Task<Photo> GetCurrentMainPhoto(int userId)
        {
            return _context.photos.Where(x=>x.UserId==userId).FirstOrDefaultAsync(x=>x.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.photos.FirstOrDefaultAsync(x=>x.Id==id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
             var user = await _context.Users.Include(x=>x.Photos).FirstOrDefaultAsync(x=>x.Id==id);
             return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(x=>x.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()>0 ;
        }
    }
}