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
namespace Seasar.Dao.Examples.AutoInsert
{
    public interface IAutoInsertLogic
    {
        void TestAutoInsert();
    }

    public class AutoInsertLogicImpl : IAutoInsertLogic
    {
        private IEmployeeDao employeeDao;

        public AutoInsertLogicImpl(IEmployeeDao employeeDao)
        {
            this.employeeDao = employeeDao;
        }

        #region IAutoInsertLogic ƒƒ“ƒo

        public void TestAutoInsert()
        {
            // ]‹Æˆõ”Ô†9999‚Ì]‹Æˆõ‚ğŠm”F
            int empno = 9999;
            Employee emp1 = employeeDao.GetEmployeeByEmpno(empno);
            if(emp1 == null)
            {
                Console.WriteLine("]‹Æˆõ”Ô†[" + empno + "]‚Ì]‹Æˆõ‚Í‘¶İ‚µ‚Ü‚¹‚ñB");
            }

            // ]‹Æˆõ”Ô†9999‚Ì]‹Æˆõ‚ğ’Ç‰Á
            Employee emp2 = new Employee();
            emp2.Empno = empno;
            emp2.Ename = "Kazuya";
            emp2.Deptnum = 12;
            int ret = employeeDao.InsertEmployee(emp2);
            Console.WriteLine("InsertEmployeeƒƒ\ƒbƒh‚Ì–ß‚è’l:" + ret);

            // ]‹Æˆõ”Ô†9999‚Ì]‹Æˆõ‚ğŠm”F
            Employee emp3 = employeeDao.GetEmployeeByEmpno(empno);
            Console.WriteLine("]‹Æˆõ”Ô†[" + empno + "]‚Ì]‹ÆˆõF" + emp3.ToString());

            throw new ForCleanupException();
        }

        #endregion
    }
}
