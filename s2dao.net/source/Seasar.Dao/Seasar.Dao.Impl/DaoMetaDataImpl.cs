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
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Seasar.Dao.Dbms;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Beans;
using Seasar.Framework.Util;
using Seasar.Framework.Log;

namespace Seasar.Dao.Impl
{
    public class DaoMetaDataImpl : IDaoMetaData
    {
        private static readonly Regex startWithOrderByReplacePattern = 
            new Regex(@"/\*.*?\*/");

        private static readonly Regex startWithBeginCommentPattern = 
            new Regex(@"/\*BEGIN\*/\s*WHERE .+", RegexOptions.IgnoreCase);

        private const string NOT_SINGLE_ROW_UPDATED = "NotSingleRowUpdated";

        protected string[] insertPrefixes = new string[]
            { "Insert", "Create", "Add" };

        protected string[] updatePrefixes = new string[]
            { "Update", "Modify", "Store" };

        protected string[] deletePrefixes = new string[]
            { "Delete", "Remove" };

        protected Type daoType;
        protected Type daoInterface;
        protected IDataSource dataSource;
        protected IAnnotationReaderFactory annotationReaderFactory;
        protected IDaoAnnotationReader annotationReader;
        protected ICommandFactory commandFactory;
        protected IDataReaderFactory dataReaderFactory;
        protected string sqlFileEncoding = Encoding.Default.WebName;
        protected IDbms dbms;
        protected Type beanType;
        protected IBeanMetaData beanMetaData;
        protected IDatabaseMetaData dbMetaData;
        protected Hashtable sqlCommands = new Hashtable();

        public DaoMetaDataImpl() 
        {
        }

        public DaoMetaDataImpl(Type daoType, IDataSource dataSource,ICommandFactory commandFactory,
            IDataReaderFactory dataReaderFactory, IDatabaseMetaData dbMetaData)
            : this(daoType, dataSource, commandFactory, dataReaderFactory, new FieldAnnotationReaderFactory(), dbMetaData)
        {
            DaoType = daoType;
            DataSource = dataSource;
            CommandFactory = commandFactory;
            DataReaderFactory = dataReaderFactory;
            DatabaseMetaData = dbMetaData;
            Initialize();
        }

        public DaoMetaDataImpl(Type daoType, IDataSource dataSource, ICommandFactory commandFactory,
            IDataReaderFactory dataReaderFactory, IAnnotationReaderFactory annotationReaderFactory, IDatabaseMetaData dbMetaData)
        {
            DaoType = daoType;
            DataSource = dataSource;
            CommandFactory = commandFactory;
            DataReaderFactory = dataReaderFactory;
            AnnotationReaderFactory = annotationReaderFactory;
            DatabaseMetaData = dbMetaData;
            Initialize();
        }

        public virtual void Initialize()
        {
            daoInterface = GetDaoInterface(daoType);
            annotationReader = AnnotationReaderFactory.CreateDaoAnnotationReader(daoType);
            beanType = annotationReader.GetBeanType();
            dbms = DbmsManager.GetDbms(dataSource);
            beanMetaData = new BeanMetaDataImpl(beanType, dbMetaData, dbms, annotationReaderFactory);
            SetupSqlCommand();
        }

        public IAnnotationReaderFactory AnnotationReaderFactory
        {
            get { return annotationReaderFactory; }
            set { annotationReaderFactory = value; }
        }

        protected virtual void SetupSqlCommand()
        {
            MethodInfo[] allMethods = daoInterface.GetMethods();
            Hashtable names = new Hashtable();
            foreach(MethodInfo mi in allMethods)
            {
                names[mi.Name] = mi;
            }
            IDictionaryEnumerator enu = names.GetEnumerator();
            while(enu.MoveNext())
            {
                try
                {
                    MethodInfo method = daoType.GetMethod((string) enu.Key);
                    if(method.IsAbstract) SetupMethod(method);
                }
                catch(AmbiguousMatchException) {}
            }
        }

        protected virtual string ReadText(string path, Assembly asm) 
        {
            using (Stream stream = ResourceUtil.GetResourceAsStream(path, asm)) 
            {
                using (TextReader reader = new StreamReader(stream, Encoding.GetEncoding(SqlFileEncoding))) 
                {
                    return reader.ReadToEnd();
                }
            }
        }

