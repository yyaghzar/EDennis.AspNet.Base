﻿using Microsoft.AspNetCore.Authorization;
using EDennis.NetStandard.Base;

namespace EDennis.AspNetIdentityServer {

    [Authorize(Policy = "AdministerIDP")]
    public class ApplicationController : IdpApplicationController {
        public ApplicationController(DomainApplicationRepo repo) : base(repo) {
        }
    }
}
