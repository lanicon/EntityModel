using System.Text;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     分页请求的参数
    /// </summary>
    public class PageArgument : IApiArgument
    {
        /// <summary>
        ///     页号
        /// </summary>
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int PageIndex { get; set; }

        /// <summary>
        ///     每页行数
        /// </summary>
        [JsonProperty("pageSize", NullValueHandling = NullValueHandling.Ignore)]
        public int PageSize { get; set; }

        /// <summary>
        ///     每页行数
        /// </summary>
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public string Order { get; set; }


        /// <summary>
        ///     反序
        /// </summary>
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public bool Desc { get; set; }


        /// <summary>
        ///     数据校验
        /// </summary>
        /// <param name="message">返回的消息</param>
        /// <returns>成功则返回真</returns>
        public virtual bool Validate(out string message)
        {
            var msg = new StringBuilder();
            var success = true;
            if (PageIndex < 0)
            {
                success = false;
                msg.Append("页号必须大于或等于0");
            }

            if (PageSize <= 0 || PageSize > 100)
            {
                success = false;
                msg.Append("行数必须大于0且小于100");
            }

            message = msg.ToString();
            return success;
        }
    }
}