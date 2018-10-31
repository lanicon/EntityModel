﻿using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.Extends;

namespace Agebull.Common.DataModel
{

    /// <summary>
    /// 版本数据更新触发器
    /// </summary>
    public class RedisDataTrigger : IDataTrigger
    {
        void IDataUpdateTrigger.OnDataSaved(EditDataObject entity, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.OnOperatorExecutd(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.OnOperatorExecuting(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.ContitionSqlCode<TEntity>(List<string> conditions)
        {
        }

        void IDataUpdateTrigger.OnPrepareSave(EditDataObject entity, DataOperatorType operatorType)
        {
        }

        void IDataTrigger.InitType<TEntity>()
        {
        }

        void IDataUpdateTrigger.BeforeUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
        }

        void IDataUpdateTrigger.AfterUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
            if (DefaultDataUpdateTrigger.IsType<TEntity>(DefaultDataUpdateTrigger.TypeofIVersionData))
            {
                long ver;
                using (RedisProxy proxy = new RedisProxy(RedisProxy.DbSystem))
                {
                    ver = proxy.Redis.Incr($"ent:ver:{table.Name}");
                }
                code.Append($@"
UPDATE `{table.WriteTableName}` 
SET `{table.FieldDictionary[nameof(IVersionData.DataVersion)]}` = {ver}");
                if (!string.IsNullOrEmpty(condition))
                {
                    code.Append($@"
WHERE {condition}");
                }
                code.AppendLine(";");
            }
        }
    }
}