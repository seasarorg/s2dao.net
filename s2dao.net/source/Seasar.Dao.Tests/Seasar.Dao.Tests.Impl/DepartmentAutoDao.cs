using System;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// DepartmentAutoDao ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
    [Bean(typeof(Department))]
    public interface DepartmentAutoDao
	{
        void Insert(Department department);

        //TODO:‘¶İ‚·‚é‚ÆNotSupportedException‚É‚È‚é
        //void InsertBatch(Department[] departents);

        void Update(Department department);

        //TODO:‘¶İ‚·‚é‚ÆNotSupportedException‚É‚È‚é
        //void UpdateBatch(Department[] departents);

        void Delete(Department department);

        //TODO:‘¶İ‚·‚é‚ÆNotSupportedException‚É‚È‚é
        //void DeleteBatch(Department[] departents);

	}
}
