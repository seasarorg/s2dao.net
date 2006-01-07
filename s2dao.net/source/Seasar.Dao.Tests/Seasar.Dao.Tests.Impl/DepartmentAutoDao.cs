using System;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// DepartmentAutoDao �̊T�v�̐����ł��B
	/// </summary>
    [Bean(typeof(Department))]
    public interface DepartmentAutoDao
	{
        void Insert(Department department);

        //TODO:���݂����NotSupportedException�ɂȂ�
        //void InsertBatch(Department[] departents);

        void Update(Department department);

        //TODO:���݂����NotSupportedException�ɂȂ�
        //void UpdateBatch(Department[] departents);

        void Delete(Department department);

        //TODO:���݂����NotSupportedException�ɂȂ�
        //void DeleteBatch(Department[] departents);

	}
}
