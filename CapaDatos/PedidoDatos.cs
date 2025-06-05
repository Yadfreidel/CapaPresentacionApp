using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class PedidoDatos
    {
        private readonly string conexion = @"Server=YAFRE-23\SQLEXPRESS;Database=PedidosDB;Trusted_Connection=True;";

        public void InsertarPedidoOnline(PedidoOnline pedido)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO Pedido (Fecha, Cliente, MontoTotal, TipoPedido) OUTPUT INSERTED.IdPedido VALUES (@Fecha, @Cliente, @MontoTotal, 'Online')", cn);
                cmd.Parameters.AddWithValue("@Fecha", pedido.Fecha);
                cmd.Parameters.AddWithValue("@Cliente", pedido.Cliente);
                cmd.Parameters.AddWithValue("@MontoTotal", pedido.MontoTotal);

                int idPedido = (int)cmd.ExecuteScalar();

                SqlCommand cmdOnline = new SqlCommand("INSERT INTO PedidoOnline (IdPedido, Url, MetodoPago) VALUES (@IdPedido, @Url, @MetodoPago)", cn);
                cmdOnline.Parameters.AddWithValue("@IdPedido", idPedido);
                cmdOnline.Parameters.AddWithValue("@Url", pedido.Url);
                cmdOnline.Parameters.AddWithValue("@MetodoPago", pedido.MetodoPago);
                cmdOnline.ExecuteNonQuery();
            }
        }

        public void InsertarPedidoPresencial(PedidoPresencial pedido)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO Pedido (Fecha, Cliente, MontoTotal, TipoPedido) OUTPUT INSERTED.IdPedido VALUES (@Fecha, @Cliente, @MontoTotal, 'Presencial')", cn);
                cmd.Parameters.AddWithValue("@Fecha", pedido.Fecha);
                cmd.Parameters.AddWithValue("@Cliente", pedido.Cliente);
                cmd.Parameters.AddWithValue("@MontoTotal", pedido.MontoTotal);

                int idPedido = (int)cmd.ExecuteScalar();

                SqlCommand cmdPresencial = new SqlCommand("INSERT INTO PedidoPresencial (IdPedido, Sucursal) VALUES (@IdPedido, @Sucursal)", cn);
                cmdPresencial.Parameters.AddWithValue("@IdPedido", idPedido);
                cmdPresencial.Parameters.AddWithValue("@Sucursal", pedido.Sucursal);
                cmdPresencial.ExecuteNonQuery();
            }
        }

        public void ActualizarPedidoOnline(PedidoOnline pedido)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Pedido SET Fecha=@Fecha, Cliente=@Cliente, MontoTotal=@MontoTotal WHERE IdPedido=@IdPedido", cn);
                cmd.Parameters.AddWithValue("@Fecha", pedido.Fecha);
                cmd.Parameters.AddWithValue("@Cliente", pedido.Cliente);
                cmd.Parameters.AddWithValue("@MontoTotal", pedido.MontoTotal);
                cmd.Parameters.AddWithValue("@IdPedido", pedido.IdPedido);
                cmd.ExecuteNonQuery();

                SqlCommand cmdOnline = new SqlCommand("UPDATE PedidoOnline SET Url=@Url, MetodoPago=@MetodoPago WHERE IdPedido=@IdPedido", cn);
                cmdOnline.Parameters.AddWithValue("@Url", pedido.Url);
                cmdOnline.Parameters.AddWithValue("@MetodoPago", pedido.MetodoPago);
                cmdOnline.Parameters.AddWithValue("@IdPedido", pedido.IdPedido);
                cmdOnline.ExecuteNonQuery();
            }
        }

        public void ActualizarPedidoPresencial(PedidoPresencial pedido)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Pedido SET Fecha=@Fecha, Cliente=@Cliente, MontoTotal=@MontoTotal WHERE IdPedido=@IdPedido", cn);
                cmd.Parameters.AddWithValue("@Fecha", pedido.Fecha);
                cmd.Parameters.AddWithValue("@Cliente", pedido.Cliente);
                cmd.Parameters.AddWithValue("@MontoTotal", pedido.MontoTotal);
                cmd.Parameters.AddWithValue("@IdPedido", pedido.IdPedido);
                cmd.ExecuteNonQuery();

                SqlCommand cmdPresencial = new SqlCommand("UPDATE PedidoPresencial SET Sucursal=@Sucursal WHERE IdPedido=@IdPedido", cn);
                cmdPresencial.Parameters.AddWithValue("@Sucursal", pedido.Sucursal);
                cmdPresencial.Parameters.AddWithValue("@IdPedido", pedido.IdPedido);
                cmdPresencial.ExecuteNonQuery();
            }
        }

        public void EliminarPedido(int idPedido)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                // Elimina primero de las tablas hijas
                SqlCommand cmd1 = new SqlCommand("DELETE FROM PedidoOnline WHERE IdPedido=@IdPedido", cn);
                cmd1.Parameters.AddWithValue("@IdPedido", idPedido);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("DELETE FROM PedidoPresencial WHERE IdPedido=@IdPedido", cn);
                cmd2.Parameters.AddWithValue("@IdPedido", idPedido);
                cmd2.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand("DELETE FROM Pedido WHERE IdPedido=@IdPedido", cn);
                cmd3.Parameters.AddWithValue("@IdPedido", idPedido);
                cmd3.ExecuteNonQuery();
            }
        }

        public List<Pedido> ListarPedidos()
        {
            List<Pedido> lista = new List<Pedido>();

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Pedido", cn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Pedido pedido = new Pedido
                    {
                        IdPedido = Convert.ToInt32(dr["IdPedido"]),
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        Cliente = dr["Cliente"].ToString(),
                        MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                        TipoPedido = dr["TipoPedido"].ToString()
                    };
                    lista.Add(pedido);
                }
            }

            return lista;
        }
    }
}
