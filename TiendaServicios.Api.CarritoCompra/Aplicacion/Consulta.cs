using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta:IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>//Damos el modelo que entrara por peticion y como segundo parametro lo que retornara
        {
            private readonly CarritoContexto _contexto;//creamos una variable para inyectar el contexto (conexion a la bd)
            private readonly ILibrosService _librosService;//Creamos objeto para inyectar el servicio que consumira el microservicio de libros

            public Manejador(CarritoContexto contexto,ILibrosService librosService)
            {
                _contexto = contexto;
                _librosService= librosService;
            }
            public async  Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)//implementación de la interfaz
            {
                var carritoSesion = await _contexto.CarritoSesions.FirstOrDefaultAsync(x=>x.CarritoSesionId==request.CarritoSesionId);//Consulta asincrona del carrito sesion con la lista De CarritoDetalle
                var carritoSesionDetalle = await _contexto.CarritoSesionDetalles.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();//Me retornara la lista de productos del carrito
                //El objeto CarritoSesionDetalle tiene una lista de los id de los productos para conocer el detalle real debo consumir la api de libro

                var listaCarritoDto = new List<CarritoDetalleDto>();

                foreach (var libros in carritoSesionDetalle)
                {
                    //metodo para sacar el id del producto y consultar uno por uno en libros
                    var response = await _librosService.GetLibro(new Guid(libros.ProductoSeleccionado)); //Consumo la api de libros
                    if (response.resultado)//Recordemos que el servicio retorna una tupla con resultado,libro y erromessage si el resultado es true funciono
                    {
                        var objetoLibro = response.Libro;//obtenemos el segundo parametro de la tupla el libro
                        //debemos crear una lista de CarritoDetalleDto que soporte estos libros devueltos que es el de la linea 36
                        //Ahora debemos convertir ese objeto libro que nos retorno la api de libroremote a carritodetalledto para eso hacemos lo siguiente
                        var carritoDetalle = new CarritoDetalleDto
                        {
                            LibroId=objetoLibro.LibreriaMaterialId,
                            TituloLibro=objetoLibro.Titulo,
                            FechaPublicacion=objetoLibro.FechaPublicacion,
                            
                        };
                        //Este objeto obtendra los datos del libro que queremos mostrar pero debemos agregarlo en una lista para que no se sobreescriba en cada ciclo
                        //para eso usamos la lista creada mas arriba
                        listaCarritoDto.Add(carritoDetalle);
                        //Ahora ya tenemos listo para devolver la lista de productos nos faltan los otros atributos de CarritoDto que son 
                        //CarritoId
                        //FechaCreacionSesion
                    }
                }

                var carritoSesionDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,//Este es el que sacamos de la base de datos de carritosesion
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = listaCarritoDto
                };

                //Por ultimo retornamos el objeto
                return carritoSesionDto;
            }
        }
    }
}
