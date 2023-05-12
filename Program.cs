using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var connString = config.GetConnectionString("DefaultConnection");

IDbConnection conn = new MySqlConnection(connString);
var repo = new DapperDepartmentRepository(conn);

Console.Write("Enter \"a\" to add a department or \"d\" to delete one record: ");
var input = Console.ReadLine();

switch (input)
{
    case "a":
        Console.Write("Type a new Department name: ");

        var newDepartment = Console.ReadLine()?.ToUpper().Trim();

        // First we make sure the department name does not already exist in the database
        var departments = repo.GetDepartments();
        var newDepartmentNameTaken = departments.Where(d => d.Name == newDepartment).ToList().Count == 1;

        if (newDepartmentNameTaken)
        {
            Console.WriteLine(
                $"The department name: \"{newDepartment} already exists, please try again with a different name.");
            return;
        }

        // convert the record to uppercase before we add it to the database
        if (newDepartment != null) repo.InsertDepartment(newDepartment.ToUpper());

        var updatedDepartments = departments.ToList();

        foreach (var dept in departments)
        {
            Console.WriteLine(dept.Name);
        }

        // We add the new department name to the list rather than doing another query
        Console.WriteLine(newDepartment);

        Console.WriteLine($"\n\"{newDepartment}\" successfully added");
        break;
    case "d":
        Console.WriteLine("Here is the list of departments:");

        var departmentsQuery = repo.GetDepartments();
        foreach (var dept in departmentsQuery)
        {
            Console.WriteLine(dept.Name);
        }

        Console.Write("\nPlease enter the department you want to delete:");
        var departmentToDelete = Console.ReadLine();

        if (departmentToDelete != null) repo.DeleteOneDepartment(departmentToDelete);
        break;
    default:
        Console.WriteLine("Invalid input.");
        break;
}