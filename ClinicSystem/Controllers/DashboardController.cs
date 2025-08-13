using ClinicSystem.BLL;
using ClinicSystem.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("doctor")]
    [Authorize(Policy = "DoctorOnly")]
    public async Task<IActionResult> GetDoctorDashboard()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _dashboardService.GetDoctorDashboard(userId!);
        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    } 
    [HttpGet("admin")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAdminDashboard()
    {
        var result = await _dashboardService.GetAdminDashboard();
        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }
    [HttpGet("admin/top-doctors/{num:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetTopDoctors(int num)
    {
        var result = await _dashboardService.GetTopDoctors(num);
        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }
    [HttpGet("admin/monthly-registrations")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetMonthlyRegistrations()
    {
        var result = await _dashboardService.GetMonthlyRegistrations();
        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }
    [HttpGet("doctor/appointment-distribution")]
    [Authorize(Policy = "DoctorOnly")]
    public async Task<IActionResult> GetDoctorAppointmentDistribution()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _dashboardService.GetDoctorAppointmentDistribution(userId);

        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }
}