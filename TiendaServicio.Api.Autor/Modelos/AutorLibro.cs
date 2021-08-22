using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicio.Api.Autor.Modelos
{
    public class AutorLibro
    {

        public int AutorLibroId { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public ICollection<GradoAcademico> ListaGradoAcadeemico { get; set; }

        public string AutorLibroGuid { get; set; }
    }
}
