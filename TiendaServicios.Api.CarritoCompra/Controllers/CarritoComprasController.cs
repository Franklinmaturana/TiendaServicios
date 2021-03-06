using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Aplicacion;

namespace TiendaServicios.Api.CarritoCompra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoComprasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarritoComprasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Post (Nuevo.Ejecuta peticion)
        {
            return await _mediator.Send(peticion);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CarritoDto>> GetUnico(int id)
        {
            return await _mediator.Send(new Consulta.Ejecuta {CarritoSesionId= id });
        }
    }
}
