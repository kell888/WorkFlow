using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KellWorkFlow
{
    [Serializable]
    public class TaskStageTemplate
    {
        public int ID { get; set; }

        public string Name { get; private set; }

        public List<string> Executors { get; private set; }

        public List<string> Approvers { get; private set; }

        public TaskStageTemplate(int id, string name, List<string> executors, List<string> approvers = null)
        {
            this.ID = id;
            this.Name = name;
            if (executors != null)
                this.Executors = executors;
            else
                this.Executors = new List<string>();
            if (approvers != null)
                this.Approvers = approvers;
            else
                this.Approvers = new List<string>();
        }

        public static TaskStage GetFlowTemplate(Func<string, TaskStage> externalDelegate, string where)
        {
            return externalDelegate(where);
        }

        public bool IsAuthorizationExecutor(string userid)
        {
            return Executors.Contains(userid);
        }

        public bool IsAuthorizationApprover(string userid)
        {
            return Approvers.Contains(userid);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
