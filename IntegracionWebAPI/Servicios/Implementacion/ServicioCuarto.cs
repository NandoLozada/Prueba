using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Servicios.Interfaz;
using IntegracionWebAPI.Utiles;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioCuarto : IServicioCuarto
    {
        private readonly DapperContext _db;
        private readonly ResultadoCuarto _resultado;

        public ServicioCuarto(DapperContext db, ResultadoCuarto resultado)
        {
            _db = db;
            _resultado = resultado;
        }
        public async Task<ResultadoCuarto> ListaCuartos()
        {
            var queryjoin = "SELECT * FROM Cuartos LEFT JOIN Notas ON Cuartos.Id = Notas.IdCuarto ";

            var diccuarto = new Dictionary<int, Cuarto>();
            try
            {
                using (var conexion = _db.SuperConexionNando())
                {
                    var listado = (await conexion.QueryAsync<Cuarto, Nota, Cuarto>(queryjoin, (cuarto, nota) =>
                    {
                        Cuarto cuartotemp;

                        if (!diccuarto.TryGetValue(cuarto.Id, out cuartotemp))
                        {
                            cuartotemp = cuarto;
                            cuartotemp.Notas = new List<Nota>();
                            diccuarto.Add(cuartotemp.Id, cuarto);
                        }

                        if (nota != null)
                        {
                            cuartotemp.Notas.Add(nota);
                        }

                        return cuartotemp;

                    })).Distinct().ToList();

                    _resultado.ok = true;
                    _resultado.cuartos = listado;
                    return _resultado;
                }
            }
            catch(Exception ex)
            {
                _resultado.ok = false;
                _resultado.mensaje = ex.Message;
                return _resultado;  
            }
        }

        public async Task<ResultadoCuarto> CuartoPorId(int Id)
        {
            var queryjoin = "SELECT * FROM Cuartos LEFT JOIN Notas ON Cuartos.Id = Notas.IdCuarto WHERE Cuartos.Id = " + Id;

            var diccuarto = new Dictionary<int, Cuarto>();

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var cuartoc = await conexion.QueryAsync<Cuarto, Nota, Cuarto>(queryjoin, (cuarto, nota) =>
                    {
                        Cuarto cuartotemp;

                        if (!diccuarto.TryGetValue(cuarto.Id, out cuartotemp))
                        {
                            cuartotemp = cuarto;
                            cuartotemp.Notas = new List<Nota>();
                            diccuarto.Add(cuartotemp.Id, cuarto);
                        }

                        if (nota != null)
                        {
                            cuartotemp.Notas.Add(nota);
                        }

                        return cuartotemp;

                    });

                    if (cuartoc != null)
                    {
                        _resultado.ok = true;
                        _resultado.mensaje = "";
                        _resultado.cuarto = cuartoc.First();
                        return _resultado;
                    }                    
                }
                catch (Exception ex)    
                {
                    _resultado.mensaje = ex.Message;
                }

                _resultado.ok = false;
                _resultado.cuarto = null;
                return _resultado;
            }
        }

        public async Task<ResultadoCuarto> AgregarCuarto(int capacidad, string foto)
        {
            var insertcuarto = "INSERT INTO Cuartos (Capacidad, Foto, IdEstado) VALUES (@capacidadq, @fotoq, @estadoq)";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(insertcuarto, new { capacidadq = capacidad, fotoq = foto, estadoq = 1 });
                    _resultado.ok = true;
                    _resultado.mensaje = "El cuarto se agrego con exito";
                    return _resultado;
                }
                catch (Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje = ex.Message;
                    return _resultado;
                }            
            }
        }

        public async Task<ResultadoCuarto> CambiarEstadoCuarto(int estado, int id)
        {
            var updatecuarto = "UPDATE Cuartos SET IdEstado = @estadoq WHERE Id = @idq";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var r = await conexion.ExecuteAsync(updatecuarto, new { estadoq = estado, idq = id });
                    
                    if(r!=0)
                    {
                        _resultado.ok = true;
                        _resultado.mensaje = "El estado del cuarto se cambio con exito";
                    }
                    else
                    {
                        _resultado.ok = false;
                        _resultado.mensaje = "No se pudo cambiar el estado del cuarto, puede que la Id sea incorrecta";
                    }
                }

                catch (Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje = ex.Message;
                }
                return _resultado;
            }
        }

        public async Task<ResultadoCuarto> ActualizarCuarto(int idcuarto, int capacidad, string foto)
        {
            var updatecuarto = "UPDATE Cuartos SET Capacidad = @capacidadq, Foto = @fotoq WHERE Id = @id";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var r = await conexion.ExecuteAsync(updatecuarto, new { capacidadq = capacidad, fotoq = foto, id = idcuarto });
                    
                    if(r!=0)
                    {
                        _resultado.ok = true;
                        _resultado.mensaje = "El cuarto se modifico con exito";
                    }
                    else
                    {
                        _resultado.ok = false;
                        _resultado.mensaje = "No se pudo cambiar el estado del cuarto, puede que la Id sea incorrecta";
                    }
                }

                catch (Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje = ex.Message;
                }
                return _resultado;
            }
        }
    }
}
