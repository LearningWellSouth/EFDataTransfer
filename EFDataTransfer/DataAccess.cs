using System;
using System.Data;
using System.Data.SqlClient;

namespace EFDataTransfer
{
    public class DataAccess : IDataAccess
    {
        private readonly SqlConnection _connection;

      public DataAccess(string connectionString)
      {
        _connection = new SqlConnection(connectionString);
      }

      public void ValidateConnectionSettings() {
        getOpenConnection();
      }

        public void InsertMany(string tableName, DataTable table, bool identityInsert, SqlBulkCopyColumnMapping[] mappings)
        {
            SqlBulkCopy bulkCopy;
            if (identityInsert)
                bulkCopy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.UseInternalTransaction, null);
            else
                bulkCopy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.UseInternalTransaction, null);

            if (mappings != null)
                foreach (var mapping in mappings)
                    bulkCopy.ColumnMappings.Add(mapping);

            bulkCopy.BulkCopyTimeout = 20000;

            try
            {
                getOpenConnection();
                 
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(table);
            }
            finally
            {
                bulkCopy.Close();
                _connection.Close();
            }
        }

        public int InsertSingle(string command)
        {
          return ExecuteCommand(command, cmd => Convert.ToInt32(cmd.ExecuteScalar()) );
        }

      private delegate T DatabaseCommandBlock<T>(SqlCommand con);

      private T ExecuteCommand<T>(string statement, DatabaseCommandBlock<T> brick)
      {
        using (var commandObject = new SqlCommand(statement, getOpenConnection()))
        {
          commandObject.CommandTimeout = 10000;
          return brick(commandObject);
        }
      }

        public DataTable SelectIntoTable(string command)
        {
          getOpenConnection();

          using (var adapter = new SqlDataAdapter(command, _connection))
          {
            var table = new DataTable();

            adapter.SelectCommand.CommandTimeout = 3000;
            adapter.Fill(table);

            return table;
          }
        }

      public void NonQuery(string command)
      {
        ExecuteCommand(command, cmd => cmd.ExecuteNonQuery() );
      }

      private SqlConnection getOpenConnection()
      {
        if (_connection.State == ConnectionState.Closed) _connection.Open();
        return _connection;
      }
    }

  public interface IDataAccess
  {
    void ValidateConnectionSettings();
    DataTable SelectIntoTable(string statement);
    void InsertMany(string format, DataTable dataTable, bool p2, SqlBulkCopyColumnMapping[] coMappings);
    int InsertSingle(string statement);
    void NonQuery(string statement);
  }
}
