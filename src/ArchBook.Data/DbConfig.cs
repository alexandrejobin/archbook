using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Data
{
    public class DbConfig : DbConfiguration
    {
        public DbConfig()
        {
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        }
    }
}
