using StringBuilder = System.Text.StringBuilder;

namespace Main {
    public class Menu {
        public static readonly Menu Empty;
        private static int FixedSize { get { return Console.WindowWidth; } }
        private List<string> Keys => Options.Keys.ToList();
        private int MaxLength { get { return Options.Select(opt => opt.Key.Length + opt.Value.Length).Max() + 3; } }
        private const string Banner = "\n███╗░░░███╗███████╗███╗░░██╗██╗░░░██╗\n████╗░████║██╔════╝████╗░██║██║░░░██║\n██╔████╔██║█████╗░░██╔██╗██║██║░░░██║\n██║╚██╔╝██║██╔══╝░░██║╚████║██║░░░██║\n██║░╚═╝░██║███████╗██║░╚███║╚██████╔╝\n╚═╝░░░░░╚═╝╚══════╝╚═╝░░╚══╝░╚═════╝░\n\n";

        static Menu() {
            Empty = new();
        }

        private readonly Dictionary<string, string> Options;

        private Menu() { Options = []; }

        public Menu(Dictionary<string, string> Options) {
            this.Options = Options;
        }

        public void PrintMenu(int MinSize = 0, bool Overflow = false) {
            static string GetLine(string Key, string Value, int MaxSize)
            {
                return (MaxSize - (Key.Length + Value.Length + 3)) switch
                {
                    > 2 => string.Join("", Key, " ", string.Join("", Enumerable.Repeat(".", (MaxSize - (Key.Length + Value.Length + 2)))), " ", Value),
                    2 => string.Join("", Key, "  ", Value),
                    1 => string.Join("", Key, " ", Value),
                    _ => string.Join("", Key, " . ", Value)[..MaxSize],
                };
            }
            List<string> Keys = this.Keys;
            int SBSize = new int[]{Menu.FixedSize, MinSize, (Overflow ? this.MaxLength : 0)}.Max();
            StringBuilder sb = new (SBSize, SBSize);
            Console.WriteLine(Menu.Banner);
            for (int i = 0; i < Keys.Count; i+=1) {
                sb.Insert(0, GetLine(Keys[i], Options[Keys[i]], SBSize));
                Console.WriteLine(sb.ToString());
                sb.Clear();
            }
        }

        public string ReadMenu(int MinSize = 0, bool Overflow = false) {
            PrintMenu(MinSize, Overflow);
            return Console.ReadLine() ?? "";
        }
    }
}