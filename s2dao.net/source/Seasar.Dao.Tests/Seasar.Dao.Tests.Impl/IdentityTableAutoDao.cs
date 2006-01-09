using System;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// IdentityTableAutoDao ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
    [Bean(typeof(IdentityTable))]
    public interface IdentityTableAutoDao
	{
        void Insert(IdentityTable identityTable);
	}
}
