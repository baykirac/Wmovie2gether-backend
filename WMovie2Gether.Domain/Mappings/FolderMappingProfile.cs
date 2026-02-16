using AutoMapper;
using WMovie2Gether.Domain.DTOs.Folder;
using WMovie2Gether.Domain.Entities;

namespace WMovie2Gether.Domain.Mappings;

public class FolderMappingProfile : Profile
{
    public FolderMappingProfile()
    {
        CreateMap<Folder, FolderDto>();
    }
}
