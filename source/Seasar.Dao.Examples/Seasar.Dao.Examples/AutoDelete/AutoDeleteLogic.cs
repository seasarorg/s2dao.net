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

namespace Seasar.Dao.Examples.AutoDelete
{
    public interface IAutoDeleteLogic
    {
        void TestAutoDelete();
    }

    public class AutoDeleteLogicImpl : IAutoDeleteLogic
    {
        private IEmployeeDao employeeDao;

        public AutoDeleteLogicImpl(IEmployeeDao employeeDao)
        {
            this.employeeDao = employeeDao;
        }

        #region IAutoDeleteLogic �����o

        public void TestAutoDelete()
        {
            // �]�ƈ��ԍ�7499�̏]�ƈ����m�F
            int empno = 7499;
            Employee emp1 = employeeDao.GetEmployeeByEmpno(empno);
            Console.WriteLine("�]�ƈ��ԍ�[" + empno + "]�̏]�ƈ��F" + emp1.ToString());

            // �]�ƈ��ԍ�7499�̏]�ƈ����폜
            int ret = employeeDao.DeleteEmployee(emp1);
            Console.WriteLine("DeleteEmployee���\�b�h�̖߂�l:" + ret);

            // �]�ƈ��ԍ�7499�̏]�ƈ����m�F
            Employee emp2 = employeeDao.GetEmployeeByEmpno(empno);
            if(emp2 == null)
            {
                Console.WriteLine("�]�ƈ��ԍ�[" + empno + "]�̏]�ƈ��͑��݂��܂���B");
            }

            throw new ForCleanupException();
        }

        #endregion
    }
}
