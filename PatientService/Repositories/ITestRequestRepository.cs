using PatientService.Models;

namespace PatientService.Repositories;

public interface ITestRequestRepository
{
    Task<TestRequest> CreateTestRequestAsync(TestRequest testRequest);
}
