using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using Wkhtmltopdf.NetCore;
using IntegracionWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/cuartos")]
    [Authorize]
    public class CuartosController : ControllerBase
    {
        private readonly CuartosDAO DAO;
        private readonly Cuartos.ServCuartos servLista;
        readonly IGeneratePdf generatePdf;

        public CuartosController(CuartosDAO DAO, Cuartos.ServCuartos servLista, IGeneratePdf generatePdf)
        {
            this.DAO = DAO;
            this.servLista = servLista;
            this.generatePdf = generatePdf;
        }


        [HttpGet("1-12-ListadeCuartos")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        public List<Cuarto> Get()
        {
            var cuartos = servLista.ListaCuartos(DAO);
            //generatePdf.GetPdf("View/CuartosVista.cshtml", cuartos);
            return cuartos;
        }

        [HttpGet("UnCuarto/{Id}")]
        public List<Cuarto> CuartoPorId(int Id)
        {
            var cuartos = servLista.CuartoPorId(DAO, Id);
            return cuartos;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("2-AgregarunCuarto")]
        public void Post([FromForm] int capacidad,  IFormFile foto)
        {
            servLista.AgregarCuarto(DAO, capacidad, foto);
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPatch("3-5-18-CambiarEstadoCuarto")]
        public void EstadoCuarto(int estado, int id)
        {
            servLista.EstadoCuarto(DAO, estado, id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPut("4-ActualizarCuarto")]
        public void ActualizarCuarto([FromForm]int id, int capacidad, IFormFile foto)
        {
            servLista.ActualizarCuarto(DAO, id, capacidad, foto);
        }
    }
}
