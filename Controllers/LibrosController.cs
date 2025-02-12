using Gestion_Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestion_Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly bibliotecaContext _bibliotecaContext;

        public LibrosController(bibliotecaContext bibliotecaContext)
        {
            _bibliotecaContext = bibliotecaContext;
        }

        [HttpGet]
        [Route("AllBooks")]
        public IActionResult Listar()
        {
            List<Libro> libros = (from a in _bibliotecaContext.Libro select a).ToList();

            if (libros.Count == 0)
            {
                return NotFound();
            }

            return Ok(libros);
        }

        [HttpGet]
        [Route("BookById/{id}")]
        public IActionResult Buscar(int id)
        {
            var libro = (from l in _bibliotecaContext.Libro
                         join a in _bibliotecaContext.Autor
                         on l.id_autor equals a.id_autor
                         where l.id_libro == id
                         select new
                         {
                             l.id_libro,
                             l.titulo,
                             l.id_categoria,
                             l.resumen,
                             autor = a.nombre,
                             l.anio_publicacion
                         }).FirstOrDefault();

            if (libro == null)
            {
                return NotFound();
            }

            return Ok(libro);
        }

        [HttpPost]
        [Route("AddBook")]
        public IActionResult Agregar([FromBody] Libro libro)
        {
            try
            {
                _bibliotecaContext.Libro.Add(libro);
                _bibliotecaContext.SaveChanges();
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateBook/{id}")]
        public IActionResult Actualizar(int id, [FromBody] Libro libroModificar)
        {
            try
            {
                Libro? libroActual = (from l in _bibliotecaContext.Libro where l.id_libro == id select l).FirstOrDefault();

                if (libroActual == null)
                {
                    return NotFound();
                }

                libroActual.id_autor = libroModificar.id_autor;
                libroActual.titulo = libroModificar.titulo;
                libroActual.id_categoria = libroModificar.id_categoria;
                libroActual.resumen = libroModificar.resumen;
                libroActual.anio_publicacion = libroModificar.anio_publicacion;

                _bibliotecaContext.Entry(libroActual).State = EntityState.Modified;
                _bibliotecaContext.SaveChanges();
                return Ok(libroModificar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteBook/{id}")]
        public IActionResult Eliminar(int id)
        {
            Libro? libro = (from l in _bibliotecaContext.Libro where l.id_libro == id select l).FirstOrDefault();

            if (libro == null)
            {
                return NotFound();
            }

            _bibliotecaContext.Libro.Attach(libro);
            _bibliotecaContext.Libro.Remove(libro);
            _bibliotecaContext.SaveChanges();

            return Ok(libro);
        }

        [HttpGet]
        [Route("BookYear>2001")]
        public IActionResult BuscarDespues2000()
        {
            List<Libro> libros = (from l in _bibliotecaContext.Libro where l.anio_publicacion > 2000 select l).ToList();

            if (libros == null)
            {
                return NotFound();
            }

            return Ok(libros);
        }

        [HttpGet]
        [Route("BookByTitle/{titulo}")]
        public IActionResult Buscar(string titulo)
        {
            var libro = (from l in _bibliotecaContext.Libro where l.titulo.Contains(titulo) select l).ToList();

            if (libro == null)
            {
                return NotFound();
            }

            return Ok(libro);
        }
    }
}
