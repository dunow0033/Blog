using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostLikeController : ControllerBase
{
    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
    {

    }
}
