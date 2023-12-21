using EntityFramework.Firebird;
using FirebirdSql.Data.FirebirdClient;
using System.Data.Entity;

namespace GoPOS.Service.Common
{
    public class DbConfig : DbConfiguration
    {
        public DbConfig()
        {
            System.Data.Common.DbProviderFactories.RegisterFactory(FbProviderServices.ProviderInvariantName, FirebirdClientFactory.Instance);
            SetProviderServices("FirebirdSql.Data.FirebirdClient", FbProviderServices.Instance);
            //SetProviderServices(FbProviderServices.ProviderInvariantName, FbProviderServices.Instance);
        }
    }
}
