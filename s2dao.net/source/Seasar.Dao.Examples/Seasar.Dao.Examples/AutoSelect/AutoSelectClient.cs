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

using System;
using System.Collections;
using System.Data.SqlTypes;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Nullables;

namespace Seasar.Dao.Examples.AutoSelect
{
    /// <summary>
    /// Select�����������̃T���v�������s���܂��B
    /// </summary>
    public class AutoSelectClient
    {
        private const string PATH = "Seasar.Dao.Examples/AutoSelect/AutoSelect.dicon";

        public void Main()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IEmployeeDao employeeDao = (IEmployeeDao) container.GetComponent(typeof(IEmployeeDao));

            // �S�Ă̏]�ƈ����擾
            IList employeeList = employeeDao.GetAllList();

            IEnumerator employees = employeeList.GetEnumerator();
            Console.WriteLine("/** �S�Ă̏]�ƈ��̃��X�g **/");
            while (employees.MoveNext())
            {
                Console.WriteLine(((Employee) employees.Current).ToString());
            }

            IDepartmentDao1 deptDao = (IDepartmentDao1) container.GetComponent(typeof(IDepartmentDao1));

            // �S�Ă̕������擾
            IList deptList = deptDao.GetAllList();

            IEnumerator depts = deptList.GetEnumerator();
            Console.WriteLine("/** �S�Ă̕����̃��X�g(System.Data.SqlTypes���g�p) **/");
            while (depts.MoveNext())
            {
                Console.WriteLine(((Department1) depts.Current).ToString());
            }

            IDepartmentDao2 deptDao2 = (IDepartmentDao2) container.GetComponent(typeof(IDepartmentDao2));

            // �S�Ă̕������擾
            IList deptList2 = deptDao2.GetAllList();

            IEnumerator depts2 = deptList2.GetEnumerator();
            Console.WriteLine("/** �S�Ă̕����̃��X�g(NullableType���g�p) **/");
            while (depts2.MoveNext())
            {
                Console.WriteLine(((Department2) depts2.Current).ToString());
            }

            // Null�ł��鍀�ڂ��擾(INullable)
            SqlInt16 active = deptDao2.GetActiveByDeptno(20);
            Console.WriteLine("/** deptno��20��40��Active���擾(INullable���g�p) **/");
            Console.WriteLine("deptno=20");
            Console.WriteLine(active.IsNull ? "null" : active.Value.ToString());
            active = deptDao2.GetActiveByDeptno(40);
            Console.WriteLine("deptno=40");
            Console.WriteLine(active.IsNull ? "null" : active.Value.ToString());

            // Null�ł��鍀�ڂ��擾(INullable)
            NullableInt16 active2 = deptDao2.GetActiveByDeptno2(20);
            Console.WriteLine("/** deptno��20��40��Active���擾(INullableType���g�p) **/");
            Console.WriteLine("deptno=20");
            Console.WriteLine(active2.HasValue ? active2.Value.ToString() : "null");
            active2 = deptDao2.GetActiveByDeptno2(40);
            Console.WriteLine("deptno=40");
            Console.WriteLine(active2.HasValue ? active2.Value.ToString() : "null");

            // Null��String�Ŏ󂯎��
            string dname = deptDao2.GetDnameByDeptno(50);
            Console.WriteLine("/** deptno��50��dname���擾(NULL�Ŏ󂯎��) **/");
            Console.WriteLine("deptno=50");
            Console.WriteLine(dname == null ? "null" : dname);

            // Null��String�Ŏ󂯎��
            dname = deptDao2.GetDnameByDeptno(60);
            Console.WriteLine("/** deptno��60��dname���擾(���݂��Ȃ��f�[�^���󂯎��) **/");
            Console.WriteLine("deptno=60");
            Console.WriteLine(dname == null ? "null" : dname);
        }
    }
}