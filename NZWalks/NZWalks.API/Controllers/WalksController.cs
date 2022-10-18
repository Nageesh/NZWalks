using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;
using NZWalks.API.Models.DTO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
 
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper,
            IRegionRepository regionRepository, 
            IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync(Models.DTO.AddWalkRequest AddWalkRequest  )
        {
            //validate input request
            if(!(await ValidateAddWalkAsync(AddWalkRequest)))
            {
                return BadRequest(ModelState);
            }

           

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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody]  Models.DTO.UpdateWalkRequest UpdateWalkRequest )
        {
            //Validate input request
            if(! (await ValidateUpdateWalkAsync(UpdateWalkRequest)))
            {
                return BadRequest(ModelState) ;
            }

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
        [Authorize(Roles = "writer")]
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
        
        
        #region Private Methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest AddWalkRequest)
        {
            #region Commented here and implemented in Fluent validator
            //if(AddWalkRequest ==null)
            // {
            //     ModelState.AddModelError(nameof(AddWalkRequest),
            //        $"{nameof(AddWalkRequest)} Add walk request data is required");
            //     return false;

            // }
            // if (string.IsNullOrWhiteSpace(AddWalkRequest.Name))
            // {
            //     ModelState.AddModelError(nameof(AddWalkRequest.Name),
            //         $"{nameof(AddWalkRequest.Name)} cannot be null or empty or whitespace ");
            // }
            // if (AddWalkRequest.Length < 0)
            // {
            //     ModelState.AddModelError(nameof(AddWalkRequest.Name),
            //         $"{nameof(AddWalkRequest.Length)} cannot be lessthan zero");
            // }
            #endregion 
            //Since need to input valid region id need to inject region repository
            var region = await regionRepository.GetAsync(AddWalkRequest.RegionId );
           if(region == null)
            {
                ModelState.AddModelError(nameof(AddWalkRequest.RegionId),
                                  $"{nameof(AddWalkRequest.RegionId)} is invalid");
            }
            //Since need to input valid Walkdifficulty id need to inject Walkdifficulty repository
           var walkDifficulty= await walkDifficultyRepository.GetAsync(AddWalkRequest.WalkDifficultyId);
            if(walkDifficulty== null)
            {
                ModelState.AddModelError(nameof(AddWalkRequest.WalkDifficultyId),
                                $"{nameof(AddWalkRequest.WalkDifficultyId)} is invalid");
            }
            if (ModelState.ErrorCount> 0)
            {
                return false;
            }
            return true;

        }
        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest UpdateWalkRequest)
        {
            #region Commented here and implemented in Fluent validator
            //if (UpdateWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(AddWalkRequest),
            //       $"{nameof(UpdateWalkRequest.Name)} Update walk request data is required");
            //    return false;

            //}
            //if (string.IsNullOrWhiteSpace(UpdateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(UpdateWalkRequest.Name),
            //        $"{nameof(UpdateWalkRequest.Name)} cannot be null or empty or whitespace ");
            //}
            //if (UpdateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(UpdateWalkRequest.Name),
            //        $"{nameof(UpdateWalkRequest.Length)} sould be greaterthan zero");
            //}
            #endregion 
            //Since need to input valid region id need to inject region repository
            var region = await regionRepository.GetAsync(UpdateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(UpdateWalkRequest.RegionId),
                                  $"{nameof(UpdateWalkRequest.RegionId)} is invalid");
            }
            //Since need to input valid Walkdifficulty id need to inject Walkdifficulty repository
            var walkDifficulty = await  walkDifficultyRepository.GetAsync(UpdateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(UpdateWalkRequest.WalkDifficultyId),
                                $"{nameof(UpdateWalkRequest.WalkDifficultyId)} is invalid");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
       
        #endregion
    }
}
