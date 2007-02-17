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
using System.Reflection;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Impl
{
    public class FieldAnnotationReader : IDaoAnnotationReader
    {
        protected Type daoBeanType;

        public FieldAnnotationReader(Type daoBeanType)
        {
            this.daoBeanType = daoBeanType;
        }

        #region IDaoAnnotationReader �����o

        public string GetQuery(string name)
        {
            MethodInfo mi = daoBeanType.GetMethod(name);
            QueryAttribute queryAttr = AttributeUtil.GetQueryAttribute(mi);
            if(queryAttr != null)
            {
                return queryAttr.Query;
            }
            else
            {
                return null;
            }
        }

        public Type GetBeanType()
        {
            BeanAttribute beanAttr = AttributeUtil.GetBeanAttribute(daoBeanType);
            return beanAttr.BeanType;
        }

        public string[] GetNoPersistentProps(string methodName)
        {
            MethodInfo mi = daoBeanType.GetMethod(methodName);
            NoPersistentPropsAttribute nppAttr = AttributeUtil.GetNoPersistentPropsAttribute(mi);
            if(nppAttr != null)
            {
                return nppAttr.Props;
            }
            else
            {
                return null;
            }
        }

        public string[] GetPersistentProps(string methodName)
        {
            MethodInfo mi = daoBeanType.GetMethod(methodName);
            PersistentPropsAttribute ppAttr = AttributeUtil.GetPersistentPropsAttribute(mi);
            if(ppAttr != null)
            {
                return ppAttr.Props;
            }
            else
            {
                return null;
            }
        }

        public string GetSql(string name, IDbms dbms)
        {
            MethodInfo mi = daoBeanType.GetMethod(name);
            SqlAttribute[] sqlAttrs = AttributeUtil.GetSqlAttributes(mi);
            SqlAttribute defaultSqlAttr = null;
            foreach(SqlAttribute sqlAttr in sqlAttrs)
            {
                if(sqlAttr.Dbms == dbms.Dbms)
                    return sqlAttr.Sql;
                if(sqlAttr.Dbms == KindOfDbms.None)
                    defaultSqlAttr = sqlAttr;
            }
            
            return defaultSqlAttr == null ? null : defaultSqlAttr.Sql;
        }

        /// <summary>
        /// �v���V�[�W�������擾����
        /// </summary>
        /// <param name="name">���\�b�h��</param>
        /// <returns>�v���V�[�W����</returns>
        public string GetProcedure(string name)
        {
            MethodInfo mi = daoBeanType.GetMethod(name);
            ProcedureAttribute procedureAttribute = AttributeUtil.GetProcedureAttribute(mi);
            if ( procedureAttribute != null )
            {
                return procedureAttribute.ProcedureName;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
