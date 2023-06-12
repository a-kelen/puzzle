using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace PuzzleServer.Models
{
    public class User: IdentityUser<Guid>
    {
        public ICollection<Score> Scores { get; set; }
        public ICollection<Backup> Backups { get; set; }
    }
}
