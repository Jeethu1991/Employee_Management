using Antlr.Runtime;
using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Xml.Linq;


namespace EmployeeManagement.Data
{
    public class EmployeeRepository
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;

        public List<Employee> GetAll()
        {
            var list = new List<Employee>();
            using(var con = new SqlConnection(_connStr))
            using(var cmd = new SqlCommand("GetAllEmployees",con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while(r.Read())
                    {
                        list.Add(new Employee
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            Name = r["Name"].ToString(),
                            Email = r["Email"].ToString(),
                            Department = r["Department"].ToString(),
                            Salary = Convert.ToDecimal(r["Salary"])
                        });
                    }
                }
            }
            return list;
        }

        public Employee GetById(int id)
        {
            using (var con = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("GetEmployeeById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                using(var r = cmd.ExecuteReader())
                {
                    if(r.Read())
                    {
                        return new Employee
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            Name = r["Name"].ToString(),
                            Email = r["Email"].ToString(),
                            Department = r["Department"].ToString(),
                            Salary = Convert.ToDecimal(r["Salary"])
                        };
                    }
                }
            }
            return null;
        }

        public int Save(Employee e)
        {
            Debug.WriteLine("CONN STRING = " + _connStr);

            using (var con = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("SaveEmployee", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", e.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", e.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Department", e.Department ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Salary", e.Salary);

                con.Open();
                var result = cmd.ExecuteNonQuery();
                Debug.WriteLine("Rows inserted: " + result);
                return result;

                //Name, Email, Department, Salary
            }
        }

        public bool Update(Employee e)
        {
            using (var con = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("UpdateEmployee", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", e.Id);
                cmd.Parameters.AddWithValue("@Name", e.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", e.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Department", e.Department ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Salary", e.Salary);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var con = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("DeleteEmployee", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                return cmd.ExecuteNonQuery() < 0;
            }
        }
    }
}