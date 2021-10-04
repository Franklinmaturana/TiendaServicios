using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteService
{
    public class LibrosService : RemoteInterface.ILibrosService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<LibrosService> _logger;

        public LibrosService(IHttpClientFactory httpClient,ILogger<LibrosService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<(bool resultado, LibroRemote Libro, string ErrorMesasage)> GetLibro(Guid LibroId)
        {
            try
            {
                var cliente = _httpClient.CreateClient("Libros");//Esta instancia crea un cliente que toma la url base de la microservice de libros que configuramos en la clase Startup
                var response = await cliente.GetAsync($"api/LibreriaMaterial/{LibroId}");
                //Ahora luego de hacer esa peticion debemos validar si la operacion se hizo con exito o no
                //lo cual lo hacemos con la siguiente condicion
                if(response.IsSuccessStatusCode)
                {
                    //lo de arriba es si response retorna un 200
                    var contenido = await response.Content.ReadAsStringAsync();//Convierte el contenido en Json string
                    //Ahora debemos convertir la respuesta que nos viene de manera que no nos genere problemas si tiene mayusculas en x propiedades
                    //eso se hace con el siguiente código
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    //Ahora ese contenido lo debemos llevar a una clase que haga match con el contenido que devuelve el json seria esta 
                    var resultado = JsonSerializer.Deserialize<LibroRemote>(contenido, options);
                    //debemos pasarle el modelo al cual se deserializara el cual el LibroRemote para que haga match con lo que trae el json
                    return (true,resultado,null);
                    //Debemos retornar 3 valores que son la tupla de arriba en el Task
                }
                return (false, null, response.ReasonPhrase);
                //este return se dara sino se ejecuto correctamente la peticion
            }
            catch(Exception e)
            {
                //Aca escribiremos en Log el error que salto
                _logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
