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
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{

    [Bean(typeof(FormUseHistory))]
    public interface IFormUseHistoryDao
    {

        /**
        * インサート 
        * @param formUseHistory WEB 画面利用履歴
        * @return 登録した数
        */
        int Insert(FormUseHistory formUseHistory);


        /**
        * エンティティ取得
        * @param webUserCode
        * @param webFormId
        * @return WEB 画面利用履歴
        */
        //static final String getEntity_ARGS = "W_USER_CD,W_FORM_ID";
        [Query("W_USER_CD=/*webUserCode*/ and W_FORM_ID=/*webFormId*/")]
        FormUseHistory GetEntity(String webUserCode,String webFormId);

        /**
        * リスト取得
        * @return WEB 画面利用履歴のリスト
        */
        IList GetList();

        //
        // 追加メソッド
        //
    }
}
