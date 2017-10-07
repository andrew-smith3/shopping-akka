using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace ShoppingCart.Data.ProjectionStore
{ 
    public class SqliteProjectionStore
    {
        private string _connectionString;

        public SqliteProjectionStore()
        {
            _connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = "c:\\users\\andrew\\desktop\\projections.db"
            }.ConnectionString;

            CreateTableIfNotExists();
        }

        public void Store(Guid id, string type, string data)
        {
            using (var connection = Connect())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "INSERT OR REPLACE INTO projections ( id, type, data ) VALUES ( $id, $type, $data)";
                    command.Parameters.AddWithValue("$id", id.ToString());
                    command.Parameters.AddWithValue("$type", type);
                    command.Parameters.AddWithValue("$data", data);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        public string Retrieve(Guid id, string type)
        {
            using (var connection = Connect())
            {
                connection.Open();
                
                var command = connection.CreateCommand();
                command.CommandText = "SELECT data FROM projections WHERE id=$id AND type=$type LIMIT 1";
                command.Parameters.AddWithValue("$id", id.ToString());
                command.Parameters.AddWithValue("$type", type);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetString(0);
                    }
                    return string.Empty;
                }
            }
        }

        private SqliteConnection Connect()
        {
            return new SqliteConnection(_connectionString);
        }

        private void CreateTableIfNotExists()
        {
            using (var connection = Connect())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS projections (id text NOT NULL, type text NOT NULL, data text NOT NULL, PRIMARY KEY(id, type))";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }
    }
}
