using PatientService.Models;

namespace PatientService.IRepositories;

public interface IPatientRepository
{
    Task<IEnumerable<PatientModel>> GetAllPatientAsync();
    Task<PatientModel> GetPatientByIdAsync(int id);
    Task AddPatientAsync(PatientModel patient);
    Task UpdatePatientAsync(PatientModel patient);
    Task DeletePatientAsync(int id);
}