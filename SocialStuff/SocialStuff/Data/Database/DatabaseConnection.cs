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

    public class DatabaseConnection
    {
        // string connectionString = @"Data Source=razvan\sqlexpress01;Initial Catalog=BankingDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private string connectionString = @"Server=localhost;Database=BankingDB;Integrated Security=True;TrustServerCertificate=True;";

        private SqlConnection conn;

        public DatabaseConnection()
        {
            this.conn = new SqlConnection(this.connectionString);
            Console.WriteLine("Database Connection Created!");
        }

        public SqlConnection GetConnection()
        {
            return this.conn;
        }

        public void OpenConnection()
        {
            if (this.conn.State == ConnectionState.Closed)
            {
                this.conn.Open();
                Console.WriteLine("Database Connected!");
            }
        }

        public int CheckConnection()
        {
            if (this.conn.State == ConnectionState.Open)
            {
                Console.WriteLine("Database Connection is Open!");

                // print something on the screen
                return 1;
            }
            else
            {
                Console.WriteLine("Database Connection is Closed!");

                // print something on the screen
                return 0;
            }
        }

        public void CloseConnection()
        {
            if (this.conn.State == ConnectionState.Open)
            {
                this.conn.Close();
                Console.WriteLine("Database Connection Closed!");

                // print something on the screen
            }
        }

        // Executes a stored procedure and returns a single scalar value (e.g., COUNT(*), SUM(), MAX(), etc.)
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

        // Executes a stored procedure and returns multiple rows and columns as a DataTable
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

        // Executes a stored procedure that modifies data (INSERT, UPDATE, DELETE) and returns the number of affected rows
        // Alexandra- i ve changes such dat it also works with query
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