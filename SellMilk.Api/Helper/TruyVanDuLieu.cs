using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SellMilk.Api.Helper.Interfaces;

namespace SellMilk.Api.Helper
{
    public class TruyVanDuLieu: ITruyVanDuLieu
    {
        public string StrConnection { get; set; }
        //Connection
        public SqlConnection sqlConnection { get; set; }
        //NpgsqlTransaction 
        public SqlTransaction sqlTransaction { get; set; }
        public TruyVanDuLieu(IConfiguration configuration)
        {
            StrConnection = configuration["ConnectionStrings:DefaultConnection"];
        }

        public void SetConnectionString(string connectionString)
        {
            StrConnection = connectionString;
        }
        public string OpenConnection()
        {
            try
            {
                if (sqlConnection == null)
                    sqlConnection = new SqlConnection(StrConnection);

                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                return "";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public string OpenConnectionAndBeginTransaction()
        {
            try
            {
                if (sqlConnection == null)
                    sqlConnection = new SqlConnection(StrConnection);

                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                sqlTransaction = sqlConnection.BeginTransaction();

                return "";
            }
            catch (Exception exception)
            {
                if (sqlConnection != null)
                    sqlConnection.Dispose();

                if (sqlTransaction != null)
                    sqlTransaction.Dispose();

                return exception.Message;
            }
        }

        public string CloseConnection()
        {
            try
            {
                if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                    sqlConnection.Close();
                return "";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public string CloseConnectionAndEndTransaction(bool isRollbackTransaction)
        {
            try
            {
                if (sqlTransaction != null)
                {
                    if (isRollbackTransaction)
                        sqlTransaction.Rollback();
                    else sqlTransaction.Commit();
                }

                if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                    sqlConnection.Close();
                return "";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public string ExecuteNoneQuery(string strquery)
        {
            string msgError = "";
            try
            {
                OpenConnection();
                var sqlCommand = new SqlCommand(strquery, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
            }
            catch (Exception exception)
            {
                msgError = exception.ToString();
            }
            finally
            {
                CloseConnection();
            }
            return msgError;
        }

        public DataTable ExecuteQueryToDataTable(string strquery, out string msgError)
        {
            msgError = "";
            var result = new DataTable();
            var sqlDataAdapter = new SqlDataAdapter(strquery, StrConnection);
            try
            {
                sqlDataAdapter.Fill(result);
            }
            catch (Exception exception)
            {
                msgError = exception.ToString();
                result = null;
            }
            finally
            {
                sqlDataAdapter.Dispose();
            }
            return result;
        }
        public object ExecuteScalar(string strquery, out string msgError)
        {
            object result = null;
            try
            {
                OpenConnection();
                var npgsqlCommand = new SqlCommand(strquery, sqlConnection);
                result = npgsqlCommand.ExecuteScalar();
                npgsqlCommand.Dispose();
                msgError = "";
            }
            catch (Exception ex) { msgError = ex.StackTrace; }
            finally
            {
                CloseConnection();
            }
            return result;
        }
        public object ExecuteScalarSProcedureWithTransaction(out string msgError, string sprocedureName, params object[] paramObjects)
        {
            msgError = "";
            object result = null;
            using (SqlConnection connection = new SqlConnection(StrConnection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = connection.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = sprocedureName;
                        cmd.Transaction = transaction;
                        cmd.Connection = connection;

                        int parameterInput = (paramObjects.Length) / 2;
                        int j = 0;
                        for (int i = 0; i < parameterInput; i++)
                        {
                            string paramName = Convert.ToString(paramObjects[j++]);
                            object value = paramObjects[j++];
                            if (paramName.ToLower().Contains("json"))
                            {
                                cmd.Parameters.Add(new SqlParameter()
                                {
                                    ParameterName = paramName,
                                    Value = value ?? DBNull.Value,
                                    SqlDbType = SqlDbType.NVarChar
                                });
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter(paramName, value ?? DBNull.Value));
                            }
                        }

                        result = cmd.ExecuteScalar();
                        cmd.Dispose();
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {

                        result = null;
                        msgError = exception.ToString();
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex) { }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return result;
        }

        public DataTable ExecuteSProcedureReturnDataTable(out string msgError, string sprocedureName, params object[] paramObjects)
        {
            DataTable tb = new DataTable();
            SqlConnection connection;
            try
            {
                SqlCommand cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = sprocedureName };
                connection = new SqlConnection(StrConnection);
                cmd.Connection = connection;

                int parameterInput = (paramObjects.Length) / 2;

                int j = 0;
                for (int i = 0; i < parameterInput; i++)
                {
                    string paramName = Convert.ToString(paramObjects[j++]).Trim();
                    object value = paramObjects[j++];
                    if (paramName.ToLower().Contains("json"))
                    {
                        cmd.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = paramName,
                            Value = value ?? DBNull.Value,
                            SqlDbType = SqlDbType.NVarChar
                        });
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter(paramName, value ?? DBNull.Value));
                    }
                }

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(tb);
                cmd.Dispose();
                ad.Dispose();
                connection.Dispose();
                msgError = "";
            }
            catch (Exception exception)
            {
                tb = null;
                msgError = exception.ToString();
            }
            return tb;
        }
        public DataSet ExecuteSProcedureReturnDataSet(out string msgError, string sprocedureName, params object[] paramObjects)
        {
            DataSet tb = new DataSet();
            SqlConnection connection;
            try
            {
                SqlCommand cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = sprocedureName };
                connection = new SqlConnection(StrConnection);
                cmd.Connection = connection;

                int parameterInput = (paramObjects.Length) / 2;

                int j = 0;
                for (int i = 0; i < parameterInput; i++)
                {
                    string paramName = Convert.ToString(paramObjects[j++]);
                    object value = paramObjects[j++];
                    if (paramName.ToLower().Contains("json"))
                    {
                        cmd.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = paramName,
                            Value = value ?? DBNull.Value,
                            SqlDbType = SqlDbType.NVarChar
                        });
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter(paramName, value ?? DBNull.Value));
                    }
                }

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(tb);
                cmd.Dispose();
                ad.Dispose();
                connection.Dispose();
                msgError = "";
            }
            catch (Exception exception)
            {
                tb = null;
                msgError = exception.ToString();
            }

            return tb;
        }
        public string ExecuteSProcedureNonQuery(string sprocedureName, params object[] paramObjects)
        {
            string result = "";
            SqlConnection connection = new SqlConnection(StrConnection);
            try
            {
                SqlCommand cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = sprocedureName };
                connection.Open();
                cmd.Connection = connection;
                int parameterInput = (paramObjects.Length) / 2;
                int j = 0;
                for (int i = 0; i < parameterInput; i++)
                {
                    string paramName = Convert.ToString(paramObjects[j++]);
                    object value = paramObjects[j++];
                    if (paramName.ToLower().Contains("json"))
                    {
                        cmd.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = paramName,
                            Value = value ?? DBNull.Value,
                            SqlDbType = SqlDbType.NVarChar
                        });
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter(paramName, value ?? DBNull.Value));
                    }
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception exception)
            {
                result = exception.ToString();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

    }
}
