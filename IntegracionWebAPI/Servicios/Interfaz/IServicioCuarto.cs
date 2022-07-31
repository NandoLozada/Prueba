using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioCuarto
    {
        public Task<List<Cuarto>> ListaCuartos();

        public Task<Cuarto> CuartoPorId(int Id);

        public void AgregarCuarto(int capacidad, string foto);

        public void EstadoCuarto(int estado, int id);

        public void ActualizarCuarto(int id, int capacidad, string foto);

    }
}
