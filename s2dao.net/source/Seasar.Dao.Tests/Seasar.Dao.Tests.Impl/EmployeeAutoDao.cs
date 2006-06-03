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
using System.Collections;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// EmployeeAutoDao の概要の説明です。
	/// </summary>
    [Bean(typeof(Employee))]
    public interface EmployeeAutoDao
	{
        //public object BEAN = typeof(Employee);
	
//        public String GetEmployeeByDeptno_ARGS = "deptno";
//        public String GetEmployeeByDeptno_ORDER = "deptno asc, empno desc";
        //TODO:QueryAttributeはAllowMultiple = trueにすべき？
        //[Query("order by deptno asc, empno desc")]
        [Query("deptno=/*deptno*/")]
        IList GetEmployeeByDeptno(int deptno);

//        String GetEmployeesBySal_QUERY = "sal BETWEEN ? AND ? ORDER BY empno";
        [Query("sal BETWEEN ? AND ? ORDER BY empno")]
        IList GetEmployeesBySal(float minSal, float maxSal);

//        String GetEmployeesByEnameJob_ARGS = "enames, jobs";
//        String GetEmployeesByEnameJob_QUERY = "ename IN /*enames*/('SCOTT','MARY') AND job IN /*jobs*/('ANALYST', 'FREE')";
        [Query("ename IN /*enames*/('SCOTT','MARY') AND job IN /*jobs*/('ANALYST', 'FREE')")]
        IList GetEmployeesByEnameJob(IList enames, IList jobs);

        //TODO:これがあると「テーブル(EMP)のカラム(dto)が見つかりません」になってしまう
        //IList GetEmployeesBySearchCondition(EmployeeSearchCondition dto);

        //IList GetEmployeesByEmployee(Employee dto);

//        String GetEmployee_ARGS = "empno";
        [Query("empno=/*empno*/")]
        Employee GetEmployee(int empno);

        void Insert(Employee employee);

//        String Insert2_NO_PERSISTENT_PROPS = "job, mgr, hiredate, sal, comm, deptno";
        [NoPersistentProps("job, mgr, hiredate, sal, comm, deptno")]
        void Insert2(Employee employee);

//        String Insert3_PERSISTENT_PROPS = "deptno";
        [PersistentProps("deptno")]
        void Insert3(Employee employee);

        //NotSupported
        //void InsertBatch(Employee[] employees);

        void Update(Employee employee);

//        String Update2_NO_PERSISTENT_PROPS = "job, mgr, hiredate, sal, comm, deptno";
        [NoPersistentProps("job, mgr, hiredate, sal, comm, deptno")]
        void Update2(Employee employee);

//        String Update3_PERSISTENT_PROPS = "deptno";
        [PersistentProps("deptno")]
        void Update3(Employee employee);

        //NotSupported
        //void UpdateBatch(Employee[] employees);

        void Delete(Employee employee);

        //NotSupported
        //void DeleteBatch(Employee[] employees);
    }
}
