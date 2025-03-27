using AutoMapper;
using PatientService.DTOs;
using PatientService.Models;
using PatientService.Repositories;

namespace PatientService.Services;

public class TestRequestService : ITestRequestService
{
    private readonly ITestRequestRepository _requestRepository;
    private readonly IMapper _mapper;
    private readonly IRedisPubSubService _redisPubSubService;
    public TestRequestService(ITestRequestRepository requestRepository, IMapper mapper, IRedisPubSubService redisPubSubService)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
        _redisPubSubService = redisPubSubService;
    }

    public async Task<TestRequest> CreateTestRequestAsync(TestRequestDto testRequestDto)
    {
        var testRequest = _mapper.Map<TestRequest>(testRequestDto);
        var createdTestRequest = await _requestRepository.CreateTestRequestAsync(testRequest);

        if (createdTestRequest is not null)
            await _redisPubSubService.PublishTestRequestAsync(testRequest.TestType!);

        return createdTestRequest!;
    }
}