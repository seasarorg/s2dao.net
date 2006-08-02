using Seasar.Dao.Context;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Context
{
    [TestFixture]
	public class CommandContextPropertyAccessorTest
	{
        [Test]
        public void TestGetProperty()
        {
		    ICommandContext ctx = new CommandContextImpl();
            ctx.AddArg("aaa", "111", typeof(string));
            Assert.AreEqual("111", ctx.GetArg("aaa"), "1");
        }
	}
}
