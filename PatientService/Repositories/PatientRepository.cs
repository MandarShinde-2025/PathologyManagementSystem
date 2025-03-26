using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.IRepositories;
using PatientService.Models;

namespace PatientService.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddPatientAsync(PatientModel patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePatientAsync(int id)
    {
        var patientToDelete = await _context.Patients.FindAsync(id);
        if (patientToDelete is not null)
        {
            _context.Patients.Remove(patientToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<PatientModel>> GetAllPatientAsync()
    {
        return await _context.Patients.ToListAsync();
    }

    public async Task<PatientModel?> GetPatientByIdAsync(int id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task UpdatePatientAsync(PatientModel patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }
}
