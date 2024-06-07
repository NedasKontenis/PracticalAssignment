using Microsoft.AspNetCore.Mvc;
using PracticalAssignment.ApiContracts;
using PracticalAssignment.Services;
using System.Net.Mime;

namespace PracticalAssignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NumbersController : ControllerBase
    {
        private readonly INumberService _numberService;

        public NumbersController(INumberService numberService)
        {
            _numberService = numberService;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            return Ok(new NumberResponse
            {
                Number = await _numberService.GetLatest()
            });
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateNumberRequest request)
        {
            var numbers = _numberService.Parse(request.Number?.Trim());
            await _numberService.Create(numbers);
            return NoContent();
        }
    }
}
