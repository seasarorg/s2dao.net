using System;

namespace Seasar.Dao.Tests.Interceptors
{
	/// <summary>
	/// Hoge の概要の説明です。
	/// </summary>
	public class Hoge
	{
        private string _val;

		public Hoge()
		{
		}

        public string Val
        {
            set { _val = value; }
            get { return _val; }
        }
	}
}
