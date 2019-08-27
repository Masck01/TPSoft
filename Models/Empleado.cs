using System;
using System.ComponentModel.DataAnnotations;

namespace Ingenieria_de_Software.Models {
    public class Empleado {
        public int id { get; set; }
        public string nombre { get; set; }
        public int legajo { get; set; }
        public int telefono { get; set; }
        public int sueldo { get; set; }
        public string domicilio{get;set;}

    }
}