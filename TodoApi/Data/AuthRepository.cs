using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ToDoContext _context;
        public AuthRepository(ToDoContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string userName, string password)
        {
            var user= await _context.Users.FirstOrDefaultAsync(x=>x.UserName==userName);
            if (user==null)
            {
                return null;
            }
            if (!verifyPaswordHash(password,user.PasswordHash,user.PasswordSalt ))
            {
                return null;
            }
            return user;
        }
        private bool verifyPaswordHash( string password,byte[] PasswordHash,byte [] PasswordSalt ){
            using (var hmac= new System.Security.Cryptography.HMACSHA512(PasswordSalt) )
            {
                var computedHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++){
                    if (computedHash[i]!=PasswordHash[i])
                    {
                        return false;
                    }
                }            
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash,passwordSalt;
            CreatPasswordHash( password , out passwordHash, out passwordSalt);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        private void CreatPasswordHash(string password, out byte[] passwordHash,out byte[] passwordSalt){
            using (var hmac= new System.Security.Cryptography.HMACSHA512())
            {
                passwordHash=hmac.Key;
                passwordSalt=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public async Task<bool> UserExists(string userName)
        {
           if (await _context.Users.AnyAsync(x=>x.UserName==userName))
           {
               return true;
           } 
           return false;
        }
    }
}