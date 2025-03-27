using PatientService.Models;

namespace PatientService.Repositories;

public interface IPatientRepository
{
    Task<IEnumerable<PatientModel>> GetAllPatientAsync();
    Task<PatientModel?> GetPatientByIdAsync(int id);
    Task<int> AddPatientAsync(PatientModel patient);
    Task UpdatePatientAsync(PatientModel patient);
    Task DeletePatientAsync(int id);
}