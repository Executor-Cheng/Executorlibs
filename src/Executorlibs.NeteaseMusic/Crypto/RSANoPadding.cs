using System;
using System.Numerics;
using System.Security.Cryptography;
#if NETSTANDARD2_0
using System.Runtime.CompilerServices;
#endif

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

#if NETSTANDARD2_0
        public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
        {
            if (_parameters.Modulus == null ||
                _parameters.Exponent == null)
            {
                throw new InvalidOperationException();
            }
            byte[] mb = new byte[_parameters.Modulus.Length + 1],
                   eb = new byte[_parameters.Exponent.Length + 1],
                   db = new byte[data.Length + 1];
            Unsafe.CopyBlock(ref mb[1], ref _parameters.Modulus[0], (uint)_parameters.Modulus.Length);
            Unsafe.CopyBlock(ref eb[1], ref _parameters.Exponent[0], (uint)_parameters.Exponent.Length);
            Unsafe.CopyBlock(ref db[1], ref data[0], (uint)data.Length);
            Array.Reverse(mb);
            Array.Reverse(eb);
            Array.Reverse(db);
            BigInteger m = new BigInteger(mb),
                       e = new BigInteger(eb),
                       d = new BigInteger(db),
                       result = BigInteger.ModPow(d, e, m);
            byte[] buffer = result.ToByteArray();
            if (buffer[buffer.Length - 1] == 0)
            {
                Array.Resize(ref buffer, buffer.Length - 1);
            }
            Array.Reverse(buffer);
            return buffer;
        }
#else
        public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
        {
            if (_parameters.Modulus == null ||
                _parameters.Exponent == null)
            {
                throw new InvalidOperationException();
            }
            BigInteger m = new BigInteger(_parameters.Modulus, true, true),
                       e = new BigInteger(_parameters.Exponent, true, true),
                       d = new BigInteger(data, true, true),
                       result = BigInteger.ModPow(d, e, m);
            return result.ToByteArray(true, true);
        }
#endif

        public override byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
        {
            throw new NotSupportedException();
        }
    }
}
