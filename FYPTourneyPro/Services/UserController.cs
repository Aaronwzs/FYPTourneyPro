using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace FYPTourneyPro.Services
{
    [Route("api/user")]
    public class UserController : AbpController
    {
        private readonly ICurrentUser currentUser;

        public UserController(ICurrentUser currentUser)
        {
            this.currentUser = currentUser;
        }

        [HttpGet("current-id")]
        public IActionResult GetCurrentUserId()
        {
            if (!currentUser.IsAuthenticated)
            {
                return Unauthorized(new { message = "You need to log in." });
            }

            return Ok(new { UserId = currentUser.Id });
        }

    }

}

