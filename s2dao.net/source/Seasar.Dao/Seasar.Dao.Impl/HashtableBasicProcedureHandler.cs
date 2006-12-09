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
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    /// <summary>
    /// �o�̓p�����[�^����������ProcedureHandler
    /// </summary>
    public class HashtableBasicProcedureHandler : AbstractProcedureHandler
    {
        private static readonly Logger logger = Logger.GetLogger(typeof(HashtableBasicProcedureHandler));
        
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="dataSource">�f�[�^�\�[�X��</param>
        /// <param name="procedureName">�v���V�[�W����</param>
        public HashtableBasicProcedureHandler(IDataSource dataSource, string procedureName)
            : base(dataSource, procedureName)
        {
            ;
        }

        /// <summary>
        /// ���s����
        /// </summary>
        /// <param name="args">����</param>
        /// <returns>�o�̓p�����[�^�l</returns>
        public Hashtable Execute(object[] args)
        {
            if ( DataSource == null ) throw new EmptyRuntimeException("dataSource");
            IDbConnection conn = DataSourceUtil.GetConnection(DataSource);

            Hashtable ret = new Hashtable();
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(ProcedureName);
                }
                
                IDbCommand cmd = null;
                try
                {
                    Type[] outParamTypes;
                    string[] outParamNames = _GetOutParamNames(conn, args, out outParamTypes);

                    cmd = GetCommand(conn, ProcedureName);

                    // �p�����[�^���Z�b�g����
                    for ( int j = 0; j < outParamNames.Length; j++ )
                    {
                        BindReturnValues(cmd, _AddParameterName(outParamNames[j], cmd), GetDbValueType(outParamTypes[j]));
                    }
                    BindParamters(cmd, args, ArgumentTypes, ArgumentNames, ArgumentDirection);
                    
                    CommandUtil.ExecuteNonQuery(DataSource, cmd);

                    // �Ԃ�l���擾����
                    for ( int j = 0; j < outParamNames.Length; j++ )
                    {
                        string paramName = _AddParameterName(outParamNames[j], cmd);
                        IDbDataParameter param = (IDbDataParameter) cmd.Parameters[paramName];
                        ret.Add(outParamNames[j], param.Value);
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

        private string[] _GetOutParamNames(IDbConnection conn, object[] args, out Type[] outParamTypes)
        {
            IDbCommand cmd = GetCommand(conn, ProcedureName);
            BindParamters(cmd, args, ArgumentTypes, ArgumentNames, ArgumentDirection);

            IDataReader reader = CommandUtil.ExecuteReader(DataSource, cmd);
            DataTable dt = reader.GetSchemaTable();
            int i = 0;
            string[] outParamNames = new string[dt.Rows.Count];
            outParamTypes = new Type[dt.Rows.Count];
            foreach (DataRow row in dt.Rows)
            {
#if DEBUG
                Console.Out.WriteLine("Out Param:" + row["ColumnName"]);
                Console.Out.WriteLine("Out Type:" + row["DataType"]);
                Console.Out.WriteLine("Out Direction:");
#endif
                outParamNames[i] = (string) row["ColumnName"];
                outParamTypes[i] = (Type) row["DataType"];
                i++;
            }
            reader.Close();

            return outParamNames;
        }

        private string _AddParameterName(string paramName, IDbCommand command)
        {
            BindVariableType vt = DataProviderUtil.GetBindVariableType(command);
            switch ( vt )
            {
                case BindVariableType.QuestionWithParam:
                    paramName = "?" + paramName;
                    break;
                case BindVariableType.ColonWithParam:
                    paramName = "" + paramName;
                    break;
                case BindVariableType.ColonWithParamToLower:
                    paramName = ":" + paramName.ToLower();
                    break;
                default:
                    paramName = "@" + paramName;
                    break;
            }

            return paramName;
        }
    }
}