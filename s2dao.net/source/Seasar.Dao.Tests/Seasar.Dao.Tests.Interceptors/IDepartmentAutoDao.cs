using System;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Interceptors
{
    [Bean(typeof(Department))]
	public interface IDepartmentAutoDao
	{
        int Update(Department department);

        int Delete(Department department);
	}
}
