using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicio.Api.Autor.Modelos;

namespace TiendaServicio.Api.Autor.Aplicacion
{
    public class MappingProfile:Profile  
    {
        public MappingProfile()
        {
            CreateMap<AutorLibro, AutorDto>();
        }
    }
}
