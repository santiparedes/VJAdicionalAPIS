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
    public IActionResult CrearNotaIrregularidad([FromBody] NotaIrregularidad nuevaNota)
    {
        try
        {
            using (var conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conexion;
                    cmd.CommandText = @"INSERT INTO NotasIrregularidades (tienda, descripcion, tipo_irregularidad, id_usuario)
                                        VALUES (@tienda, @descripcion, @tipoIrregularidad, @idUsuario)";
                    
                    cmd.Parameters.AddWithValue("@tienda", nuevaNota.Tienda);
                    cmd.Parameters.AddWithValue("@descripcion", nuevaNota.Descripcion);
                    cmd.Parameters.AddWithValue("@tipoIrregularidad", nuevaNota.TipoIrregularidad);
                    cmd.Parameters.AddWithValue("@idUsuario", nuevaNota.IdUsuario);

                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { message = "Nota registrada exitosamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al registrar la nota.", error = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetNotasIrregularidades([FromQuery] string tienda, [FromQuery] string tipoIrregularidad)
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

                    if (!string.IsNullOrEmpty(tienda) && !string.IsNullOrEmpty(tipoIrregularidad))
                    {
                        cmd.CommandText = @"SELECT id_notas, tienda, descripcion, fecha, tipo_irregularidad 
                                            FROM NotasIrregularidades 
                                            WHERE tienda = @tienda AND tipo_irregularidad = @tipoIrregularidad";
                        cmd.Parameters.AddWithValue("@tienda", tienda);
                        cmd.Parameters.AddWithValue("@tipoIrregularidad", tipoIrregularidad);
                    }
                    else if (!string.IsNullOrEmpty(tienda))
                    {
                        cmd.CommandText = @"SELECT id_notas, tienda, descripcion, fecha, tipo_irregularidad 
                                            FROM NotasIrregularidades 
                                            WHERE tienda = @tienda";
                        cmd.Parameters.AddWithValue("@tienda", tienda);
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT id_notas, tienda, descripcion, fecha, tipo_irregularidad 
                                            FROM NotasIrregularidades";
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
                                Fecha = reader.GetDateTime("fecha"),
                                TipoIrregularidad = reader.GetString("tipo_irregularidad")
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