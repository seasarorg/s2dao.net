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
using Seasar.Framework.Exceptions;

namespace Seasar.Dao
{
    public class DaoNotFoundRuntimeException : SRuntimeException
    {
        private readonly Type targetType;

        public DaoNotFoundRuntimeException(Type targetType)
            : base("EDAO0008", new object[] { targetType.Name })
        {
            this.targetType = targetType;
        }

        public Type TargetType
        {
            get { return targetType; }
        }
    }

    public class EndCommentNotFoundRuntimeException : SRuntimeException
    {
        public EndCommentNotFoundRuntimeException()
            : base("EDAO0007")
        {
        }
    }

    public class IfConditionNotFoundRuntimeException : SRuntimeException
    {
        public IfConditionNotFoundRuntimeException()
            : base("EDAO0004")
        {
        }
    }

    public class IllegalBoolExpressionRuntimeException : SRuntimeException
    {
        private readonly string expression;

        public IllegalBoolExpressionRuntimeException(string expression)
            : base("EDAO0003", new object[] { expression })
        {
            this.expression = expression;
        }

        public string Expression
        {
            get { return expression; }
        }
    }

    public class IllegalSignatureRuntimeException : SRuntimeException
    {
        private readonly string signature;

        public IllegalSignatureRuntimeException(string messageCode, string signature)
            : base(messageCode, new object[] { signature })
        {
            this.signature = signature;
        }

        public string Signature
        {
            get { return signature; }
        }
    }

    public class UpdateFailureRuntimeException : SRuntimeException
    {
        private readonly object bean;
        private readonly int rows;

        public UpdateFailureRuntimeException(object bean, int rows)
            : base("EDAO0005", new object[] { bean.ToString(), rows.ToString() })
        {
            this.bean = bean;
            this.rows = rows;
        }

        public object Bean
        {
            get { return bean; }
        }

        public int Rows
        {
            get { return rows; }
        }
    }

    public class NotSingleRowUpdatedRuntimeException : UpdateFailureRuntimeException
    {
        public NotSingleRowUpdatedRuntimeException(object bean, int rows)
            : base(bean, rows)
        {
        }
    }

    public class PrimaryKeyNotFoundRuntimeException : SRuntimeException
    {
        private readonly Type targetType;

        public PrimaryKeyNotFoundRuntimeException(Type targetType)
            : base("EDAO0009", new object[] { targetType.Name })
        {
            this.targetType = targetType;
        }

        public Type TargetType
        {
            get { return targetType; }
        }
    }

    public class TokenNotClosedRuntimeException : SRuntimeException
    {
        private readonly string token;
        private readonly string sql;

        public TokenNotClosedRuntimeException(string token, string sql)
            : base("EDAO0002", new object[] { token, sql })
        {
            this.token = token;
            this.sql = sql;
        }

        public string Token
        {
            get { return token; }
        }

        public string Sql
        {
            get { return sql; }
        }
    }

    public class WrongPropertyTypeOfTimestampException : SRuntimeException
    {
        private readonly string propertyName;
        private readonly string propertyType;

        public WrongPropertyTypeOfTimestampException(string propertyName, string propertyType)
            : base("EDAO0010", new object[] { propertyName, propertyType })
        {
            this.propertyName = propertyName;
            this.propertyType = propertyType;
        }

        public string PropertyName
        {
            get { return propertyName; }
        }

        public string PropertyType
        {
            get { return propertyType; }
        }
    }
}
