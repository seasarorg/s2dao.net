using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
    [Bean(typeof(Employee))]
	public interface IEmployeeDataSetDao
	{
        EmployeeDataSet.EmpAndDeptDataTable SelectDataTableAuto(int empno);

        [Query("SELECT emp.empno,emp.ename,dept.deptno,dept.dname FROM emp left outer join dept on emp.deptno = dept.deptno where emp.empno = /*employeeNo*/7369")]
        EmployeeDataSet.EmpAndDeptDataTable SelectDataTable(int employeeNo);

        [Query("SELECT emp.empno,emp.ename,dept.deptno,dept.dname FROM emp left outer join dept on emp.deptno = dept.deptno where emp.empno = /*employeeNo*/7369")]
        EmployeeDataSet SelectDataSet(int employeeNo);
	}
}
