using System;
using BemEstar.ApiMotivacional.Models;
using BemEstar.ApiMotivacional.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AulaWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotivacionalController : ControllerBase
    {
        private MotivacionalService _service = new MotivacionalService();

        [HttpGet]
        public List<Motivacional> Get()
        {
            return this._service.Read();
        }


        [HttpGet("{id}")]
        public Motivacional Get(int id)
        {
            return this._service.ReadById(id);
        }


        [HttpPost]
        public void Post([FromBody] Motivacional model)
        {
            this._service.Create(model);
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Motivacional model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("O ID do Objeto Person não é igual ao Id da URL.");
            }
            this._service.Update(model);
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this._service.Delete(id);
        }
    }
}