using IntegracionWebAPI.Controllers;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using Microsoft.AspNetCore.Identity;
using Wkhtmltopdf.NetCore;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using IntegracionWebAPI.Servicios.Interfaz;
using IntegracionWebAPI.Servicios.Implementacion;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Utiles;

namespace IntegracionWebAPI
{
    public class Startup
    {
        //private readonly ConexionDB conexionDB;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //this.conexionDB = conexionDB;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IntegracionWebAPI", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddWkhtmltopdf("wkhtmltopdf");

            services.AddControllers();               
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddTransient<ConexionDB>();

            services.AddTransient<Clientes.ServClientes>();
            services.AddTransient<ClientesDAO>();            

            services.AddTransient<Cuartos.ServCuartos>();
            services.AddTransient<CuartosDAO>();

            services.AddTransient<Notas.ServNotas>();
            services.AddTransient<NotasDAO>();

            services.AddTransient<Ordenes.ServOrdenes>();
            services.AddTransient<OrdenesDAO>();

            services.AddTransient<Reservas.ServReservas>();
            services.AddTransient<ReservasDAO>();

            //Nuevoooo
            services.AddScoped<Resultado>();
            services.AddScoped<DapperContext>();
            services.AddScoped<IServicioCliente, ServicioCliente>();
            services.AddScoped<IServicioCuarto, ServicioCuarto>();
            services.AddScoped<IServicioNota, ServicioNota>();
            services.AddScoped<IServicioReserva, ServicioReserva>();
            services.AddScoped<IServicioOrden, ServicioOrden>();

            //


            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("conexionstr")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opciones =>
            opciones.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                ClockSkew = TimeSpan.Zero
            });

            services.AddAuthorization(opciones =>
            {
                opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));
                opciones.AddPolicy("EsAC", politica => politica.RequireClaim("esAC"));
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
