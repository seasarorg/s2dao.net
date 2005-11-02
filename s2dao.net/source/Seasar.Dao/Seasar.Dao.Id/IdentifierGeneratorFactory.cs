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
using Seasar.Dao.Attrs;
using Seasar.Framework.Util;

namespace Seasar.Dao.Id
{
    /// <summary>
    /// IdentifierGeneratorFactory ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
    /// </summary>
    public class IdentifierGeneratorFactory
    {
        private static Hashtable generatorTypes = new Hashtable();

        static IdentifierGeneratorFactory()
        {
            AddIdentifierGeneratorType("assigned", typeof(AssignedIdentifierGenerator));
            AddIdentifierGeneratorType("identity", typeof(IdentityIdentifierGenerator));
            AddIdentifierGeneratorType("sequence", typeof(SequenceIdentifierGenerator));
        }

        private IdentifierGeneratorFactory()
        {
        }

        public static void AddIdentifierGeneratorType(string name, Type type)
        {
            generatorTypes[name] = type;
        }

        public static IIdentifierGenerator CreateIdentifierGenerator(
            string propertyName, IDbms dbms)
        {
            return CreateIdentifierGenerator(propertyName, dbms, null);
        }

        public static IIdentifierGenerator CreateIdentifierGenerator(
            string propertyName, IDbms dbms, IDAttribute idAttr)
        {
            if(idAttr == null) 
                return new AssignedIdentifierGenerator(propertyName, dbms);
            Type type = GetGeneratorType(idAttr.ID);
            IIdentifierGenerator generator = CreateIdentifierGenerator(type, propertyName, dbms);
            if(idAttr.SequenceName != null)
                SetProperty(generator, "SequenceName", idAttr.SequenceName);
            return generator;
        }

        protected static Type GetGeneratorType(string name)
        {
            Type type = (Type) generatorTypes[name];
            if(type != null) return type;
            return ClassUtil.ForName(name, AppDomain.CurrentDomain.GetAssemblies());
        }

        protected static IIdentifierGenerator CreateIdentifierGenerator(
            Type type, string propertyName, IDbms dbms)
        {
            ConstructorInfo constructor = 
                ClassUtil.GetConstructorInfo(type, new Type[] { typeof(string), typeof(IDbms) });
            return (IIdentifierGenerator) 
                ConstructorUtil.NewInstance(constructor, new object[] { propertyName, dbms });
        }

        protected static void SetProperty(IIdentifierGenerator generator, string propertyName, string value)
        {
            PropertyInfo property = generator.GetType().GetProperty(propertyName);
            property.SetValue(generator, value, null);
        }
    }
}
