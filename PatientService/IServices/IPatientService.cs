using PatientService.DTOs;

namespace PatientService.IServices;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientAsync();
    Task<PatientDto> GetPatientByIdAsync(int id);
    Task AddPatientAsync(PatientDto patient);
    Task UpdatePatientAsync(PatientDto patient);
    Task DeletePatientAsync(int id);
}