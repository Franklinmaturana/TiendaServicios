using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelos;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {

        public IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            var listaFake = A.ListOf<LibreriaMaterial>(30);
            listaFake[0].LibreriaMaterialId = Guid.Empty;

            return listaFake;
        }

        private Mock<ContextoLibreria> CrearContexto()
        {
            var dataPrueba = ObtenerDataPrueba().AsQueryable();//para que retorne un formato especial de coleccion

            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator);

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));

            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x=>x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
        }
        [Fact]
        public async void GetLibroPorId()
        {
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg=>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            //objeto que guardara el libro id y luego usarlo para consultarlo

            var request = new ConsultaFiltro.Ejecuta();
            request.LibroId =  Guid.Empty;

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object,mapper);
            var libro =await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId==Guid.Empty);
        }

        [Fact]

        public async void GetLibros()
        {
            //System.Diagnostics.Debugger.Launch();
            //Que metodo dentro de mi microservice Libro se esta encargando de realizar 
            //la consulta de libros de la base de datos
            //1. Emular a la instancia de Entity framework core el contexto
            //Para emular las acciones y eventos de un objeto en un ambiente de Unit test
            /*utilizamos objetos de tipo mock (representacion de un objeto que puede actuar como un componente de software real)
             * representa cualquier elemento del código
             * Debemos instalar el paquete dentro del proyecto
             */
            var mockContexto = CrearContexto();

            //2. Emular al mapping IMapper
            var mapConfig = new MapperConfiguration(cfg=>
            {
                cfg.AddProfile(new MappingTest());
            });
            var mapper = mapConfig.CreateMapper();


            //3. Instanciar a la clase Manejador y pasarle como parametros los mocks que he creado
            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);
            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());//la prueba pase si la lista tiene algun valor, si retorna un True es que hubo un valor sino es falso
        }

        [Fact]
        public async void InsertarLibro()
        {
            System.Diagnostics.Debugger.Launch();
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            var contexto = new ContextoLibreria(options);

            var request = new Nuevo.Ejecuta();
            request.Titulo = "Libro de Microservice";
            request.AutorLibro = null;
            request.FechaPublicacion = DateTime.Now;

            var manejador = new Nuevo.Manejador(contexto);
            var librocreado = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(librocreado!=null);

        }
    }
}