        protected virtual void SetupMethod(MethodInfo mi)
        {
            string sql = null;
            sql = annotationReader.GetSql(mi.Name, dbms);
            if(sql != null)
            {
                SetupMethodByManual(mi, sql);
                return;
            }
            if ( sql == null )
            {
                sql = annotationReader.GetProcedure(mi.Name);
                if ( sql != null )
                {
                    SetupProcedure(mi, sql);
                    return;
                }
            }
            string baseName = daoInterface.FullName + "_" + mi.Name;
            string dbmsPath = baseName + dbms.Suffix + ".sql";
            string standardPath = baseName + ".sql";
            Assembly asm = daoInterface.Assembly;
            
            if(ResourceUtil.IsExist(dbmsPath, asm))
            {
                sql = ReadText(dbmsPath, asm);
                SetupMethodByManual(mi, sql);
            }
            else if(ResourceUtil.IsExist(standardPath, asm))
            {
                sql = ReadText(standardPath, asm);
                SetupMethodByManual(mi, sql);
            }
            else
            {
                SetupMethodByAuto(mi);
            }
        }

        protected virtual void SetupMethodByManual(MethodInfo mi, string sql)
        {
            if(IsSelect(mi))
            {
                SetupSelectMethodByManual(mi, sql);
            }
            else
            {
                SetupUpdateMethodByManual(mi, sql);
            }
        }

        protected virtual void SetupMethodByAuto(MethodInfo mi)
        {
            if(IsInsert(mi.Name))
            {
                SetupInsertMethodByAuto(mi);
            }
            else if(IsUpdate(mi.Name))
            {
                SetupUpdateMethodByAuto(mi);
            }
            else if(IsDelete(mi.Name))
            {
                SetupDeleteMethodByAuto(mi);
            }
            else
            {
                SetupSelectMethodByAuto(mi);
            }
        }

