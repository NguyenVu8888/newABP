using ecommerce.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ecommerce.Admin.Permissions;

public class ecommercePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {


        //---------------------------------------------------------------------------------------------


        //product
        var ProductGroup = context.AddGroup(ecommercePermissions.CatalogGroupName);

        //add prodcut
        var ProductPermission = ProductGroup.AddPermission(ecommercePermissions.Product.Default, L("Permission:Catalog.Product"));
        ProductPermission.AddChild(ecommercePermissions.Product.Create, L("Permission:Catalog.Product.Create"));
        ProductPermission.AddChild(ecommercePermissions.Product.Update, L("Permission:Catalog.Product.Update"));
        ProductPermission.AddChild(ecommercePermissions.Product.Delete, L("Permission:Catalog.Product.Delete"));
        ProductPermission.AddChild(ecommercePermissions.Product.AttributeManage, L("Permission:Catalog.Product.AttributeManage"));


        //add attribute
        var AttributePermission = ProductGroup.AddPermission(ecommercePermissions.Attribute.Default, L("Permission:Catalog.Attribute"));
        AttributePermission.AddChild(ecommercePermissions.Attribute.Create, L("Permission:Catalog.Attribute.Create"));
        AttributePermission.AddChild(ecommercePermissions.Attribute.Update, L("Permission:Catalog.Attribute.Update"));
        AttributePermission.AddChild(ecommercePermissions.Attribute.Delete, L("Permission:Catalog.Attribute.Delete"));


        //add product attribute
        var ProductAttributePermission = ProductGroup.AddPermission(ecommercePermissions.ProductAttribute.Default, L("Permission:Catalog.ProductAttribute"));
        ProductAttributePermission.AddChild(ecommercePermissions.ProductAttribute.Create, L("Permission:Catalog.ProductAttribute.Create"));
        ProductAttributePermission.AddChild(ecommercePermissions.ProductAttribute.Update, L("Permission:Catalog.ProductAttribute.Update"));
        ProductAttributePermission.AddChild(ecommercePermissions.ProductAttribute.Delete, L("Permission:Catalog.ProductAttribute.Delete"));



        //Manufacture
        var manufacturerPermission = ProductGroup.AddPermission(ecommercePermissions.Manufacture.Default, L("Permission:Catalog.Manufacturer"));
        manufacturerPermission.AddChild(ecommercePermissions.Manufacture.Create, L("Permission:Catalog.Manufacturer.Create"));
        manufacturerPermission.AddChild(ecommercePermissions.Manufacture.Update, L("Permission:Catalog.Manufacturer.Update"));
        manufacturerPermission.AddChild(ecommercePermissions.Manufacture.Delete, L("Permission:Catalog.Manufacturer.Delete"));
       
        //Product Category
        var productCategoryPermission = ProductGroup.AddPermission(ecommercePermissions.ProductCategory.Default, L("Permission:Catalog.ProductCategory"));
        productCategoryPermission.AddChild(ecommercePermissions.ProductCategory.Create, L("Permission:Catalog.ProductCategory.Create"));
        productCategoryPermission.AddChild(ecommercePermissions.ProductCategory.Update, L("Permission:Catalog.ProductCategory.Update"));
        productCategoryPermission.AddChild(ecommercePermissions.ProductCategory.Delete, L("Permission:Catalog.ProductCategory.Delete"));
        


    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ecommerceResource>(name);
    }
}
