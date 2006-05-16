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

namespace Seasar.Dao.Tests.Interceptors
{
    [Bean(typeof(Employee))]
    public interface IEmployeeDao
    {
        /// <summary>
        /// ‘S‚Ä‚Ì]‹Æˆõ‚ğæ“¾‚·‚é
        /// </summary>
        /// <returns>Employee‚ÌƒŠƒXƒg</returns>
        IList GetAllEmployees();

        /// <summary>
        /// ]‹Æˆõ”Ô†‚©‚ç]‹Æˆõ‚ğæ“¾‚·‚é
        /// </summary>
        /// <param name="empno">]‹Æˆõ”Ô†</param>
        /// <returns>]‹Æˆõ</returns>
        [Query("empno=/*empno*/")]
        Employee GetEmployee(int empno);
//	
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="deptno"></param>
//        /// <returns></returns>
//        Employee[] GetEmployeesByDeptno(int deptno);

	    /// <summary>
	    /// ]‹Æˆõ‚ÌŒ”‚ğæ“¾‚·‚é
	    /// </summary>
	    /// <returns>]‹Æˆõ”</returns>
        [Sql("select count(*) from EMP")]
        int GetCount();

        /// <summary>
        /// ]‹Æˆõ‚ğ’Ç‰Á‚·‚é
        /// </summary>
        /// <param name="empno">]‹Æˆõ”Ô†</param>
        /// <param name="ename">]‹Æˆõ–¼</param>
        /// <returns>’Ç‰ÁŒ”</returns>
        int Insert(int empno, String ename);

        /// <summary>
        /// ]‹Æˆõ‚ğXV‚·‚é
        /// </summary>
        /// <param name="employee">]‹Æˆõ</param>
        /// <returns>XVŒ”</returns>
        int Update(Employee employee);
    }
}
