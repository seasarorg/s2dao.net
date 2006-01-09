using System;
using Seasar.Dao.Context;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Context
{
	/// <summary>
	/// CommandContextPropertyAccessorTest の概要の説明です。
	/// </summary>
    [TestFixture]
	public class CommandContextPropertyAccessorTest
	{

        [Test]
        public void TestGetProperty()
        {
		    ICommandContext ctx = new SqlClientCommandContextImpl();//TODO:OleDbCommandContextImplもテスト
		    ctx.AddArg("aaa", "111", typeof(String));
		    OgnlRuntime.setPropertyAccessor(
			    typeof(CommandContext),
			    new CommandContextPropertyAccessor());
		    Assert.AreEqual("111", Ognl.getValue("aaa", ctx), "1");
		    String s = "ELSEhogeEND";
		    //System.out.println(s.substring(4, s.length() - 3));
        }
	}
}
