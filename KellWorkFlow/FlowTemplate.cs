using System;
using System.Collections.Generic;
using System.Text;

namespace KellWorkFlow
{
    [Serializable]
    public class FlowTemplate
    {
        private List<TaskStageTemplate> stages;

        public List<TaskStageTemplate> Stages
        {
            get { return stages; }
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public FlowTemplate(int id, string name, List<TaskStageTemplate> stages)
        {
            this.ID = id;
            this.Name = name;
            if (stages != null)
                this.stages = stages;
            else
                this.stages = new List<TaskStageTemplate>();
        }

        public static FlowTemplate GetFlowTemplate(Func<string, FlowTemplate> externalDelegate, string where)
        {
            return externalDelegate(where);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TaskStageTemplate stage in this.stages)
            {
                if (sb.Length == 0)
                    sb.Append(stage.Name);
                else
                    sb.Append(", " + stage.Name);
            }
            return Name + "[" + sb.ToString() + "]";
        }

        public int StageCount
        {
            get
            {
                return this.stages.Count;
            }
        }

        public List<string> StageIdList
        {
            get
            {
                List<string> ids = new List<string>();
                foreach (TaskStageTemplate stage in this.stages)
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
                foreach (TaskStageTemplate stage in this.stages)
                {
                    if (sb.Length == 0)
                        sb.Append(stage.ID.ToString());
                    else
                        sb.Append("," + stage.ID);
                }
                return sb.ToString();
            }
        }
    }
}
