using DllImport = System.Runtime.InteropServices.DllImportAttribute;
using CallingConvention = System.Runtime.InteropServices.CallingConvention;
using StringBuilder = System.Text.StringBuilder;

namespace PoCs.Classes.Security {
	public class NaClLibrary {
        private const string Name = "libsodium";
        public static int id_ALG_ARGON2ID13 = 2;
        public static long OPSLIMIT_SENSITIVE = 2;
        public static int MEMLIMIT_SENSITIVE = 19922944;

        static NaClLibrary() {
            sodium_init();
        }

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sodium_init();

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void randombytes_buf(byte[] buffer, int size);

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_pwhash(byte[] buffer, long bufferLen, byte[] password, long passwordLen, byte[] salt, long opsLimit, int memLimit, int alg);

        public static byte[] CreateSalt(int bytes_size = 16) {
            byte[] buffer = new byte[bytes_size];
            randombytes_buf(buffer, buffer.Length);
            return buffer;
        }

        public static byte[] HashPassword(string password, byte[] salt, int bytes_size = 16) {
            byte[] hash = new byte[bytes_size];

            int result = crypto_pwhash(
                hash,
                hash.Length,
                System.Text.Encoding.UTF8.GetBytes(password),
                password.Length,
                salt,
                OPSLIMIT_SENSITIVE,
                MEMLIMIT_SENSITIVE,
                id_ALG_ARGON2ID13
                );

            if (result != 0)
                throw new Exception("An unexpected error has occurred.");
            using (System.Security.Cryptography.SHA512 hash_512 = System.Security.Cryptography.SHA512.Create()) {
                byte[] hashedInputBytes = hash_512.ComputeHash(hash);
                StringBuilder hashedInputStringBuilder = new StringBuilder(128);
                foreach (byte b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return System.Text.Encoding.UTF8.GetBytes(hashedInputStringBuilder.ToString());
            }
        }

        public static bool VerifyHash(string password, byte[] salt, byte[] hash) {
            byte[] newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }
    }
}