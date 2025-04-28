using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
namespace ApiUnity.Controllers;
using System.Data;

[ApiController]
[Route("[controller]")]
public class OxxoController : ControllerBase
{
    string connectionString = "Server=;placeholder";



    [Route("VerificarUsuario/{usrName}/{contra}")]
    [HttpGet]
    public bool VerificarUsuario(string usrName, string contra) {
        bool existe = false;
        MySqlConnection conexion = new MySqlConnection(connectionString);
        conexion.Open();
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "SP_Menu_IniciarSesion";
        cmd.Connection = conexion;

        cmd.Parameters.AddWithValue("@usrName", usrName);
        cmd.Parameters.AddWithValue("@contra", contra);
        
        using (var reader = cmd.ExecuteReader()) {
                if (reader.Read())
                {
                    existe = Convert.ToInt32(reader["existe"]) > 0;
                }
            }
        cmd.Prepare();
        cmd.ExecuteNonQuery();
        conexion.Close();

        return existe;
    }

    [Route("GetUsuario/{usrName}")]
    [HttpGet]
    public Usuario GetUsuario(string usrName) {
        Usuario usuario = new Usuario();
        MySqlConnection conexion = new MySqlConnection(connectionString);
        conexion.Open();
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "SP_Menu_GetUsuario";
        cmd.Connection = conexion;

        cmd.Parameters.AddWithValue("@usrName", usrName);
        using (var reader = cmd.ExecuteReader()) {
                if (reader.Read())
                {
                    usuario.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    usuario.monedas = Convert.ToInt32(reader["monedas"]);
                    usuario.nivel = Convert.ToInt32(reader["nivel"]);
                }
            }

        cmd.Prepare();
        cmd.ExecuteNonQuery();
        conexion.Close();

        return usuario;
    }

 


}
