using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
namespace ApiUnity.Controllers;
using System.Data;


[ApiController]
[Route("[controller]")]
public class LeaderBoardController : ControllerBase
{

    [HttpGet("GetRanking")]
    public List<Usuario> GetRanking()
    {
        List<Usuario> Ranking = new List<Usuario>();
        string connectionString = "Server=blah;Port=12345;Database=quiensabe;Uid=nose;password=dokioe;";

        using (var conexion = new MySqlConnection(connectionString))
        {
            conexion.Open();
            using (var cmd = new MySqlCommand("SP_LB_RankUsuarios", conexion))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // <<---- NOTA el while aquí
                    {
                        Usuario usuario = new Usuario();
                        usuario.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                        usuario.nombre_usuario = Convert.ToString(reader["nombre_usuario"]);
                        usuario.imagen = Convert.ToString(reader["imagen"]);
                        Ranking.Add(usuario);
                    }
                }
            }
        }
        return Ranking;
    }




}