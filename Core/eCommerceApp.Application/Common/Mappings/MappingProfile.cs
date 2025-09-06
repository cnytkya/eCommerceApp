using AutoMapper;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Subcategory;
using eCommerceApp.Domain.Entities;

namespace eCommerceApp.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region category
            //Category için mapping
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.Subcategories));

            //Create
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
            //Edit
            CreateMap<EditCategoryDto, Category>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

            CreateMap<CategoryDto, EditCategoryDto>().ReverseMap();
            
            //subcategory için mapping.
            CreateMap<Subcategory,SubCategoryDto>().ReverseMap();
            //create
            CreateMap<CreateSubCategoryDto, Subcategory>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src=>false));
            //edit
            CreateMap<EditSubCategoryDto, Subcategory>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            #endregion

            #region product
            #endregion
        }
    }
}
