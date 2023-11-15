using BenchmarkDotNet.Attributes;
using PoCs.Classes.TestCases;
using JObject = Newtonsoft.Json.Linq.JObject;
using JToken = Newtonsoft.Json.Linq.JToken;
using JsonTextReader = Newtonsoft.Json.JsonTextReader;

namespace Main.Benchmarks {
    [MemoryDiagnoser]
    public class BenchmarkJsonSerialization {
        public const string StringData = @"{""Color"":""Rojo"",""Doors"":0,""Branch"":""Acura"",""HPs"":350,""Tires"":{""Width"":220,""AspectRatio"":55,""Architecture"":""R"",""Diameter"":17,""LoadIndex"":125,""SpeedRating"":""Z""},""Headlights"":{""Type"":""Led"",""Watts"":75,""Voltage"":12}}";
        public static readonly MemoryStream memoryStreamData;

        static BenchmarkJsonSerialization() {
            memoryStreamData = new MemoryStream();
            StreamWriter writer = new (memoryStreamData);
            writer.Write(StringData);
            writer.Flush();
            memoryStreamData.Position = 0;
        }


        [Benchmark(Baseline = true)]
        public void ReadObjectFromStringWithNewtonsoft() {
            JObject jObject = JObject.Parse(StringData) ?? new JObject();
            Automobil _ = jObject.ToObject<Automobil>() ?? Automobil.Empty;
        }

        [Benchmark]
        public void ReadObjectFromStringWithUtf8Json() {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(StringData);
            writer.Flush();
            stream.Position = 0;
            if (!Automobil.TryParse(memoryStreamData, out Automobil _)) {
                throw new Exception("Some Exception");
            }
        }

        [Benchmark]
        public void ReadObjectFromMemoryStreamWithNewtonsoft() {
            try {
                StreamReader streamReader = new (memoryStreamData);
                JsonTextReader reader = new (streamReader);
                JObject jObject = (JObject)(JToken.ReadFrom(reader) ?? new JObject());
                Automobil auto = jObject.ToObject<Automobil>() ?? Automobil.Empty;
                memoryStreamData.Position = 0;
            } catch {
                throw;
            } finally {
                memoryStreamData.Position = 0;
            }
        }

        [Benchmark]
        public void ReadObjectMemoryStreamWithUtf8Json() {
            if (!Automobil.TryParse(memoryStreamData, out Automobil _)) {
                throw new Exception("Some Exception");
            }
        }
    }
}