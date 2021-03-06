﻿using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示访问条件和参数
    /// </summary>
    public interface ITransactionScope : IAsyncDisposable
    {
        /// <summary>
        ///     设置操作状态
        /// </summary>
        /// <param name="succeed">是否成功</param>
        bool SetState(bool succeed);
    }
}