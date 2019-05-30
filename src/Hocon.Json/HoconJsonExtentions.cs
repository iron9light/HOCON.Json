using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace Hocon.Json
{
    public static class HoconJsonExtentions
    {
        public static JToken? ToJToken(this HoconRoot hoconRoot, Func<JValue, JValue>? jValueHandler = null)
        {
            if (hoconRoot == null)
            {
                throw new ArgumentNullException(nameof(hoconRoot));
            }

            return hoconRoot.Value.ToJToken(jValueHandler);
        }

        public static JToken? ToJToken(this HoconValue hoconValue, Func<JValue, JValue>? jValueHandler = null)
        {
            if (hoconValue == null)
            {
                throw new ArgumentNullException(nameof(hoconValue));
            }

            switch (hoconValue.Type)
            {
                case HoconType.Object:
                    return hoconValue.GetObject().ToJObject(jValueHandler);
                case HoconType.Array:
                    return hoconValue.GetArray().ToJArray(jValueHandler);
                case HoconType.Literal:
                    if (hoconValue.Count == 1)
                    {
                        var hoconElement = hoconValue[0];
                        if (hoconElement is HoconSubstitution hoconSubstitution)
                        {
                            return hoconSubstitution.ResolvedValue.ToJToken(jValueHandler);
                        }

                        if (hoconElement is HoconLiteral hoconLiteral)
                        {
                            return hoconLiteral.ToJValue(jValueHandler);
                        }

                        throw new InvalidOperationException($"Invalid Hocon element type when hocon type is Literal and has only one element: {hoconElement.GetType().FullName}");
                    }
                    else
                    {
                        var jValue = JValue.CreateString(hoconValue.GetString());
                        if (jValueHandler != null)
                        {
                            return jValueHandler(jValue);
                        }
                        else
                        {
                            return jValue;
                        }
                    }

                case HoconType.Empty:
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                    return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
                default:
                    throw new InvalidOperationException($"Unknown Type: {hoconValue.Type}");
            }
        }

        public static JObject ToJObject(this HoconObject hoconObject, Func<JValue, JValue>? jValueHandler = null)
        {
            if (hoconObject == null)
            {
                throw new ArgumentNullException(nameof(hoconObject));
            }

            var jObject = new JObject();
            foreach (var hoconField in hoconObject)
            {
                var key = hoconField.Key;
                JToken? value;

                var fieldHoconObject = hoconField.Value.GetObject();
                if (fieldHoconObject != null)
                {
                    value = fieldHoconObject.ToJObject(jValueHandler);
                }
                else
                {
                    value = hoconField.Value.Value.ToJToken(jValueHandler);
                }

                if (value != null)
                {
                    jObject.Add(key, value);
                }
            }

            return jObject;
        }

        public static JArray ToJArray(this List<HoconValue> hoconValues, Func<JValue, JValue>? jValueHandler = null)
        {
            if (hoconValues == null)
            {
                throw new ArgumentNullException(nameof(hoconValues));
            }

            var jArray = new JArray();
            foreach (var hoconValue in hoconValues)
            {
                var item = hoconValue.ToJToken(jValueHandler);
                if (item != null)
                {
                    jArray.Add(item);
                }
            }

            return jArray;
        }

        public static JValue ToJValue(this HoconLiteral hoconLiteral, Func<JValue, JValue>? jValueHandler = null)
        {
            if (hoconLiteral == null)
            {
                throw new ArgumentNullException(nameof(hoconLiteral));
            }

            var hoconValue = new HoconValue(null)
            {
                hoconLiteral,
            };

            JValue jValue;
            switch (hoconLiteral.LiteralType)
            {
                case HoconLiteralType.Null:
                    jValue = JValue.CreateNull();
                    break;
                case HoconLiteralType.Whitespace:
                case HoconLiteralType.UnquotedString:
                case HoconLiteralType.QuotedString:
                case HoconLiteralType.TripleQuotedString:
                    jValue = JValue.CreateString(hoconValue.GetString());
                    break;
                case HoconLiteralType.Bool:
                    jValue = new JValue(hoconValue.GetBoolean());
                    break;
                case HoconLiteralType.Long:
                case HoconLiteralType.Hex:
                case HoconLiteralType.Octal:
                    jValue = new JValue(hoconValue.GetLong());
                    break;
                case HoconLiteralType.Double:
                    jValue = new JValue(hoconValue.GetDouble());
                    break;
                default:
                    throw new InvalidOperationException($"Unknown LiteralType: {hoconLiteral.LiteralType}");
            }

            if (jValueHandler != null)
            {
                return jValueHandler(jValue);
            }
            else
            {
                return jValue;
            }
        }
    }
}
