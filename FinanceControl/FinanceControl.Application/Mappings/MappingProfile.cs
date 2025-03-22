using AutoMapper;
using FinanceControl.FinanceControl.Application.DTOs.Category;
using FinanceControl.FinanceControl.Application.DTOs.RecurringTransaction;
using FinanceControl.FinanceControl.Application.DTOs.Transaction;
using FinanceControl.FinanceControl.Application.DTOs.User;
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
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<UserReadDto, User>().ReverseMap();
            CreateMap<Transaction, TransactionCreateDto>().ReverseMap();
            CreateMap<Transaction, TransactionUpdateDto>().ReverseMap();
            CreateMap<TransactionReadDto, Transaction>().ReverseMap();
            CreateMap<RecurringTransaction, RecurringTransactionCreateDto>().ReverseMap();
            CreateMap<RecurringTransaction, RecurringTransactionUpdateDto>().ReverseMap();
            CreateMap<RecurringTransactionReadDto, RecurringTransaction>().ReverseMap();
            CreateMap<Transaction, RecurringTransaction>().ReverseMap()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src=> DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
