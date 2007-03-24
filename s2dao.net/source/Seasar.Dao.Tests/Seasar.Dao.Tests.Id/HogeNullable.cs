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
