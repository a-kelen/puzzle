using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PuzzleServer.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PuzzleServer
{
    public interface IUserAccessor
    {
        string GetUsername();
        Task<Guid> GetId();
        Task<User> GetUser();
    }
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DbContext db;

        public UserAccessor(IHttpContextAccessor http, DbContext db)
        {
            httpContextAccessor = http;
            this.db = db;
        }

        public string GetUsername()
        {
            string username = httpContextAccessor.HttpContext.User?.Claims?
                                .FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            return username;
        }

        public async Task<User> GetUser()
        {
            string username = httpContextAccessor.HttpContext.User?.Claims?
                                .FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
           
            return await db.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<Guid> GetId()
        {
            return (await GetUser()).Id;
        }
    }
}
