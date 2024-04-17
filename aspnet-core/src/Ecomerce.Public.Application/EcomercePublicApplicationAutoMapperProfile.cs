using AutoMapper;
using Ecomerce.Public.Orders;
using ecommerce.Attributes;
using ecommerce.Manufactrures;
using ecommerce.Orders;
using ecommerce.ProductCategories;
using ecommerce.Products;
using Ecommerce.Public.Catalogs.Manufactures;
using Ecommerce.Public.Catalogs.ProductAttributes;
using Ecommerce.Public.Catalogs.ProductCategories;
using Ecommerce.Public.Catalogs.Products;

namespace Ecomerce.Public;

public class EcomercePublicApplicationAutoMapperProfile : Profile
{
    public EcomercePublicApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */


        // product category
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryInListDto>();
       


        //product 
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductInListDto>();



        //manufacture 
        CreateMap<Manufacture, ManufactureDto>();
        CreateMap<Manufacture, ManufactureInListDto>();




        //productAttribute
        CreateMap<ProductAttribute, ProductAttributeDto>();
        CreateMap<ProductAttribute, ProductAttributeInListDto>();


        //Order
        CreateMap<Order, OrderDto>();
    }
}
