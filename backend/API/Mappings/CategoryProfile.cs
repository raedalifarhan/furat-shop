using API.DTOs.CategoryDTOs;
using API.Models;
using AutoMapper;

namespace API.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>(); 
            CreateMap<Category, CategoryListDto>()
                .IncludeMembers(x => x.Parent)
                .ForMember(dis => dis.ParentCategoryName, o => o.MapFrom(src => src.Parent!.CategoryName)); 
            CreateMap<CategorySaveDto, Category>(); 
        }
    }
}