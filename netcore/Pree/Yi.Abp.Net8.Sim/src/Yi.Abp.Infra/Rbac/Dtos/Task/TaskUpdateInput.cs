﻿using Yi.Abp.Infra.Rbac.Enums;

namespace Yi.Abp.Infra.Rbac.Dtos.Task
{
    public class TaskUpdateInput
    {
        public string AssemblyName { get; set; }

        public string JobType { get; set; }
        public string JobId { get; set; }
        public string? GroupName { get; set; }

        public JobTypeEnum Type { get; set; }

        public string? Cron { get; set; }

        public int? Millisecond { get; set; }

        public bool Concurrent { get; set; }

      //  public Dictionary<string, object>? Properties { get; set; }

        public string? Description { get; set; }
    }
}
