using CSharpOsu.Util.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpOsu.Util.Converters
{
    // Thanks to Game4all and his circles.NET project (https://github.com/Game4all/circles.NET)
    public class ModsConvert : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Mods[]))
                return true;
            else
                return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            var encodedFlags = int.Parse((string)reader.Value);
            return GetUniqueFlags((Mods)encodedFlags);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        private Mods[] GetUniqueFlags(Mods flags)
        {
            ulong flag = 1;
            var mods = new List<Mods>();
            foreach (var value in System.Enum.GetValues(flags.GetType()).Cast<Mods>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value))
                {
                    mods.Add(value);
                }
            }

            if (mods.Contains(Mods.DoubleTime) && mods.Contains(Mods.Nightcore))
            {
                mods.RemoveAll((s) => s == Mods.DoubleTime || s == Mods.Nightcore);
                mods.Add(Mods.Nightcore);
            }
            if (mods.Contains(Mods.SuddenDeath) && mods.Contains(Mods.Perfect))
            {
                mods.RemoveAll((s) => s == Mods.SuddenDeath || s == Mods.Perfect);
                mods.Add(Mods.Perfect);
            }

            return mods.ToArray();
        }
    }
}
