using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.X509;
using System.IO;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;


namespace DAL.Classes
{
    public class SignatureValidator : IDisposable
    {
        public string Text { get; set; }
        public string CryptedSignature { get; set; }
        public string Cert { get; set; }
        
        public void Dispose()
        {
          //  throw new NotImplementedException();
        }

        private byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;

        }

        public bool Validate()
        {
            string msg = Crypter.CalcHash(Text);
            string hsig = CryptedSignature;


           // throw new Exception("MSG = " + msg + " HSIG = " + hsig);


            TextReader tr = new StringReader(Cert);
            PemReader pr = new PemReader(tr);
            X509Certificate cert = (X509Certificate)pr.ReadObject();

            AsymmetricKeyParameter publicKey = cert.GetPublicKey();
            ISigner signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(false, publicKey);

            byte[] bytes = FromHex(hsig);
            string s = Convert.ToBase64String(bytes);
            byte[] hsig2 = Convert.FromBase64String(s);

            byte[] msgBytes = Encoding.UTF8.GetBytes(msg);

            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);

            return signer.VerifySignature(hsig2);
        }
    }
}
