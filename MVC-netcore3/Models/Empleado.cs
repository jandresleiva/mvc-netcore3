using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_netcore3.Models
{
    public class Empleado
    {
        public int id { set; get; }
        public string nombre { set; get; }
        public int dni { set; get; }
        //public DateTime fecha_nacimiento { set; get; }
        public int telefono { set; get; }
        public int telefono2 { set; get; }
        public string email { set; get; }
        public string domicilio { set; get; }
        public string rol { set; get; }
    //public DateTime fecha_alta { set; get; }
    //public DateTime fecha_borrado { set; get; }

  }
}
