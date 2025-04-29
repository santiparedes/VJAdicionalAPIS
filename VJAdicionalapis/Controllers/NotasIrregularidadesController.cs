using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using VJAdicionalapis.Models;

namespace VJAdicionalapis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotasIrregularidadesController : ControllerBase
    {
        private readonly string connectionString = "PlaceHolder";

        // Método para crear una nueva nota de irregularidad
        [HttpPost]
        public IActionResult CrearNotaIrregularidad([FromBody] NotaIrregularidad nuevaNota)
        {
            try
            {
                using (var conexion = new MySqlConnection(connectionString))
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("SP_PostNotasIrregularidades", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Asignación de parámetros
                        cmd.Parameters.AddWithValue("p_tienda", nuevaNota.Tienda);
                        cmd.Parameters.AddWithValue("p_descripcion", nuevaNota.Descripcion);
                        cmd.Parameters.AddWithValue("p_fecha", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("p_tipoIrregularidad", nuevaNota.TipoIrregularidad);
                        cmd.Parameters.AddWithValue("p_idUsuario", nuevaNota.IdUsuario);

                        // Ejecuta el procedimiento almacenado
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

        // Método para obtener las notas de irregularidades
        [HttpGet]
        public IActionResult GetNotasIrregularidades([FromQuery] string tienda, [FromQuery] string tipoIrregularidad)
        {
            try
            {
                var notas = new List<NotaIrregularidad>();

                using (var conexion = new MySqlConnection(connectionString))
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("SP_GetNotasIrregularidades", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Asignación de parámetros
                        cmd.Parameters.AddWithValue("p_tienda", tienda ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("p_tipoIrregularidad", tipoIrregularidad ?? (object)DBNull.Value);

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