﻿using System;
using System.Collections.Generic;

namespace PuzzleServer.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public List<ScoreDTO> Scores { get; set; }
        public BackupDTO Backup { get; set; }
    }
}
