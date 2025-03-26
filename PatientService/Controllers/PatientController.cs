using Microsoft.AspNetCore.Mvc;
using PatientService.DTOs;
using PatientService.IServices;

namespace PatientService.Controllers
{
    [Route("api/patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatientsAsync()
        {
            var patients = await _patientService.GetAllPatientAsync();
            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPatientByIdAsync(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientAsync([FromBody] PatientDto patient)
        {
            await _patientService.AddPatientAsync(patient);
            return CreatedAtAction(nameof(GetPatientByIdAsync), new { id = patient.Id }, patient);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePatientAsync(int id, [FromBody] PatientDto patient)
        {
            if (id != patient.Id) return BadRequest("Incorrect patient details!");

            await _patientService.UpdatePatientAsync(patient);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePatientAsync(int id)
        {
            await _patientService.DeletePatientAsync(id);
            return NoContent();
        }
    }
}