using ClinicSystem.BLL;
using ClinicSystem.BLL.Interfaces;
using ClinicSystem.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    [Authorize(Roles = "Doctor,Patient")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyAppointmentHistory()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
            return Unauthorized();

        var result = await _appointmentService.GetMyAppointments(userId, userRole);

        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }
    [Authorize(Policy = "PatientOnly")]
    [HttpPost("appointments")]
    public async Task<IActionResult> CreateAppointment(CreateAppointmentDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _appointmentService.CreateAppointment(userId, dto);

        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }

    [Authorize(Policy = "DoctorOnly")]
    [HttpPut("{id}/Confirm")]
    public async Task<IActionResult> ConfirmAppointment(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _appointmentService.ConfirmAppointment(id, userId);

        if (result.Succeeded)
            return Ok(new { Message = result.Data });

        return BadRequest(result.Error);
    }

    [Authorize(Roles = "Doctor,Patient")]
    [HttpPut("{id}/Cancel")]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
            return Unauthorized();

        var result = await _appointmentService.CancelAppointment(id, userId, userRole);

        if (result.Succeeded)
            return Ok(new { Message = result.Data });

        return BadRequest(result.Error);
    }

    [Authorize(Policy = "DoctorOnly")]
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteAppointment(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _appointmentService.CompleteAppointment(id, userId);

        if (result.Succeeded)
            return Ok(new { Message = result.Data });

        return BadRequest(result.Error);
    }
}