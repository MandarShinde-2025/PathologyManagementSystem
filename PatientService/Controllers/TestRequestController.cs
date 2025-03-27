using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientService.DTOs;
using PatientService.Services;

namespace PatientService.Controllers
{
    [Authorize(Roles = "Patient")]
    [Route("api/test-request")]
    [ApiController]
    public class TestRequestController : ControllerBase
    {
        private readonly ITestRequestService _requestService;

        public TestRequestController(ITestRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestRequest([FromBody] TestRequestDto requestDto)
        {
            var result = await _requestService.CreateTestRequestAsync(requestDto);
            return Ok(result);
        }
    }
}