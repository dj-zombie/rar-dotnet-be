using AutoMapper;
using ProductService.Dtos.Models;
using ProductService.Dtos.Requests;
using ProductService.Models;

namespace ProductService.MappingProfiles;

public class ProductSizeMappingProfile : Profile
{
    public ProductSizeMappingProfile()
    {
        // For reading: Map the entity to the DTO
        CreateMap<ProductSize, ProductSizeDto>();

        // For creating: Map the request model to the entity
        CreateMap<CreateProductSizeRequest, ProductSize>().ReverseMap();

        // For updating: Map the request model to the entity
        CreateMap<UpdateProductSizeRequest, ProductSize>().ReverseMap();
    }
}