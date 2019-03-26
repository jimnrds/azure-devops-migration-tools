using System;
using VstsSyncMigrator.Engine.ComponentContext;

namespace VstsSyncMigrator.Engine.Configuration.FieldMap
{
    public class FieldMappingFileMapConfig : IFieldMapConfig
    {
        public string WorkItemTypeName { get; set; }

        public string sourceField { get; set; }

        public string targetField { get; set; }

        public string MappingFile { get; set; }

        public Type FieldMap
        {
            get
            {
                return typeof(FieldMappingFileMap);
            }
        }
    }
}
