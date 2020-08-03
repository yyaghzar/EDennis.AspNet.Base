﻿using System;
using System.Collections.Generic;

namespace EDennis.NetStandard.Base {
    public class ExpandedDomainUser {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset LockoutBegin { get; set; }
        public DateTimeOffset LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string SysUser { get; set; }
        public SysStatus SysStatus { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }
        public string Properties { get; set; }
        public Dictionary<string,List<string>> RolesDictionary { get; set; }
        public Dictionary<string,List<string>> ClaimsDictionary { get; set; }

    }
}
