using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProfileDto, MyProfile>();
            CreateMap<UpdateProfileDto, MyProfile>();
        }
    }
}