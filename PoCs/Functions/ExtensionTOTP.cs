using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOTP = OtpNet.Totp;

namespace PoCs.Functions
{
    public static class ExtensionTOTP {
        public static bool ValidateTOTP(this TOTP Self, string totp) {
            return Self.ComputeTotp().Equals(totp);
        }

        public static bool ValidateTOTP(this TOTP Self, uint totp) {
            return Self.ComputeTotp().Equals(totp.ToString());
        }

    }
}