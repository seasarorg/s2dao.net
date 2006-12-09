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
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    /// <summary>
    /// �o�̓p�����[�^���P��ł���ProcedureHandler
    /// </summary>
    public class ObjectBasicProcedureHandler : AbstractProcedureHandler
    {
        private static readonly Logger logger = Logger.GetLogger(typeof(ObjectBasicProcedureHandler));
        
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="dataSource">�f�[�^�\�[�X��</param>
        /// <param name="procedureName">�v���V�[�W����</param>
        public ObjectBasicProcedureHandler(IDataSource dataSource, string procedureName)
            : base(dataSource, procedureName)
        {
            ;
        }

        /// <summary>
        /// �X�g�A�h�v���V�[�W�������s����
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="returnType">�߂�l�^�C�v</param>
        /// <returns>�o�̓p�����[�^�l</returns>
        public object Execute(object[] args, Type returnType)
        {
            if ( DataSource == null ) throw new EmptyRuntimeException("dataSource");
            IDbConnection conn = DataSourceUtil.GetConnection(DataSource);

            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(ProcedureName);
                }
                
                IDbCommand cmd = null;
                try
                {
                    object ret = null;
                    cmd = GetCommand(conn, ProcedureName);

                    // �p�����[�^���Z�b�g���A�Ԓl���擾����
                    if ( returnType != typeof (void) )
                    {
                        // ODP.NET�ł́A�ŏ���RETURN�p�����[�^���Z�b�g���Ȃ���RETURN�l���擾�ł��Ȃ��H
                        string returnParamName = BindReturnValues(cmd, "RetValue", GetDbValueType(returnType));

                        BindParamters(cmd, args, ArgumentTypes, ArgumentNames, ArgumentDirection);

                        CommandUtil.ExecuteNonQuery(DataSource, cmd);

                        IDbDataParameter param = (IDbDataParameter) cmd.Parameters[returnParamName];
                        ret = param.Value;
                    }
                    else
                    {
                        BindParamters(cmd, args, ArgumentTypes, ArgumentNames, ArgumentDirection);
                        CommandUtil.ExecuteNonQuery(DataSource, cmd);
                    }

                    // Out�܂���InOut�p�����[�^�l���擾����
                    for ( int i = 0; i < args.Length; i++ )
                    {
                        if ( ArgumentDirection[i] == ParameterDirection.InputOutput ||
                             ArgumentDirection[i] == ParameterDirection.Output )
                        {
                            args[i] = ( (IDataParameter) cmd.Parameters[i] ).Value;
                        }
                    }

                    return ret;
                }
                finally
                {
                    CommandUtil.Close(cmd);
                }
            }
            catch ( Exception e )
            {
                throw new SQLRuntimeException(e);
            }
            finally
            {
                DataSourceUtil.CloseConnection(DataSource, conn);
            }
        }
    }
}