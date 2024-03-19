using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductionFacilitiesManagement.API.Requests;


namespace ProductionFacilitiesManagement.API.Controllers
{
    // Additional Task 3: Customized register route pls use /api/RoleAuth/register instead of /register
    [Route("/api/[controller]")]
    [ApiController]
    public class RoleAuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RoleAuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Add(UserRegisterRequest entity)
        {
            try
            {
                var existingEntity = await _userManager.FindByEmailAsync(entity.email);
                if (existingEntity != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        statusCode = 400,
                        message = "User name already exists"
                    });
                }
                if(entity.role != "Operator" && entity.role != "Client")
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        statusCode = 400,
                        message = "Role field must be either 'Operator' or 'Client'"
                    });
                }
                var user = new IdentityUser();
                user.UserName = entity.email;
                user.Email = entity.email;
                await _userManager.CreateAsync(user, entity.password);
                await _userManager.AddToRoleAsync(user, entity.role);
               
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
    }
}
