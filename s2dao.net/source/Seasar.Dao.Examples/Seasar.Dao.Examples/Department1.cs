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

using System;
using System.Data.SqlTypes;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Examples
{
    [Table("DEPT2")]
    public class Department1
    {
        private SqlInt32 deptno;
        private SqlString dname;
        private SqlInt16 active;

        public Department1()
        {
        }

        public SqlInt32 Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        public SqlString Dname
        {
            set { dname = value; }
            get { return dname; }
        }

        public SqlInt16 Active
        {
            set { active = value; }
            get { return active; }
        }

        public override string ToString()
        {
            return "deptno=" + (deptno.IsNull ? "null" : deptno.Value.ToString())
                + ", dname=" + (dname.IsNull ? "null" : dname.Value)
                + ", active=" + (active.IsNull ? "null" : active.Value.ToString());
        }
    }
}
