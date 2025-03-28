using PatientService.Models;

namespace PatientService.DTOs;

public class TestRequestDto
{
    public int PatientId { get; set; }
    public virtual PatientModel? Patient { get; set; }
    public string? TestType { get; set; }
    public string? Status { get; set; }
}
