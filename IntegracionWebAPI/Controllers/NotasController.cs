using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using Wkhtmltopdf.NetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IntegracionWebAPI.Servicios.Interfaz;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/Notas")]
    [Authorize]
    public class NotasController: ControllerBase
    {       
        private readonly IServicioNota _nota;
        private readonly IGeneratePdf generatePdf;

        public NotasController(IServicioNota nota, IGeneratePdf generatePdf)
        {           
            _nota = nota;
            this.generatePdf = generatePdf;
        }
        [HttpGet]
        public async Task<List<Nota>> Get()
        {
            var notas = await _nota.ListaNotas();
            return notas;
        }

        [HttpGet("{idcuarto}")]
        public async Task<List<Nota>> GetPorCuarto(int idcuarto)
        {
            var notas = await _nota.NotasPorCuarto(idcuarto);
            return notas;
        }

        [HttpGet("PDF/{idcuarto}")]
        public Task<IActionResult> GetPorCuartoPDF(int idcuarto)
        {
            var notas = _nota.NotasPorCuarto(idcuarto);
            return generatePdf.GetPdf("View/NotasVista.cshtml", notas);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPost("AgregarNota")]
        public void Post(int idcuarto, string descripcion)
        {
            _nota.AgregarNota(idcuarto, descripcion);
        }

        [HttpPut]
        public void ActualizarNota(int id, string descripcion)
        {
            _nota.ActualizarNota(id, descripcion);
        }
    }
}
