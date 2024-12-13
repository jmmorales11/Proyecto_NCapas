using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RepositoryFactory
    {
        public static IRepository CreateRepository()
        {
            //return new EFRepository(new Entities.Sales_DB_PruebaEntities());
            var Context = new Entities.Sales_DB_PruebaEntities();
            Context.Configuration.ProxyCreationEnabled = false;
            return new EFRepository(Context);
        }
    }
}
