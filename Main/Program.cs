using BenchmarkRunner = BenchmarkDotNet.Running.BenchmarkRunner;
using Main.Benchmarks;

namespace Main {

    public static class MainClass {

        private static readonly Dictionary<string, string> Options = new() {
            {"s", "Salir"},
            {"1", "[Benchmark] Json Serialization"}
        };
        private static readonly Menu Menu = new (MainClass.Options);

        static int Main(string[] _) {
            Func<object?> Continue = (() => { Console.WriteLine("Press any key to continue..."); Console.ReadKey(); return null; });
            Console.Clear();
            string choice = Menu.ReadMenu();

            while (!choice.Equals("s")) {
                switch (choice) {
                    case "1":
                        BenchmarkRunner.Run<BenchmarkJsonSerialization>();
                        break;
                    case "s":
                    default:
                        break;
                }
                Continue();
                Console.Clear();
                choice = Menu.ReadMenu();
                Console.Clear();
            }
            return 0;
        }
    }
}