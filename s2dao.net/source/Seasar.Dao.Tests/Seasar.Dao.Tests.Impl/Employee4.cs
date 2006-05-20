#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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

namespace Seasar.Dao.Tests.Impl
{
    [Table("EMP")]
    public class Employee4
	{
        private long empno;
        private string ename;

        private string job;

        private SqlInt16 mgr;

        private DateTime hiredate;

        private float sal;

        private float comm;

        private int deptno;
    
        private Employee4 parent;

		public Employee4()
		{
		}

        public long Empno
        {
            set { empno = value; }
            get { return empno; }
        }

        public string Ename
        {
            set { ename = value; }
            get { return ename; }
        }

        public string Job
        {
            set { job = value; }
            get { return job; }
        }

        public SqlInt16 Mgr
        {
            set { mgr = value; }
            get { return mgr; }
        }

        public DateTime Hiredate
        {
            set { hiredate = value; }
            get { return hiredate; }
        }

        public float Sal
        {
            set { sal = value; }
            get { return sal; }
        }

        public float Comm
        {
            set { comm = value; }
            get { return comm; }
        }

        public int Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        [Relno(0), Relkeys("mgr:empno")]
        public Employee4 Parent
        {
            set { parent = value; }
            get { return parent; }
        }

	}
}
