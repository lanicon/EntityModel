
namespace Agebull.Common.OAuth
{
    /// <summary>
    /// 权限范围枚举类型
    /// </summary>
    /// <remark>
    /// 权限范围
    /// </remark>
    public enum DataScopeType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 本人
        /// </summary>
        Person = 0x1,
        /// <summary>
        /// 本级
        /// </summary>
        Home = 0x2,
        /// <summary>
        /// 本人及本级
        /// </summary>
        PersonAndHome = 0x3,
        /// <summary>
        /// 下级
        /// </summary>
        Lower = 0x4,
        /// <summary>
        /// 本级及以下
        /// </summary>
        HomeAndLower = 0x6,
        /// <summary>
        /// 本人本级及下级
        /// </summary>
        Full = 0x7,
        /// <summary>
        /// 无限制
        /// </summary>
        Unlimited = 0x8,
    }
    /// <summary>
    /// 角色权限类型
    /// </summary>
    public enum RolePowerType
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None,
        /// <summary>
        /// 允许
        /// </summary>
        Allow,
        /// <summary>
        /// 拒绝
        /// </summary>
        Deny
    }
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class AuthEnumHelper
    {
        /// <summary>
        ///     权限范围枚举类型名称转换
        /// </summary>
        public static string ToCaption(this DataScopeType value)
        {
            switch (value)
            {
                case DataScopeType.None:
                    return "无";
                case DataScopeType.Person:
                    return "本人";
                case DataScopeType.Home:
                    return "本级";
                case DataScopeType.PersonAndHome:
                    return "本人及本级";
                case DataScopeType.Lower:
                    return "下级";
                case DataScopeType.HomeAndLower:
                    return "本级及以下";
                case DataScopeType.Full:
                    return "本人本级及下级";
                case DataScopeType.Unlimited:
                    return "无限制";
                default:
                    return "权限范围枚举类型(错误)";
            }
        }

        /// <summary>
        ///     权限枚举类型名称转换
        /// </summary>
        public static string ToCaption(this RolePowerType value)
        {
            switch (value)
            {
                case RolePowerType.None:
                    return "未设置";
                case RolePowerType.Allow:
                    return "允许";
                case RolePowerType.Deny:
                    return "拒绝";
                default:
                    return "权限枚举类型(未知)";
            }
        }

    }
}