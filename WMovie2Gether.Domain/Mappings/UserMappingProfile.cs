using AutoMapper;
using WMovie2Gether.Domain.DTOs.User;
using WMovie2Gether.Domain.Entities;

namespace WMovie2Gether.Domain.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
    }
}
