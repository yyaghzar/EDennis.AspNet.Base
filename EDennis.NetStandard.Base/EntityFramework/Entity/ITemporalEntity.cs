﻿using System;

namespace EDennis.NetStandard.Base {
    public interface ITemporalEntity : ICrudEntity {
        DateTime SysEnd { get; set; }
        DateTime SysStart { get; set; }
    }
}