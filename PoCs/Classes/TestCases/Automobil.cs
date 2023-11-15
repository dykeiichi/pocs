using JsonReader = Utf8Json.JsonReader;
using JsonToken = Utf8Json.JsonToken;
using JsonResolver = PoCs.Functions.JsonResolver;

namespace PoCs.Classes.TestCases{

	public class Automobil {
		public static readonly Automobil Empty;

		static Automobil() {
			Empty = new Automobil();
        }

        public string Color;
		public uint Doors;
		public string Branch;
		public uint HPs;
		public Tire Tires;
		public Headligth Headlights;

        public Automobil(string Color, uint Doors, string Branch,
						 uint HPs, Tire Tires, Headligth Headlights) {
			this.Color = Color;
			this.Doors = Doors;
			this.Branch = Branch;
			this.HPs = HPs;
			this.Tires = Tires;
			this.Headlights = Headlights;
        }

        private Automobil(): this("", 0, "", 0, Tire.Empty, Headligth.Empty) { }

        public static bool TryParse(Stream jsonData, out Automobil automobil) {
            //Obtener Stream y usar JsonReader
            MemoryStream stream = new MemoryStream();
            jsonData.CopyTo(stream);
            JsonReader reader = new JsonReader(stream.ToArray());

            return TryParse(ref reader, out automobil);
        }

        public static bool TryParse(MemoryStream jsonData, out Automobil automobil) {
            JsonReader reader = new JsonReader(jsonData.ToArray());
            return TryParse(ref reader, out automobil);
        }

        public static bool TryParse(ref JsonReader reader, out Automobil automobil) {

            //Separar memoria para las variables
            automobil = new Automobil();

            if (reader.GetCurrentJsonToken() == JsonToken.BeginObject) {
                while (reader.GetCurrentJsonToken() != JsonToken.EndObject) {
                    reader.ReadNext();
                    switch (reader.GetCurrentJsonToken()) {
                        case JsonToken.String:
                            switch (reader.ReadString()) {
                                case "Color":
                                    if (JsonResolver.TryGetString(ref reader, out automobil.Color)) {
                                        continue;
                                    }
                                    automobil = Empty;
                                    return false;
                                case "Doors":
                                    if (JsonResolver.TryGetUInt(ref reader, out automobil.Doors)) {
                                        continue;
                                    }
                                    automobil = Empty;
                                    return false;
                                case "Branch":
                                    if (JsonResolver.TryGetString(ref reader, out automobil.Branch)) {
                                        continue;
                                    }
                                    automobil = Empty;
                                    return false;
                                case "HPs":
                                    if (JsonResolver.TryGetUInt(ref reader, out automobil.HPs)) {
                                        continue;
                                    }
                                    automobil = Empty;
                                    return false;
                                case "Tires":
                                    while (reader.GetCurrentJsonToken() != JsonToken.BeginObject) {
                                        reader.ReadNext();
                                    }
                                    if (Tire.TryParse(ref reader, out automobil.Tires)) {
                                        if (reader.GetCurrentJsonToken() == JsonToken.EndObject) {
                                            reader.ReadNext();
                                        }
                                        continue;
                                    }
                                    automobil = Empty;
                                    return false;
                                case "Headlights":
                                    while (reader.GetCurrentJsonToken() != JsonToken.BeginObject) {
                                        reader.ReadNext();
                                    }
                                    if (Headligth.TryParse(ref reader, out automobil.Headlights)) {
                                        if (reader.GetCurrentJsonToken() == JsonToken.EndObject) {
                                            reader.ReadNext();
                                        }
                                        continue;
                                    }
                                    automobil = Empty;
                                    return false;
                                default:
                                    break;
                            }
                            break;
                        case JsonToken.None:
                            automobil = Empty;
                            return false;
                        default:
                            continue;
                    }

                }
            }
            return true;
        }

    }

	public class Tire {

        public static readonly Tire Empty;

        static Tire() {
            Empty = new Tire();
        }

        public uint Width;
		public uint AspectRatio;
		public char Architecture;
		public uint Diameter;
		public uint LoadIndex;
		public char SpeedRating;
		
		public Tire(uint Width, uint AspectRatio, char Architecture,
						 uint Diameter, uint LoadIndex,
						 char SpeedRating) {
			this.Width = Width;
			this.AspectRatio = AspectRatio;
			this.Architecture = Architecture;
			this.Diameter = Diameter;
			this.LoadIndex = LoadIndex;
			this.SpeedRating = SpeedRating;
        }

        private Tire() : this(0, 0, '\0', 0, 0, '\0') { }

