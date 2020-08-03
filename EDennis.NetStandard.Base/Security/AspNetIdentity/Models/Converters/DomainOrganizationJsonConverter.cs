﻿using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EDennis.NetStandard.Base {

    /// <summary>
    /// Used to handle serialization/deserialization of Properties property
    /// </summary>
    public class DomainOrganizationJsonConverter : JsonConverter<DomainOrganization> {

        OtherProperties _otherProperties;

        public override DomainOrganization Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var obj = new DomainOrganization();
            DeserializeInto(obj, reader);
            return obj;
        }

        /// <summary>
        /// This method can be used to directly deserialize an HttpRequest body
        /// into an object.
        /// If used, do not include a [FromBody] parameter
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="request"></param>
        public void DeserializeInto(DomainOrganization obj, HttpRequest request) {
            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var str = Encoding.UTF8.GetBytes(Task.Run(() => reader.ReadToEndAsync()).Result);
            var bytes = new ReadOnlySpan<byte>(str);
            DeserializeInto(obj, new Utf8JsonReader(bytes));
        }


        public void DeserializeInto(DomainOrganization obj, Utf8JsonReader reader) {
            while (reader.Read()) {
                switch (reader.TokenType) {
                    case JsonTokenType.PropertyName:
                        var prop = reader.GetString();
                        reader.Read();
                        switch (prop) {
                            case "Id":
                            case "id":
                                obj.Id = reader.GetGuid();
                                break;
                            case "Name":
                            case "name":
                                obj.Name = reader.GetString();
                                break;
                            case "SysUser":
                            case "sysUser":
                                obj.SysUser = reader.GetString();
                                break;
                            case "SysStatus":
                            case "sysStatus":
                                obj.SysStatus = (SysStatus)Enum.Parse(typeof(SysStatus), reader.GetString());
                                break;
                            case "SysStart":
                            case "sysStart":
                                obj.SysStart = reader.GetDateTime();
                                break;
                            case "SysEnd":
                            case "sysEnd":
                                obj.SysEnd = reader.GetDateTime();
                                break;
                            default:
                                if (_otherProperties == null)
                                    _otherProperties = new OtherProperties();
                                _otherProperties.Add(prop, ref reader);
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (_otherProperties != null) {
                obj.Properties = _otherProperties.ToString();
            }

        }

        public override void Write(Utf8JsonWriter writer, DomainOrganization value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            {
                writer.WriteString("Id", value.Id.ToString());
                writer.WriteString("Name", value.Name);
                writer.WriteString("SysUser", value.SysUser);
                writer.WriteString("SysStatus", value.SysStatus.ToString());
                writer.WriteString("SysStart", value.SysStart.ToString("u"));
                writer.WriteString("SysEnd", value.SysStart.ToString("u"));
                //extract catch-all properties and promote to top-level in JSON
                if (value.Properties != null) {
                    using var doc = JsonDocument.Parse(value.Properties);
                    foreach (var prop in doc.RootElement.EnumerateObject())
                        prop.WriteTo(writer);
                }
            }
            writer.WriteEndObject();
        }
    }
}
