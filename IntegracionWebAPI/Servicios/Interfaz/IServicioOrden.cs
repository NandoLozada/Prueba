namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioOrden
    {
        public Task<int> AgregarOrden(int idcliente);
    }
}
