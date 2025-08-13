using ClinicSystem.BLL;
using ClinicSystem.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    [Authorize(Policy = "PatientOnly")]
    public async Task<IActionResult> CreateReview(CreateReviewDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _reviewService.CreateReview(userId, dto);

        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }

    [HttpGet("doctor/{doctorId}")]
    [Authorize]
    public async Task<IActionResult> GetReviewsForDoctor(int doctorId)
    {
        var result = await _reviewService.GetReviewsForDoctor(doctorId);

        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }
    [HttpPut("{id}")]
    [Authorize(Policy = "PatientOnly")]
    public async Task<IActionResult> UpdateMyReview(int id, [FromBody] UpdateReviewDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _reviewService.UpdateMyReview(id, userId!, dto);

        if (result.Succeeded)
            return Ok(result.Data);

        return BadRequest(result.Error);
    }


    [HttpDelete("{id}")]
    [Authorize(Policy = "PatientOnly")]
    public async Task<IActionResult> DeleteMyReview(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _reviewService.DeleteMyReview(id, userId!);

        if (result.Succeeded)
            return Ok(new MassegeResponnse
            {
                Massege = $"Review is Deleted"
            });

        return BadRequest(result.Error);
    }
}
