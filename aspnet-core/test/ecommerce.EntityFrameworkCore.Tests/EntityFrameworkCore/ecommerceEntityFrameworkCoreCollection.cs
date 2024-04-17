using Xunit;

namespace ecommerce.EntityFrameworkCore;

[CollectionDefinition(ecommerceTestConsts.CollectionDefinitionName)]
public class ecommerceEntityFrameworkCoreCollection : ICollectionFixture<ecommerceEntityFrameworkCoreFixture>
{

}
