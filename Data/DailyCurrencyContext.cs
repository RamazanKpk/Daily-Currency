using Entitiy.Concrate;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DailyCurrencyContext : DbContext
    {
        public DbSet<DailyCurrency> DailyCurrencies {  get; set; }
        public DbSet<User> Users { get; set; }
    }
}
