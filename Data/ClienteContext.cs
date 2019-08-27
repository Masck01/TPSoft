using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ingenieria_de_Software.Models;
using Microsoft.EntityFrameworkCore;

public class ClienteContext : DbContext {
    public ClienteContext (DbContextOptions<ClienteContext> options) : base (options) { }


    public DbSet<Cliente> Cliente { get; set; }
}