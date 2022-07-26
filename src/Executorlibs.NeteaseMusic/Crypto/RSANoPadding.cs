using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Executorlibs.NeteaseMusic.Crypto
{
    public sealed class RSANoPadding : RSA
    {
        private RSAParameters _parameters;

        public override RSAParameters ExportParameters(bool includePrivateParameters)
        {
            throw new NotSupportedException();
        }

        public override void ImportParameters(RSAParameters parameters)
        {
            _parameters = parameters;
        }

        public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
        {
            if (_parameters.Modulus == null ||
                _parameters.Exponent == null)
            {
                throw new InvalidOperationException();
            }
            byte[] mb = new byte[_parameters.Modulus.Length],
                   eb = new byte[_parameters.Exponent.Length],
                   db = new byte[data.Length];
            Unsafe.CopyBlock(ref mb[0], ref _parameters.Modulus[0], (uint)mb.Length);
            Unsafe.CopyBlock(ref eb[0], ref _parameters.Exponent[0], (uint)eb.Length);
            Unsafe.CopyBlock(ref db[0], ref data[0], (uint)data.Length);
            Array.Reverse(mb);
            Array.Reverse(eb);
            Array.Reverse(db);
            BigInteger m = new BigInteger(mb),
                       e = new BigInteger(eb),
                       d = new BigInteger(db),
                       result = BigInteger.ModPow(d, e, m);
            byte[] buffer = result.ToByteArray();
            Array.Reverse(buffer);
            return buffer;
        }

        public override byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
        {
            throw new NotSupportedException();
        }
    }
}