        protected virtual void SetupSelectMethodByManual(MethodInfo mi, string sql)
        {
            SelectDynamicCommand cmd = CreateSelectDynamicCommand(CreateDataReaderHandler(mi));
            cmd.Sql = sql;
            cmd.ArgNames = MethodUtil.GetParameterNames(mi);
            cmd.ArgTypes = MethodUtil.GetParameterTypes(mi);
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual SelectDynamicCommand CreateSelectDynamicCommand(IDataReaderHandler drh)
        {
            return new SelectDynamicCommand(dataSource, commandFactory, drh, dataReaderFactory);
        }

        protected virtual SelectDynamicCommand CreateSelectDynamicCommand(
            IDataReaderHandler dataReaderHandler, string query)
        {
            SelectDynamicCommand cmd = CreateSelectDynamicCommand(dataReaderHandler);
            StringBuilder buf = new StringBuilder(255);
            if(StartsWithSelect(query)) 
            {
                buf.Append(query);
            }
            else
            {
                string sql = dbms.GetAutoSelectSql(BeanMetaData);
                buf.Append(sql);
                if(query != null)
                {
                    if(StartsWithOrderBy(query)) 
                    {
                        buf.Append(" ");
                    }
                    else if(StartsWithBeginComment(query)) 
                    {
                        buf.Append(" ");
                    }
                    else if(sql.LastIndexOf("WHERE") < 0) 
                    {
                        buf.Append(" WHERE ");
                    }
                    else 
                    {
                        buf.Append(" AND ");
                    }
                    buf.Append(query);
                }
            }
            cmd.Sql = buf.ToString();
            return cmd;
        }

        protected static bool StartsWithBeginComment(string query)
        {
            if(query != null) 
            {
                Match m = startWithBeginCommentPattern.Match(query);
                if(m.Success) 
                {
                    return true;
                }
            }
            return false;
        }

        protected static bool StartsWithSelect(string query)
        {
            return StringUtil.StartWith(query, "select");
        }

        protected static bool StartsWithOrderBy(string query)
        {
            if(query != null)
            {
                string replaceQuery = startWithOrderByReplacePattern.Replace(query, string.Empty);
                if(StringUtil.StartWith(replaceQuery.Trim(), "order by"))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual IDataReaderHandler CreateDataReaderHandler(MethodInfo mi)
        {
            return CreateDataReaderHandler(mi, beanMetaData);
        }

        protected virtual IDataReaderHandler CreateDataReaderHandler(MethodInfo mi, IBeanMetaData bmd)
        {
            Type retType = mi.ReturnType;

            if (retType.IsArray)
            {
                return CreateBeanArrayMetaDataDataReaderHandler(bmd);
            }
#if NET_1_1
            else if (typeof(IList).IsAssignableFrom(retType))
#else
            else if (!retType.IsGenericType && typeof(IList).IsAssignableFrom(retType))
#endif
            {
                return CreateBeanListMetaDataDataReaderHandler(bmd);
            }
            else if (IsBeanTypeAssignable(retType))
            {
                return CreateBeanMetaDataDataReaderHandler(bmd);
            }
            else if (Array.CreateInstance(
                beanType, 0).GetType().IsAssignableFrom(retType))
            {
                return CreateBeanArrayMetaDataDataReaderHandler(bmd);
            }
#if !NET_1_1
            else if(retType.IsGenericType
                && (retType.GetGenericTypeDefinition().Equals(
                    typeof(System.Collections.Generic.IList<>))
                || retType.GetGenericTypeDefinition().Equals(
                    typeof(System.Collections.Generic.List<>))))
            {
                return CreateBeanGenericListMetaDataDataReaderHandler(bmd);
            }
#endif
            else
            {
                return CreateObjectDataReaderHandler();
            }
        }

        protected virtual BeanListMetaDataDataReaderHandler CreateBeanListMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanListMetaDataDataReaderHandler(bmd);
        }

        protected virtual BeanMetaDataDataReaderHandler CreateBeanMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanMetaDataDataReaderHandler(bmd);
        }

        protected virtual BeanArrayMetaDataDataReaderHandler CreateBeanArrayMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanArrayMetaDataDataReaderHandler(bmd);
        }

#if !NET_1_1
        protected virtual BeanGenericListMetaDataDataReaderHandler CreateBeanGenericListMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanGenericListMetaDataDataReaderHandler(bmd);
        }
#endif
        protected virtual ObjectDataReaderHandler CreateObjectDataReaderHandler()
        {
            return new ObjectDataReaderHandler();
        }

        protected virtual bool IsBeanTypeAssignable(Type type)
        {
            return beanType.IsAssignableFrom(type) ||
                type.IsAssignableFrom(beanType);
        }

