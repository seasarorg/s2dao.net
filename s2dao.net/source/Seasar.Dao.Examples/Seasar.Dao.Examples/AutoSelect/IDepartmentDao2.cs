using System;
using System.Collections;
using System.Data.SqlTypes;
using Seasar.Dao.Attrs;
using Nullables;

namespace Seasar.Dao.Examples.AutoSelect
{
	/// <summary>
	/// IDepartmentDao
	/// </summary>
	[Bean(typeof(Department2))]
	public interface IDepartmentDao2
	{
        IList GetAllList();

        [Sql("select active from dept2 where deptno=/*deptno*/")]
        SqlInt16 GetActiveByDeptno(int deptno);

        [Sql("select active from dept2 where deptno=/*deptno*/")]
        NullableInt16 GetActiveByDeptno2(int deptno);

        [Sql("select dname from dept2 where deptno=/*deptno*/")]
        string GetDnameByDeptno(int deptno);
	}
}
