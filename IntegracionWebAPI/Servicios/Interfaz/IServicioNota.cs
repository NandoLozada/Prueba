using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Utiles;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioNota
    {
        public Task<List<Nota>> ListaNotas();

        public Task<ResultadoNota> NotasPorCuarto(int idcuarto);

        public Task<ResultadoNota> AgregarNota(int idcuarto, string descripcion);

        public Task<ResultadoNota> ActualizarNota(int id, string descripcion);
    }
}
