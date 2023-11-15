using ECDsa = System.Security.Cryptography.ECDsa;
using ECCurve = System.Security.Cryptography.ECCurve;
using PbeParameters = System.Security.Cryptography.PbeParameters;
using PbeEncryptionAlgorithm = System.Security.Cryptography.PbeEncryptionAlgorithm;
using HashAlgorithmName = System.Security.Cryptography.HashAlgorithmName;
using JwtSecurityTokenHandler = System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler;
using SecurityTokenDescriptor = Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor;
using ClaimsIdentity = System.Security.Claims.ClaimsIdentity;
using Claim = System.Security.Claims.Claim;
using ClaimTypes = System.Security.Claims.ClaimTypes;
using SigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;
using ECDsaSecurityKey = Microsoft.IdentityModel.Tokens.ECDsaSecurityKey;
using SecurityAlgorithms = Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using TokenValidationParameters = Microsoft.IdentityModel.Tokens.TokenValidationParameters;
using SecurityToken = Microsoft.IdentityModel.Tokens.SecurityToken;

namespace PoCs.Classes.Security {
    public class JSONWebToken {
        public const string EncryptionPassowrd = "8zgXbK9atdKjjVJ/XSCdEg==";  // This Base64 password is just for PoC purposes, don't use this one on a production project
        public const int Exp = 300; // expiracion en segundos
        public const string Iss = "issuer";
        public const string Aud = "audience";

        public static byte[] GenerateKey() {
            ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            return key.ExportEncryptedPkcs8PrivateKey(EncryptionPassowrd, new PbeParameters(PbeEncryptionAlgorithm.Aes256Cbc, HashAlgorithmName.SHA512, 100));
        }

        public static string GenerateToken(string user, byte[]? password = null) {
            password ??= GenerateKey();

            ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            key.ImportEncryptedPkcs8PrivateKey(EncryptionPassowrd, new ReadOnlySpan<byte>(password), out _);

            DateTime Now = DateTime.UtcNow;

            JwtSecurityTokenHandler tokenHandler = new ();
            SecurityTokenDescriptor tokenDescriptor = new () {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new (ClaimTypes.NameIdentifier, user),
                }),
                Expires = Now.AddSeconds(Exp),
                Issuer = Iss,
                Audience = Aud,
                SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(key), SecurityAlgorithms.EcdsaSha512Signature),
                NotBefore = Now
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateCurrentToken(string token, byte[] secret) {
            ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            key.ImportEncryptedPkcs8PrivateKey(EncryptionPassowrd, new ReadOnlySpan<byte>(secret), out _);

            JwtSecurityTokenHandler tokenHandler = new ();
            try {
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Iss,
                    ValidAudience = Aud,
                    IssuerSigningKey = new ECDsaSecurityKey(key)
                }, out SecurityToken validatedToken);
            } catch {
                return false;
            }
            return true;
        }
    }
}