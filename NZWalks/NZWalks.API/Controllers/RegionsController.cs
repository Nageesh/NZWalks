using AutoMapper;
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
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository ,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
       public async Task<IActionResult> GetAllRegions()
        {
         var regions= await  regionRepository.GetAllAsync();
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
            var regionsDTO =   mapper.Map<List<Models.DTO.Region>>(regions);
          return Ok(regionsDTO);
        
        }

    }
}
