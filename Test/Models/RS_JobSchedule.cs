using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Model
{
    public class RS_JobSchedule
    {
        public RS_JobSchedule(Type jobType, string cronExpression)
        {
            this.JobType = jobType ?? throw new ArgumentNullException(nameof(jobType));
            CronExpression = cronExpression ?? throw new ArgumentNullException(nameof(cronExpression));
        }
        public Type JobType { get; private set; }
        /// <summary>
        /// Cron表示式
        /// </summary>
        public string CronExpression { get; private set; }
        /// <summary>
        /// Job狀態
        /// </summary>
        public JobStatus JobStatus { get; set; } = JobStatus.Init;
    }
    /// <summary>
    /// Job執行狀態
    /// </summary>
    public enum JobStatus : byte
    {
        [Description("初始化")]
        Init = 0,
        [Description("執行中")]
        Running = 1,
        [Description("排程中")]
        Scheduling = 2,
        [Description("已停止")]
        Stopped = 3,
    }
}
