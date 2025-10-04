using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldModel;

namespace comp_584_sever.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(Comp584DataContext context) : ControllerBase
    {
        [HttpPost("Counties")]
        public async Task<ActionResult> PostCountry()
        {
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Cities")]
        public async Task<ActionResult> PostCities()
        {
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
