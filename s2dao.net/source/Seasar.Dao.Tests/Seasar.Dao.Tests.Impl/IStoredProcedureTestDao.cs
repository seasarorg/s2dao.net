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

using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
    [Bean(typeof(Employee))]
    public interface IStoredProcedureTestDao
    {
        [Procedure("sales_tax")]
        void GetSalesTax(double sales, out double tax);

        [Procedure("sales_tax2")]
        double GetSalesTax2(double sales);

        [Procedure("sales_tax3")]
        void GetSalesTax3(ref double sales);

        [Procedure("sales_tax4")]
        void GetSalesTax4(double sales, out double tax, out double total);
    }
}
