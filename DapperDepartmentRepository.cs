using System;
using System.Collections.Generic;
using System.Data;

namespace Dapper
{
    public class DapperDepartmentRepository : IDepartmentRepository
    {
        private readonly IDbConnection _connection;

        public DapperDepartmentRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _connection.Query<Department>("SELECT * FROM Departments;").ToList();
        }

        public void InsertDepartment(string newDepartmentName)
        {
            _connection.Execute("INSERT INTO departments (Name) VALUES (@departmentName);",
                new { departmentName = newDepartmentName });
        }

        public void DeleteOneDepartment(string departmentName)
        {
            try
            {
                var department = _connection.QuerySingle(
                    "SELECT DepartmentID FROM departments WHERE Name = @departmentName",
                    new { departmentName = departmentName });
                _connection.Execute("DELETE FROM departments WHERE DepartmentId = (@departmentId);",
                    new { departmentId = department.DepartmentID });

                Console.WriteLine($"{departmentName} successfully deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}