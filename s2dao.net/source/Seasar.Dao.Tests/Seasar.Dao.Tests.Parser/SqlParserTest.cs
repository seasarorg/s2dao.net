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
using Seasar.Dao;
using Seasar.Dao.Parser;
using Seasar.Dao.Node;
using Seasar.Dao.Context;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Parser
{
    /// <summary>
    /// SqlParserTest の概要の説明です。
    /// </summary>
    [TestFixture]
    public class SqlParserTest
    {

        public SqlParserTest() {
        }

        [Test]
        public void TestParse() {
	        String sql = "SELECT * FROM emp";
	        ISqlParser parser = new SqlParserImpl(sql);
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        INode node = parser.Parse();
	        node.Accept(ctx);
	        Assert.AreEqual(sql, ctx.Sql,"1");
        }

        [Test]
        public void TestParseEndSemicolon() {
	        parseEndSemicolon(";");
	        parseEndSemicolon(";\t");
	        parseEndSemicolon("; ");
        }

        //黄色になってしまうのでメソッド名変更
        private void parseEndSemicolon(String endChar) {
	        String sql = "SELECT * FROM emp";
	        ISqlParser parser = new SqlParserImpl(sql + endChar);
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        INode node = parser.Parse();
	        node.Accept(ctx);
	        Assert.AreEqual(sql, ctx.Sql,"1");
        }
        	
        [Test, ExpectedException(typeof(TokenNotClosedRuntimeException))]
        public void TestCommentEndNotFound() 
            {
	        String sql = "SELECT * FROM emp/*hoge";
	        ISqlParser parser = new SqlParserImpl(sql);
		    parser.Parse();
		    Assert.Fail("1");
        }

        [Test]
        public void TestParseBindVariable() {
	        String sql = "SELECT * FROM emp WHERE job = /*job*/'CLERK' AND deptno = /*deptno*/20";
            //java版とは実装を変えています
            //String sql2 = "SELECT * FROM emp WHERE job = ? AND deptno = ?";
            String sql2 = "SELECT * FROM emp WHERE job = @job AND deptno = @deptno";
            String sql3 = "SELECT * FROM emp WHERE job = ";
	        String sql4 = " AND deptno = ";
	        ISqlParser parser = new SqlParserImpl(sql);
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        String job = "CLERK";
	        Int32 deptno = 20;
	        ctx.AddArg("job", job, job.GetType());
	        ctx.AddArg("deptno", deptno, deptno.GetType());
	        INode root = parser.Parse();
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual(sql2, ctx.Sql,"1");
	        Object[] vars = ctx.BindVariables;
	        Assert.AreEqual( 2, vars.Length,"2");
	        Assert.AreEqual( job, vars[0],"3");
	        Assert.AreEqual( deptno, vars[1],"4");
	        Assert.AreEqual( 4, root.ChildSize,"5");
	        SqlNode sqlNode = (SqlNode) root.GetChild(0);
	        Assert.AreEqual( sql3, sqlNode.Sql,"6");
	        BindVariableNode varNode = (BindVariableNode) root.GetChild(1);
	        Assert.AreEqual( "job", varNode.Expression,"7");
	        SqlNode sqlNode2 = (SqlNode) root.GetChild(2);
	        Assert.AreEqual( sql4, sqlNode2.Sql,"8");
	        BindVariableNode varNode2 = (BindVariableNode) root.GetChild(3);
	        Assert.AreEqual( "deptno", varNode2.Expression,"9");
        }

        [Test]
        public void TestParseBindVariable2() {
	        String sql = "SELECT * FROM emp WHERE job = /* job*/'CLERK'";
	        String sql2 = "SELECT * FROM emp WHERE job = 'CLERK'";
	        String sql3 = "SELECT * FROM emp WHERE job = ";
	        String sql4 = "'CLERK'";
	        ISqlParser parser = new SqlParserImpl(sql);
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        INode root = parser.Parse();
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual(sql2, ctx.Sql,"1");
	        Assert.AreEqual(2,root.ChildSize,"2");
	        SqlNode sqlNode = (SqlNode) root.GetChild(0);
	        Assert.AreEqual(sql3, sqlNode.Sql,"3");
	        SqlNode sqlNode2 = (SqlNode) root.GetChild(1);
	        Assert.AreEqual(sql4, sqlNode2.Sql,"4");
        }

        [Test]
        public void TestParseWhiteSpace() {
	        String sql = "SELECT * FROM emp WHERE empno = /*empno*/1 AND 1 = 1";
	        //String sql2 = "SELECT * FROM emp WHERE empno = ? AND 1 = 1";
			String sql2 = "SELECT * FROM emp WHERE empno = @empno AND 1 = 1";
	        String sql3 = " AND 1 = 1";
            ISqlParser parser = new SqlParserImpl(sql);
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        Int32 empno = 7788;
	        ctx.AddArg("empno", empno, empno.GetType());
	        INode root = parser.Parse();
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual(sql2, ctx.Sql,"1");
	        SqlNode sqlNode = (SqlNode) root.GetChild(2);
	        Assert.AreEqual(sql3, sqlNode.Sql,"2");
        }

        [Test]
        public void TestParseIf() {
	        String sql = "SELECT * FROM emp/*IF job != null*/ WHERE job = /*job*/'CLERK'/*END*/";
	        //String sql2 = "SELECT * FROM emp WHERE job = ?";
			String sql2 = "SELECT * FROM emp WHERE job = @job";
	        String sql3 = "SELECT * FROM emp";
	        ISqlParser parser = new SqlParserImpl(sql);
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        String job = "CLERK";
	        ctx.AddArg("job", job, job.GetType());
            INode root = parser.Parse();
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual(sql2, ctx.Sql,"1");
	        Object[] vars = ctx.BindVariables;
	        Assert.AreEqual(1, vars.Length,"2");
	        Assert.AreEqual(job, vars[0],"3");
	        Assert.AreEqual(2, root.ChildSize,"4");
	        SqlNode sqlNode = (SqlNode) root.GetChild(0);
	        Assert.AreEqual(sql3, sqlNode.Sql,"5");
	        IfNode ifNode = (IfNode) root.GetChild(1);
	        Assert.AreEqual("self.GetArg('job') != null", ifNode.Expression,"6");
	        Assert.AreEqual(2, ifNode.ChildSize,"7");
	        SqlNode sqlNode2 = (SqlNode) ifNode.GetChild(0);
	        Assert.AreEqual(" WHERE job = ", sqlNode2.Sql,"8");
	        BindVariableNode varNode = (BindVariableNode) ifNode.GetChild(1);
	        Assert.AreEqual("job", varNode.Expression,"9");
	        ICommandContext ctx2 = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        root.Accept(ctx2);
	        //System.out.println(ctx2.Sql);
	        Assert.AreEqual(sql3, ctx2.Sql,"10");
        }

        [Test]
        public void TestParseIf2() {
	        String sql = "/*IF aaa != null*/aaa/*IF bbb != null*/bbb/*END*//*END*/";
	        ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
            INode root = parser.Parse();
	        root.Accept(ctx);
	        //System.out.println("[" + ctx.Sql + "]");
	        Assert.AreEqual("", ctx.Sql,"1");
	        ctx.AddArg("aaa", null, typeof(String));
	        ctx.AddArg("bbb", "hoge", typeof(String));
	        root.Accept(ctx);
	        //System.out.println("[" + ctx.Sql + "]");
	        Assert.AreEqual("", ctx.Sql,"2");
	        ctx.AddArg("aaa", "hoge", typeof(String));
	        root.Accept(ctx);
	        //System.out.println("[" + ctx.Sql + "]");
	        Assert.AreEqual("aaabbb", ctx.Sql,"3");
	        ICommandContext ctx2 = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        ctx2.AddArg("aaa", "hoge", typeof(String));
	        ctx2.AddArg("bbb", null, typeof(String));
	        root.Accept(ctx2);
	        //System.out.println("[" + ctx2.Sql + "]");
	        Assert.AreEqual("aaa", ctx2.Sql,"4");
        }

        [Test]
        public void TestParseElse() {
	        String sql = "SELECT * FROM emp WHERE /*IF job != null*/job = /*job*/'CLERK'-- ELSE job is null/*END*/";
	        string sql2 = "SELECT * FROM emp WHERE job = @job";
	        String sql3 = "SELECT * FROM emp WHERE job is null";
            ISqlParser parser = new SqlParserImpl(sql);
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        String job = "CLERK";
	        ctx.AddArg("job", job, job.GetType());
	        INode root = parser.Parse();
	        root.Accept(ctx);
	        //System.out.println("[" + ctx.Sql + "]");
	        Assert.AreEqual(sql2, ctx.Sql,"1");
	        Object[] vars = ctx.BindVariables;
	        Assert.AreEqual(1, vars.Length,"2");
	        Assert.AreEqual(job, vars[0],"3");
	        ICommandContext ctx2 = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        root.Accept(ctx2);
	        //System.out.println("[" + ctx2.Sql + "]");
	        Assert.AreEqual(sql3, ctx2.Sql,"4");
        }

        [Test]
        public void TestParseElse2() {
	        String sql = "/*IF false*/aaa--ELSE bbb = /*bbb*/123/*END*/";
	        ISqlParser parser = new SqlParserImpl(sql);
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        Int32 bbb = 123;
	        ctx.AddArg("bbb", bbb, bbb.GetType());
            INode root = parser.Parse();
	        root.Accept(ctx);
	        //System.out.println("[" + ctx.Sql + "]");
	        Assert.AreEqual("bbb = @bbb", ctx.Sql,"1");
	        Object[] vars = ctx.BindVariables;
	        Assert.AreEqual(1, vars.Length,"2");
	        Assert.AreEqual(bbb, vars[0],"3");
        }

        [Test]
        public void TestParseElse3() {
	        String sql = "/*IF false*/aaa--ELSE bbb/*IF false*/ccc--ELSE ddd/*END*//*END*/";
	        ISqlParser parser = new SqlParserImpl(sql);
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
            INode root = parser.Parse();
	        root.Accept(ctx);
	        //System.out.println("[" + ctx.Sql + "]");
	        Assert.AreEqual("bbbddd", ctx.Sql,"1");
        }

		[Test]
        public void TestElse4() {
	        String sql = "SELECT * FROM emp/*BEGIN*/ WHERE /*IF false*/aaa-- ELSE AND deptno = 10/*END*//*END*/";
	        String sql2 = "SELECT * FROM emp WHERE deptno = 10";
	        ISqlParser parser = new SqlParserImpl(sql);
	        INode root = parser.Parse();
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual(sql2, ctx.Sql,"1");
        }

        [Test]
        public void TestBegin() {
	        String sql = "SELECT * FROM emp/*BEGIN*/ WHERE /*IF job != null*/job = /*job*/'CLERK'/*END*//*IF deptno != null*/ AND deptno = /*deptno*/20/*END*//*END*/";
	        String sql2 = "SELECT * FROM emp";
	        String sql3 = "SELECT * FROM emp WHERE job = @job";
	        String sql4 = "SELECT * FROM emp WHERE job = @job AND deptno = @deptno";
	        String sql5 = "SELECT * FROM emp WHERE deptno = @deptno";
	        ISqlParser parser = new SqlParserImpl(sql);
	        INode root = parser.Parse();
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
            Assert.AreEqual(sql2, ctx.Sql,"1");
        	
	        ICommandContext ctx2 = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        ctx2.AddArg("job", "CLERK", typeof(String));
	        ctx2.AddArg("deptno", null, typeof(Int32));
	        root.Accept(ctx2);
	        //System.out.println(ctx2.Sql);
	        Assert.AreEqual(sql3, ctx2.Sql,"2");
        	
	        ICommandContext ctx3 = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        ctx3.AddArg("job", "CLERK", typeof(String));
	        ctx3.AddArg("deptno", 20, typeof(Int32));
	        root.Accept(ctx3);
	        //System.out.println(ctx3.Sql);
	        Assert.AreEqual(sql4, ctx3.Sql,"3");
        	
	        ICommandContext ctx4 = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        ctx4.AddArg("deptno", 20, typeof(Int32));
	        ctx4.AddArg("job", null, typeof(String));
	        root.Accept(ctx4);
	        //System.out.println(ctx4.Sql);
	        Assert.AreEqual(sql5, ctx4.Sql,"4");
        }

        [Test]
		public void TestBeginAnd() {
	        String sql = "/*BEGIN*/WHERE /*IF true*/aaa BETWEEN /*bbb*/111 AND /*ccc*/123/*END*//*END*/";
	        String sql2 = "WHERE aaa BETWEEN @bbb AND @ccc";
	        ISqlParser parser = new SqlParserImpl(sql);
	        INode root = parser.Parse();
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        ctx.AddArg("bbb", "111", typeof(String));
	        ctx.AddArg("ccc", "222", typeof(String));
	        root.Accept(ctx);
	        //System.out.println("[" + ctx.Sql + "]");
	        Assert.AreEqual(sql2, ctx.Sql,"1");
        }

        [Test]
        public void TestIn() {
	        String sql = "SELECT * FROM emp WHERE deptno IN /*deptnoList*/(10, 20) ORDER BY ename";
            //java版とは実装を変えています
            //String sql2 = "SELECT * FROM emp WHERE deptno IN (?, ?) ORDER BY ename";
            String sql2 = "SELECT * FROM emp WHERE deptno IN (@deptnoList1, @deptnoList2) ORDER BY ename";
            String sql3 = "SELECT * FROM emp WHERE deptno IN (@deptnoList1, @deptnoList2, @deptnoList3, @deptnoList4, @deptnoList5, @deptnoList6, @deptnoList7, @deptnoList8, @deptnoList9, @deptnoList10) ORDER BY ename";
            ISqlParser parser = new SqlParserImpl(sql);
	        INode root = parser.Parse();
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        IList deptnoList = new ArrayList();
	        deptnoList.Add(10);
	        deptnoList.Add(20);
	        ctx.AddArg("deptnoList", deptnoList, typeof(IList));
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
            Assert.AreEqual(sql2, ctx.Sql,"1");
            Object[] vars = ctx.BindVariables;
	        Assert.AreEqual(2, vars.Length,"2");
            Assert.AreEqual(10, vars[0],"3");
	        Assert.AreEqual(20, vars[1],"4");
            //追加テスト
            ctx = new CommandContextImpl();
            deptnoList = new ArrayList();
            deptnoList.Add(10);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(100);
            ctx.AddArg("deptnoList", deptnoList, typeof(IList));
            root.Accept(ctx);
            Assert.AreEqual(sql3, ctx.Sql,"5");
            vars = ctx.BindVariables;
            Assert.AreEqual(10, vars.Length,"6");
            Assert.AreEqual(10, vars[0],"7");
            Assert.AreEqual(100, vars[9],"8");

        }

        [Test]
        public void TestIn2() {
	        String sql = "SELECT * FROM emp WHERE deptno IN /*deptnoList*/(10, 20) ORDER BY ename";
            //java版とは実装を変えています
            //String sql2 = "SELECT * FROM emp WHERE deptno IN (?, ?) ORDER BY ename";
            String sql2 = "SELECT * FROM emp WHERE deptno IN (@deptnoList1, @deptnoList2) ORDER BY ename";
            ISqlParser parser = new SqlParserImpl(sql);
	        INode root = parser.Parse();
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        int[] deptnoArray = {10, 20};
	        ctx.AddArg("deptnoList", deptnoArray, deptnoArray.GetType());
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual(sql2, ctx.Sql,"1");
            Object[] vars = ctx.BindVariables;
	        Assert.AreEqual( 2, vars.Length,"2");
	        Assert.AreEqual( 10, vars[0],"3");
	        Assert.AreEqual( 20, vars[1],"4");
        }

        [Test]
        public void TestIn3() {
	        String sql = "SELECT * FROM emp WHERE ename IN /*enames*/('SCOTT','MARY') AND job IN /*jobs*/('ANALYST', 'FREE')";
            //java版とは実装を変えています
            //String sql2 = "SELECT * FROM emp WHERE ename IN (?, ?) AND job IN (?, ?)";
            String sql2 = "SELECT * FROM emp WHERE ename IN (@enames1, @enames2) AND job IN (@jobs1, @jobs2)";
            ISqlParser parser = new SqlParserImpl(sql);
	        INode root = parser.Parse();
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        String[] enames = {"SCOTT", "MARY"};
	        String[] jobs = {"ANALYST", "FREE"};
	        ctx.AddArg("enames", enames, enames.GetType());
	        ctx.AddArg("jobs", jobs, jobs.GetType());
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual(sql2, ctx.Sql,"1");
            Object[] vars = ctx.BindVariables;
	        Assert.AreEqual(4, vars.Length,"2");
	        Assert.AreEqual("SCOTT", vars[0],"3");
	        Assert.AreEqual("MARY", vars[1],"4");
	        Assert.AreEqual("ANALYST", vars[2],"5");
	        Assert.AreEqual("FREE", vars[3],"6");
        }

        [Test]
        public void TestParseBindVariable3() {
            //java版とは実装を変えています
            //String sql = "BETWEEN sal ? AND ?";
			string sql = "BETWEEN sal @p AND @p";
			string sql2 = "BETWEEN sal @$1p AND @$2p";

            ISqlParser parser = new SqlParserImpl(sql);
	        INode root = parser.Parse();
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        ctx.AddArg("$1", 0, typeof(Int32));
	        ctx.AddArg("$2", 1000, typeof(Int32));
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual(sql2, ctx.Sql, "1");
	        Object[] vars = ctx.BindVariables;
	        Assert.AreEqual(2, vars.Length,"2");
	        Assert.AreEqual(0, vars[0],"3");
	        Assert.AreEqual(1000, vars[1],"4");
        }

        [Test, ExpectedException(typeof(EndCommentNotFoundRuntimeException))]
        public void TestEndNotFound() 
            {
	        String sql = "/*BEGIN*/";
	        ISqlParser parser = new SqlParserImpl(sql);
		    parser.Parse();
		    Assert.Fail("1");
        }

        [Test]
        public void TestEndParent() {
	        String sql = "INSERT INTO ITEM (ID, NUM) VALUES (/*id*/1, /*num*/20)";
	        ISqlParser parser = new SqlParserImpl(sql);
	        INode root = parser.Parse();
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        ctx.AddArg("id", 0, typeof(Int32));
	        ctx.AddArg("num", 1, typeof(Int32));
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual(true, ctx.Sql.EndsWith(")"),"1");
        }

        [Test]
        public void TestEmbeddedValue() {
	        String sql = "/*$aaa*/";
	        ISqlParser parser = new SqlParserImpl(sql);
	        INode root = parser.Parse();
	        ICommandContext ctx = new CommandContextImpl();//TODO:OleDbCommandContextImplもテスト
	        ctx.AddArg("aaa", 0, typeof(Int32));
	        root.Accept(ctx);
	        //System.out.println(ctx.Sql);
	        Assert.AreEqual("0", ctx.Sql,"1");
        }
    }
}
