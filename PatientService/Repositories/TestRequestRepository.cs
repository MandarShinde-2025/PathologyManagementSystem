using PatientService.Data;
using PatientService.Models;

namespace PatientService.Repositories;

public class TestRequestRepository : ITestRequestRepository
{
    private readonly AppDbContext _context;

    public TestRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TestRequest> CreateTestRequestAsync(TestRequest testRequest)
    {
        await _context.TestRequests.AddAsync(testRequest);
        await _context.SaveChangesAsync();
        return testRequest;
    }
}
