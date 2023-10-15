using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Testris.App.Core.DB;

namespace Testris.App.Core.Utils
{
    public class Score
    {
        public int MaxScore { get; private set; } = 0;
        public Connection _connection;

        private string scoreTable = "score";

        public Score()
        {
            _connection = new();
            GetDBScore();
        }


        private void GetDBScore()
        {
            try
            {
                _connection.Open();
                _connection.NonQuery($"CREATE TABLE IF NOT EXISTS {scoreTable}(value INT NOT NULL)");
                int value = _connection.GetValue($"SELECT value FROM {scoreTable}");
                if (value > 0)
                    MaxScore = value;
                else
                    _connection.NonQuery($"INSERT INTO {scoreTable} VALUES(0)");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                _connection.Close();
            }
        }

        private void SaveScore(int score)
        {
            try
            {
                _connection.Open();
                _connection.NonQuery($"UPDATE {scoreTable} SET value={score}");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Save score error");
            }
            finally
            {
                _connection.Close();
            }
        }

        public bool SetScore(int score)
        {
            if (score <= MaxScore)
                return false;

            MaxScore = score;
            SaveScore(MaxScore);
            return true;
        }

    }
}
