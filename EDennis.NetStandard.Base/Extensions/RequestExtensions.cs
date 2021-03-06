﻿using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace EDennis.NetStandard.Base {
    public static partial class RequestExtensions {
        public static bool ContainsPathHeaderOrQueryKey(this HttpRequest request, string key, out string value, string defaultValue = null) {
            value = defaultValue;
            if (request.Path.Value.ToLower().Contains(key.ToLower())) {
                return true;
            } else if (request.Headers.Keys.Any(k => k.Equals(key, StringComparison.OrdinalIgnoreCase))) {
                value = request.Headers[key];
                if (string.IsNullOrEmpty(value))
                    value = defaultValue;
                return true;
            } else if (request.Query.Keys.Any(k => k.Equals(key, StringComparison.OrdinalIgnoreCase))) {
                value = request.Query[key];
                if (string.IsNullOrEmpty(value))
                    value = defaultValue;
                return true;
            } else {
                value = null;
                return false;
            }
        }
    }

}
