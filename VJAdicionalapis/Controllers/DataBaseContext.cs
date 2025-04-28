using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;


namespace Reto.Model
{
    public class DataBaseContext
    {
        public string ConnectionString { get; set; }
        public DataBaseContext()
        {
             ConnectionString = "Server=addServer;Port=14683;Database=OxxoDB;Uid=avnadmin;password='ADDPASSWORD';";   }    
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        

       
    }
}