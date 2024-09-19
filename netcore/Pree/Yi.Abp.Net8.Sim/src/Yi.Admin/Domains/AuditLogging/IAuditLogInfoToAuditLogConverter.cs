﻿using Volo.Abp.Auditing;
using Yi.Admin.Domains.AuditLogging.Entities;

namespace Yi.Admin.Domains.AuditLogging;

public interface IAuditLogInfoToAuditLogConverter
{
    Task<AuditLogEntity> ConvertAsync(AuditLogInfo auditLogInfo);
}