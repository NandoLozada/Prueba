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
        private readonly Resultado _resultado;

        public ServicioCuarto(DapperContext db, Resultado resultado)
        {
            _db = db;
            _resultado = resultado;
        }
        public async Task<List<Cuarto>> ListaCuartos()
        {
            var queryjoin = "SELECT * FROM Cuartos LEFT JOIN Notas ON Cuartos.Id = Notas.IdCuarto ";

            var diccuarto = new Dictionary<int, Cuarto>();

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

                return listado.ToList();
            }
        }

        public async Task<List<Cuarto>> CuartoPorId(int Id)
        {
            var queryjoin = "SELECT * FROM Cuartos LEFT JOIN Notas ON Cuartos.Id = Notas.IdCuarto WHERE Cuartos.Id = " + Id;

            var diccuarto = new Dictionary<int, Cuarto>();

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var listado = await conexion.QueryAsync<Cuarto, Nota, Cuarto>(queryjoin, (cuarto, nota) =>
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

                    return listado.ToList();
                }
                catch (Exception ex)    
                {
                    return null;
                }
            }
        }

        public async Task<Resultado> AgregarCuarto(int capacidad, string foto)
        {
            var insertcuarto = "INSERT INTO Cuartos (Capacidad, Foto, IdEstado) VALUES (@capacidadq, @fotoq, @estadoq)";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(insertcuarto, new { capacidadq = capacidad, fotoq = foto, estadoq = 1 });
                    _resultado.ok = true;

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

        public async Task<Resultado> EstadoCuarto(int estado, int id)
        {
            var updatecuarto = "UPDATE Cuartos SET IdEstado = @estadoq WHERE Id = @idq";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(updatecuarto, new { estadoq = estado, idq = id });
                    _resultado.ok =true;
                    return _resultado;
                }

                catch(Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje = ex.Message;
                    return _resultado;
                }
            }
        }

        public async Task<Resultado> ActualizarCuarto(int idcuarto, int capacidad, string foto)
        {
            var updatecuarto = "UPDATE Cuartos SET Capacidad = @capacidadq, Foto = @fotoq WHERE Id = @id";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(updatecuarto, new { capacidadq = capacidad, fotoq = foto, id = idcuarto });
                    _resultado.ok = true;
                    return _resultado;
                }
                catch(Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje=ex.Message;
                    return _resultado;
                }
            }

        }
    }
}
