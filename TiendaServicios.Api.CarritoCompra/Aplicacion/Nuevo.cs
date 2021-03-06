using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelos;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime FechaCreacionSesion { get; set; }

            public List<string> ProductoLista { get; set; }

        }

        //public class Validation:AbstractValidator<Ejecuta>
        //{

        //}

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _contexto;

            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };

                _contexto.CarritoSesions.Add(carritoSesion);

                var resultado = await _contexto.SaveChangesAsync();

                if(resultado==0)
                {
                    throw new Exception("Errores en la insercion del carrito de compras");
                }

                //Segunda parte de la lógica

                int id =carritoSesion.CarritoSesionId;

                foreach(var obj in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle()
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = id,
                        ProductoSeleccionado = obj
                    };
                    _contexto.CarritoSesionDetalles.Add(detalleSesion);
                }

                 resultado = await _contexto.SaveChangesAsync();

                if(resultado>0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el carrito de compras");
            }
        }
    }
}
