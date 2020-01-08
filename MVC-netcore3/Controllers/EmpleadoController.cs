using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_netcore3.Models;
using MVC_netcore3.Data;

namespace MVC_netcore3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpleadoController : ControllerBase
    {
      private readonly APIDbContext _context;

        
        private readonly ILogger<EmpleadoController> _logger;

        public EmpleadoController(ILogger<EmpleadoController> logger, APIDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<Empleado> Get(int id)
          {
            var empleado = await _context.Empleado.FindAsync(id);


            return empleado;
        }
    }
}
