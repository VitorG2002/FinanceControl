using AutoMapper;
using FinanceControl.FinanceControl.Application.DTOs;
using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
            CreateMap<CategoryReadDto, Category>().ReverseMap();
        }
    }
}
