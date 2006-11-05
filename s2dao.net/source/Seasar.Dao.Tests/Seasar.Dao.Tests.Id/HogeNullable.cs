using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Id
{
	public class HogeNullable
	{
        private Nullables.NullableDecimal id = -1;

        public HogeNullable()
        {
        }

        [ID("identity")]
        public Nullables.NullableDecimal Id
        {
            set { id = value; }
            get { return id; }
        }
	}
}
