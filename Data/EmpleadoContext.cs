using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ingenieria_de_Software.Models;
using Microsoft.EntityFrameworkCore;

public class EmpleadoContext : DbContext {
    public EmpleadoContext (DbContextOptions<EmpleadoContext> options) : base (options) { }

    public DbSet<Ingenieria_de_Software.Models.Empleado> Empleado { get; set; }
}