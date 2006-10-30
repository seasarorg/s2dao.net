using System;
using System.Collections.Generic;
using System.Text;

namespace Seasar.Dao.Tests.Id
{
	public class HogeNullable
	{
        private Nullables.NullableDecimal id = -1;

        public HogeNullable()
        {
        }

        public Nullables.NullableDecimal Id
        {
            set { id = value; }
            get { return id; }
        }
	}
}
