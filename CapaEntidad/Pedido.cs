using System;

namespace CapaEntidad
{
    // Clase base Pedido
    public class Pedido
    {
        public int IdPedido { get; set; }           // ID del pedido
        public DateTime Fecha { get; set; }         // Fecha del pedido
        public string Cliente { get; set; }         // Nombre del cliente
        public decimal MontoTotal { get; set; }     // Total del pedido
        public string TipoPedido { get; set; }      // Tipo: "Online" o "Presencial"
    }

    // Clase hija para pedidos online
    public class PedidoOnline : Pedido
    {
        public string Url { get; set; }             // Dirección web
        public string MetodoPago { get; set; }      // Ej: Tarjeta, PayPal
    }

    // Clase hija para pedidos presenciales
    public class PedidoPresencial : Pedido
    {
        public string Sucursal { get; set; }        // Nombre o código de la sucursal
    }
}
