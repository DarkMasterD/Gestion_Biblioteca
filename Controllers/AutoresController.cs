using Gestion_Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestion_Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly bibliotecaContext _bibliotecaContext;

        public AutoresController(bibliotecaContext bibliotecaContext)
        {
            _bibliotecaContext = bibliotecaContext;
        }

        [HttpGet]
        [Route("AllAuthors")]
        public IActionResult Listar()
        {
            List<Autor> autores = (from a in _bibliotecaContext.Autor select a).ToList();

            if (autores.Count == 0) 
            {
                return NotFound();
            }

            return Ok(autores);
        }

        [HttpGet]
        [Route("AuthorById/{id}")]
        public IActionResult Buscar(int id) 
        {
            var autor = (from a in _bibliotecaContext.Autor where a.id_autor == id select new
            {
                a.id_autor,
                a.nombre,
                a.nacionalidad,
                libro = (from au in _bibliotecaContext.Autor
                         join l in _bibliotecaContext.Libro
                         on au.id_autor equals l.id_autor
                         where au.id_autor == id select l).ToList()
            }).FirstOrDefault();

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }

        [HttpPost]
        [Route("AddAuthor")]
        public IActionResult Agregar([FromBody] Autor autor)
        {
            try
            {
                _bibliotecaContext.Autor.Add(autor);
                _bibliotecaContext.SaveChanges();
                return Ok(autor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateAuthor/{id}")]
        public IActionResult Actualizar(int id, [FromBody] Autor autorModificar)
        {
            try
            {
                Autor? autorActual = (from a in _bibliotecaContext.Autor where a.id_autor == id select a).FirstOrDefault();
                
                if(autorActual == null)
                {
                    return NotFound();
                }

                autorActual.nombre = autorModificar.nombre;
                autorActual.nacionalidad = autorModificar.nacionalidad;

                _bibliotecaContext.Entry(autorActual).State = EntityState.Modified;
                _bibliotecaContext.SaveChanges();
                return Ok(autorModificar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteAuthor/{id}")]
        public IActionResult Eliminar(int id)
        {
            Autor? autor = (from a in _bibliotecaContext.Autor where a.id_autor == id select a).FirstOrDefault();

            if (autor == null)
            {
                return NotFound();
            }

            _bibliotecaContext.Autor.Attach(autor);
            _bibliotecaContext.Autor.Remove(autor);
            _bibliotecaContext.SaveChanges();

            return Ok(autor);
        }

        [HttpGet]
        [Route("BooksCountByAuthor/{id}")]
        public IActionResult BuscarCantLibros(int id)
        {
            List<Libro> libros = (from l in _bibliotecaContext.Libro where l.id_autor == id select l).ToList();
            string Cantidad = "";
            if (libros == null)
            {
                return NotFound();
            }
            Cantidad = "Cantidad de libros escritos: " + libros.Count();

            return Ok(Cantidad);
        }
    }
}
