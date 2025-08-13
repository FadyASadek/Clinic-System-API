using ClinicSystem.BLL.Services;
using ClinicSystem.DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            this._roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _roleService.GetAllRoles();
            if(result.Succeeded)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Error);
        }
        [HttpPost("add-role")]
        public async Task<ActionResult> AddRole([FromBody]AddRoleDto addRoleDto)
        {
            var result = await
                _roleService.AddRole(addRoleDto);
            if (result.Succeeded)
            {
                return Ok(new ApiResponseDto<AddRoleDto>
                {
                    Message = "Role was added successfully!",
                    Data = result.Data
                });
            }
            return BadRequest(result.Error);
        }
        [HttpPost("assign-role")]
        public async Task<ActionResult> AssignRole([FromBody]AssignRoleDto assignRoleDto)
        {
            var result = await _roleService.AssignRole(assignRoleDto);
            if (result.Succeeded)
            {
                return Ok(new ApiResponseDto<AssignRoleDto>
                {
                    Message = "Role assigned successfully.", 
                    Data = result.Data
                }); 
            }
            return BadRequest(result.Error);
        }
    }
}
