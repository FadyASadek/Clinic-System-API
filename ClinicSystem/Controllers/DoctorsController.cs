using ClinicSystem.BLL.Interfaces;
using ClinicSystem.DAL;
using ClinicSystem.DAL.DTOs.WorkingHourDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }


    [HttpGet] 
    public async Task<IActionResult> GetAllDoctors([FromQuery] DoctorFilterDto doctorFilterDto)
    {
        var result = await _doctorService.GetAll(doctorFilterDto);
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize] 
    public async Task<IActionResult> GetDoctorById(int id)
    {
        var result = await _doctorService.GetById(id);
        if (result.Succeeded)
            return Ok(result.Data);

        return NotFound(result.Error);
    }
    [Authorize]
    [HttpGet("{id}/available-slots")] 
    public async Task<IActionResult> GetAvailableSlots(int id, [FromQuery] DateTime day)
    {
        var result = await _doctorService.GetAvailableSlots(id, day);
        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }


    [Authorize(Policy = "DoctorOnly")]
    [HttpGet("me/profile")] 
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _doctorService.GetMyProfile(userId);
        if (result.Succeeded)
            return Ok(result.Data);

        return NotFound(result.Error);
    }
    [Authorize(Policy = "DoctorOnly")]
    [HttpPut("me/profile")] 
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateDoctorProfileDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _doctorService.UpdateMyProfile(userId, dto);
        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }
    [Authorize(Policy = "DoctorOnly")]
    [HttpPost("working-hours")] 
    public async Task<IActionResult> SetWorkingHours([FromBody] IEnumerable<WorkingHourDto> dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _doctorService.SetWorkingHours(userId, dto);
        if (result.Succeeded)
            return Ok(new { Message = result.Data });

        return BadRequest(result.Error);
    }
}