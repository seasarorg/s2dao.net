#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

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
