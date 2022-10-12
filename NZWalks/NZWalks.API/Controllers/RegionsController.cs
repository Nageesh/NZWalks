using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    // [Route("Region")]
    [Route("[Controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;

        public RegionsController(IRegionRepository regionRepository )
        {
            this.regionRepository = regionRepository;
        }
        [HttpGet]
       public IActionResult GetAllRegions()
        {
         var regions=   regionRepository.GetAll();

            //return DTO Regions Implementaion
            var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Long = region.Long,
                    Lat = region.Lat,
                    Population = region.Population

                };
                regionsDTO.Add(regionDTO);
            });
                return Ok(regionsDTO);
        
        }

    }
}
