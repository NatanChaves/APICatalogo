using APICatalogo.Context;
using APICatalogo.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias.ToList();
        }
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategorias()
        {
            var categorias = _context.Categorias.AsNoTracking().Include(p=> p.Produtos).ToList();
            if (categorias is null)
            {
                return NotFound("Categorias não encontradas");
            }
            return categorias;
        }


        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(x => x.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias.Add(categoria);

            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest();

            _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);

            if (categoria is null)
                return NotFound();

            _context.Categorias.Remove(categoria);

            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
