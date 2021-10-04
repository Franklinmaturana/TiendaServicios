using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelos;

namespace TiendaServicios.Api.CarritoCompra.Persistencia
{
    public class CarritoContexto:DbContext
    {
        public CarritoContexto(DbContextOptions<CarritoContexto> options):base(options)
        {

        }

        public DbSet<CarritoSesion> CarritoSesions { get; set; }

        public DbSet<CarritoSesionDetalle> CarritoSesionDetalles { get; set; }
    }
}
