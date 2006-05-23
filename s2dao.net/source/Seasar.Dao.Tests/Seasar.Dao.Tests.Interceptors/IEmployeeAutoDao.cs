using System;
using System.Collections;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Interceptors
{
    [Bean(typeof(Employee))]
	public interface IEmployeeAutoDao
	{

        Employee GetEmployee(int empno);

        [Query("sal BETWEEN /*minSal*/ and /*maxSal*/")]
        IList GetEmployeesBySal(float minSal, float maxSal);

        int Insert(Employee employee);

	}
}
