using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Collections;
using System.Diagnostics;
using System.IO;
using VstsSyncMigrator.Engine.Configuration.FieldMap;
using System;

namespace VstsSyncMigrator.Engine.ComponentContext
{
    public class FieldMappingFileMap : FieldMapBase
    {
        public Hashtable itemMap;
        private FieldMappingFileMapConfig config;

        public FieldMappingFileMap(FieldMappingFileMapConfig config)
        {
            this.config = config;
            this.itemMap = new Hashtable();
            this.ReadItemMap(config.MappingFile);
        }

        public override string MappingDisplayName => $"{config.sourceField} ";
        internal override void InternalExecute(WorkItem source, WorkItem target)
        {
            if (!source.Fields.Contains(this.config.sourceField) || source.Fields[this.config.sourceField].Value.ToString().Length <= 0 || !target.Fields.Contains(this.config.targetField))
                return;
            if (this.itemMap.Contains(source.Fields[this.config.sourceField].Value))
            {
                string str = this.itemMap[source.Fields[this.config.sourceField].Value].ToString();
                target.Fields[this.config.targetField].Value = (object)str;
                Trace.WriteLine(string.Format("  [UPDATE] field mapped {0}:{1} using mapping file{2} (items: {3}) to: {4}", (object)source.Id, (object)this.config.sourceField, (object)this.config.MappingFile, (object)this.itemMap.Count.ToString(), target.Fields[this.config.targetField].Value));
            }
            else
            {
                this.itemMap.Add(source.Fields[this.config.sourceField].Value, source.Fields[this.config.sourceField].Value);
                Trace.WriteLine(string.Format("  [SKIP] field mapped {0}:{1} using mapping file{2} (items: {3}) no mapping value found for {4}.", (object)source.Id, (object)this.config.sourceField, (object)this.config.MappingFile, (object)this.itemMap.Count.ToString(), (object)source.Fields[this.config.sourceField].Value.ToString()));
            }
        }

        private void ReadItemMap(string mapFileName)
        {
            string path = string.Format("{0}", (object)mapFileName);
            this.itemMap = new Hashtable();
            if (!File.Exists(path))
                return;
            StreamReader streamReader = new StreamReader(path);
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
                if (!str.Contains("Source ID|Target ID"))
                {
                    string[] strArray = str.Split('|');
                    if (strArray[0].Trim() != "" && strArray[1].Trim() != "")
                        this.itemMap.Add((object)strArray[0].Trim(), (object)strArray[1].Trim());
                }
            }
            streamReader.Close();
        }
    }
}
