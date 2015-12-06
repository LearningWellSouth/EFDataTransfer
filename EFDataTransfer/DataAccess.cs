using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataTransfer
{
    public class DataAccess
    {
        private SqlConnection _connection;

      public DataAccess(string connectionString)
      {
        _connection = new SqlConnection(connectionString);
      }

      public void ValidateConnectionSettings() {
        _connection.Open();
        _connection.Close();
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
                _connection.Open();
                 

                //using (var bulkCopy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.UseInternalTransaction, null))
                //{
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(table);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bulkCopy.Close();
                _connection.Close();
            }
        }

        public int InsertSingle(string command)
        {
            try
            {
                getOpenConnection();

                using (var cmd = new SqlCommand(command, _connection))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

      private delegate T DatabaseWork<T>(SqlConnection con);

      private T runWithConnection<T>(DatabaseWork<T> work)
      {
        var con = getOpenConnection();
        try
        {
          return work(getOpenConnection());
        }
        finally
        {
          con.Close();
        }
      }
        public DataTable SelectIntoTable(string command)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public void NonQuery(string command)
        {
            try
            {
              getOpenConnection();

              using (var cmd = new SqlCommand(command, _connection))
                {
                    cmd.CommandTimeout = 3900;
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                //_connection.Close();
            }
        }

      private SqlConnection getOpenConnection()
      {
        if (_connection.State == ConnectionState.Closed) _connection.Open();
        return _connection;
      }
    }
}
