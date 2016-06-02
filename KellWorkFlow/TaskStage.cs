using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KellWorkFlow
{
    [Serializable]
    public class TaskStage
    {
        public int ID { get; set; }

        public string Name { get; private set; }

        public TaskStatus Status { get; internal set; }

        public TaskStageTemplate Template { get; private set; }

        public string ActualExec { get; set; }

        public DateTime ExecTime { get; set; }

        public string ActualAppr { get; set; }

        public DateTime ApprTime { get; set; }

        public string Remark { get; set; }

        public bool CanReceive
        {
            get
            {
                return Status == TaskStatus.Initiative;
            }
        }

        public bool CanExecute
        {
            get
            {
                return Status == TaskStatus.Processing;
            }
        }

        public bool CanApprove
        {
            get
            {
                return Status == TaskStatus.Processed;
            }
        }

        public static TaskStatus GetTaskStatus(int value)
        {
            return (TaskStatus)Enum.ToObject(typeof(TaskStatus), value);
        }

        public static List<string> GetUsers(string ids)
        {
            List<string> users = new List<string>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] Ids = ids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string id in Ids)
                {
                    users.Add(id);
                }
            }
            return users;
        }

        public static string GetUserIds(List<string> users)
        {
            StringBuilder sb = new StringBuilder();
            if (users != null && users.Count > 0)
            {
                foreach (string user in users)
                {
                    if (sb.Length == 0)
                        sb.Append(user);
                    else
                        sb.Append("," + user);
                }
            }
            return sb.ToString();
        }

        public TaskStage(int id, string name, TaskStageTemplate template, TaskStatus status, string actualExec, string actualAppr, DateTime execTime, DateTime apprTime, string remark)
        {
            this.ID = id;
            this.Name = name;
            this.Template = template;
            this.Status = status;
            this.ActualExec = actualExec;
            this.ActualAppr = actualAppr;
            this.ExecTime = execTime;
            this.ApprTime = apprTime;
            this.Remark = remark;
        }

        public TaskStage(int id, string name, TaskStageTemplate template, TaskStatus status, string remark)
        {
            this.ID = id;
            this.Name = name;
            this.Template = template;
            this.Status = status;
            this.Remark = remark;
        }

        public static TaskStage GetFlowTemplate(Func<string, TaskStage> externalDelegate, string where)
        {
            return externalDelegate(where);
        }

        public bool IsAuthorizationExecutor(string userid)
        {
            return this.Template.IsAuthorizationExecutor(userid);
        }

        public bool IsAuthorizationApprover(string userid)
        {
            return this.Template.IsAuthorizationApprover(userid);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
