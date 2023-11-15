using BenchmarkRunner = BenchmarkDotNet.Running.BenchmarkRunner;
using Main.Benchmarks;
using User = PoCs.Classes.TestCases.User;
using PoCs.Classes.Security;
using Encoding = System.Text.Encoding;

namespace Main {

    public static class MainClass {

        private static readonly Dictionary<string, string> Options = new() {
            {"s", "Salir"},
            {"1", "[Benchmark] Json Serialization"},
            {"2", "Create User"},
            {"3", "Sign Up User"},
            {"4", "Sign In User"}
        };
        private static readonly Menu Menu = new (MainClass.Options);

        static int Main(string[] _) {
            Func<object?> Continue = (() => { Console.WriteLine("Press any key to continue..."); Console.ReadKey(); return null; });
            Console.Clear();
            string choice = Menu.ReadMenu();
            Console.Clear();
            while (!choice.Equals("s"))
            {
                string username;
                string password;
                switch (choice)
                {
                    case "1":
                        BenchmarkRunner.Run<BenchmarkJsonSerialization>();
                        break;
                    case "2":
                        byte[] Salt = NaClLibrary.CreateSalt();
                        Console.WriteLine(
                            new User(
                                "dykeiichi",
                                NaClLibrary.HashPassword("A5Up3r53Cr3tP45w0rD", Salt),
                                Salt,
                                JSONWebToken.GenerateKey(),
                                Encoding.UTF8.GetBytes("A5Up3r53Cr3t53Cr3t")
                            )
                        );
                        break;
                    case "3":
                        Console.WriteLine("Set username");
                        username = Console.ReadLine() ?? "";
                        Console.WriteLine("Set password");
                        password = Console.ReadLine() ?? "";
                        if (User.SignUp(username, password, out User SignUpUser))
                        {
                            Console.WriteLine(SignUpUser);
                            break;
                        }
                        Console.WriteLine("Error");
                        break;
                    case "4":
                        Console.WriteLine("Type your registered username");
                        username = Console.ReadLine() ?? "";
                        Console.WriteLine("Type your password");
                        password = Console.ReadLine() ?? "";
                        if (User.ReadFromDB(username, out User SignInUser))
                        {
                            if (SignInUser.Login(password, out string jwtToken))
                            {
                                Console.WriteLine("Succesfull Signed In, your JWT Token is: {0}", jwtToken);
                                break;
                            }
                            Console.WriteLine("Error, your password must be incorrect");
                            break;
                        }
                        Console.WriteLine("Error, user not found on DB");
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