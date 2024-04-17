using AutoMapper;
using ecommerce.Admin.Catalog.Inventories;
using ecommerce.Admin.Catalog.InventoryTickets;
using ecommerce.Admin.Catalog.InventoryTickets.IventoryTicketItems;
using ecommerce.Admin.Catalog.Manufactures;
using ecommerce.Admin.Catalog.ProductAttributes;
using ecommerce.Admin.Catalog.ProductCategories;
using ecommerce.Admin.Catalog.Products;
using ecommerce.Admin.System.Roles;
using ecommerce.Admin.System.Users;
using ecommerce.Attributes;
using ecommerce.Inventories;
using ecommerce.InventoryTickets;
using ecommerce.Manufactrures;
using ecommerce.Orders;
using ecommerce.ProductCategories;
using ecommerce.Products;
using ecommerce.Roles;
using Volo.Abp.Identity;

namespace ecommerce.Admin;

public class ecommerceAdminApplicationAutoMapperProfile : Profile
{
    public ecommerceAdminApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */


        // product category
        CreateMap<ProductCategory,ProductCategoryDto>();
        CreateMap<ProductCategory,ProductCategoryInListDto>();
        CreateMap<CreateUpdateProductCategoryDto, ProductCategory>();


        //product 
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductInListDto>();
        CreateMap<CreateUpdateProductDto, Product>();


        //manufacture 
        CreateMap<Manufacture, ManufactureDto>();
        CreateMap<Manufacture, ManufactureInListDto>();
        CreateMap<CreateUpdateManufactureDto, Manufacture>();



        //productAttribute
        CreateMap<ProductAttribute, ProductAttributeDto>();
        CreateMap<ProductAttribute, ProductAttributeInListDto>();
        CreateMap<CreateUpdateProductAttributeDto, ProductAttribute>();



        //role 
        CreateMap<IdentityRole, RoleDto>().ForMember(x => x.Desciption,
            map => map.MapFrom(x => x.ExtraProperties.ContainsKey(RoleConstant.DescriptionFieldName)
            ?
            x.ExtraProperties[RoleConstant.DescriptionFieldName]
            :
            null));
        CreateMap<IdentityRole, RoleInListDto>().ForMember(x =>x.Desciption,
            map => map.MapFrom(x=>x.ExtraProperties.ContainsKey(RoleConstant.DescriptionFieldName)
            ? 
            x.ExtraProperties[RoleConstant.DescriptionFieldName] 
            : 
            null));
        CreateMap<CreateUpdateProductDto, Product>();




        //user
        CreateMap<IdentityUser, UserDto>();
        CreateMap<IdentityUser, UserInListDto>();


        //inventory 
        CreateMap<Inventory, InventoryDto>();
        CreateMap<Inventory, InventoryInListDto>();
        CreateMap<CreateUpdateInventoryDto, Inventory>();


        //inventoryTicket 
        CreateMap<InvenoryTicket, InventoryTicketDto>();
        CreateMap<InvenoryTicket, InventoryTicketInListDto>();
        CreateMap<CreateUpdateInventoryTicketDto, InvenoryTicket>();


        //IventoryTicketItem 
        CreateMap<InventoryTicketItem, InventoryTicketItemInListDto>();
        CreateMap<CreateUpdateProductDto, InventoryTicketItem>();


       
    }
}
