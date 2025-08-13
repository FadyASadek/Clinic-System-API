using ClinicSystem.BLL;
using ClinicSystem.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtyController(ISpecialtyService specialtyService)
        {
            this._specialtyService = specialtyService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _specialtyService.GetAll();
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Error);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _specialtyService.GetById(id);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSpecialtyDto dto)
        {
            var result = await _specialtyService.Add(dto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data!.id },
                result.Data);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateSpecialtyDto dto)
        {
            
            var result = await _specialtyService.Update(id,dto);

            if (!result.Succeeded)
                return BadRequest(result.Error);

            return NoContent(); 
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _specialtyService.DeleteById(id);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }
    }
}
