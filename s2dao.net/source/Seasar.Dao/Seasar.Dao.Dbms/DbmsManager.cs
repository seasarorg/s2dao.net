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
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Reflection;
using System.Resources;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Dbms
{
    public sealed class DbmsManager
    {
        private static ResourceManager resourceManager;

        static DbmsManager()
        {
            resourceManager = new ResourceManager(
                "Dbms", Assembly.GetExecutingAssembly());
        }

        private DbmsManager()
        {
        }

        public static IDbms GetDbms(IDataSource dataSource)
        {
            // IDbConnection��DataSource����擾����
            IDbConnection cn = dataSource.GetConnection();

            //IDbms�̎����N���X���擾���邽�߂�Key
            string dbmsKey = null;

            if (cn is OleDbConnection)
            {
                // OleDbConnection�̏ꍇ��Key��Type����Provider������쐬����
                OleDbConnection oleDbCn = cn as OleDbConnection;
                dbmsKey = cn.GetType().Name + "_" + oleDbCn.Provider;
            }
            else if (cn is OdbcConnection)
            {
                // OdbcConnection�̏ꍇ��Key��Type����Driver������쐬����
                OdbcConnection odbcCn = cn as OdbcConnection;
                dbmsKey = cn.GetType().Name + "_" + odbcCn.Driver;
            }
            else
            {
                dbmsKey = cn.GetType().Name;
            }

            // Key����IDbms�����N���X�̃C���X�^���X���擾����
            return GetDbms(dbmsKey);
        }

        /// <summary>
        /// Dbms.resx��dbmsKey�ŒT���AIDbms�����N���X�̃C���X�^���X���擾����
        /// </summary>
        /// <param name="dbmsKey">Dbms.resx����������ׂ�Key</param>
        /// <returns>IDbms�����N���X�̃C���X�^���X</returns>
        /// <remarks>dbmsKey�ɑΉ�������̂�������Ȃ��ꍇ�́A
        /// �W����Standard���g�p����</remarks>
        public static IDbms GetDbms(string dbmsKey)
        {
            // Dbms.resx����IDbms�̎����N���X�����擾����
            string typeName = resourceManager.GetString(dbmsKey);

            // IDbms�����N���X��Type���擾����
            // Dbms.resx�ɑΉ�����IDbms�����N���X�������ꍇ�́A�W����Standard���g�p����
            Type type = typeName == null ? typeof(Standard) : Type.GetType(typeName);

            // IDbms�����N���X�̃C���X�^���X���쐬���ĕԂ�
            return (IDbms) Activator.CreateInstance(type, false);
        }
    }
}
