using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Servicios.Interfaz;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioCuarto : IServicioCuarto
    {
        private readonly DapperContext _db;
        public ServicioCuarto(DapperContext db)
        {
            _db = db;
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

        public async Task <List<Cuarto>> CuartoPorId(int Id)
        {
            var queryjoin = "SELECT * FROM Cuartos LEFT JOIN Notas ON Cuartos.Id = Notas.IdCuarto WHERE Cuartos.Id = " + Id;

            var diccuarto = new Dictionary<int, Cuarto>();

            using (var conexion = _db.SuperConexionNando())

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
        }

        public void AgregarCuarto(int capacidad, string foto)
        {
            var insertcuarto = "INSERT INTO Cuartos (Capacidad, Foto, IdEstado) VALUES (@capacidadq, @fotoq, @estadoq)";

            using (var conexion = _db.SuperConexionNando())

            {
                conexion.Execute(insertcuarto, new { capacidadq = capacidad, fotoq = foto, estadoq = 1 });
            }
        }

        public void EstadoCuarto(int estado, int id)
        {
            var updatecuarto = "UPDATE Cuartos SET IdEstado = @estadoq WHERE Id = @idq";

            using (var conexion = _db.SuperConexionNando())

            {
                conexion.Execute(updatecuarto, new { estadoq = estado, idq = id });
            }
        }

        public void ActualizarCuarto(int idcuarto, int capacidad, string foto)
        {
            var updatecuarto = "UPDATE Cuartos SET Capacidad = @capacidadq, Foto = @fotoq WHERE Id = @id";

            using (var conexion = _db.SuperConexionNando())
            {
                conexion.Execute(updatecuarto, new { capacidadq = capacidad, fotoq = foto, id = idcuarto }); ;
            }

        }
    }
}
