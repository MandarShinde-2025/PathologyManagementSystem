using AutoMapper;
using PatientService.DTOs;
using PatientService.Models;

namespace PatientService.Profiles;

public class PatientProfile: Profile
{
    public PatientProfile()
    {
        CreateMap<PatientModel, PatientDto>().ReverseMap();
    }
}