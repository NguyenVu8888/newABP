using ecommerce.Samples;
using Xunit;

namespace ecommerce.EntityFrameworkCore.Domains;

[Collection(ecommerceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ecommerceEntityFrameworkCoreTestModule>
{

}
