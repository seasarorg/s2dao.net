using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Id
{
	public class HogeNullable
	{
        private Nullables.NullableDecimal id = -1;

        public HogeNullable()
        {
        }

        [ID(IDType.IDENTITY)]
        public Nullables.NullableDecimal Id
        {
            set { id = value; }
            get { return id; }
        }
	}
}
