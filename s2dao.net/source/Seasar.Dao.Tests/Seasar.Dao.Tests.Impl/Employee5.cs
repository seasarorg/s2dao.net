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
    public class Employee5
    {
        private long empno;

        private string ename;

        private string job;

        private SqlInt16 mgr;

        private DateTime hiredate;

        private SqlSingle sal;

        private SqlSingle comm;

        private int deptno;

        private byte[] password;

        private string dummy;

        private Department department;

        private DateTime timestamp;

        public Employee5()
        {
        }

        public Employee5(long empno)
        {
            this.empno = empno;
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

        public SqlSingle Sal
        {
            set { sal = value; }
            get { return sal; }
        }

        public SqlSingle Comm
        {
            set { comm = value; }
            get { return comm; }
        }

        public int Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        public byte[] Password
        {
            set { password = value; }
            get { return password; }
        }

        public string Dummy
        {
            set { dummy = value; }
            get { return dummy; }
        }

        [Column("tstamp")]
        public DateTime Timestamp
        {
            set { timestamp = value; }
            get { return timestamp; }
        }

        [Relno(0), Relkeys("deptno:deptno, ename:dname")]
        public Department Department
        {
            set { department = value; }
            get { return department; }
        }
    }
}
