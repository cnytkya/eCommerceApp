using AutoMapper;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Product;
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
            CreateMap<Subcategory, SubCategoryDto>().ReverseMap();
            //create
            CreateMap<CreateSubCategoryDto, Subcategory>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
            //edit
            CreateMap<EditSubCategoryDto, Subcategory>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Subcategory, SubCategoryDto>().ReverseMap();
            CreateMap<SubCategoryDto, EditSubCategoryDto>().ReverseMap();
            #endregion

            #region product 
            //product için mapping
            //List products
            CreateMap<Product, ProductDto>()
                .ForMember(p => p.SubcategoryName, opt => opt.MapFrom(src => src.Subcategory.Name))
                .ForMember(p => p.CategoryName, opt => opt.MapFrom(src => src.Subcategory.Category.Name));

            //create product
            CreateMap<CreateProductDto, Product>()
                .ForMember(p => p.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(p => p.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(p => p.IsDeleted, opt => opt.MapFrom(src => false));

            //edit product
            CreateMap<EditProductDto, Product>()
                .ForMember(p => p.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<ProductDto, EditProductDto>()
                .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(src => src.SubcategoryId)).ReverseMap();
            #endregion
        }
    }
}
