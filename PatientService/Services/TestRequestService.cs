using AutoMapper;
using PatientService.DTOs;
using PatientService.Models;
using PatientService.Repositories;

namespace PatientService.Services;

public class TestRequestService : ITestRequestService
{
    private readonly ITestRequestRepository _requestRepository;
    private readonly IMapper _mapper;
    public TestRequestService(ITestRequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<TestRequest> CreateTestRequestAsync(TestRequestDto testRequestDto)
    {
        var testRequest = _mapper.Map<TestRequest>(testRequestDto);
        var createdTestRequest = await _requestRepository.CreateTestRequestAsync(testRequest);
        return createdTestRequest;
    }
}