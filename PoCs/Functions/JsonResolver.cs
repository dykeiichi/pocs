using JsonReader = Utf8Json.JsonReader;
using JsonToken = Utf8Json.JsonToken;

namespace PoCs.Functions
{
    public static class JsonResolver {
        public static bool TryGetInt(ref JsonReader reader, out int value) {
            reader.ReadNext();
            switch (reader.GetCurrentJsonToken()) {
                case JsonToken.Number:
                    value = reader.ReadInt32();
                    return true;
                case JsonToken.String:
                    if (int.TryParse(reader.ReadString(), out int intValue)) {
                        value = intValue;
                        return true;
                    }
                    break;
                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                case JsonToken.False:
                case JsonToken.NameSeparator:
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.True:
                case JsonToken.ValueSeparator:
                default:
                    break;
            }

            value = 0;
            return false;
        }

        public static bool TryGetUInt(ref JsonReader reader, out uint value) {
            reader.ReadNext();
            switch (reader.GetCurrentJsonToken()) {
                case JsonToken.Number:
                    value = reader.ReadUInt32();
                    return true;
                case JsonToken.String:
                    if (uint.TryParse(reader.ReadString(), out uint intValue)) {
                        value = intValue;
                        return true;
                    }
                    break;
                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                case JsonToken.False:
                case JsonToken.NameSeparator:
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.True:
                case JsonToken.ValueSeparator:
                default:
                    break;
            }

            value = 0;
            return false;
        }

        public static bool TryGetLong(ref JsonReader reader, out long value) {
            reader.ReadNext();
            switch (reader.GetCurrentJsonToken()) {
                case JsonToken.Number:
                    value = reader.ReadInt64();
                    return true;
                case JsonToken.String:
                    if (long.TryParse(reader.ReadString(), out long intValue)) {
                        value = intValue;
                        return true;
                    }
                    break;
                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                case JsonToken.False:
                case JsonToken.NameSeparator:
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.True:
                case JsonToken.ValueSeparator:
                default:
                    break;
            }

            value = 0;
            return false;
        }

        public static bool TryGetShort(ref JsonReader reader, out short value) {
            reader.ReadNext();
            switch (reader.GetCurrentJsonToken()) {
                case JsonToken.Number:
                    value = reader.ReadInt16();
                    return true;
                case JsonToken.String:
                    if (short.TryParse(reader.ReadString(), out short intValue)) {
                        value = intValue;
                        return true;
                    }
                    break;
                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                case JsonToken.False:
                case JsonToken.NameSeparator:
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.True:
                case JsonToken.ValueSeparator:
                default:
                    break;
            }

            value = 0;
            return false;
        }

        public static bool TryGetUShort(ref JsonReader reader, out ushort value) {
            reader.ReadNext();
            switch (reader.GetCurrentJsonToken()) {
                case JsonToken.Number:
                    value = reader.ReadUInt16();
                    return true;
                case JsonToken.String:
                    if (ushort.TryParse(reader.ReadString(), out ushort intValue)) {
                        value = intValue;
                        return true;
                    }
                    break;
                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                case JsonToken.False:
                case JsonToken.NameSeparator:
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.True:
                case JsonToken.ValueSeparator:
                default:
                    break;
            }

            value = 0;
            return false;
        }

        public static bool TryGetDecimal(ref JsonReader reader, out decimal value) {
            reader.ReadNext();
            switch (reader.GetCurrentJsonToken()) {
                case JsonToken.Number:
                    value = (decimal)reader.ReadDouble();
                    return true;
                case JsonToken.String:
                    if (decimal.TryParse(reader.ReadString(), out decimal intValue)) {
                        value = intValue;
                        return true;
                    }
                    break;
                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                case JsonToken.False:
                case JsonToken.NameSeparator:
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.True:
                case JsonToken.ValueSeparator:
                default:
                    break;
            }

            value = 0;
            return false;
        }

        public static bool TryGetULong(ref JsonReader reader, out ulong value) {
            reader.ReadNext();
            switch (reader.GetCurrentJsonToken()) {
                case JsonToken.Number:
                    value = (ulong)reader.ReadDouble();
                    return true;
                case JsonToken.String:
                    if (ulong.TryParse(reader.ReadString(), out ulong intValue)) {
                        value = intValue;
                        return true;
                    }
                    break;
                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                case JsonToken.False:
                case JsonToken.NameSeparator:
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.True:
                case JsonToken.ValueSeparator:
                default:
                    break;
            }

            value = 0;
            return false;
        }

        public static bool TryGetString(ref JsonReader reader, out string value) {
            reader.ReadNext();
            switch (reader.GetCurrentJsonToken()) {
                case JsonToken.Number:
                    value = reader.ReadDouble().ToString();
                    return true;
                case JsonToken.String:
                    value = reader.ReadString();
                    return true;
                case JsonToken.True:
                case JsonToken.ValueSeparator:
                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                case JsonToken.False:
                case JsonToken.NameSeparator:
                case JsonToken.None:
                case JsonToken.Null:
                default:
                    break;
            }

            value = String.Empty;
            return false;
        }

        public static bool TryGetChar(ref JsonReader reader, out char value) {
            reader.ReadNext();
            string tempvalue;
            switch (reader.GetCurrentJsonToken()) {
                case JsonToken.Number:
                    tempvalue = reader.ReadDouble().ToString();
                    if (tempvalue.Length == 1) {
                        value = tempvalue[0];
                        return true;
                    }
                    break;
                case JsonToken.String:
                    tempvalue = reader.ReadString();
                    if (tempvalue.Length == 1) {
                        value = tempvalue[0];
                        return true;
                    }
                    break;
                case JsonToken.True:
                case JsonToken.ValueSeparator:
                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                case JsonToken.False:
                case JsonToken.NameSeparator:
                case JsonToken.None:
                case JsonToken.Null:
                default:
                    break;
            }

            value = '\0';
            return false;
        }

    }
}