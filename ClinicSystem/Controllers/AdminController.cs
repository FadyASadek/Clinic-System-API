using ClinicSystem.BLL;
using ClinicSystem.BLL.Services;
using ClinicSystem.DAL;
using ClinicSystem.DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminServics)
        {
            this._adminService = adminServics;
        }
        [HttpGet("users")]
        public async Task<ActionResult> Users()
        {
            var result =await _adminService.GetUsers();
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("promote-to-doctor")]
        public async Task<IActionResult> PromoteToDoctor(PromoteToDoctorDto dto)
        {
            var result = await _adminService.PromoteToDoctor(dto);
            if (!result.Succeeded)
                return BadRequest(result.Error);

            return Ok(new { Message = result.Data });
        }
        [HttpPost("users/{id}/block")]
        public async Task<IActionResult> BlockUser(string id)
        {
            var result = await _adminService.BlockUser(id);
            if (result.Succeeded)
                return Ok(new MassegeResponnse {
                Massege = $"{result.Data}, User Is Blocked",
                });

            return BadRequest(result.Error);
        }

        [HttpPost("users/{id}/unblock")]
        public async Task<IActionResult> UnblockUser(string id)
        {
            var result = await _adminService.UnblockUser(id);
            if (result.Succeeded)
                return Ok(new MassegeResponnse
                {
                    Massege = $"{result.Data}, User unblocked",
                });

            return BadRequest(result.Error);
        }
        [HttpDelete("reviews/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await _adminService.DeleteReview(id);
            if (result.Succeeded)
                return Ok(new MassegeResponnse
                {
                    Massege = $"{result.Data},Review Is Deleted"
                });

            return NotFound(result.Error);
        }

        [HttpPut("reviews/{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto dto)
        {
            var result = await _adminService.UpdateReview(id, dto);
            if (result.Succeeded)
                return Ok(result.Data);

            return NotFound(result.Error);
        }
    }
}
