using AutoMapper;
using PuzzleServer.Models;
using System.Linq;

namespace PuzzleServer.DTO
{
    public class Mappers: Profile
    {
        public Mappers()
        {
            CreateMap<User, UserDTO>()
                .ForMember(u => u.Name, opt => opt.MapFrom(x => x.UserName))
                .ForMember(u => u.Backup, opt => opt.MapFrom(x => x.Backups.FirstOrDefault()))
                .ForMember(u => u.Scores, opt => opt.MapFrom(x => x.Scores.OrderByDescending(s => s.Created)));
            CreateMap<Score, ScoreDTO>();
            CreateMap<Backup, BackupDTO>();
            CreateMap<Tile, TileDTO>()
                .ForMember(u => u.Value, opt => opt.MapFrom(x => x.Number));
        }
    }
}
