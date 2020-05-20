﻿using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC;
using ZeroTeam.MessageMVC.AddIn;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///   组件注册
    /// </summary>
    //[Export(typeof(IAutoRegister))]
    //[ExportMetadata("Symbol", '%')]
    public sealed class AutoRegister : IAutoRegister
    {
        /// <summary>
        /// 注册
        /// </summary>
        Task<bool> IAutoRegister.AutoRegist(IServiceCollection services)
        {
            services.AddSingleton<IFlowMiddleware, MySqlConnectionsManager>();
            services.AddSingleton<IHealthCheck, MySqlConnectionsManager>();
            return Task.FromResult(false);
        }

    }

}