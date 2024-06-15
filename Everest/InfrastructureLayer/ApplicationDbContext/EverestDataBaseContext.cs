using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.ApplicationDbContext
{
    public class EverestDataBaseContext : DbContext
    {
        public EverestDataBaseContext(DbContextOptions<EverestDataBaseContext> options) : base(options)
        {
            
        }
    }
}
