using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Generators;
using OpenSSL.Crypto;
using OpenSSL.Core;
using Org.BouncyCastle.Crypto.Paddings;

namespace Zhilfond.Keys
{  
    class KeyGenerator
    {
        public string Encrypt(string text, String password)
        {
            Byte[] data = Encoding.UTF8.GetBytes(text);
            //Just random 8 bytes for salt
            var salt = new Byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };


            using (var cc = new CipherContext(Cipher.AES_256_CBC))
            {
                //Constructing key and init vector from string password
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] iv;
                byte[] key = cc.BytesToKey(MessageDigest.MD5, salt, passwordBytes, 1, out iv);

                var memoryStream = new MemoryStream();

                //Performing encryption thru unmanaged wrapper
                var aesData = cc.Crypt(data, key, iv, true);

                //Append salt so final data will look Salted___SALT|RESTOFTHEDATA
                memoryStream.Write(Encoding.UTF8.GetBytes("Salted__"), 0, 8);
                memoryStream.Write(salt, 0, 8);
                memoryStream.Write(aesData, 0, aesData.Length);

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public string Decrypt(string encryptedString, String password)
        {
            Byte[] encryptedData = Convert.FromBase64String(encryptedString); 

            byte[] salt = null;
            //extracting salt if presented
            if (encryptedData.Length > 16)
            {
                if (Encoding.UTF8.GetString(encryptedData).StartsWith("Salted__"))
                {
                    salt = new Byte[8];
                    Buffer.BlockCopy(encryptedData, 8, salt, 0, 8);
                }
            }

            //Removing salt from the original array
            int aesDataLength = encryptedData.Length - 16;
            byte[] aesData = new byte[aesDataLength];
            Buffer.BlockCopy(encryptedData, 16, aesData, 0, aesDataLength);


            using (var cc = new CipherContext(Cipher.AES_256_CBC))
            {
                //Constructing key and init vector from string password and salt
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] iv;
                byte[] key = cc.BytesToKey(MessageDigest.MD5, salt, passwordBytes, 1, out iv);

                //Decrypting
                return  Encoding.UTF8.GetString(cc.Decrypt(aesData, key, iv, 0));
            }
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();  // SHA1.Create()
           // return algorithm.ComputeHash(Convert.FromBase64String(inputString));
             return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
           /* StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();*/
            return Convert.ToBase64String(GetHash(inputString));
        }

        public static byte[] AsymmetricDecrypt(string publicKeyAsPem, byte[] payload)
        {
            CryptoKey d = CryptoKey.FromPublicKey(publicKeyAsPem, null);
            OpenSSL.Crypto.RSA rsa = d.GetRSA();
            byte[] result = rsa.PublicDecrypt(payload, OpenSSL.Crypto.RSA.Padding.None);

            rsa.Dispose();
            return result;
        }



        public void Process(string path, string username, string password, out string pub_cert, out string priv_key)
        {
            UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            if (string.IsNullOrEmpty(path))
                path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
                       

            var randomGenerator = new CryptoApiRandomGenerator();
            var random = new SecureRandom(randomGenerator);

            var certificateGenerator = new X509V3CertificateGenerator();
            var serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
            certificateGenerator.SetSerialNumber(serialNumber);

            const string signatureAlgorithm = "SHA256WithRSA";
            certificateGenerator.SetSignatureAlgorithm(signatureAlgorithm);

            var subjectDN = new Org.BouncyCastle.Asn1.X509.X509Name("C=AU, O=Zhilfond, L=Tomsk + OU=" + username);
            var issuerDN = subjectDN;
            certificateGenerator.SetIssuerDN(issuerDN);
            certificateGenerator.SetSubjectDN(subjectDN);

            var notBefore = DateTime.UtcNow.Date;
            var notAfter = notBefore.AddYears(2);

            certificateGenerator.SetNotBefore(notBefore);
            certificateGenerator.SetNotAfter(notAfter);


            const int strength = 512;
            var keyGenerationParameters = new KeyGenerationParameters(random, strength);

            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            var subjectKeyPair = keyPairGenerator.GenerateKeyPair();
            certificateGenerator.SetPublicKey(subjectKeyPair.Public);

            var certificate = certificateGenerator.Generate(subjectKeyPair.Private, random);

            // Convert private key to PEM string
            TextWriter textWriterPrivate = new StringWriter();
            PemWriter pemWriterPrivate = new PemWriter(textWriterPrivate);
            pemWriterPrivate.WriteObject(subjectKeyPair.Private);
            pemWriterPrivate.Writer.Flush();
            string CA2_PrivKey = textWriterPrivate.ToString();


            // Encode private key with AES
            string EncPrivate = Encrypt(CA2_PrivKey, password);

            byte[] ba = Encoding.Default.GetBytes(EncPrivate);
            string hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");

            File.WriteAllText(path + "\\private.key", hexString);

            // Convert public key to PEM string
            TextWriter textWriterPublic = new StringWriter();
            PemWriter pemWriterPublic = new PemWriter(textWriterPublic);
            pemWriterPublic.WriteObject(subjectKeyPair.Public);
            pemWriterPublic.Writer.Flush();
            string CA2_PubKey = textWriterPublic.ToString();

           // File.WriteAllText(path + "\\public.key", CA2_PubKey);

            TextWriter w = new StringWriter();
            PemWriter pw = new PemWriter(w);
            pw.WriteObject(certificate);
            pw.Writer.Close();
            string CA2_Cert = w.ToString();

            File.WriteAllText(path + "\\public.cer", CA2_Cert);

            pub_cert =  CA2_Cert;
            priv_key = CA2_PrivKey;
        }
      
    }
}
