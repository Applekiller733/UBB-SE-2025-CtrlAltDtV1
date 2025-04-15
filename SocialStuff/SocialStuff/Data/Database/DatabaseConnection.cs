// <copyright file="DatabaseConnection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Data.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Represents a connection to a SQL database.
    /// </summary>
    public class DatabaseConnection
    {
        private string connectionString = @"Server=localhost;Database=BankingDB;Integrated Security=True;TrustServerCertificate=True;";
        private SqlConnection conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnection"/> class.
        /// </summary>
        public DatabaseConnection()
        {
            this.conn = new SqlConnection(this.connectionString);
            Console.WriteLine("Database Connection Created!");
        }

        /// <summary>
        /// Gets the SQL connection.
        /// </summary>
        /// <returns>The SQL connection.</returns>
        public SqlConnection GetConnection()
        {
            return this.conn;
        }

        /// <summary>
        /// Opens the SQL connection.
        /// </summary>
        public void OpenConnection()
        {
            if (this.conn.State == ConnectionState.Closed)
            {
                this.conn.Open();
                Console.WriteLine("Database Connected!");
            }
        }

        /// <summary>
        /// Checks the state of the SQL connection.
        /// </summary>
        /// <returns>1 if the connection is open, otherwise 0.</returns>
        public int CheckConnection()
        {
            if (this.conn.State == ConnectionState.Open)
            {
                Console.WriteLine("Database Connection is Open!");
                return 1;
            }
            else
            {
                Console.WriteLine("Database Connection is Closed!");
                return 0;
            }
        }

        /// <summary>
        /// Closes the SQL connection.
        /// </summary>
        public void CloseConnection()
        {
            if (this.conn.State == ConnectionState.Open)
            {
                this.conn.Close();
                Console.WriteLine("Database Connection Closed!");
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns a single scalar value.
        /// </summary>
        /// <typeparam name="T">The type of the scalar value.</typeparam>
        /// <param name="storedProcedure">The name of the stored procedure.</param>
        /// <param name="sqlParameters">The parameters for the stored procedure.</param>
        /// <returns>The scalar value.</returns>
        public T? ExecuteScalar<T>(string storedProcedure, SqlParameter[] sqlParameters)
        {
            try
            {
                this.OpenConnection();
                using (SqlCommand command = new SqlCommand(storedProcedure, this.conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    var result = command.ExecuteScalar();
                    if (result == DBNull.Value || result == null)
                    {
                        return default;
                    }

                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecutingScalar: {ex.Message}");
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// Executes a query or stored procedure and returns the result as a DataTable.
        /// </summary>
        /// <param name="query">The query or stored procedure name.</param>
        /// <param name="sqlParameters">The parameters for the query or stored procedure.</param>
        /// <param name="isStoredProcedure">Indicates whether the query is a stored procedure.</param>
        /// <returns>The result as a DataTable.</returns>
        public DataTable ExecuteReader(string query, SqlParameter[] sqlParameters, bool isStoredProcedure = true)
        {
            try
            {
                this.OpenConnection();
                using (SqlCommand command = new SqlCommand(query, this.conn))
                {
                    if (isStoredProcedure)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        command.CommandType = CommandType.Text;
                    }

                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteReader: {ex.Message}");
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// Executes a stored procedure or query that modifies data and returns the number of affected rows.
        /// </summary>
        /// <param name="storedProcedure">The name of the stored procedure or query.</param>
        /// <param name="sqlParameters">The parameters for the stored procedure or query.</param>
        /// <returns>The number of affected rows.</returns>
        public int ExecuteNonQuery(string storedProcedure, SqlParameter[] sqlParameters)
        {
            try
            {
                this.OpenConnection();
                using (SqlCommand command = new SqlCommand(storedProcedure, this.conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteNonQuery: {ex.Message}");
            }
            finally
            {
                this.CloseConnection();
            }
        }
    }
}