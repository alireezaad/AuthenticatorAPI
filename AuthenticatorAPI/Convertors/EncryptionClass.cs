using System;
using System.Text;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.IO;
using System.Collections;  
using System.Security.Claims;

namespace AuthenticatorAPI.Convertors
{
    public partial class Encription
    {
        private static string vPP = "449#463#474#463#475#471#432#467#470#432#477#487#467#441#470#477#481#470#467#431#481#470#467#476#463#467#471";
        private static string vSV = "441#463#480#471#475#431#464#466#477#474#440#463#464#464#463#480";
        private static string vHA = "443#434#419";
        private static int vPI = 2;
        private static string vIV = "@1@2@3@4@5@6@7@8";
        private static int vKS = 256;

        public static string PP
        {
            get
            {
                string s = vPP;
                string s3 = "";
                string vPPFinal = "";

                for (int i = 0; i <= s.Length - 1; i++)
                {
                    if (s[i].ToString() == "#")
                    {
                        vPPFinal = vPPFinal + (char)((int.Parse(s3) - 73 + 21 - 423 + 220 - 111));
                        s3 = "";
                    }
                    else
                        s3 = s3 + s[i];

                    if (i == s.Length - 1)
                        vPPFinal = vPPFinal + (char)((int.Parse(s3) - 73 + 21 - 423 + 220 - 111));
                }

                return vPPFinal;
            }
            set { vPP = value; }
        }

        public static string SV
        {
            get
            {
                string s = vSV;
                string s3 = "";
                string vSVFinal = "";

                for (int i = 0; i <= s.Length - 1; i++)
                {
                    if (s[i].ToString() == "#")
                    {
                        vSVFinal = vSVFinal + (char)((int.Parse(s3) - 73 + 21 - 423 + 220 - 111));
                        s3 = "";
                    }
                    else
                        s3 = s3 + s[i];

                    if (i == s.Length - 1)
                        vSVFinal = vSVFinal + (char)((int.Parse(s3) - 73 + 21 - 423 + 220 - 111));
                }

                return vSVFinal;
            }
            set { vSV = value; }
        }

        public static string HA
        {
            get
            {
                string s = vHA;
                string s3 = "";
                string vHAFinal = "";
                for (int i = 0; i <= s.Length - 1; i++)
                {
                    if (s[i].ToString() == "#")
                    {
                        vHAFinal = vHAFinal + (char)((int.Parse(s3) - 73 + 21 - 423 + 220 - 111));
                        s3 = "";
                    }
                    else
                        s3 = s3 + s[i];

                    if (i == s.Length - 1)
                        vHAFinal = vHAFinal + (char)((int.Parse(s3) - 73 + 21 - 423 + 220 - 111));
                }

                return vHAFinal;
            }
            set { vHA = value; }
        }

        public static int PI
        {
            get { return vPI; }
            set { vPI = value; }
        }

        public static string IV
        {
            get { return vIV; }
            set { vIV = value; }
        }

        public static int KS
        {
            get { return vKS; }
            set { vKS = value; }
        }

        public static string Encrypt(string plainText)
        {
            byte[] IVBytes = Encoding.ASCII.GetBytes(IV);
            byte[] SVBytes = Encoding.ASCII.GetBytes(SV);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            PP,
                                                            SVBytes,
                                                            HA,
                                                            PI);

            byte[] keyBytes = password.GetBytes(KS / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             IVBytes);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherTextBytes);

