using System.Text;
namespace SteamShortcut.VdfHelper
{
    public enum VDFType
    {
        MapStart = 0x00,
        MapEnd = 0x08,
        Integer = 0x02,
        String = 0x01,
    }
    
    public abstract class VDFBaseType
    {
        public VDFType Type { get; set; }
        public uint Integer { get; set; }
        public string Text { get; set; }
        public Dictionary<String, VDFBaseType> Map { get; protected set; }

        public abstract void Write(BinaryWriter writer, string key);

        public VDFMap ToMap()
        {
            if (GetType() == typeof(VDFMap))
            {
                return (VDFMap)this;
            }

            return null;
        }
    }
    
    public class VDFInteger : VDFBaseType
    {
        public VDFInteger(VDFStream stream)
        {
            Type = VDFType.Integer;
            Integer = stream.ReadInteger();
        }

        public override void Write(BinaryWriter writer, string key)
        {
            writer.Write((byte)Type);
            writer.Write(Encoding.UTF8.GetBytes(key));
            writer.Write((byte)0);
            writer.Write(Integer);
        }

        public VDFInteger(uint value)
        {
            Type = VDFType.Integer;
            Integer = value;
        }
    }
    public class VDFMap : VDFBaseType
    {
        private static Random random = new Random();

        public VDFMap(VDFStream stream)
        {
            Type = VDFType.MapStart;
            Map = new Dictionary<string, VDFBaseType>();

            while (true)
            {
                byte op = stream.ReadByte();

                if (op == (byte)VDFType.MapEnd)
                    break;

                string key = stream.ReadString();

                VDFBaseType value;
                if (op == (byte)VDFType.MapStart)
                    value = new VDFMap(stream);
                else if (op == (byte)VDFType.Integer)
                    value = new VDFInteger(stream);
                else if (op == (byte)VDFType.String)
                    value = new VDFString(stream);
                else
                    throw new Exception("Unknown opcode");

                Map.Add(key, value);
            }
        }

        public VDFMap()
        {
            Type = VDFType.MapStart;
            Map = new Dictionary<string, VDFBaseType>();
        }

        public void FillWithDefaultShortcutEntry()
        {
            Map.Add("appid", new VDFInteger((uint)random.Next()));
            Map.Add("appName", new VDFString("appName"));
            Map.Add("exe", new VDFString(""));
            Map.Add("StartDir", new VDFString("."));
            Map.Add("icon", new VDFString(""));
            Map.Add("ShortcutPath", new VDFString(""));
            Map.Add("LaunchOptions", new VDFString(""));
            Map.Add("isHidden", new VDFInteger(0));
            Map.Add("AllowDesktopConfig", new VDFInteger(1));
            Map.Add("AllowOverlay", new VDFInteger(1));
            Map.Add("openvr", new VDFInteger(0));
            Map.Add("Devkit", new VDFInteger(0));
            Map.Add("DevkitGameID", new VDFString("0"));
            Map.Add("DevkitOverrideAppID", new VDFInteger(0));
            Map.Add("LastPlayTime", new VDFInteger(0));
            Map.Add("tags", new VDFMap());
        }

        public void RemoveFromArray(int idx)
        {
            Map.Remove(idx.ToString());

            Dictionary<string, VDFBaseType> newMap = new Dictionary<string, VDFBaseType>();

            int i = 0;
            foreach (KeyValuePair<string, VDFBaseType> kv in Map)
            {
                newMap.Add(i.ToString(), kv.Value);
                i++;
            }

            Map = newMap;
        }

        public override void Write(BinaryWriter writer, string key)
        {
            if (key != null)
            {
                writer.Write((byte)Type);
                writer.Write(Encoding.UTF8.GetBytes(key));
                writer.Write((byte)0);
            }
            
            foreach (KeyValuePair<String, VDFBaseType> keyValue in Map)
            {
                keyValue.Value.Write(writer, keyValue.Key);
            }

            writer.Write((byte)VDFType.MapEnd);
        }

        public VDFBaseType GetValue(string key)
        {
            if (Map.ContainsKey(key))
                return Map[key];

            string temp = char.ToUpper(key[0]) + key.Substring(1);

            if (Map.ContainsKey(temp))
                return Map[temp];

            return null;
        }
        public int GetSize() => Map.Count;
    }
    
    public class VDFStream
    {
        private BinaryReader reader;

        public VDFStream(string path)
        {
            reader = new BinaryReader(new FileStream(path, FileMode.Open));
        }

        public void Close() => reader.Close();

        public string ReadString()
        {
            List<byte> text = new();
            while (true)
            {
                byte c = ReadByte();
                if (c == 0) break;
                text.Add(c);
            }

            return Encoding.UTF8.GetString(text.ToArray());
        }

        public uint ReadInteger()
        {
            return reader.ReadUInt32();
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }
    }
    
    public class VDFString : VDFBaseType
    {
        public VDFString(VDFStream stream)
        {
            Type = VDFType.String;
            Text = stream.ReadString();
        }

        public VDFString(string text)
        {
            Type = VDFType.String;
            Text = text;
        }

        public override void Write(BinaryWriter writer, string key)
        {
            writer.Write((byte)Type);
            writer.Write(Encoding.UTF8.GetBytes(key));
            writer.Write((byte)0);
            writer.Write(Encoding.UTF8.GetBytes(Text));
            writer.Write((byte)0);
        }
    }
}
