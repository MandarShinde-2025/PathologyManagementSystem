using AutoMapper;
using PatientService.DTOs;
using PatientService.Models;
using PatientService.Repositories;
using System.Text.Json;

namespace PatientService.Services;

public class PatientServices : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly IRedisPubSubService _redisPubSubService;

    public PatientServices(IPatientRepository patientRepository, IMapper mapper, IRedisPubSubService redisPubSubService)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
        _redisPubSubService = redisPubSubService;
    }

    public async Task<int> AddPatientAsync(PatientDto patientDto)
    {
        var patient = _mapper.Map<PatientModel>(patientDto);
        await _patientRepository.AddPatientAsync(patient);
        return patient.Id;
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

    public async Task<PatientDto?> GetPatientByIdAsync(int id)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(id);
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task UpdatePatientAsync(PatientDto patientDto)
    {
        var patient = _mapper.Map<PatientModel>(patientDto);
        await _patientRepository.UpdatePatientAsync(patient);
    }

    public async Task<bool> RequestTestAsync(TestRequestDto requestDto)
    {
        var request = _mapper.Map<TestRequest>(requestDto);
        request.Status = "Pending";
        var result = await _patientRepository.RequestTestAsync(request);
        await _redisPubSubService.PublishTestRequestAsync(JsonSerializer.Serialize(request));
        return result;
    }
}