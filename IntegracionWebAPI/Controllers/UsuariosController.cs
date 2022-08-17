using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.Servicios;
using IntegracionWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    //[Authorize]
    public class UsuariosController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public UsuariosController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("Registrar")]       
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(string emailusu, string pass)
        {
            CredencialesUsuario credencialesUsuario = new CredencialesUsuario();

            credencialesUsuario.Email = emailusu;
            credencialesUsuario.Password = pass;

            var usuario = new IdentityUser {  UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Password);

            if (resultado.Succeeded)
            {
                usuario = await userManager.FindByEmailAsync(credencialesUsuario.Email);
                await userManager.AddClaimAsync(usuario, new Claim("esAC", "2"));

                return Ok("El usuario se ha registrado como Atencion al Cliente");
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Cambiar Password")]
        public async Task<ActionResult<RespuestaAutenticacion>> CambiarPass(string emailusu, string pass, string nuevapass)
        {
            CredencialesUsuario credencialesUsuario = new CredencialesUsuario();

            credencialesUsuario.Email = emailusu;
            credencialesUsuario.Password = pass;
            credencialesUsuario.NuevaPass = nuevapass;

            var usuario = await userManager.FindByEmailAsync(credencialesUsuario.Email);
            var resultado = await userManager.ChangePasswordAsync(usuario, credencialesUsuario.Password, credencialesUsuario.NuevaPass);

            if (resultado.Succeeded)
            {
                return Ok("La contraseña se a modificado con exito");
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email),
            };

            var usuario = await userManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(5);

            var securitytoken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securitytoken),
                Expiracion = expiracion
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
        {

            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await  ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin (EditarAdmin editarAdmin)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdmin.Email);
            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("HacerAC")]
        public async Task<ActionResult> HacerAC(EditarAdmin editarAdmin)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdmin.Email);
            await userManager.AddClaimAsync(usuario, new Claim("esAC", "2"));
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("RemoverAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdmin editarAdmin)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdmin.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("RemoverAC")]
        public async Task<ActionResult> RemoverAC(EditarAdmin editarAdmin)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdmin.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("esAC", "2"));
            return NoContent();
        }
    }
}
