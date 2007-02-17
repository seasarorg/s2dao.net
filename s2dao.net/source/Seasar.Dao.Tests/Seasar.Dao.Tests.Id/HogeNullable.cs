using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Id
{
	public class HogeNullableDecimal
	{
        private Nullables.NullableDecimal id;

        public HogeNullableDecimal()
        {
        }

        [ID(IDType.IDENTITY)]
        public Nullables.NullableDecimal Id
        {
            set { id = value; }
            get { return id; }
        }
	}

    public class HogeNullableInt16
    {
        private Nullables.NullableInt16 id;

        public HogeNullableInt16()
        {
        }

        [ID(IDType.IDENTITY)]
        public Nullables.NullableInt16 Id
        {
            set { id = value; }
            get { return id; }
        }
    }

    public class HogeNullableInt32
    {
        private Nullables.NullableInt32 id;

        public HogeNullableInt32()
        {
        }

        [ID(IDType.IDENTITY)]
        public Nullables.NullableInt32 Id
        {
            set { id = value; }
            get { return id; }
        }
    }

    public class HogeNullableInt64
    {
        private Nullables.NullableInt64 id;

        public HogeNullableInt64()
        {
        }

        [ID(IDType.IDENTITY)]
        public Nullables.NullableInt64 Id
        {
            set { id = value; }
            get { return id; }
        }
    }

    public class HogeNullableSingle
    {
        private Nullables.NullableSingle id;

        public HogeNullableSingle()
        {
        }

        [ID(IDType.IDENTITY)]
        public Nullables.NullableSingle Id
        {
            set { id = value; }
            get { return id; }
        }
    }

    public class HogeNullableDouble
    {
        private Nullables.NullableDouble id;

        public HogeNullableDouble()
        {
        }

        [ID(IDType.IDENTITY)]
        public Nullables.NullableDouble Id
        {
            set { id = value; }
            get { return id; }
        }
    }
}
