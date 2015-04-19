using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace DAL.Classes
{
    public class SignatureCreator
    {
        public string Sign(byte[] bytes, string key)
        {
            StringReader sr = new StringReader(key);
            AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)new PemReader(sr).ReadObject();
            sr.Close();
            
            ISigner sig = SignerUtilities.GetSigner("SHA256withRSA");
            sig.Init(true, keyPair.Private);

            sig.BlockUpdate(bytes, 0, bytes.Length);
            byte[] signature = sig.GenerateSignature();

            string s = BitConverter.ToString(signature).Replace("-", "");

            return s;
        }
    }
}
