using System.Threading.Tasks;

namespace ecommerce.Data;

public interface IecommerceDbSchemaMigrator
{
    Task MigrateAsync();
}
