﻿namespace PatientService.DTOs;

public class PatientDto
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? WhatsAppNumber { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