        public static bool TryParse(Stream jsonData, out Tire tire) {
            //Obtener Stream y usar JsonReader
            MemoryStream stream = new MemoryStream();
            jsonData.CopyTo(stream);
            JsonReader reader = new JsonReader(stream.ToArray());

            return TryParse(ref reader, out tire);
        }

        public static bool TryParse(MemoryStream jsonData, out Tire tire) {
            JsonReader reader = new JsonReader(jsonData.ToArray());
            return TryParse(ref reader, out tire);
        }

        public static bool TryParse(ref JsonReader reader, out Tire tire) {
            //Separar memoria para las variables
            tire = new Tire();

            if (reader.GetCurrentJsonToken() == JsonToken.BeginObject) {
                while (reader.GetCurrentJsonToken() != JsonToken.EndObject) {
                    reader.ReadNext();
                    switch (reader.GetCurrentJsonToken()) {
                        case JsonToken.String:
                            switch (reader.ReadString()) {
                                case "Width":
                                    if (JsonResolver.TryGetUInt(ref reader, out tire.Width)) {
                                        continue;
                                    }
                                    tire = Empty;
                                    return false;
                                case "AspectRatio":
                                    if (JsonResolver.TryGetUInt(ref reader, out tire.AspectRatio)) {
                                        continue;
                                    }
                                    tire = Empty;
                                    return false;
                                case "Architecture":
                                    if (JsonResolver.TryGetChar(ref reader, out tire.Architecture)) {
                                        continue;
                                    }
                                    tire = Empty;
                                    return false;
                                case "Diameter":
                                    if (JsonResolver.TryGetUInt(ref reader, out tire.Diameter)) {
                                        continue;
                                    }
                                    tire = Empty;
                                    return false;
                                case "LoadIndex":
                                    if (JsonResolver.TryGetUInt(ref reader, out tire.LoadIndex)) {
                                        continue;
                                    }
                                    tire = Empty;
                                    return false;
                                case "SpeedRating":
                                    if (JsonResolver.TryGetChar(ref reader, out tire.SpeedRating)) {
                                        continue;
                                    }
                                    tire = Empty;
                                    return false;
                                default:
                                    break;
                            }
                            break;
                        case JsonToken.None:
                            tire = Empty;
                            return false;
                        default:
                            continue;
                    }

                }
            }
            return true;
        }

    }

	public class Headligth {
        public static readonly Headligth Empty;

        static Headligth() {
            Empty = new Headligth();
        }

        public string Type;
		public uint Watts;
		public uint Voltage;

		public Headligth(string Type, uint Watts, uint Voltage) {
			this.Type = Type;
			this.Watts = Watts;
			this.Voltage = Voltage;
        }

        private Headligth() : this("", 0, 0) { }

        public static bool TryParse(Stream jsonData, out Headligth headligth) {
            //Obtener Stream y usar JsonReader
            MemoryStream stream = new MemoryStream();
            jsonData.CopyTo(stream);
            JsonReader reader = new JsonReader(stream.ToArray());

            return TryParse(ref reader, out headligth);
        }

        public static bool TryParse(MemoryStream jsonData, out Headligth headligth) {
            JsonReader reader = new JsonReader(jsonData.ToArray());
            return TryParse(ref reader, out headligth);
        }

        public static bool TryParse(ref JsonReader reader, out Headligth headligth) {
            //Separar memoria para las variables
            headligth = new Headligth();

            if (reader.GetCurrentJsonToken() == JsonToken.BeginObject) {
                while (reader.GetCurrentJsonToken() != JsonToken.EndObject) {
                    reader.ReadNext();
                    switch (reader.GetCurrentJsonToken()) {
                        case JsonToken.String:
                            switch (reader.ReadString()) {
                                case "Type":
                                    if (JsonResolver.TryGetString(ref reader, out headligth.Type)) {
                                        continue;
                                    }
                                    headligth = Empty;
                                    return false;
                                case "Watts":
                                    if (JsonResolver.TryGetUInt(ref reader, out headligth.Watts)) {
                                        continue;
                                    }
                                    headligth = Empty;
                                    return false;
                                case "Voltage":
                                    if (JsonResolver.TryGetUInt(ref reader, out headligth.Voltage)) {
                                        continue;
                                    }
                                    headligth = Empty;
                                    return false;
                                default:
                                    break;
                            }
                            break;
                        case JsonToken.None:
                            headligth = Empty;
                            return false;
                        default:
                            continue;
                    }

                }
            }
            return true;
        }
    }
}