using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace Hocon.Json
{
    public static class HoconJsonExtentions
    {
        public static JToken? ToJToken(this HoconRoot hoconRoot)
        {
            if (hoconRoot == null)
            {
                throw new ArgumentNullException(nameof(hoconRoot));
            }

            return hoconRoot.Value.ToJToken();
        }

        public static JToken? ToJToken(this HoconValue hoconValue)
        {
            if (hoconValue == null)
            {
                throw new ArgumentNullException(nameof(hoconValue));
            }

            switch (hoconValue.Type)
            {
                case HoconType.Object:
                    return hoconValue.GetObject().ToJObject();
                case HoconType.Array:
                    return hoconValue.GetArray().ToJArray();
                case HoconType.Literal:
                    if (hoconValue.Count == 1)
                    {
                        if (hoconValue[0] is HoconSubstitution)
                        {
                            return ((HoconSubstitution)hoconValue[0]).ResolvedValue.ToJToken();
                        }
                        else
                        {
                            return ((HoconLiteral)hoconValue[0]).ToJValue();
                        }
                    }
                    else
                    {
                        return JValue.CreateString(hoconValue.GetString());
                    }

                case HoconType.Empty:
                    return null;
                default:
                    throw new InvalidOperationException($"Unknown Type: {hoconValue.Type}");
            }
        }

        public static JObject ToJObject(this HoconObject hoconObject)
        {
            if (hoconObject == null)
            {
                throw new ArgumentNullException(nameof(hoconObject));
            }

            var jObject = new JObject();
            foreach (var hoconField in hoconObject)
            {
                var key = hoconField.Key;
                var value = hoconField.Value.Value.ToJToken();
                if (value != null)
                {
                    jObject.Add(key, value);
                }
            }

            return jObject;
        }

        public static JArray ToJArray(this List<HoconValue> hoconValues)
        {
            if (hoconValues == null)
            {
                throw new ArgumentNullException(nameof(hoconValues));
            }

            var jArray = new JArray();
            foreach (var hoconValue in hoconValues)
            {
                var item = hoconValue.ToJToken();
                if (item != null)
                {
                    jArray.Add(item);
                }
            }

            return jArray;
        }

        public static JValue ToJValue(this HoconLiteral hoconLiteral)
        {
            if (hoconLiteral == null)
            {
                throw new ArgumentNullException(nameof(hoconLiteral));
            }

            var hoconValue = new HoconValue(null)
            {
                hoconLiteral,
            };
            switch (hoconLiteral.LiteralType)
            {
                case HoconLiteralType.Null:
                    return JValue.CreateNull();
                case HoconLiteralType.Whitespace:
                case HoconLiteralType.UnquotedString:
                case HoconLiteralType.QuotedString:
                case HoconLiteralType.TripleQuotedString:
                    return JValue.CreateString(hoconValue.GetString());
                case HoconLiteralType.Bool:
                    return new JValue(hoconValue.GetBoolean());
                case HoconLiteralType.Long:
                case HoconLiteralType.Hex:
                case HoconLiteralType.Octal:
                    return new JValue(hoconValue.GetLong());
                case HoconLiteralType.Double:
                    return new JValue(hoconValue.GetDouble());
                default:
                    throw new InvalidOperationException($"Unknown LiteralType: {hoconLiteral.LiteralType}");
            }
        }
    }
}
