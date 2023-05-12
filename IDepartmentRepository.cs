using System;
using System.Collections.Generic;

namespace Dapper
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartments();
    }
}

