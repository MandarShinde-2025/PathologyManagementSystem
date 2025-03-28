using PatientService.DTOs;

namespace PatientService.Services;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientAsync();
    Task<PatientDto?> GetPatientByIdAsync(int id);
    Task<int> AddPatientAsync(PatientDto patient);
    Task<bool> RequestTestAsync(TestRequestDto requestDto);
    Task UpdatePatientAsync(PatientDto patient);
    Task DeletePatientAsync(int id);
}