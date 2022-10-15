using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;




namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }


        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty
                 .ToListAsync();
        }
       
        //Get by ID starts
        public async  Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            
        }

        //Add new record

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }
        //Update record
        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var ExistingWalkD = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            if(ExistingWalkD==null)
            {
                return null;
            }
            ExistingWalkD.Code= walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return ExistingWalkD;
        }
        //Delete 
        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            //var walk = nZWalksDbContext.Walks.FirstOrDefault(x => x.Id == id);
            //if (walk == null)
            //{
            //    return null;
            //}
            ////delete the selected record
            //nZWalksDbContext.Remove(walk);
            //await nZWalksDbContext.SaveChangesAsync();
            //return walk;
            var walkD = nZWalksDbContext.WalkDifficulty.FirstOrDefault(x => x.Id == id);
            if(walkD ==null)
            {
                return null;
            }
            //delete the selected record
            nZWalksDbContext.Remove(walkD);
            await nZWalksDbContext.SaveChangesAsync();
            return walkD;
        }
    }
}
