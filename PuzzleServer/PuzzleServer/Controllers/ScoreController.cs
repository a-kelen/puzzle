using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuzzleServer.DTO;
using PuzzleServer.Models;
using PuzzleServer.Services;
using PuzzleServer.ViewModels;
using System.Threading.Tasks;

namespace PuzzleServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScoreController : ControllerBase
    {

        private readonly IScoreService scoreService;
        private readonly IUserAccessor userAccessor;
        private readonly IMapper mapper;

        public ScoreController(IScoreService scoreService, IUserAccessor userAccessor, IMapper mapper)
        {
            this.scoreService = scoreService;
            this.userAccessor = userAccessor;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddScore(ScoreVM scoreVM)
        {
            var userId = await userAccessor.GetId();
            return Ok(mapper.Map<Score, ScoreDTO>(await scoreService.Add(scoreVM, userId)));
        }
    }
}
