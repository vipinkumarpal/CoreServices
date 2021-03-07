using CoreServices.Handler;
using CoreServices.Handler.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreServices.API.Controller
{
    [Route("api")]
    [ApiController]
    public class CoreServiceController : ControllerBase
    {
        private readonly ICoreServiceHandler _coreservicesHandler;
      

        public CoreServiceController(ICoreServiceHandler coreServiceHandler)
        {
            _coreservicesHandler = coreServiceHandler;
        }
        [Route("coreservice/post")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<List<PostDto>> GetPost()
        {
            var result = await _coreservicesHandler.GetPost();
            return result;
        }

        [Route("coreservice/category")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<List<CategoryDto>> GetCategory()
        {
            var result = await _coreservicesHandler.GetCategory();
            return result;
        }
    }
}