using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;
using NZWalks.API.Models.DTO;
using System.Xml.Linq;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
 
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;   
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkAsync()
        {
            //fetch data from database -- domain walks
            var walksdomain = await walkRepository.GetAllAsync();

            //conver domain walks to dto walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksdomain); 
            //return ok response
            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]

        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get walk domain object from database
            var walkdomain = await walkRepository.GetAsync(id);
            if (walkdomain == null)
            {
                return NotFound();
            }
            //Convert domain object to DTO
            var walksDTO = mapper.Map<Models.DTO.Walk>(walkdomain);
            return Ok(walksDTO);
        }

       [HttpPost]

       public async Task<IActionResult> AddWalkAsync(Models.DTO.AddWalkRequest AddWalkRequest  )
        {
            // Request to Domain Model 
          var walkdomain = new Models.Domain.Walk()
            {
                Name = AddWalkRequest.Name,
                Length = AddWalkRequest.Length,
                RegionId = AddWalkRequest.RegionId,
                WalkDifficultyId = AddWalkRequest.WalkDifficultyId
            };
            //Pass details to repository
           
            walkdomain = await  walkRepository.AddAsync(walkdomain);
            //Convert data back to DTO
   
            var walkDTO = new Models.DTO.Walk()
            {
                Id = walkdomain.Id,
                Name = walkdomain.Name,
                Length = walkdomain.Length,
                RegionId = walkdomain.RegionId,
                WalkDifficultyId = walkdomain.WalkDifficultyId
            };
         
            return CreatedAtAction(nameof(GetAllWalkAsync), new {id = walkdomain.Id}, walkDTO);
           
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDAsync([FromRoute] Guid id, [FromBody]  Models.DTO.UpdateWalkRequest UpdateWalkRequest )
        {
            //Convert DTO to Domain Model
            var walkDomain = new Models.Domain.Walk()
            {
                Name = UpdateWalkRequest.Name,
                Length = UpdateWalkRequest.Length,
                RegionId = UpdateWalkRequest.RegionId,
                WalkDifficultyId = UpdateWalkRequest.WalkDifficultyId
            };
            //Update region using repository 
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            //If Null then Not found
              if (walkDomain == null)
              { return NotFound(); }

            //Convert Domain back to DTO
            var walkDTO = new Models.DTO.Walk()
            {
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
           // Return Ok response
            return Ok(walkDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Get Region from database
          
            var walk = await walkRepository.DeleteAsync(id);
           //If Null Not Found
           if(walk == null)
            {
                return NotFound();
            }
            //Convert Response back to DTO
            var walkDTO = new Models.DTO.Walk()
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId
            };
            //Return OK Response
          return Ok(walkDTO);

        }
    }
}
