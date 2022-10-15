using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    // [Route("Region")]
    [Route("[Controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();
            #region without auto mapper
            //return DTO Regions Implementaion
            //  var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Long = region.Long,
            //        Lat = region.Lat,
            //        Population = region.Population

            //    };
            //    regionsDTO.Add(regionDTO);
            // });
            #endregion
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            
            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionsAsync")]
        public async Task<IActionResult> GetRegionsAsync(Guid id)
        {
            var regions = await regionRepository.GetAsync(id);
            if(regions == null)
            {
                return NotFound();
            }
            var regionsDTO = mapper.Map<Models.DTO.Region>(regions);
            return Ok(regionsDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest AddRegionRequest )
        {
            //Request to Domain Model 
            var region = new Models.Domain.Region()
            {
                Code = AddRegionRequest.Code,
                Name = AddRegionRequest.Name,
                Area = AddRegionRequest.Area,
                Lat = AddRegionRequest.Lat,
                Long = AddRegionRequest.Long,
                Population = AddRegionRequest.Population
            };
            //Pass details to repository
             region = await regionRepository.AddAsync(region);

            //Convert data back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population

            };

            return CreatedAtAction(nameof(GetRegionsAsync),new { id = regionDTO.Id },regionDTO  );
        }

   
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id,[FromBody] Models.DTO.UpdateRegionRequest UpdateRegionRequest)
        {
            //Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = UpdateRegionRequest.Code,
                Name = UpdateRegionRequest.Name,
                Area = UpdateRegionRequest.Area,
                Lat = UpdateRegionRequest.Lat,
                Long = UpdateRegionRequest.Long,
                Population = UpdateRegionRequest.Population
            };

            //Update region using repository

            region= await regionRepository.UpdateAsync(id,region);

            //If Null then Not found
            if(region == null)
            { return NotFound();}

            //Convert Domain back to DTO

            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population

            };

            // Return Ok response
            return Ok(regionDTO);

        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get Region from database
            var region = await regionRepository.DeleteAsync(id);

            //If Null Not Found
            if (region == null)
            {
                return NotFound();
            }

            //Convert Response back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population

            };
            //Return OK Response
            return Ok(regionDTO);

        }

    }
}
