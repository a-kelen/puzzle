using PuzzleServer.Models;
using PuzzleServer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleServer.Services
{
    public interface IScoreService
    {
        Task<Score> Add(ScoreVM scoreVM, Guid userId);
    }

    public class ScoreService :IScoreService
    {
        private readonly DbContext db;

        public ScoreService(DbContext db)
        {
            this.db = db;
        }

        public async Task<Score> Add(ScoreVM scoreVM, Guid userId)
        {
            var score = new Score
            {
                UserId = userId,
                Created = DateTime.Now,
                Seconds = scoreVM.Seconds,
                Slides = scoreVM.Slides,
            };
            db.Scores.Add(score);
            await db.SaveChangesAsync();

            return score;
        }
    }
}
