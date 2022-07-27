using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Servicios
{
    public interface ICuartos
    {
    }

    public class Cuartos : ICuartos
    {
        public class ServCuartos : ICuartos
        {
            public List<Cuarto> ListaCuartos(CuartosDAO DAO)
            {
                return DAO.ListaCuartosDAO();
            }

            public List<Cuarto> CuartoPorId(CuartosDAO DAO, int idcuarto)
            {
                return DAO.CuartoPorId(idcuarto);
            }

            public void AgregarCuarto(CuartosDAO DAO, int capacidad, IFormFile foto)
            {
                string foto64;

                using (var ms = new MemoryStream())
                {                    
                    foto.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    foto64 = Convert.ToBase64String(fileBytes);
                }

                DAO.AgregarCuarto(capacidad, foto64);
            }

            public void EstadoCuarto(CuartosDAO DAO, int estado, int id)
            {
                DAO.EstadoCuarto(estado, id);
            }

            public void ActualizarCuarto(CuartosDAO DAO, int id, int capacidad, IFormFile foto)
            {
                string foto64;

                using (var ms = new MemoryStream())
                {
                    foto.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    foto64 = Convert.ToBase64String(fileBytes);
                }

                DAO.ActualizarCuarto(id, capacidad,foto64);
            }

        }


    }
}
