using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Walk = NZWalks.API.Models.Domain.Walk;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }



        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.Walks
                  .Include(x => x.Region)
                  .Include(x => x.WalkDifficulty)
                  .ToListAsync();
        }

        public Task<Walk> GetAsync(Guid id)
        {
            return nZWalksDbContext.Walks
            .Include(x => x.Region)
              .Include(x => x.WalkDifficulty)
              .FirstOrDefaultAsync(x => x.Id == id);
        }
        //Add New record Method
        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }
        //Update Method 

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var ExistingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(ExistingWalk == null)
            {
                return null;
            }
            ExistingWalk.Name= walk.Name;
            ExistingWalk.Length=    walk.Length;
            ExistingWalk.RegionId = walk.RegionId;
            ExistingWalk.WalkDifficulty =walk.WalkDifficulty;
            await  nZWalksDbContext.SaveChangesAsync();
            return ExistingWalk;
        }
        //Delete Mothod
        public async Task<Walk> DeleteAsync(Guid id)
        {
          var walk = nZWalksDbContext.Walks.FirstOrDefault(x => x.Id == id);
            if(walk == null)
            {
                return null;
            }
            //delete the selected record
            nZWalksDbContext.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

    }
}
