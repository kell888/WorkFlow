using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace KellWorkFlow
{
    [Serializable]
    public class TaskInfo
    {
        public int ID { get; private set; }

        public int EntityId { get; private set; }

        public string Sponsor { get; private set; }

        public Flow Flow { get; private set; }

        public string Remark { get; set; }

        public TaskInfo(int id, int entityid, Flow flow, string sponsor, string remark)
        {
            if (flow == null)
                throw new ArgumentNullException("flow");

            this.ID = id;
            this.EntityId = entityid;
            this.Flow = flow;
            this.Sponsor = sponsor;
            this.Remark = remark;
        }

        public static TaskInfo GetTaskInfo(Func<string, TaskInfo> externalDelegate, string where)
        {
            return externalDelegate(where);
        }

        public override string ToString()
        {
            return Flow.Name;
        }

        public TaskStage CurrentStage
        {
            get
            {
                return Flow.Current;
            }
        }

        public bool IsFinished
        {
            get
            {
                return Flow.IsFinished;
            }
        }
    }
}
