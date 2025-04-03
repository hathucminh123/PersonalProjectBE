using AutoMapper;
using SalesProject.Dtos;
using SalesProject.DTOs;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs;
using SalesProject.Models.DTOs.Request;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<Discounts , DiscountResponseDto>().ReverseMap();
            CreateMap<Discounts, DiscountRequestDto>().ReverseMap();
            CreateMap<UserRequest, Users>().ReverseMap();
            CreateMap<Users, UserDto>().ReverseMap();
            CreateMap<Products, ProductDto>()
             
                .ReverseMap();
            



            CreateMap<AddProductDto, Products>()
                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) // Gán thời gian hiện tại vào CreatedAt
                  .ForMember(dest => dest.Tags, opt => opt.Ignore()) // Bỏ qua Tags, sẽ được gán sau
                  .ForMember(dest => dest.IsBestSeller, opt => opt.MapFrom(src => src.IsBestSeller)) // Nếu muốn ánh xạ IsBestSeller
                  .ReverseMap(); // Ánh xạ ngược từ Products về AddProductDto (nếu cần thiết)


            CreateMap<UpdateProductRequestDto, Products>()
              .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Không cho phép cập nhật CreatedAt
              .ForMember(dest => dest.Tags, opt => opt.Ignore()) // Được sinh ra tự động từ trạng thái sản phẩm
              .ReverseMap();

            CreateMap<Payment, PaymentDto>()
               .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString())); // Chuyển enum thành string

            CreateMap<Orders, OrderDto>()
                .ForMember(dest => dest.Discounts, opt =>
                    opt.MapFrom(src => src.OrderDiscounts != null
                        ? src.OrderDiscounts.Select(od => od.Discount)
                        : new List<Discounts>()))   
                .ForMember(dest => dest.OrderDetails, opt =>
                    opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.User, opt =>
                    opt.MapFrom(src => src.User))
                .ForMember(dest => dest.ShippingAddress, opt =>
                    opt.MapFrom(src => src.ShippingAddress))
               .ForMember(dest => dest.PaymentMethod, opt =>
                    opt.MapFrom(src => src.PaymentMethod));

            CreateMap<OrderDetails, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product !=null ? src.Product.Name : "No name"))
                          .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : "No Image"));

            CreateMap<OrderDiscounts, DiscountDto>()
       .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Discount != null ? src.Discount.Code : "no code"))
       .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => src.Discount != null ? src.Discount.DiscountType.ToString() : "No Type"))
       .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.Discount != null ? src.Discount.DiscountAmount : 0));


            CreateMap<Category, CategoryResponse>()
             .ForMember(dest => dest.SubCategories,
                        opt => opt.MapFrom(src => src.SubCategories))
             .ReverseMap();

            CreateMap<CategoryCreateRequest, Category>().ReverseMap();
                CreateMap<CategoryUpdateRequest, Category>().ReverseMap(); ;

            CreateMap<SubCategory, SubCategoryResponse>()
         .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // One-way mapping for Create and Update
            CreateMap<SubCategoryCreateRequest, SubCategory>().ReverseMap(); ;
            CreateMap<SubCategoryUpdateRequest, SubCategory>().ReverseMap(); ;


            CreateMap<SubCategory, SubCategoryDTO>().ReverseMap();

            CreateMap<CreateAddressRequest, Address>().ReverseMap();


            CreateMap<CreateAddressRequestDto, Address>().ReverseMap();



            CreateMap<UpdateAddressRequestDto, Address>().ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => src.AddressId)).ReverseMap();

            CreateMap<Address, AddressResponse>().ReverseMap();


            CreateMap<DeleteAddressUser,Address>().ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => src.AddressId))
             .ReverseMap(); 

        }

    }
}
