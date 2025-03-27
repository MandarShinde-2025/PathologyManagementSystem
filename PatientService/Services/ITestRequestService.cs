using PatientService.DTOs;
using PatientService.Models;

namespace PatientService.Services;

public interface ITestRequestService
{
    Task<TestRequest> CreateTestRequestAsync(TestRequestDto testRequest);
}
