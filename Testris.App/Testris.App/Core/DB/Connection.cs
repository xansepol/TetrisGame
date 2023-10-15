using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testris.App.Core.DB
{
    public class Connection : IDisposable
    {
        private SQLiteConnection _con;

        public Connection()
        {
            _con = new SQLiteConnection($"URI=file:{AppDomain.CurrentDomain.BaseDirectory}TetrisGame.db");
        }

        public void Open()
        {
            if(_con.State == System.Data.ConnectionState.Closed)
                _con.Open();
        }

        public void Close()
        {
            if(_con.State == System.Data.ConnectionState.Open)
                _con.Close();
        }

        public int NonQuery(string query)
        {
            SQLiteCommand cmd = new(query, _con);
            return cmd.ExecuteNonQuery();
        }

        public SQLiteDataReader GetReader(string query, int timeout = 30)
        {
            SQLiteCommand cmd = new(query, _con);
            cmd.CommandTimeout = timeout;
            return cmd.ExecuteReader();
        }

        public int GetValue(string query)
        {
            using (SQLiteDataReader reader = GetReader(query))
            {
                if (reader.Read())
                    return reader.GetInt32(0);
            }
            return 0;
        }

        public void Dispose()
        {
            _con.Close();
            _con.Dispose();
        }
    }
}
