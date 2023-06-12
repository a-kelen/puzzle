using Microsoft.EntityFrameworkCore;
using PuzzleServer.Models;
using PuzzleServer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleServer.Services
{
    public interface IBackupService
    {
        Task<Backup> Set(BackupVM backupVM, Guid userId);
        Task<Backup> Get(Guid userId);
        Task Delete(Guid userId);
    }

    public class BackupService : IBackupService
    {
        private readonly DbContext db;
        public BackupService(DbContext db)
        {
            this.db = db;
        }

        public async Task Delete(Guid userId)
        {
            var backup = await db.Backups.FirstOrDefaultAsync(x => x.UserId == userId);
            if (backup != null)
            {
                db.Backups.Remove(backup);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Backup> Get(Guid userId)
        {
            return await db.Backups.Include(x => x.Tiles).FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Backup> Set(BackupVM backupVM, Guid userId)
        {
            await Delete(userId);
            int tileSize = (int)Math.Sqrt(backupVM.Tiles.Count);
            var tiles = backupVM.Tiles.Select((tile, i) => new Tile
            {
                X = (int)Math.Floor((decimal)i / tileSize),
                Y = i % tileSize,
                Number = tile
            }).ToList();

            var newBackup = new Backup
            {
                Created = DateTime.UtcNow,
                UserId = userId,
                Seconds = backupVM.Seconds,
                Slides = backupVM.Slides,
                Tiles = tiles
            };
            db.Backups.Add(newBackup);
            await db.SaveChangesAsync();
            return newBackup;
        }
    }
}
