using System;
using System.Data;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class PedidoNegocio
    {
        private PedidoDatos datos = new PedidoDatos();

        public void InsertarPedidoOnline(PedidoOnline pedido)
        {
            datos.InsertarPedidoOnline(pedido);
        }

        public void InsertarPedidoPresencial(PedidoPresencial pedido)
        {
            datos.InsertarPedidoPresencial(pedido);
        }

        public void ActualizarPedidoOnline(PedidoOnline pedido)
        {
            datos.ActualizarPedidoOnline(pedido);
        }

        public void ActualizarPedidoPresencial(PedidoPresencial pedido)
        {
            datos.ActualizarPedidoPresencial(pedido);
        }

        public DataTable ListarPedidos()
        {
            var lista = datos.ListarPedidos();

            DataTable tabla = new DataTable();
            tabla.Columns.Add("IdPedido", typeof(int));
            tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("Cliente", typeof(string));
            tabla.Columns.Add("MontoTotal", typeof(decimal));
            tabla.Columns.Add("TipoPedido", typeof(string));

            foreach (var pedido in lista)
            {
                tabla.Rows.Add(pedido.IdPedido, pedido.Fecha, pedido.Cliente, pedido.MontoTotal, pedido.TipoPedido);
            }

            return tabla;
        }

        public void EliminarPedido(int idPedido)
        {
            datos.EliminarPedido(idPedido);
        }
    }
}
