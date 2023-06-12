using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PuzzleServer.DTO;
using PuzzleServer.Models;
using PuzzleServer.Services;
using PuzzleServer.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly IUserService userService;
        private readonly IBackupService backupService;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUserAccessor userAccessor;

        public UserController(
            IUserService userService,
            IBackupService backupService,
            IMapper mapper,
            IConfiguration configuration,
            IUserAccessor userAccessor
        )
        {
            this.userService = userService;
            this.backupService = backupService;
            this.configuration = configuration;
            this.mapper = mapper;
            this.userAccessor = userAccessor;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var user = await userService.Login(loginVM);
            if (user != null)
            {
                var mappedUser = mapper.Map<User, UserDTO>(user);
                mappedUser.Token = GenerateToken(mappedUser);
                return Ok(mappedUser);
            } else
            {
                return BadRequest();
            }
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var username = userAccessor.GetUsername();
            var user = await userService.Get(username);
            if (user != null)
            {
                var mappedUser = mapper.Map<User, UserDTO>(user);
                return Ok(mappedUser);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("backup")]
        public async Task<IActionResult> SetBackup(BackupVM backupVM)
        {
            var userId = await userAccessor.GetId();
            var backup = await backupService.Set(backupVM, userId);
            return Ok(mapper.Map<Backup, BackupDTO>(backup));
        }

        [HttpGet("backup")]
        public async Task<IActionResult> GetBackup()
        {
            var userId = await userAccessor.GetId();
            var backup = await backupService.Get(userId);
            return Ok(mapper.Map<Backup, BackupDTO>(backup));
        }

        [HttpDelete("backup")]
        public async Task<IActionResult> DeleteBackup()
        {
            var userId = await userAccessor.GetId();
            await backupService.Delete(userId);
            return Ok();
        }

        private string GenerateToken(UserDTO user)
        {
           
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

            
        }
    }
}
