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
using System.Collections;
using System.Reflection;
using System.Text;
using Seasar.Framework.Log;

namespace Seasar.Dao.Context
{
    public abstract class AbstractCommandContext : ICommandContext 
    {
        private static readonly Logger logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        /// TODO CaseInsensitiveMapÇ∆å›ä∑ÇÃé¿ëïÇ≈Ç»Ç¢à◊ÉpÉâÉÅÅ[É^ÇÃçƒìoò^ÇÕNG
        Hashtable args = new Hashtable( new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer() );
        Hashtable argTypes = new Hashtable( new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer() );
        Hashtable argNames = new Hashtable( new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer() );

        private StringBuilder sqlBuf = new StringBuilder(100);
        private IList bindVariables = new ArrayList();
        private IList bindVariableTypes = new ArrayList();
        private IList bindVariableNames = new ArrayList();
        private bool enabled = true;
        private ICommandContext parent;

        public AbstractCommandContext() 
        {
        }

        public AbstractCommandContext(ICommandContext parent) 
        {
            this.parent = parent;
            this.enabled = false;
        }

        public virtual object GetArg(string name) 
        {
            if (this.args.ContainsKey(name)) 
            {
                return this.args[name];
            } 
            else if (this.parent != null) 
            {
                return this.parent.GetArg(name);
            } 
            else 
            {
                if (this.args.Count == 1) 
                {
                    return this.args[0];
                }
                logger.Log("WDAO0001", new object[] { name });
                return null;
            }
        }
    
        public virtual Type GetArgType(string name) 
        {
            if (this.argTypes.ContainsKey(name)) 
            {
                return (Type) this.argTypes[name];
            } 
            else if (this.parent != null) 
            {
                return this.parent.GetArgType(name);
            } 
            else 
            {
                if (this.argTypes.Count == 1) 
                {
                    return (Type) this.argTypes[0];
                }
                logger.Log("WDAO0001", new object[] { name });
                return null;
            }
        }

        public virtual void AddArg(string name, object arg, Type argType) 
        {
            if (this.args.ContainsKey(name))
            {
                this.args.Remove(name);
            }
            this.args.Add(name, arg);
            
            if (this.argTypes.ContainsKey(name))
            {
                this.argTypes.Remove(name);
            }
            this.argTypes.Add(name, argType);

            if (this.argNames.ContainsKey(name))
            {
                this.argNames.Remove(name);
            }
            this.argNames.Add(name, name);
        }

        public virtual string Sql
        {
            get { return this.sqlBuf.ToString(); }
        }

        public virtual object[] BindVariables
        {
            get
            {
                object[] variables = new object[this.bindVariables.Count];
                this.bindVariables.CopyTo(variables, 0);
                return variables;
            }
        }
    
        public virtual Type[] BindVariableTypes
        {
            get
            {
                Type[] variables = new Type[this.bindVariableTypes.Count];
                this.bindVariableTypes.CopyTo(variables, 0);
                return variables;
            }
        }

        public virtual string[] BindVariableNames 
        {
            get
            {
                string[] variableNames = new string[this.bindVariableNames.Count];
                this.bindVariableNames.CopyTo(variableNames, 0);
                return variableNames;
            }
        }

        public virtual ICommandContext AddSql(string sql) 
        {
            this.sqlBuf.Append(sql);
            return this;
        }

        public virtual ICommandContext AddSql(string sql, object bindVariable,
            Type bindVariableType, string bindVariableName) 
        {
        
            this.sqlBuf.Append(sql);
            this.bindVariables.Add(bindVariable);
            this.bindVariableTypes.Add(bindVariableType);
            this.bindVariableNames.Add(bindVariableName);
            return this;
        }

        public virtual ICommandContext AddSql(object bindVariable, Type bindVariableType, string bindVariableName)
        {
            throw new NotImplementedException();
        }

        public virtual ICommandContext AddSql(string sql, object[] bindVariables,
            Type[] bindVariableTypes, string[] bindVariableNames) 
        {
        
            this.sqlBuf.Append(sql);
            for (int i = 0; i < bindVariables.Length; ++i) 
            {
                this.bindVariables.Add(bindVariables[i]);
                this.bindVariableTypes.Add(bindVariableTypes[i]);
                this.bindVariableNames.Add(bindVariableNames[i]);
            }
            return this;
        }

        public virtual ICommandContext AppendSql(object bindVariable, Type bindVariableType, string bindVariableName)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }

        }
    }
}
