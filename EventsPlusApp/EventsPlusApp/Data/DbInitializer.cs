using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusApp.Data
{
    public static class DbInitializer
    {
        //Initialize database
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }

    }
}
