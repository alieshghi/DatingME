using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TodoApi.helper;

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

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users= _context.Users.Include(x=>x.Photos).OrderByDescending(x =>x.LastActive).AsQueryable();
            if (userParams.SortType == (int) SortOrderType.Assending )
            {
                users=users.OrderBy(x => x.LastActive);
            }
            users= users.Where(x => x.Id!=userParams.currentUserId);
            if (!string.IsNullOrEmpty(userParams.Country) )
            {
                users = users.Where(x=>x.Country.Contains(userParams.Country));
            }
            if (!string.IsNullOrEmpty(userParams.City) )
            {
                users = users.Where(x=>x.City.Contains(userParams.City));
            }
            if (!string.IsNullOrEmpty(userParams.Gender))
            {
                users = users.Where(x => x.Gender== userParams.Gender);
            }
            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
                {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }
            if (!string.IsNullOrEmpty(userParams.SortOrder))
            {
                if (userParams.SortType != (int) SortOrderType.Assending)
                {
                    switch (userParams.SortOrder)
                {
                    case "createdDate":
                    users= users.OrderByDescending(x => x.Created);
                    break;
                    case "age":
                    users= users.OrderBy(x => x.DateOfBirth);
                    break;
                    default:                    
                    users= users.OrderByDescending(x => x.LastActive);
                    break;
                }
                    
                }
                else
                {
                    switch (userParams.SortOrder )
                {
                    case "createdDate":
                    users= users.OrderBy(x => x.Created);
                    break;
                    case "age":
                    users= users.OrderByDescending(x => x.DateOfBirth);
                    break;
                    default:                    
                    users= users.OrderBy(x => x.LastActive);
                    break;
                }
                    
                }
                
            }            
                        
            var result= await PagedList<User>.CreatePagintion(users ,userParams.PageSize,userParams.CurentPage);
            return  result;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()>0 ;
        }
    }
}