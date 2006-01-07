using System;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// DepartmentAutoDao の概要の説明です。
	/// </summary>
    [Bean(typeof(Department))]
    public interface DepartmentAutoDao
	{
        void Insert(Department department);

        //TODO:存在するとNotSupportedExceptionになる
        //void InsertBatch(Department[] departents);

        void Update(Department department);

        //TODO:存在するとNotSupportedExceptionになる
        //void UpdateBatch(Department[] departents);

        void Delete(Department department);

        //TODO:存在するとNotSupportedExceptionになる
        //void DeleteBatch(Department[] departents);

	}
}
