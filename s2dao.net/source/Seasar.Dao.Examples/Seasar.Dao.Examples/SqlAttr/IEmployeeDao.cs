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
using System.Collections;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Examples.SqlAttr
{
    [Bean(typeof(Employee))]
    public interface IEmployeeDao
    {
        /// <summary>
        /// �]�ƈ����E�����ԍ�����]�ƈ��ԍ����擾���܂��B
        /// </summary>
        /// <param name="ename">�]�ƈ���</param>
        /// <param name="deptnum">�����ԍ�</param>
        /// <returns>�]�ƈ��ԍ�</returns>
        [Sql("select empno from emp2 where ename=/*ename*/'ALLEN' and deptnum=/*deptnum*/30")]
        int GetEmpnoByEnameDeptnum(string ename, short deptnum);
    }
}
