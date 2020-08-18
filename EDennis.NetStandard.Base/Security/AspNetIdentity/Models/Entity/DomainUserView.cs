﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EDennis.NetStandard.Base {

    [JsonConverter(typeof(DomainUserViewJsonConverter))]
    public class DomainUserView : IdentityUser<int> {

        public override bool LockoutEnabled { get; set; }

        public DateTimeOffset? _lockoutBegin;
        public DateTimeOffset? _lockoutEnd;


        public DateTimeOffset? LockoutBegin {
            get => _lockoutBegin;
            set {
                _lockoutBegin = value;
                if (_lockoutBegin <= DateTime.Now && _lockoutEnd > DateTime.Now)
                    LockoutEnabled = true;
                else
                    LockoutEnabled = false;
            }
        }

        public override DateTimeOffset? LockoutEnd {
            get => _lockoutEnd;
            set {
                _lockoutEnd = value;
                if (_lockoutBegin <= DateTime.Now && _lockoutEnd > DateTime.Now)
                    LockoutEnabled = true;
                else
                    LockoutEnabled = false;
            }
        }

        public string Organization { get; set; }

        public string RolesDictionary { get; set; }
        public string ClaimsDictionary { get; set; }

    }


    public class DomainUserViewJsonConverter : JsonConverter<DomainUserView> {

        public override DomainUserView Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return (DomainUserView)JsonSerializer.Deserialize(ref reader, typeof(DomainUserView), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public override void Write(Utf8JsonWriter writer, DomainUserView value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            {
                writer.WriteNumber("Id", value.Id);
                writer.WriteString("UserName", value.UserName);
                writer.WriteString("NormalizedUserName", value.NormalizedUserName);
                if (value.AccessFailedCount != default)
                    writer.WriteNumber("AccessFailedCount", value.AccessFailedCount);
                writer.WriteString("Email", value.Email);
                writer.WriteString("NormalizedEmail", value.NormalizedEmail);
                writer.WriteBoolean("EmailConfirmed", value.EmailConfirmed);
                writer.WriteBoolean("LockoutEnabled", value.LockoutEnabled);
                if (value.LockoutBegin != default)
                    writer.WriteString("LockoutBegin", value.LockoutBegin.Value.ToString("u"));
                if (value.LockoutEnd != default)
                    writer.WriteString("LockoutEnd", value.LockoutEnd.Value.ToString("u"));
                if (value.Organization != default)
                    writer.WriteString("Organization", value.Organization);
                if (value.PhoneNumber != default)
                    writer.WriteString("PhoneNumber", value.PhoneNumber);
                if (value.PhoneNumberConfirmed != default)
                    writer.WriteBoolean("PhoneNumberConfirmed", value.PhoneNumberConfirmed);
                if (value.TwoFactorEnabled != default)
                    writer.WriteBoolean("TwoFactorEnabled", value.TwoFactorEnabled);
                if (value.RolesDictionary != null) {
                    writer.WriteStartObject("RolesDictionary");
                    {
                        using var doc = JsonDocument.Parse(value.RolesDictionary);
                        foreach (var prop in doc.RootElement.EnumerateObject())
                            prop.WriteTo(writer);
                    }
                    writer.WriteEndObject();
                }
                if (value.ClaimsDictionary != null) {
                    writer.WriteStartObject("ClaimsDictionary");
                    {
                        using var doc = JsonDocument.Parse(value.ClaimsDictionary);
                        foreach (var prop in doc.RootElement.EnumerateObject())
                            prop.WriteTo(writer);
                    }
                    writer.WriteEndObject();
                }
            }
            writer.WriteEndObject();
        }

    }


}
