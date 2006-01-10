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

namespace Seasar.Dao.Node
{
    public class BindVariableNode : AbstractNode
    {
        private string expression;
        private string baseName;
        private string propertyName;

        public BindVariableNode(string expression)
        {
            this.expression = expression;
            string[] array = expression.Split('.');
            baseName = array[0];
            if (array.Length > 1) propertyName = array[1];
        }

        public string Expression
        {
            get { return expression; }
        }

        public override void Accept(ICommandContext ctx)
        {
            object value = ctx.GetArg(baseName);
            Type type = ctx.GetArgType(baseName);
            if(propertyName != null)
            {
                PropertyInfo pi = type.GetProperty(propertyName);
                value = pi.GetValue(value, null);
                type = pi.PropertyType;
            }
            ctx.AddSql(value, type, propertyName);
        }

    }
}
