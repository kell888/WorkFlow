using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KellWorkFlow
{
    [Serializable]
    public class Flow
    {
        internal List<TaskStage> stages;

        public List<TaskStage> Stages
        {
            get { return stages; }
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        int index = -1;

        public int CurrentIndex
        {
            get { return index; }
            internal set { index = value; }
        }

        public void StartFlow()
        {
            index = 0;
        }

        public FlowTemplate Template { get; set; }

        public Flow(int id, string name, FlowTemplate template, int currentIndex, string remark, List<TaskStage> stages)
        {
            this.ID = id;
            this.Name = name;
            this.Template = template;
            this.CurrentIndex = currentIndex;
            this.Remark = remark;
            if (stages != null)
                this.stages = stages;
            else
                this.stages = new List<TaskStage>();
        }

        public static Flow GetFlow(Func<string, Flow> externalDelegate, string where)
        {
            return externalDelegate(where);
        }

        public bool IsFinished
        {
            get
            {
                return this.index == this.stages.Count - 1 && this.Current.Status == TaskStatus.Finished;
            }
        }

        public TaskStage Current
        {
            get
            {
                return this.stages[index];
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TaskStage stage in this.stages)
            {
                string appr = "";
                string exec = "";
                string app = "", exe = "";
                if (stage.Status == TaskStatus.Processed)
                {
                    exec = "执行人:" + stage.ActualExec;
                    exe = "提交时间:" + stage.ExecTime.ToString("yyyy-MM-dd HH:mm");
                }
                if (stage.Status == TaskStatus.Finished)
                {
                    exec = "执行人:" + stage.ActualExec;
                    exe = "提交时间:" + stage.ExecTime.ToString("yyyy-MM-dd HH:mm");
                    appr = "审批人:" + stage.ActualAppr;
                    app = "审批时间:" + stage.ApprTime.ToString("yyyy-MM-dd HH:mm");
                }
                string info = "";
                if (exec != "")
                {
                    info += exec;
                    info += " " + exe;
                }
                if (appr != "")
                {
                    info += " " + appr;
                    info += " " + app;
                }
                if (info != "")
                    info = "  " + info;
                if (sb.Length == 0)
                    sb.Append(stage.Name + "(" + stage.Status.ToString() + info + ")");
                else
                    sb.Append(", " + stage.Name + "(" + stage.Status.ToString() + info + ")");
            }
            return Name + "[" + sb.ToString() + "]";
        }

        public void Reject()
        {
            if (this.Current.Status == TaskStatus.Processed)
            {
                this.Current.Status = TaskStatus.Processing;
            }
        }

        public bool Execute()
        {
            if (this.Current.Status == TaskStatus.Processing)
            {
                this.Current.Status = TaskStatus.Processed;
                return true;
            }
            return false;
        }

        public void Pause()
        {
            this.Current.Status = TaskStatus.Paused;
        }

        public void Receive()
        {
            this.Current.Status = TaskStatus.Processing;
        }

        public bool Approve()
        {
            if (stages.Count > 0 && index < stages.Count - 1 && this.Current.Status == TaskStatus.Processed)
            {
                this.Current.Status = TaskStatus.Finished;
                index++;
                this.Current.Status = TaskStatus.Processing;
                return true;
            }
            return false;
        }

        public bool Against()
        {
            if (stages.Count > 0 && index > -1 && this.Current.Status == TaskStatus.Processed)
            {
                this.Current.Status = TaskStatus.Initiative;
                index--;
                this.Current.Status = TaskStatus.Processing;
                return true;
            }
            return false;
        }

        public List<string> StageIdList
        {
            get
            {
                List<string> ids = new List<string>();
                foreach (TaskStage stage in this.stages)
                {
                    ids.Add(stage.ID.ToString());
                }
                return ids;
            }
        }

        public string StageIds
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (TaskStage stage in this.stages)
                {
                    if (sb.Length == 0)
                        sb.Append(stage.ID.ToString());
                    else
                        sb.Append("," + stage.ID);
                }
                return sb.ToString();
            }
        }

        public static List<string> GetSatges(string ids)
        {
            List<string> stages = new List<string>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] Ids = ids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string id in Ids)
                {
                    stages.Add(id);
                }
            }
            return stages;
        }

        public static string GetStageIds(List<string> stages)
        {
            StringBuilder sb = new StringBuilder();
            if (stages != null && stages.Count > 0)
            {
                foreach (string stage in stages)
                {
                    if (sb.Length == 0)
                        sb.Append(stage);
                    else
                        sb.Append("," + stage);
                }
            }
            return sb.ToString();
        }
    }
}
