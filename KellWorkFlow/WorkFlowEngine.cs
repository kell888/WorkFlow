using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace KellWorkFlow
{
    public static class WorkFlowEngine
    {
        public static List<TaskInfo> GetTasksByEntity(Func<string, List<TaskInfo>> getTask, string entityid = null)
        {
            string where = "(1=1)";
            if (!string.IsNullOrEmpty(entityid))
                where += " and Entityid='" + entityid + "'";
            return getTask(where);
        }

        public static List<TaskInfo> GetTasksByEntity(Func<string, List<TaskInfo>> getTask, string entityid = null, bool finished = false)
        {
            string where = "IsFinished=" + (finished ? "1" : "0");
            if (!string.IsNullOrEmpty(entityid))
                where += " and Entityid='" + entityid + "'";
            return getTask(where);
        }

        public static List<TaskInfo> GetTasksBySponsor(Func<string, List<TaskInfo>> getTask, string sponsor = null)
        {
            string where = "(1=1)";
            if (!string.IsNullOrEmpty(sponsor))
                where += " and Sponsor='" + sponsor + "'";
            return getTask(where);
        }

        public static List<TaskInfo> GetTasksBySponsor(Func<string, List<TaskInfo>> getTask, string sponsor = null, bool finished = false)
        {
            string where = "IsFinished=" + (finished ? "1" : "0");
            if (!string.IsNullOrEmpty(sponsor))
                where += " and Sponsor='" + sponsor + "'";
            return getTask(where);
        }

        public static TaskInfo GetTask(Func<string, TaskInfo> getTask, int taskId)
        {
            return TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
        }

        public static bool CanReceive(Func<string, TaskInfo> getTask, int taskId, int userId)
        {
            TaskInfo task = TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
            return task.CurrentStage.IsAuthorizationExecutor(userId.ToString()) && task.CurrentStage.CanReceive;
        }

        public static bool CanExecute(Func<string, TaskInfo> getTask, int taskId, int userId)
        {
            TaskInfo task = TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
            return task.CurrentStage.IsAuthorizationExecutor(userId.ToString()) && task.CurrentStage.CanExecute;
        }

        public static bool CanApprove(Func<string, TaskInfo> getTask, int taskId, int userId)
        {
            TaskInfo task = TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
            return task.CurrentStage.IsAuthorizationApprover(userId.ToString()) && task.CurrentStage.CanApprove;
        }

        public static bool AgreeTask(Func<string, TaskInfo> getTask, Func<TaskInfo, string, bool> setTask, int taskId, int userId)
        {
            TaskInfo task = TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
            if (task.CurrentStage.IsAuthorizationApprover(userId.ToString()))
            {
                return task.Flow.Approve() && setTask(task, "ID=" + taskId);//更新数据库
            }
            return false;
        }

        public static bool AgainstTask(Func<string, TaskInfo> getTask, Func<TaskInfo, string, bool> setTask, int taskId, int userId)
        {
            TaskInfo task = TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
            if (task.CurrentStage.IsAuthorizationApprover(userId.ToString()))
            {
                return task.Flow.Against() && setTask(task, "ID=" + taskId);//更新数据库
            }
            return false;
        }

        public static bool RejectTask(Func<string, TaskInfo> getTask, Func<TaskInfo, string, bool> setTask, int taskId, int userId)
        {
            TaskInfo task = TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
            if (task.CurrentStage.IsAuthorizationApprover(userId.ToString()))
            {
                task.Flow.Reject();
                return setTask(task, "ID=" + taskId);//更新数据库
            }
            return false;
        }

        public static bool ReceiveTask(Func<string, TaskInfo> getTask, Func<TaskInfo, string, bool> setTask, int taskId, int userId)
        {
            TaskInfo task = TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
            if (task.CurrentStage.IsAuthorizationExecutor(userId.ToString()))
            {
                task.Flow.Receive();
                return setTask(task, "ID=" + taskId);//更新数据库
            }
            return false;
        }

        public static bool ExecuteTask(Func<string, TaskInfo> getTask, Func<TaskInfo, string, bool> setTask, int taskId, int userId)
        {
            TaskInfo task = TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
            if (task.CurrentStage.IsAuthorizationExecutor(userId.ToString()))
            {
                return task.Flow.Execute() && setTask(task, "ID=" + taskId);//更新数据库
            }
            return false;
        }

        public static bool Pause(Func<string, TaskInfo> getTask, Func<TaskInfo, string, bool> setTask, int taskId, int userId)
        {            
            TaskInfo task = TaskInfo.GetTaskInfo(getTask, "ID=" + taskId);
            if (task.CurrentStage.IsAuthorizationApprover(userId.ToString()))
            {
                task.Flow.Pause();
                return setTask(task, "ID=" + taskId);//更新数据库
            }
            return false;
        }

        public static bool SaveWorkFlowToLocal(List<TaskInfo> tasks, string localXmlFile)
        {
            if (File.Exists(localXmlFile))
            {
                try
                {
                    DataTable data = ConvertToDataTable(tasks);
                    data.WriteXml(localXmlFile, XmlWriteMode.WriteSchema);
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return false;
        }

        public static List<TaskInfo> LoadWorkFlowFromLocal(string localXmlFile, Func<string, Flow> getFlow)
        {
            if (File.Exists(localXmlFile))
            {
                try
                {
                    DataTable data = new DataTable();
                    XmlReadMode xrm = data.ReadXml(localXmlFile);
                    return CovertToTaskInfo(data, getFlow);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return null;
        }

        internal static List<TaskInfo> CovertToTaskInfo(DataTable dt, Func<string, Flow> getFlow)
        {
            List<TaskInfo> tasks = new List<TaskInfo>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TaskInfo task = new TaskInfo(Convert.ToInt32(dt.Rows[i]["ID"]), Convert.ToInt32(dt.Rows[i]["EntityId"]), getFlow(dt.Rows[i]["FlowID"].ToString()), dt.Rows[i]["Sponsor"].ToString(), dt.Rows[i]["Remark"].ToString());
                    tasks.Add(task);
                }
            }
            return tasks;
        }

        internal static DataTable ConvertToDataTable(List<TaskInfo> tasks)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("EntityId", typeof(int));
            dt.Columns.Add("FlowID", typeof(int));
            dt.Columns.Add("Sponsor", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            if (tasks != null)
            {
                foreach (TaskInfo task in tasks)
                {
                    DataRow row = dt.NewRow();
                    row.ItemArray = new object[] { task.ID, task.EntityId, task.Flow.ID, task.Sponsor, task.Remark };
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }
    }
}
