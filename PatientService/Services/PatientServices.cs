using AutoMapper;
using PatientService.DTOs;
using PatientService.IRepositories;
using PatientService.IServices;
using PatientService.Models;

namespace PatientService.Services;

public class PatientServices : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public PatientServices(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task AddPatientAsync(PatientDto patientDto)
    {
        var patient = _mapper.Map<PatientModel>(patientDto);
        await _patientRepository.AddPatientAsync(patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        await _patientRepository.DeletePatientAsync(id);
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientAsync()
    {
        var patients = await _patientRepository.GetAllPatientAsync();
        return _mapper.Map<IEnumerable<PatientDto>>(patients);
    }

    public async Task<PatientDto> GetPatientByIdAsync(int id)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(id);
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task UpdatePatientAsync(PatientDto patientDto)
    {
        var patient = _mapper.Map<PatientModel>(patientDto);
        await _patientRepository.UpdatePatientAsync(patient);
    }
}