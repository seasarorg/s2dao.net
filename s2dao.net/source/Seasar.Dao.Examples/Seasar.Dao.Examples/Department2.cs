#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using Nullables;

namespace Seasar.Dao.Examples
{
    [Table("DEPT2")]
    public class Department2
    {
        private NullableInt32 deptno;
        private string dname;
        private NullableInt16 active;

        public Department2()
        {
        }

        public NullableInt32 Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        public string Dname
        {
            set { dname = value; }
            get { return dname; }
        }

        public NullableInt16 Active
        {
            set { active = value; }
            get { return active; }
        }

        public override string ToString()
        {
            return "deptno=" + (!deptno.HasValue ? "null" : deptno.Value.ToString()) 
                + ", dname=" + (dname == null ? "null" : dname)
                + ", active=" + (!active.HasValue ? "null" : active.Value.ToString());
        }

    }
}
