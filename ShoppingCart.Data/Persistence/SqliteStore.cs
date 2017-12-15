using System;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using ShoppingCart.Data.ReadModels;

namespace ShoppingCart.Data.Persistence
{ 
    public class SqliteStore : IRetrieveReadModels, ISaveReadModels
    {
        private readonly string _connectionString;

        public SqliteStore()
        {
            _connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = "c:\\users\\andrew\\desktop\\projections.db"
            }.ConnectionString;

            CreateTableIfNotExists();
        }

        public void Save<T>(T readModel) where T : ReadModel
        {
            var data = JsonConvert.SerializeObject(readModel);
            var type = nameof(T);
            using (var connection = Connect())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "INSERT OR REPLACE INTO projections ( id, type, data ) VALUES ( $id, $type, $data)";
                    command.Parameters.AddWithValue("$id", readModel.Id.ToString());
                    command.Parameters.AddWithValue("$type", type);
                    command.Parameters.AddWithValue("$data", data);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        public T Retrieve<T>(Guid id) where T : ReadModel
        {
            using (var connection = Connect())
            {
                connection.Open();

                var type = typeof(T).FullName;
                var command = connection.CreateCommand();
                command.CommandText = "SELECT data FROM projections WHERE id=$id AND type=$type LIMIT 1";
                command.Parameters.AddWithValue("$id", id.ToString());
                command.Parameters.AddWithValue("$type", type);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var json = reader.GetString(0);
                        return JsonConvert.DeserializeObject<T>(json);
                    }
                }
            }
            return null;
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
