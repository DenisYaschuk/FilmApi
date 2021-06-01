using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Contexts
{
    public class SqlDbManualRepo
    {
        public SqlDbManualRepo()
        {
        }
        public MySqlConnection Connection { get; set; }

        public async Task<bool> IsConnect()
        {
            if (Connection == null)
            {
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", "filmgamebookapi.mariadb.database.azure.com", "filmsgamesbooks", "sql5415893@filmgamebookapi", "iPft6nxgal");
                Connection = new MySqlConnection(connstring);
                await Connection.OpenAsync();
                return true;
            }

            return true;
        }

        public async Task Close()
        {
            await Connection.CloseAsync();
        }
    }
}
