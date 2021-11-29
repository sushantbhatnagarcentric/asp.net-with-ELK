using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace SampleWebApiAspNetCore.v2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FoodsController : ControllerBase
    {

        [HttpGet]
        public ActionResult Get()       
        {
            try
            {
                var rng = new Random();
                    
                if (rng.Next(0,5) < 2)
                {
                    throw new Exception("Oops what happened");
                }                
                return Ok("2.0");
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Something bad happened...");      
                return new StatusCodeResult(500);
            }
        }
        
        /*
        {
            return Ok("2.0");
        }
        */
    }
}
