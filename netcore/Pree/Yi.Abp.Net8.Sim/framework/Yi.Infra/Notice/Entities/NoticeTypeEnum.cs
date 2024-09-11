﻿using System.ComponentModel;

namespace Yi.Infra.Notice.Entities;

public enum NoticeTypeEnum
{
    [Description("走马灯")] MerryGoRound = 0,
    [Description("提示弹窗")] Popup = 1
}