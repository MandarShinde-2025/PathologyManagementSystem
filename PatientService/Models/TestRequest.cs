namespace PatientService.Models;

public class TestRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int PatientId { get; set; }
    public virtual PatientModel? Patient { get; set; }
    public string? TestType { get; set; }
    public string? TestResult { get; set; }
    public DateTime TestDate { get; set; }
    public string? Status { get; set; }
}