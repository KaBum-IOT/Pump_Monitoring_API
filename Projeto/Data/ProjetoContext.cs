using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projeto;

namespace Projeto.Data
{
    public class ProjetoContext : DbContext
    {
        public ProjetoContext (DbContextOptions<ProjetoContext> options)
            : base(options)
        {
        }

        public DbSet<Projeto.Sensor> Sensor { get; set; } = default!;
        public DbSet<Projeto.User> User { get; set; } = default!;
    }
}
