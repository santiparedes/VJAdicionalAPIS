using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using VJAdicionalapis.Models;

namespace VJAdicionalapis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotasIrregularidadesController : ControllerBase
    {
        private readonly string connectionString = "Server=blah;Port=12345;Database=quiensabe;Uid=nose;password=dokioe;";

        [HttpPost]
        public IActionResult PostNotaIrregularidad([FromBody] NotaIrregularidad nota)
        {
            try
            {
                using (var conexion = new MySqlConnection(connectionString))
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = conexion;
                        cmd.CommandText = "INSERT INTO NotasIrregularidades (tienda, descripcion, fecha) VALUES (@tienda, @descripcion, @fecha)";
                        cmd.Parameters.AddWithValue("@tienda", nota.Tienda);
                        cmd.Parameters.AddWithValue("@descripcion", nota.Descripcion);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.UtcNow);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Nota irregularidad creada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la nota irregularidad.", error = ex.Message });
            }
        }

    [HttpGet]
    public IActionResult GetNotasIrregularidades([FromQuery] string tienda)
    {
        try
        {
            var notas = new List<NotaIrregularidad>();
            using (var conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conexion;

                    if (!string.IsNullOrEmpty(tienda))
                    {
                        cmd.CommandText = "SELECT id_notas, tienda, descripcion, fecha FROM NotasIrregularidades WHERE tienda = @tienda";
                        cmd.Parameters.AddWithValue("@tienda", tienda);
                    }
                    else
                    {
                        cmd.CommandText = "SELECT id_notas, tienda, descripcion, fecha FROM NotasIrregularidades";
                    }

                    cmd.Prepare();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notas.Add(new NotaIrregularidad
                            {
                                IdNotas = reader.GetInt32("id_notas"),
                                Tienda = reader.GetString("tienda"),
                                Descripcion = reader.GetString("descripcion"),
                                Fecha = reader.GetDateTime("fecha")
                            });
                        }
                    }
                }
            }
            return Ok(notas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener las notas irregularidades.", error = ex.Message });
        }
    }
    }
} 