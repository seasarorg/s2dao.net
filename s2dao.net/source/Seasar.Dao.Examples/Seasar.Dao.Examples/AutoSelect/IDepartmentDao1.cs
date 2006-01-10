using System;
using System.Collections;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Examples.AutoSelect
{
	/// <summary>
	/// IDepartmentDao
	/// </summary>
	[Bean(typeof(Department1))]
	public interface IDepartmentDao1
	{
        IList GetAllList();
	}
}
