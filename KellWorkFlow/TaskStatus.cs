using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace KellWorkFlow
{
    public enum TaskStatus : int
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [Description("初始化")]
        Initiative = 0,
        /// <summary>
        /// 已接收
        /// </summary>
        [Description("已接收")]
        Processing = 1,
        /// <summary>
        /// 已提交
        /// </summary>
        [Description("已提交")]
        Processed = 2,
        /// <summary>
        /// 已暂停
        /// </summary>
        [Description("已暂停")]
        Paused = 88,
        /// <summary>
        /// 已审批
        /// </summary>
        [Description("已审批")]
        Finished = 99
    }
}
