using System;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace Ableton_Project_Timer
{
    internal class DBLogic
    {
#if(DEBUG)
        private string dbLocation = "..\\..\\Data.db";
#else
        private string dbLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ableton Project Timer\\Data.db");
#endif
        public DBLogic()
        {
            ConnectDB();            
            CheckInitialized();
        }

        private void ConnectDB()
        {
            try
            {
                this.connectionString = $"Data Source={dbLocation};Version=3;";
                this.sqliteConnection = new SQLiteConnection(connectionString);
                this.adapter = new SQLiteDataAdapter("SELECT * FROM projects", this.sqliteConnection);
                this.sqliteConnection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void CheckInitialized()
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='projects';", sqliteConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                bool tableExists = reader.Read();
                if (!tableExists)
                {
                    InitalizeDatabase();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void InitalizeDatabase()
        {
            SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS projects (ID INTEGER PRIMARY KEY AUTOINCREMENT, ProjectName TEXT UNIQUE, TotalTime TEXT)", sqliteConnection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DataTable GetDataAsTable()
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM projects", sqliteConnection);
            try
            {
                DataTable dataTable = new DataTable();
                DataTable formattedTable = new DataTable();

                adapter.Fill(dataTable);

                formattedTable.Columns.Add("Project", typeof(string));
                formattedTable.Columns.Add("Time", typeof(string));

                foreach (DataRow row in dataTable.Rows)
                {
                    string projectName = (string)row["ProjectName"];
                    string projectTime = (string)row["TotalTime"];
                    Console.WriteLine(projectName + " " + projectTime);
                    formattedTable.Rows.Add(projectName, projectTime);
                }
                return formattedTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private bool? CheckProjectExists(string projectName)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT ProjectName FROM projects WHERE ProjectName = @pname LIMIT 1;", sqliteConnection);
            try
            {
                command.Parameters.AddWithValue("@pname", projectName);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine("CheckProjectExists()");
                Console.WriteLine (ex.Message);
                return null;
            }
        }

        public void WriteProjectTime(string projectName, string time)
        {
            bool? exists = CheckProjectExists(projectName);
            try
            {
                if (exists != null)
                {
                    if ((bool)exists)
                    {
                        SQLiteCommand dbWrite = new SQLiteCommand("UPDATE projects SET TotalTime = @ttime WHERE ProjectName = @pname;", this.sqliteConnection);
                        dbWrite.Parameters.AddWithValue("@ttime", time);
                        dbWrite.Parameters.AddWithValue("@pname", projectName);
                        dbWrite.ExecuteNonQuery();
                    }
                    else
                    {
                        SQLiteCommand dbWrite = new SQLiteCommand("INSERT INTO projects (ProjectName, TotalTime) VALUES (@pname, '00:00:01');", this.sqliteConnection);
                        dbWrite.Parameters.AddWithValue("@pname", projectName);
                        dbWrite.ExecuteNonQuery();
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string GetProjectTime(string projectName)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT TotalTime FROM projects WHERE ProjectName = @pname;", this.sqliteConnection);
            try
            {
                command.Parameters.AddWithValue("@pname", projectName);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    string time = reader.GetString(0);
                    return time;
                } else
                {
                    Console.WriteLine("Not found");
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine (ex.Message);
                return null;
            }
            
        }

        private string connectionString;
        private SQLiteConnection sqliteConnection;
        private SQLiteDataAdapter adapter;
    }   
}
