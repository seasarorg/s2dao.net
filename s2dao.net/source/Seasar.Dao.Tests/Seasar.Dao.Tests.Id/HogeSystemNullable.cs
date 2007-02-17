using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Id
{
#if !NET_1_1

    public class HogeSystemNullableDecimal
    {
        private System.Nullable<decimal> id;

        public HogeSystemNullableDecimal()
        {
        }

        [ID(IDType.IDENTITY)]
        public System.Nullable<decimal> Id
        {
            set { id = value; }
            get { return id; }
        }
    }

    public class HogeSystemNullableInt
    {
        private System.Nullable<int> id;

        public HogeSystemNullableInt()
        {
        }

        [ID(IDType.IDENTITY)]
        public System.Nullable<int> Id
        {
            set { id = value; }
            get { return id; }
        }
    }

    public class HogeSystemNullableShort
    {
        private System.Nullable<short> id;

        public HogeSystemNullableShort()
        {
        }

        [ID(IDType.IDENTITY)]
        public System.Nullable<short> Id
        {
            set { id = value; }
            get { return id; }
        }
    }

    public class HogeSystemNullableLong
    {
        private System.Nullable<long> id;

        public HogeSystemNullableLong()
        {
        }

        [ID(IDType.IDENTITY)]
        public System.Nullable<long> Id
        {
            set { id = value; }
            get { return id; }
        }
    }
    public class HogeSystemNullableFloat
    {
        private System.Nullable<float> id;

        public HogeSystemNullableFloat()
        {
        }

        [ID(IDType.IDENTITY)]
        public System.Nullable<float> Id
        {
            set { id = value; }
            get { return id; }
        }
    }
    public class HogeSystemNullableDouble
    {
        private System.Nullable<double> id;

        public HogeSystemNullableDouble()
        {
        }

        [ID(IDType.IDENTITY)]
        public System.Nullable<double> Id
        {
            set { id = value; }
            get { return id; }
        }
    }

#endif
}
