﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicio.Api.Autor.Modelos;
using TiendaServicio.Api.Autor.Persistencia;

namespace TiendaServicio.Api.Autor.Aplicacion
{
    public class Consulta
    {
        public class ListaAutor : IRequest<List<AutorDto>> { }

        public class Manejador : IRequestHandler<ListaAutor,List<AutorDto>>
        {
            public readonly ContextoAutor _contexto;
            private readonly IMapper _mapper;
            public Manejador(ContextoAutor contexto,IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                var autores= await _contexto.AutorLibro.ToListAsync();
                var autoresdto = _mapper.Map<List<AutorLibro>,List<AutorDto>>(autores);
                return autoresdto;
            }
        }
    }
}
