using ecommerce.Admin.Samples;
using Xunit;

namespace ecommerce.EntityFrameworkCore.Applications;

[Collection(ecommerceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ecommerceEntityFrameworkCoreTestModule>
{

}
