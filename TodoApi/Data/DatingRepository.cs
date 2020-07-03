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

        public async Task<Photo> GetCurrentMainPhoto(int userId)
        {
            return await _context.photos.Where(x=>x.UserId==userId).FirstOrDefaultAsync(x=>x.IsMain);
        }

        public async Task<Like> GetLike(int userId, int resciveId)
        {
            return await _context.Likes.FirstOrDefaultAsync(x => x.LikerId== userId &&
             x.LikedId == resciveId);
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
            if (userParams.Likers)
            {
                var userLikeIdies = await GetLikesID(userParams.currentUserId,userParams.Likers);
                users= users.Where(x => userLikeIdies.Contains(x.Id));
            }
            if (userParams.Likeds)
            {
                var userLikedByIdies = await GetLikesID(userParams.currentUserId,userParams.Likers);
                users= users.Where(x => userLikedByIdies.Contains(x.Id));
            }
            if (!string.IsNullOrEmpty(userParams.City))
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
        private async Task <IEnumerable<int>> GetLikesID(int userId, bool isLiker){
            var user = await _context.Users.Include(x => x.Likeds)
                     .Include(x =>x.Likers).FirstOrDefaultAsync(x => x.Id==userId);
            if (isLiker)
            {
                return user.Likers.Where(x =>x.LikedId == userId).Select(x =>x.LikerId);
            }
            else
            {
                return user.Likeds.Where(x =>x.LikerId == userId).Select(x =>x.LikedId);
            }
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()>0 ;
        }

        public async Task<Message> GetMessageById(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync( x =>x.Id==id);
        }

        public async Task<PagedList<Message>> GetMessages(MessageParams messageParams)
        {
            var messages= _context.Messages.Include(x => x.Sender).ThenInclude(x => x.Photos)
            .Include(x => x.Recipient).ThenInclude(x => x.Photos)
            .AsQueryable();
            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                messages= messages.Where(x =>x.RecipientId== messageParams.UserId &&
                 x.RecipientDeleted==false);
                break;
                case "Outbox":
                messages= messages.Where(x =>x.SenderId== messageParams.UserId &&
                 x.SenderDeleted==false);
                 break;
                default:
                 messages = messages.Where(u => u.RecipientId == messageParams.UserId 
                        && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }
            messages= messages.OrderByDescending(x => x.MessageSent);
            return await PagedList<Message>.CreatePagintion(messages,messageParams.PageSize,messageParams.PageNumber);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int resciveId)
        {
            var message = await _context.Messages.Include(x => x.Sender).ThenInclude(x => x.Photos)
            .Include(x => x.Recipient).ThenInclude(x => x.Photos).Where(
                x => x.RecipientId == userId && x.RecipientDeleted==false &&
                 x.SenderId == resciveId
                || x.RecipientId==resciveId
                && x.SenderId== userId
                && x.SenderDeleted == false
                ).OrderByDescending(x => x.MessageSent).ToListAsync();
                return message;
        }
         
    }
}