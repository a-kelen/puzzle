using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PuzzleServer.Models;
using PuzzleServer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleServer.Services
{
    public interface IUserService
    {
        Task<User> Login(LoginVM loginVM);
        Task<User> Get(string name);
    }

    public class UserService : IUserService
    {
        private readonly DbContext db;
        private readonly UserManager<User> userManager;

        public UserService(DbContext dbContext, UserManager<User> userManager)
        {
            this.db = dbContext;
            this.userManager = userManager;
        }

        public async Task<User> Get(string name)
        {
            return await db.Users.Include(x => x.Scores)
                .Include(x => x.Backups)
                .ThenInclude(x => x.Tiles)
                .FirstOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<User> Login(LoginVM loginVM)
        {
            var existingUser = await Get(loginVM.Name);
            User user;
            if (existingUser == null)
            {
                user = new User { Email = loginVM.Name, UserName = loginVM.Name };
                await userManager.CreateAsync(user, loginVM.Password);
                return user;
            } else
            {
                var result = await userManager.CheckPasswordAsync(existingUser, loginVM.Password);
                if (result)
                {
                    return existingUser;
                } else
                {
                    return null;
                }
            }
           

            return user;
        }

       
    }
}
