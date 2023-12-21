using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service
{
    public class POSDbContextInitializer<TContext> : DropCreateDatabaseIfModelChanges<TContext>
        where TContext : DataContext

    {

        protected override void Seed(TContext dbContext)
        {
            // seed data

            base.Seed(dbContext);
        }
    }
}