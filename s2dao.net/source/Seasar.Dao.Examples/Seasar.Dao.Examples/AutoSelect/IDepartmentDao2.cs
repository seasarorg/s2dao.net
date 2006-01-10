using System;
using System.Collections;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Examples.AutoSelect
{
	/// <summary>
	/// IDepartmentDao
	/// </summary>
	[Bean(typeof(Department2))]
	public interface IDepartmentDao2
	{
        IList GetAllList();
	}
}
