namespace Dapper
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartments();
    }
}