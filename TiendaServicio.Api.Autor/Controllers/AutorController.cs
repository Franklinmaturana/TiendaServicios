using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicio.Api.Autor.Aplicacion;
using TiendaServicio.Api.Autor.Modelos;

namespace TiendaServicio.Api.Autor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorController(IMediator mediator)
        {
            _mediator=mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }
        [HttpGet]

        public async Task<ActionResult<List<AutorLibro>>> GetAutores()
        {
            return await _mediator.Send(new Consulta.ListaAutor());
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<AutorLibro>> GetAutorLibro(string Id)
        {
            return await _mediator.Send(new ConsultaFiltro.AutorUnico{ AutorGuid=Id});
        }
    }
}
