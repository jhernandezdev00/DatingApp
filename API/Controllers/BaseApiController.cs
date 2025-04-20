using API.Helpers;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/v1/[controller]")]
public class BaseApiController : ControllerBase{
    
}



