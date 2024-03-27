using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL.Encriptacion
{
    public class Encript
    {


        public static string GetToken(string password)
        {
            string stringConcar =  password;

            byte[] bytes = Encoding.UTF8.GetBytes(stringConcar);
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(bytes);
                string token = EncodeToBase64(hashBytes);

                return token.Substring(0, 23);
            }
        }

        private static string EncodeToBase64(byte[] toEncode)
        {
            int len = toEncode.Length;
            int len64, groups, remainder, pos64, pos256, pos;
            byte[] rpta;

            groups = len / 3;
            remainder = len % 3;
            len64 = groups;
            if (remainder > 0)
                len64 = len64 + 1;

            rpta = new byte[len64 * 4];
            pos64 = 0;
            pos256 = 0;

            // Generates complete groups
            for (int i = 0; i < groups; i++)
            {
                pos = (toEncode[pos256] & 252) >> 2;
                rpta[pos64++] = (byte)mapa[pos];

                pos = ((toEncode[pos256++] & 3) << 4);
                pos = pos + ((toEncode[pos256] & 240) >> 4);
                rpta[pos64++] = (byte)mapa[pos];

                pos = ((toEncode[pos256++] & 15) << 2);
                pos = pos + ((toEncode[pos256] & 192) >> 6);
                rpta[pos64++] = (byte)mapa[pos];

                pos = toEncode[pos256++] & 63;
                rpta[pos64++] = (byte)mapa[pos];
            }

            if (remainder == 1)
            {
                pos = (toEncode[pos256] & 252) >> 2;
                rpta[pos64++] = (byte)mapa[pos];

                pos = ((toEncode[pos256++] & 3) << 4);
                rpta[pos64++] = (byte)mapa[pos];

                rpta[pos64++] = (byte)'=';
                rpta[pos64++] = (byte)'=';
            }
            else if (remainder == 2)
            {
                pos = (toEncode[pos256] & 252) >> 2;
                rpta[pos64++] = (byte)mapa[pos];

                pos = ((toEncode[pos256++] & 3) << 4);
                pos = pos + ((toEncode[pos256] & 240) >> 4);
                rpta[pos64++] = (byte)mapa[pos];

                pos = ((toEncode[pos256++] & 15) << 2);
                rpta[pos64++] = (byte)mapa[pos];

                rpta[pos64++] = (byte)'=';
            }

            return Encoding.ASCII.GetString(rpta, 0, pos64);
        }

        private static readonly char[] mapa = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();

    }
}
