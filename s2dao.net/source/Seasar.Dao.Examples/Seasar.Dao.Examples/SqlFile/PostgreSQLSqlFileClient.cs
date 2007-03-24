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
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Dao.Examples.SqlFile
{
    /// <summary>
    /// SQL�t�@�C���̃T���v�������s���܂��B
    /// </summary>
    public class PostgreSQLSqlFileClient
    {
        private const string PATH = "Seasar.Dao.Examples/SqlFile/PostgreSQLSqlFile.dicon";

        public void Main()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IEmployeeDao employeeDao = (IEmployeeDao) container.GetComponent(typeof(IEmployeeDao));

            // �]�ƈ��ԍ�����]�ƈ������擾���܂��B
            string ename = employeeDao.GetEnameByEmpno(7499);
            Console.WriteLine("�]�ƈ��ԍ���[7499]�̏]�ƈ�����[" + ename + "]�ł�");
        }
    }
}
