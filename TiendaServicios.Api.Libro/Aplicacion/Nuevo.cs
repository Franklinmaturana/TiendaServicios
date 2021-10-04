using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelos;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta:IRequest
        {
            public string Titulo { get; set; }

            public DateTime? FechaPublicacion { get; set; }

            public Guid? AutorLibro { get; set; }
        }

        public class EjecutaValidacion:AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                //Validaciones
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly ContextoLibreria _contexto;
            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto; 
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libro = new LibreriaMaterial
                {
                    Titulo=request.Titulo,
                    FechaPublicacion=request.FechaPublicacion,
                    AutorLibro=request.AutorLibro
                };

                _contexto.LibreriaMaterial.Add(libro);

                var resultado = await _contexto.SaveChangesAsync();

                if(resultado>0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo registrar el cliente");
            }
        }
    }
}
