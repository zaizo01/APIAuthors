using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIAutores.Entities;
using WebAPIAutores.Filters;
using WebAPIAutores.Services;

namespace WebAPIAutores.Controllers
{
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IServicio servicio,
            ServicioTransient servicioTransient, ServicioScoped servicioScoped, ServicioSingleton servicioSingleton, ILogger<AutoresController> logger)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;
        }

        [HttpGet("GUID")]
        //[ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FilterOfAction))]
        public ActionResult GetGuids()
        {
            //throw new NotImplementedException();
            return Ok(new
            {
                AutoresControllerTransient = servicioTransient.Guid,
                ServicioA_Transient = servicio.GetTransient(),
                AutoresControllerScoped = servicioScoped.Guid,
                ServicioA_Scoped = servicio.GetScoped(),
                AutoresControllerSingleton = servicioSingleton.Guid,
                ServicioA_Singleton = servicio.GetSingleton(),
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> GetById(int id)
        {
            var author = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null) return NotFound();
            return author;
        }

        [HttpGet]
        [ServiceFilter(typeof(FilterOfAction))]
        public async Task<ActionResult<List<Autor>>> GetAll()
        {
            //throw new NotImplementedException();
            servicio.RealizarTarea();
            logger.LogInformation("Get list of authors");
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            var authorExist = await context.Autores.AnyAsync(x => x.Name == autor.Name);
            if (authorExist) return BadRequest($"El {autor.Name} ya existe.");
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id) return BadRequest("Invalid Id");
            var authorExist = await context.Autores.AnyAsync(x => x.Id == id);
            if (!authorExist) return NotFound();
            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var authorExist = await context.Autores.AnyAsync(x => x.Id == id);
            if (!authorExist) return NotFound();
            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
