using System.Collections.Generic;
using TodoApi.Models;
using System.Linq;
using Newtonsoft.Json;

namespace TodoApi.Data
{
    public class Seed
    {
        public static void seedUsers(ToDoContext context) {
            if (!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordHash,passwordSalt;
                    CreatPasswordHash("password",out passwordHash,out passwordSalt);
                    user.PasswordHash=passwordHash;
                    user.PasswordSalt=passwordSalt;
                    user.UserName= user.UserName.ToLower();
                    context.Users.Add(user);
                }
                context.SaveChanges();
            }
        }
        private static void CreatPasswordHash(string password, out byte[] passwordHash,out byte[] passwordSalt){
            using (var hmac= new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt=hmac.Key;
                passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }
    }
}