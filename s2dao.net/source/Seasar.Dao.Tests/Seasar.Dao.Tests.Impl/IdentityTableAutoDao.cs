using System;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// IdentityTableAutoDao �̊T�v�̐����ł��B
	/// </summary>
    [Bean(typeof(IdentityTable))]
    public interface IdentityTableAutoDao
	{
        void Insert(IdentityTable identityTable);
	}
}
