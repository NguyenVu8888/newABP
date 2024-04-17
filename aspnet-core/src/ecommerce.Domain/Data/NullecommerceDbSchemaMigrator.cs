using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ecommerce.Data;

/* This is used if database provider does't define
 * IecommerceDbSchemaMigrator implementation.
 */
public class NullecommerceDbSchemaMigrator : IecommerceDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