            string cipherTextFinal = EncryptDll.EncrptDll_Class.Encrypt(cipherText);
            return cipherTextFinal;
        }

        public static string Decrypt(string cipherTextFinal)
        {
            string cipherText = EncryptDll.EncrptDll_Class.Decrypt(cipherTextFinal);

            byte[] IVBytes = Encoding.ASCII.GetBytes(IV);
            byte[] SVBytes = Encoding.ASCII.GetBytes(SV);

            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            PP,
                                                            SVBytes,
                                                            HA,
                                                            PI);

            byte[] keyBytes = password.GetBytes(KS / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             IVBytes);

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);

            return plainText;
        }

        public static Boolean CompareDecrypt(string TextNoEncrypted, string TextEncrypted)
        {
            string cipherText = Encrypt(TextNoEncrypted);
            if (cipherText != TextEncrypted)
                return false;
            else
                return true;
        }

        private static string Codes64 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/";
        private static Int64[] Crc32Table;// : Array[0 ..255] of Integer ;

        private static string MakeRNDString(string Chars, int Count, int Ran)
        {
            int i, x;
            string Result = "";
            Random r = new Random();
            for (i = 0; i <= Count - 1; i++)
            {
                x = Chars.Length - /*r.Next(Chars.Length - 1)*/ Ran;
                Result = Result + Chars[x - 1];
                Chars = Chars.Substring(0, x - 1) + Chars.Substring(x);
            }

            return Result;
        }

        public static string EncodePWDEx(string Data, string SecurityString, int MinV, int MaxV, int Ran1, int Ran2, int Ran3)
        {
            int i, x;
            string s1, s2, ss;
            string Result;
            Random r = new Random();

            if (MinV > MaxV)
            {
                i = MinV;
                MinV = MaxV;
                MaxV = i;
            }

            if (MinV < 0) MinV = 0;
            if (MaxV > 100) MaxV = 100;
            Result = "";

            if (SecurityString.Length < 16) return "";
            for (i = 0; i <= SecurityString.Length - 1; i++)
            {
                s1 = SecurityString.Substring(i + 1);
                if (s1.IndexOf(SecurityString[i]) >= 0) return "";
                if (Codes64.IndexOf(SecurityString[i]) < 0) return "";
            }

            s1 = Codes64;
            s2 = "";
            for (i = 0; i <= SecurityString.Length - 1; i++)
            {
                x = s1.IndexOf(SecurityString[i]);
                if (x > 0)
                    s1 = s1.Substring(0, x) + s1.Substring(x + 1);
            }

            Encoding ascii = Encoding.Default;
            ss = SecurityString;
            for (i = 0; i <= Data.Length - 1; i++)
            {
                s2 = s2 + ss[(int)SqlInt32.Mod(((byte)((ascii.GetBytes(new char[] { Data[i] })[0]))), 16)];
                ss = ss.Substring(ss.Length - 1) + ss.Substring(0, ss.Length - 1);
                s2 = s2 + ss[(int)SqlInt32.Divide(((byte)((ascii.GetBytes(new char[] { Data[i] })[0]))), 16)];
                ss = ss.Substring(ss.Length - 1) + ss.Substring(0, ss.Length - 1);
            }

            int rCount1 = /*r.Next(MaxV - MinV)*/ Ran1;
            Result = MakeRNDString(s1, rCount1 + MinV + 1, Ran3);
            for (i = 0; i <= s2.Length - 1; i++)
            {
                Result = Result + s2[i] + MakeRNDString(s1, /*r.Next(MaxV - MinV)*/Ran2 + MinV, Ran3);
            }

            /*textBox23.Text = DecodePWDEx(Result, "u1sSDC+3eAOBT6/fYM0FR");
            if (textBox23.Text.Trim() != textBox21.Text.Trim())
                textBox23.Text = DecodePWDEx(textBox22.Text, "u1sSDC+3eAOBT6/fYM0FR");*/


            return Result;
        }

        public static string DecodePWDEx(string Data, string SecurityString)
        {
            int i, x, x2;
            string s1, s2, ss;
            string Result;
            Encoding ascii = Encoding.Default;

            Result = "";
            if (SecurityString.Length < 16) return "";
            for (i = 0; i <= SecurityString.Length - 1; i++)
            {
                s1 = SecurityString.Substring(i + 1);
                if (s1.IndexOf(SecurityString[i]) >= 0) return "";
                if (Codes64.IndexOf(SecurityString[i]) < 0) return "";
            }
            s1 = Codes64;
            s2 = "";
            ss = SecurityString;

            for (i = 0; i <= Data.Length - 1; i++)
                if (ss.IndexOf(Data[i]) >= 0)
                    s2 = s2 + Data[i];

            Data = s2;
            s2 = "";
            if (SqlInt32.Mod(Data.Length, 2) != 0) return "";

            for (i = 0; i <= SqlInt32.Divide(Data.Length, 2) - 1; i++)
            {
                x = ss.IndexOf(Data[i * 2]);

                if (x < 0) return "";
                ss = ss.Substring(ss.Length - 1) + ss.Substring(0, ss.Length - 1);

                x2 = ss.IndexOf(Data[i * 2 + 1]);
                if (x2 < 0) return "";

                x = x + x2 * 16;

                s2 = s2 + (ascii.GetString(new byte[] { (byte)x }));
                ss = ss.Substring(ss.Length - 1) + ss.Substring(0, ss.Length - 1);
            }

            Result = s2;
            return Result;

        }

        public static void InitCrc32()
        {
            Crc32Table = new Int64[256];
            ulong Seed, L9, L8, CRC, TempCRC;
            Seed = 0xEDB80000;
            CRC = 0;
            for (L9 = 0; L9 <= 255; L9++)
            {
                CRC = L9;
                for (L8 = 0; L8 <= 7; L8++)
                {
                    TempCRC = CRC >> 1;

                    if ((Int64)(CRC & 1) != 0)
                        CRC = TempCRC ^ Seed;
                    else
                        CRC = TempCRC;
                }

                /*                 if (L9 == 128)
                                     Crc32Table[L9] = Int64.Parse(CRC.ToString());*/
                Crc32Table[L9] = Int64.Parse(CRC.ToString());
            }

        }

        public static ulong Crc32(string s, ulong Seed)
        {
            ulong ByteVal, CRC, AccValue, Q, Index;
            int L9;

            CRC = Seed;

            Encoding ascii = Encoding.Default;

            for (L9 = 0; L9 <= s.Length - 1; L9++)
            {

                ByteVal = ((byte)((ascii.GetBytes(new char[] { s[L9] })[0])));
                AccValue = CRC >> 8;

                Index = CRC & 0xFF;
                Index = Index ^ ByteVal;

                Q = (ulong)Crc32Table[Index];

                CRC = AccValue ^ Q;
            }

            return CRC;
        }

        //Start Add Naderi(138) PayamResani --Attach
        public static string Encode(string EncodeType, int ID, string Name)
        {
            //string CodeTypeNumber;

            EncodeType = EncodeType.ToUpper();

            InitCrc32();
            string CodeType = (Crc32(EncodeType, 0xE85746)).ToString();

            while (CodeType.Length < 10)

                CodeType = "0" + CodeType;

            //InitCrc32();
            string IDNumber = ID.ToString();

            while (IDNumber.Length < 10)

                IDNumber = "0" + IDNumber;

            InitCrc32();
            string NameNumber = (Crc32(Name, 0xE87654)).ToString();

            while (NameNumber.Length < 10)

                NameNumber = "0" + NameNumber;

            return (IDNumber[1].ToString() + IDNumber[4].ToString() + NameNumber[5].ToString() + CodeType[2].ToString() + IDNumber[9].ToString() + IDNumber[0].ToString() + NameNumber[8].ToString() + CodeType[6].ToString() + NameNumber[7].ToString() +
                NameNumber[6].ToString() + CodeType[0].ToString() + IDNumber[8].ToString() + CodeType[7].ToString() + CodeType[1].ToString() + NameNumber[4].ToString() + NameNumber[9].ToString() + IDNumber[7].ToString() + CodeType[9].ToString() +
                CodeType[5].ToString() + NameNumber[3].ToString() + NameNumber[0].ToString() + IDNumber[2].ToString() + CodeType[4].ToString() + IDNumber[3].ToString() +
                IDNumber[6].ToString() + NameNumber[2].ToString() + NameNumber[1].ToString() + CodeType[3].ToString() +
                IDNumber[5].ToString() + CodeType[8].ToString());
        }
        //Start Add Naderi(138) PayamResani --Attach

        //Start Add Azami (345) Lock Revision 90-03-21
        public class MD5Hash
        {
            // Hash an input string and return the hash as
            // a 32 character hexadecimal string.
            public static string GetMD5Hash(string input)
            {
                MD5 md5Hasher = MD5.Create();

                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }

            // Verify a hash against a string.
            public static bool VerifyMD5Hash(string input, string hash)
            {
                string hashOfInput = GetMD5Hash(input);

                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }    
        }

        public class CRC32 : HashAlgorithm
        {

            #region CONSTRUCTORS
            /// <summary>Creates a CRC32 object using the <see cref="DefaultPolynomial"/>.</summary>
            public CRC32()
                : this(DefaultPolynomial)
            {
            }

            /// <summary>Creates a CRC32 object using the specified polynomial.</summary>
            /// <remarks>The polynomical should be supplied in its bit-reflected form. <see cref="DefaultPolynomial"/>.</remarks>
            [CLSCompliant(false)]
            public CRC32(uint polynomial)
            {
                HashSizeValue = 32;
                _crc32Table = (uint[])_crc32TablesCache[polynomial];
                if (_crc32Table == null)
                {
                    _crc32Table = CRC32._buildCRC32Table(polynomial);
                    _crc32TablesCache.Add(polynomial, _crc32Table);
                }
                Initialize();
            }

            // static constructor
            static CRC32()
            {
                _crc32TablesCache = Hashtable.Synchronized(new Hashtable());
                _defaultCRC = new CRC32();
            }
            #endregion

            #region PROPERTIES
            /// <summary>Gets the default polynomial (used in WinZip, Ethernet, etc.)</summary>
            /// <remarks>The default polynomial is a bit-reflected version of the standard polynomial 0x04C11DB7 used by WinZip, Ethernet, etc.</remarks>
            [CLSCompliant(false)]
            public static readonly uint DefaultPolynomial = 0xEDB88320; // Bitwise reflection of 0x04C11DB7;
            #endregion

            #region METHODS
            /// <summary>Initializes an implementation of HashAlgorithm.</summary>
            public override void Initialize()
            {
                _crc = _allOnes;
            }

            /// <summary>Routes data written to the object into the hash algorithm for computing the hash.</summary>
            protected override void HashCore(byte[] buffer, int offset, int count)
            {
                for (int i = offset; i < count; i++)
                {
                    ulong ptr = (_crc & 0xFF) ^ buffer[i];
                    _crc >>= 8;
                    _crc ^= _crc32Table[ptr];
                }
            }

            /// <summary>Finalizes the hash computation after the last data is processed by the cryptographic stream object.</summary>
            protected override byte[] HashFinal()
            {
                byte[] finalHash = new byte[4];
                ulong finalCRC = _crc ^ _allOnes;

                finalHash[0] = (byte)((finalCRC >> 0) & 0xFF);
                finalHash[1] = (byte)((finalCRC >> 8) & 0xFF);
                finalHash[2] = (byte)((finalCRC >> 16) & 0xFF);
                finalHash[3] = (byte)((finalCRC >> 24) & 0xFF);

                return finalHash;
            }

            /// <summary>Computes the CRC32 value for the given ASCII string using the <see cref="DefaultPolynomial"/>.</summary>
            public static int Compute(string asciiString)
            {
                _defaultCRC.Initialize();
                return ToInt32(_defaultCRC.ComputeHash(asciiString));
            }

            /// <summary>Computes the CRC32 value for the given input stream using the <see cref="DefaultPolynomial"/>.</summary>
            public static int Compute(Stream inputStream)
            {
                long position = inputStream.Position;
                _defaultCRC.Initialize();
                int result = ToInt32(_defaultCRC.ComputeHash(inputStream));
                inputStream.Position = position;
                return result;
            }

            /// <summary>Computes the CRC32 value for the input data using the <see cref="DefaultPolynomial"/>.</summary>
            public static int Compute(byte[] buffer)
            {
                _defaultCRC.Initialize();
                return ToInt32(_defaultCRC.ComputeHash(buffer));
            }

            /// <summary>Computes the hash value for the input data using the <see cref="DefaultPolynomial"/>.</summary>
            public static int Compute(byte[] buffer, int offset, int count)
            {
                _defaultCRC.Initialize();
                return ToInt32(_defaultCRC.ComputeHash(buffer, offset, count));
            }

            /// <summary>Computes the hash value for the given ASCII string.</summary>
            /// <remarks>The computation preserves the internal state between the calls, so it can be used for computation of a stream data.</remarks>
            public byte[] ComputeHash(string asciiString)
            {
                byte[] rawBytes = ASCIIEncoding.ASCII.GetBytes(asciiString);
                return ComputeHash(rawBytes);
            }

            /// <summary>Computes the hash value for the given input stream.</summary>
            /// <remarks>The computation preserves the internal state between the calls, so it can be used for computation of a stream data.</remarks>
            new public byte[] ComputeHash(Stream inputStream)
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = inputStream.Read(buffer, 0, 4096)) > 0)
                {
                    HashCore(buffer, 0, bytesRead);
                }
                return HashFinal();
            }

            /// <summary>Computes the hash value for the input data.</summary>
            /// <remarks>The computation preserves the internal state between the calls, so it can be used for computation of a stream data.</remarks>
            new public byte[] ComputeHash(byte[] buffer)
            {
                return ComputeHash(buffer, 0, buffer.Length);
            }

            /// <summary>Computes the hash value for the input data.</summary>
            /// <remarks>The computation preserves the internal state between the calls, so it can be used for computation of a stream data.</remarks>
            new public byte[] ComputeHash(byte[] buffer, int offset, int count)
            {
                HashCore(buffer, offset, count);
                return HashFinal();
            }
            #endregion

            #region PRIVATE SECTION
            private static uint _allOnes = 0xffffffff;
            private static CRC32 _defaultCRC;
            private static Hashtable _crc32TablesCache;
            private uint[] _crc32Table;
            private uint _crc;

            // Builds a crc32 table given a polynomial
            private static uint[] _buildCRC32Table(uint polynomial)
            {
                uint crc;
                uint[] table = new uint[256];

                // 256 values representing ASCII character codes. 
                for (int i = 0; i < 256; i++)
                {
                    crc = (uint)i;
                    for (int j = 8; j > 0; j--)
                    {
                        if ((crc & 1) == 1)
                            crc = (crc >> 1) ^ polynomial;
                        else
                            crc >>= 1;
                    }
                    table[i] = crc;
                }

                return table;
            }

            private static int ToInt32(byte[] buffer)
            {
                return BitConverter.ToInt32(buffer, 0);
            }
            #endregion

        }
        //End Add Azami (345) Lock Revision 90-03-21

    }
}