        protected virtual void SetupUpdateMethodByManual(MethodInfo mi, string sql)
        {
            UpdateDynamicCommand cmd = new UpdateDynamicCommand(dataSource, commandFactory);
            cmd.Sql = sql;
            string[] argNames = MethodUtil.GetParameterNames(mi);
            if(argNames.Length == 0 && IsUpdateSignatureForBean(mi))
                argNames = new string[] { StringUtil.Decapitalize(beanType.Name) };
            cmd.ArgNames = argNames;
            cmd.ArgTypes = MethodUtil.GetParameterTypes(mi);
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual bool IsUpdateSignatureForBean(MethodInfo mi)
        {
            Type[] paramTypes = MethodUtil.GetParameterTypes(mi);
            return paramTypes.Length == 1
                && IsBeanTypeAssignable(paramTypes[0]);
        }

        protected virtual void SetupInsertMethodByAuto(MethodInfo mi)
        {
            CheckAutoUpdateMethod(mi);
            string[] propertyNames = GetPersistentPropertyNames(mi.Name);
            ISqlCommand cmd = null;
            if(IsUpdateSignatureForBean(mi))
                cmd = new InsertAutoStaticCommand(dataSource, commandFactory,
                    beanMetaData, propertyNames);
            else
                throw new NotSupportedException("InsertBatchAutoStaticCommand");
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual void SetupUpdateMethodByAuto(MethodInfo mi)
        {
            CheckAutoUpdateMethod(mi);
            string[] propertyNames = GetPersistentPropertyNames(mi.Name);
            AbstractSqlCommand cmd = null;
            if(IsUpdateSignatureForBean(mi))
                cmd = new UpdateAutoStaticCommand(dataSource, commandFactory,
                    beanMetaData, propertyNames);
            else
                throw new NotSupportedException("UpdateBatchAutoStaticCommand");
            
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual void SetupDeleteMethodByAuto(MethodInfo mi)
        {
            CheckAutoUpdateMethod(mi);
            string[] propertyNames = GetPersistentPropertyNames(mi.Name);
            ISqlCommand cmd = null;
            if(IsUpdateSignatureForBean(mi))
                cmd = new DeleteAutoStaticCommand(dataSource, commandFactory,
                    beanMetaData, propertyNames);
            else
                throw new NotSupportedException("DeleteBatchAutoStaticCommand");
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual string[] GetPersistentPropertyNames(string methodName)
        {
            ArrayList names = new ArrayList();
            string[] props = annotationReader.GetNoPersistentProps(methodName);
            if(props != null)
            {
                for(int i = 0; i < beanMetaData.PropertyTypeSize; ++i)
                {
                    IPropertyType pt = beanMetaData.GetPropertyType(i);
                    if(pt.IsPersistent
                        && !IsPropertyExist(props, pt.PropertyName))
                        names.Add(pt.PropertyName);
                }
            }
            else
            {
                props = annotationReader.GetPersistentProps(methodName);
                if(props != null)
                {
                    
                    foreach(string prop in props) names.Add(prop);
                    for(int i = 0; i < beanMetaData.PrimaryKeySize; ++i)
                    {
                        string pk = beanMetaData.GetPrimaryKey(i);
                        IPropertyType pt = beanMetaData.GetPropertyTypeByColumnName(pk);
                        names.Add(pt.PropertyName);
                    }
                    if(beanMetaData.HasVersionNoPropertyType)
                        names.Add(beanMetaData.VersionNoPropertyName);
                    if(beanMetaData.HasTimestampPropertyType)
                        names.Add(beanMetaData.TimestampPropertyName);
                }
            }
            if(names.Count == 0)
            {
                for(int i = 0; i < beanMetaData.PropertyTypeSize; ++i)
                {
                    IPropertyType pt = beanMetaData.GetPropertyType(i);
                    if(pt.IsPersistent) names.Add(pt.PropertyName);
                }
            }
            return (string[]) names.ToArray(typeof(string));
        }

        protected virtual bool IsPropertyExist(string[] props, string propertyName)
        {
            foreach(string prop in props)
            {
                if(string.Compare(prop, propertyName, true) == 0)
                    return true;
            }
            return false;
        }

        protected virtual void SetupSelectMethodByAuto(MethodInfo mi)
        {
            string query = annotationReader.GetQuery(mi.Name);
            IDataReaderHandler handler = CreateDataReaderHandler(mi);
            SelectDynamicCommand cmd = null;
            string[] argNames = MethodUtil.GetParameterNames(mi);
            Type[] argTypes = MethodUtil.GetParameterTypes(mi);
            if(query != null && !StartsWithOrderBy(query))
            {
                cmd = CreateSelectDynamicCommand(handler, query);
            }
            else
            {
                cmd = CreateSelectDynamicCommand(handler);
                string sql = null;
                
                if(argTypes.Length == 1 
                    && ValueTypes.GetValueType(argTypes[0]) == ValueTypes.OBJECT)
                {
                    argNames = new string[] { "dto" };
                    sql = CreateAutoSelectSqlByDto(argTypes[0]);
                }
                else
                {
                    sql = CreateAutoSelectSql(argNames, argTypes);
                }
                if(query != null) sql += " " + query;
                cmd.Sql = sql;
            }
            cmd.ArgNames = argNames;
            cmd.ArgTypes = argTypes;
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual string CreateAutoSelectSqlByDto(Type dtoType)
        {
            string sql = dbms.GetAutoSelectSql(BeanMetaData);
            StringBuilder buf = new StringBuilder(sql);
            IDtoMetaData dmd = new DtoMetaDataImpl(dtoType, annotationReaderFactory.CreateBeanAnnotationReader(dtoType));
            bool began = false;
            if(!(sql.LastIndexOf("WHERE") > 0))
            {
                buf.Append("/*BEGIN*/ WHERE ");
                began = true;
            }
            for(int i = 0; i < dmd.PropertyTypeSize; ++i)
            {
                IPropertyType pt = dmd.GetPropertyType(i);
                string aliasName = pt.ColumnName;
                if(!beanMetaData.HasPropertyTypeByAliasName(aliasName))
                {
                    continue;
                }
                if(!beanMetaData.GetPropertyTypeByAliasName(aliasName).IsPersistent)
                {
                    continue;
                }
                string columnName = beanMetaData.ConvertFullColumnName(aliasName);
                string propertyName = "dto." + pt.PropertyName;

                // IFコメントを作成する
                CreateAutoIFComment(buf, columnName, propertyName, pt.PropertyType, i == 0, began);
            }
            if(began) buf.Append("/*END*/");
            return buf.ToString();
        }

        protected virtual string CreateAutoSelectSql(string[] argNames, Type[] argTypes)
        {
            string sql = dbms.GetAutoSelectSql(BeanMetaData);
            StringBuilder buf = new StringBuilder(sql);
            if(argNames.Length != 0)
            {
                bool began = false;
                if(!(sql.LastIndexOf("WHERE") > 0))
                {
                    buf.Append("/*BEGIN*/ WHERE ");
                    began = true;
                }
                for(int i = 0; i < argNames.Length; ++i)
                {
                    string columnName = beanMetaData.ConvertFullColumnName(argNames[i]);

                    // IFコメントを作成する
                    CreateAutoIFComment(buf, columnName, argNames[i], argTypes[i], i == 0, began);
                }
                if(began) buf.Append("/*END*/");
            }
            return buf.ToString();
        }

        /// <summary>
        /// IFコメントを作成する
        /// </summary>
        /// <param name="buf">SQLを格納する</param>
        /// <param name="columnName">テーブルの列名</param>
        /// <param name="propertyName">プロパティの名前</param>
        /// <param name="propertyType">プロパティの型</param>
        /// <param name="first">1つめのプロパティかどうか</param>
        /// <param name="began">BEGINコメントが開始されているかどうか</param>
        protected virtual void CreateAutoIFComment(StringBuilder buf, string columnName, 
            string propertyName, Type propertyType, bool first, bool began)
        {
            buf.Append("/*IF ");

            // 評価式を作成する
            CreateIFExpression(buf, propertyName, propertyType);

            buf.Append("*/");
            buf.Append(" ");
            if(!began || !first)
            {
                buf.Append("AND ");
            }
            buf.Append(columnName);
            buf.Append(" = /*");
            buf.Append(propertyName);
            buf.Append("*/null");
            buf.Append("/*END*/");
        }

        /// <summary>
        /// 評価式を作成する
        /// </summary>
        /// <param name="buf">SQL</param>
        /// <param name="propertyName">プロパティの名前</param>
        /// <param name="propertyType">プロパティの型</param>
        protected virtual void CreateIFExpression(StringBuilder buf, string propertyName, Type propertyType)
        {
            IValueType valueType = ValueTypes.GetValueType(propertyType);

            buf.Append(propertyName);

            if(valueType is NHibernateNullableBaseType)
            {
                buf.Append(".HasValue == true");
            }
#if !NET_1_1
            else if (valueType is NullableBaseType)
            {
                buf.Append(".HasValue == true");
            }
#endif
            else if (valueType is SqlBaseType)
            {
                buf.Append(".IsNull == false");
            }
            else
            {
                buf.Append(" != null");
            }
        }

        protected virtual void CheckAutoUpdateMethod(MethodInfo mi)
        {
            Type[] parameterTypes = MethodUtil.GetParameterTypes(mi);
            if(parameterTypes.Length != 1
                || !IsBeanTypeAssignable(parameterTypes[0])
                && !parameterTypes[0].IsAssignableFrom(typeof(IList))
                && !parameterTypes[0].IsArray)
            {
                throw new IllegalSignatureRuntimeException("EDAO0006", mi.ToString());
            }
        }

        protected virtual bool IsSelect(MethodInfo mi)
        {
            if(IsInsert(mi.Name)) return false;
            if(IsUpdate(mi.Name)) return false;
            if(IsDelete(mi.Name)) return false;
            return true;
        }

        protected virtual bool IsInsert(string methodName)
        {
            foreach(string insertName in insertPrefixes)
            {
                if(methodName.StartsWith(insertName)) return true;
            }
            return false;
        }

        protected virtual bool IsUpdate(string methodName)
        {
            foreach(string updateName in updatePrefixes)
            {
                if(methodName.StartsWith(updateName)) return true;
            }
            return false;
        }

        protected virtual bool IsDelete(string methodName)
        {
            foreach(string deleteName in deletePrefixes)
            {
                if(methodName.StartsWith(deleteName)) return true;
            }
            return false;
        }

        #region IDaoMetaData メンバ

        public Type BeanType
        {
            get
            {
                return beanType;
            }
        }

        public IBeanMetaData BeanMetaData
        {
            get
            {
                return beanMetaData;
            }
        }

        public bool HasSqlCommand(string methodName)
        {
            return sqlCommands.Contains(methodName);
        }

        public ISqlCommand GetSqlCommand(string methodName)
        {
            ISqlCommand cmd = (ISqlCommand) sqlCommands[methodName];
            if(cmd == null)
                throw new MethodNotFoundRuntimeException(daoType,
                    methodName, null);
            return cmd;
        }

        public ISqlCommand CreateFindCommand(string query)
        {
            return CreateSelectDynamicCommand(new BeanListMetaDataDataReaderHandler(
                beanMetaData), query);
        }

        public ISqlCommand CreateFindArrayCommand(string query)
        {
            return CreateSelectDynamicCommand(new BeanArrayMetaDataDataReaderHandler(
                beanMetaData), query);
        }

        public ISqlCommand CreateFindBeanCommand(string query)
        {
            return CreateSelectDynamicCommand(new BeanMetaDataDataReaderHandler(
                beanMetaData), query);
        }

        public ISqlCommand CreateFindObjectCommand(string query)
        {
            return CreateSelectDynamicCommand(new ObjectDataReaderHandler(), query);
        }

        #endregion

        public static Type GetDaoInterface(Type type)
        {
            if(type.IsInterface) 
            {
                return type;
            }
            throw new DaoNotFoundRuntimeException(type);
        }

        public IDbms Dbms
        {
            set { dbms = value; }
        }

        public IDataSource DataSource 
        {
            set { dataSource = value; }
        }

        public IDataReaderFactory DataReaderFactory
        {
            set { dataReaderFactory = value; }
        }

        public ICommandFactory CommandFactory
        {
            set { commandFactory = value; }
        }

        public string[] InsertPrefixes 
        {
            set { insertPrefixes = value; }
        }

        public string[] UpdatePrefixes 
        {
            set { updatePrefixes = value; }
        }

        public string[] DeletePrefixes 
        {
            set { deletePrefixes = value; }
        }

        public string SqlFileEncoding 
        {
            get { return sqlFileEncoding; }
            set { sqlFileEncoding = value; }
        }

        public Type DaoType
        {
            get { return daoType; }
            set { daoType = value; }
        }

        public IDatabaseMetaData DatabaseMetaData
        {
            set { dbMetaData = value; }
        }

        /// <summary>
        /// プロシージャの組み立て
        /// </summary>
        /// <param name="mi">メソッド情報</param>
        /// <param name="sql">ストアドプロシージャ名</param>
        protected void SetupProcedure( MethodInfo mi, string sql )
        {
            ProcedureDynamicCommand cmd = new ProcedureDynamicCommand(dataSource, commandFactory);
            cmd.Sql = sql;
            cmd.ArgNames = MethodUtil.GetParameterNames(mi);
            cmd.ArgTypes = MethodUtil.GetParameterTypes(mi);
            cmd.ArgDirections = AbstractProcedureHandler.GetParameterDirections(mi);
            cmd.ReturnType = mi.ReturnType;

            sqlCommands[mi.Name] = cmd;
        }

    }
}
