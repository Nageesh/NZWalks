using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository ,IMapper mapper )
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultyAsync()
        {
            //fetch data from database -- domain walks
            
            var walksDiffiDomain = await  walkDifficultyRepository.GetAllAsync();
            //conver domain walks to dto walks
        
           var walksDDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walksDiffiDomain);

            //return ok response
         
            return Ok(walksDDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDAsync")]
        public async Task<IActionResult> GetWalkDAsync(Guid id)
        {
            //Get walk domain object from database

            var walkDDomain = await walkDifficultyRepository.GetAsync(id);

            if (walkDDomain == null)
            {
                return NotFound();
            }
            //Convert domain object to DTO

            var walksDDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDDomain);
            return Ok(walksDDTO);
        }
        [HttpPost]
        public async Task<IActionResult>  AddWalkDAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest )
        {
            // Request to Domain Model 
  
            var walkDDomain = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code

            };
            //Pass details to repository
            walkDDomain = await walkDifficultyRepository.AddAsync(walkDDomain);
            //Convert data back to DTO
            var walkDDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDDomain.Id ,
                Code = walkDDomain.Code
            };
            return CreatedAtAction(nameof(GetAllWalkDifficultyAsync),new  { id=walkDDomain.Id }, walkDDTO);    
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDAsync([FromRoute] Guid id, [FromBody]  Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest )
        {
            //Convert DTO to Domain Model
            var walkDDomain = new Models.Domain.WalkDifficulty()
            { 
                Code = updateWalkDifficultyRequest.Code 
            };
            //Update region using repository 
           
            walkDDomain = await walkDifficultyRepository.UpdateAsync(id, walkDDomain);
            //If Null then Not found
           
            if(walkDDomain== null)
            {
                return NotFound();
            }
            //Convert Domain back to DTO
            var walkDDTO = new Models.DTO.WalkDifficulty()
            { Code = walkDDomain.Code };

            // Return Ok response
            return Ok(walkDDTO);

        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDAsync(Guid id)
        {
            //Get Region from database

            var walkDDomain =  await walkDifficultyRepository.DeleteAsync(id);
             //If Null Not Found
             if(walkDDomain==null)
            {
                return NotFound();
            }
            //Convert Response back to DTO
          
            var walkDDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDDomain.Id,
                Code = walkDDomain.Code
            };
            //Return OK Response
            return Ok(walkDDTO);
        }
    }
}
