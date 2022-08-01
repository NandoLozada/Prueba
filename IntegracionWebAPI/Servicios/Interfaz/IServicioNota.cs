using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioNota
    {
        public Task<List<Nota>> ListaNotas();

        public Task<List<Nota>> NotasPorCuarto(int idcuarto);

        public void AgregarNota(int idcuarto, string descripcion);

        public void ActualizarNota(int id, string descripcion);
    }
}
