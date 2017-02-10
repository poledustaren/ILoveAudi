using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ILoveAudi.Models
{
    public class ILoveAudiContext:DbContext
    {
        public ILoveAudiContext(DbContextOptions<ILoveAudiContext> options)
            : base(options)
        {}

        public DbSet<Car> Cars { get; set; }
    }
}
