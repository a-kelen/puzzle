using System.Collections.Generic;

namespace PuzzleServer.ViewModels
{
    public class BackupVM
    {
        public int Slides { get; set; }
        public int Seconds { get; set; }

        public List<int> Tiles { get; set; }
    }
}
