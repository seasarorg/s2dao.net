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
using System.Reflection;
using Seasar.Framework.Aop.Interceptors;

namespace Seasar.Dao.Interceptors
{
    public class S2DaoInterceptor : AbstractInterceptor
    {
        private IDaoMetaDataFactory daoMetaDataFactory;

        public S2DaoInterceptor(IDaoMetaDataFactory daoMetaDataFactory)
        {
            this.daoMetaDataFactory = daoMetaDataFactory;
        }

        public override object Invoke(Seasar.Framework.Aop.IMethodInvocation invocation)
        {
            MethodBase method = invocation.Method;
            if(!method.IsAbstract) return invocation.Proceed();
            Type targetType = GetComponentDef(invocation).ComponentType;
            IDaoMetaData dmd = daoMetaDataFactory.GetDaoMetaData(targetType);
            ISqlCommand cmd = dmd.GetSqlCommand(method.Name);
            object ret = cmd.Execute(invocation.Arguments);
            
            Type retType = ((MethodInfo) method).ReturnType;
            if(typeof(IConvertible).IsAssignableFrom(retType))
            {
                if(ret == null) 
                {
                    if(!retType.Equals(typeof(string))) 
                        ret = Convert.ChangeType(decimal.Zero, retType);
                }
                else
                {
                    ret = Convert.ChangeType(ret, retType);
                }
            }
            return ret;
        }

    }
}
