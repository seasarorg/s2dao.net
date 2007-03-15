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
using System.Data.SqlClient;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    /// <summary>
    /// Procedure��{Handler
    /// </summary>
    public class AbstractProcedureHandler : BasicHandler
    {
        private static Logger logger = Logger.GetLogger(typeof(AbstractProcedureHandler));

        /// <summary>
        /// �����^�C�v
        /// </summary>
        private Type[] argumentTypes;

        /// <summary>
        /// ������
        /// </summary>
        private string[] argumentNames;

        /// <summary>
        /// �����̂̓��o�͎��
        /// </summary>
        private ParameterDirection[] argumentDirection;

        /// <summary>
        /// �X�g�A�h�v���V�[�W����
        /// </summary>
        private string _procedureName;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="dataSource">�f�[�^�\�[�X��</param>
        /// <param name="commandFactory">IDbCommand Factory</param>
        /// <param name="procedureName">�X�g�A�h�v���V�[�W����</param>
        public AbstractProcedureHandler(IDataSource dataSource, ICommandFactory commandFactory, string procedureName)
        {
            DataSource = dataSource;
            CommandFactory = commandFactory;
            _procedureName = procedureName;
        }

        public static Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        /// <summary>
        /// �����^�C�v
        /// </summary>
        public Type[] ArgumentTypes
        {
            get { return argumentTypes; }
            set { argumentTypes = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public string[] ArgumentNames
        {
            get { return argumentNames; }
            set { argumentNames = value; }
        }

        /// <summary>
        /// �����̂̓��o�͎��
        /// </summary>
        public ParameterDirection[] ArgumentDirection
        {
            get { return argumentDirection; }
            set { argumentDirection = value; }
        }

        /// <summary>
        /// �X�g�A�h�v���V�[�W����
        /// </summary>
        public string ProcedureName
        {
            get { return _procedureName; }
            set { _procedureName = value; }
        }

        /// <summary>
        /// IDbCommand�I�u�W�F�N�g���擾����
        /// </summary>
        /// <param name="connection">�R�l�N�V�����I�u�W�F�N�g</param>
        /// <param name="procedureName">�X�g�A�h�v���V�[�W����</param>
        /// <returns></returns>
        protected IDbCommand GetCommand(IDbConnection connection, string procedureName)
        {
            if (procedureName == null)
                throw new EmptyRuntimeException("procedureName");

            IDbCommand cmd = CommandFactory.CreateCommand(connection, procedureName);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

            return cmd;
        }

        /// <summary>
        /// �X�g�A�h�v���V�[�W���pIN�p�����[�^�����蓖�Ă�
        /// </summary>
        /// <param name="command">IDbCommand�I�u�W�F�N�g</param>
        /// <param name="args">�����l</param>
        /// <param name="argTypes">�����^�C�v</param>
        /// <param name="argNames">������</param>
        /// <param name="argDirection">�����̓��o�͎��</param>
        protected void BindParamters(IDbCommand command, object[] args, Type[] argTypes,
                                     string[] argNames, ParameterDirection[] argDirection)
        {
            if (args == null) return;
            for (int i = 0; i < args.Length; ++i)
            {
                string columnName = argNames[i];
                BindVariableType vt = DataProviderUtil.GetBindVariableType(command);
                switch (vt)
                {
                    case BindVariableType.QuestionWithParam:
                        columnName = "?" + columnName;
                        break;
                    case BindVariableType.ColonWithParam:
                        columnName = "" + columnName;
                        break;
                    case BindVariableType.ColonWithParamToLower:
                        columnName = ":" + columnName.ToLower();
                        break;
                    default:
                        columnName = "@" + columnName;
                        break;
                }

                DbType dbType = GetDbValueType(argTypes[i]);
                IDbDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = columnName;
                parameter.Direction = argDirection[i];
                parameter.Value = args[i];
                parameter.DbType = dbType;
                parameter.Size = 4096;
                if ("OleDbCommand".Equals(command.GetType().Name) && dbType == DbType.String)
                {
                    OleDbParameter oleDbParam = parameter as OleDbParameter;
                    oleDbParam.OleDbType = OleDbType.VarChar;
                }
                else if ("SqlCommand".Equals(command.GetType().Name) && dbType == DbType.String)
                {
                    SqlParameter sqlDbParam = parameter as SqlParameter;
                    sqlDbParam.SqlDbType = SqlDbType.VarChar;
                }
                command.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// �߂�l�p�o�C���h�ϐ������蓖�Ă�
        /// </summary>
        /// <param name="command">IDbCommand�I�u�W�F�N�g</param>
        /// <param name="parameterName">�߂�l�p�����[�^��</param>
        /// <param name="dbType">DB�^�C�v</param>
        protected string BindReturnValues(IDbCommand command, string parameterName, DbType dbType)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.ReturnValue;
            parameter.DbType = dbType;
            parameter.Size = 4096;
            if ("OleDbCommand".Equals(command.GetType().Name) && dbType == DbType.String)
            {
                OleDbParameter oleDbParam = parameter as OleDbParameter;
                oleDbParam.OleDbType = OleDbType.VarChar;
            }
            else if ("SqlDbCommand".Equals(command.GetType().Name) && dbType == DbType.String)
            {
                SqlParameter sqlDbParam = parameter as SqlParameter;
                sqlDbParam.SqlDbType = SqlDbType.VarChar;
            }
            command.Parameters.Add(parameter);

            return parameter.ParameterName;
        }


        /// <summary>
        /// DBType�֕ϊ�����
        /// </summary>
        /// <param name="type">�^�C�v</param>
        /// <returns></returns>
        protected static DbType GetDbValueType(Type type)
        {
            if (type == typeof(Byte) || type.FullName == "System.Byte&")
                return DbType.Byte;
            if (type == typeof(SByte) || type.FullName == "System.SByte&")
                return DbType.SByte;
            if (type == typeof(Int16) || type.FullName == "System.Int16&")
                return DbType.Int16;
            if (type == typeof(Int32) || type.FullName == "System.Int32&")
                return DbType.Int32;
            if (type == typeof(Int64) || type.FullName == "System.Int64&")
                return DbType.Int64;
            if (type == typeof(Single) || type.FullName == "System.Single&")
                return DbType.Single;
            if (type == typeof(Double) || type.FullName == "System.Double&")
                return DbType.Double;
            if (type == typeof(Decimal) || type.FullName == "System.Decimal&")
                return DbType.Decimal;
            if (type == typeof(DateTime) || type.FullName == "System.DateTime&")
                return DbType.DateTime;
            if (type == ValueTypes.BYTE_ARRAY_TYPE)
                return DbType.Binary;
            if (type == typeof(String) || type.FullName == "System.String&")
                return DbType.String;
            if (type == typeof(Boolean) || type.FullName == "System.Boolean&")
                return DbType.Boolean;
            if (type == typeof(Guid) || type.FullName == "System.Guid&")
                return DbType.Guid;
            else
                return DbType.Object;
        }

        /// <summary>
        /// �p�����[�^�̕������擾����
        /// </summary>
        /// <param name="mi">���\�b�h���</param>
        /// <returns>�p�����[�^�����z��</returns>
        public static ParameterDirection[] GetParameterDirections(MethodInfo mi)
        {
            ParameterInfo[] parameters = mi.GetParameters();
            ParameterDirection[] ret = new ParameterDirection[parameters.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                if (parameters[i].IsOut)
                {
                    ret[i] = ParameterDirection.Output;
                }
                else if (parameters[i].ParameterType.IsByRef)
                {
                    ret[i] = ParameterDirection.InputOutput;
                }
                else
                {
                    ret[i] = ParameterDirection.Input;
                }
            }
            return ret;
        }
    }
}