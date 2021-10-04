using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Aplicacion;

namespace TiendaServicios.Api.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaMaterialController : ControllerBase
    {

        private readonly IMediator _mediator;
        public LibreriaMaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Post(Nuevo.Ejecuta peticion)
        {
            return await _mediator.Send(peticion);
        }

        [HttpGet]
        public async Task<ActionResult<List<LibreriaMaterialDto>>> GetLibros()
        {
            return await _mediator.Send(new Consulta.Ejecuta());
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<LibreriaMaterialDto>> getLibroUnico(Guid Id)
        {
            return await _mediator.Send(new ConsultaFiltro.Ejecuta { LibroId=Id});
        }
    }
}
