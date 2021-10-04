using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelos;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta:IRequest<List<LibreriaMaterialDto>>
        {

        }

        public class Manejador : IRequestHandler<Ejecuta, List<LibreriaMaterialDto>>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper; 
            public Manejador(ContextoLibreria contexto,IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;

            }
            public async Task<List<LibreriaMaterialDto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibreriaMaterial.ToListAsync();
                var librosDto= _mapper.Map<List<LibreriaMaterial>, List<LibreriaMaterialDto>>(libros);
                return librosDto;

            }
        }
    }
}
