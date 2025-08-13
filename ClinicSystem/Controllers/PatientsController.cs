using ClinicSystem.BLL;
using ClinicSystem.BLL.Interfaces;
using ClinicSystem.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "PatientOnly")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("me/profile")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _patientService.GetMyProfile(userId);
        if (result.Succeeded)
            return Ok(result.Data);

        return NotFound(result.Error);
    }

    [HttpPut("me/profile")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdatePatientProfileDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _patientService.UpdateMyProfile(userId, dto);
        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }
}