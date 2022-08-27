using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Executorlibs.Shared.Helpers
{
    public static class RsaHelper
    {
        private static ReadOnlySpan<byte> OidSequence => new byte[15] { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };

        private static unsafe byte[] GetBytes(byte** keyPtr)
        {
            int count = 0;
            byte temp = 0;
            byte* localKeyPtr = *keyPtr;
            if (*localKeyPtr++ != 0x02)
            {
                throw new ArgumentException("Invalid data found in buffer.", nameof(keyPtr));
            }
            switch (temp = *localKeyPtr++)
            {
                case 0x81:
                    {
                        count = *localKeyPtr++;
                        break;
                    }
                case 0x82:
                    {
                        count = BinaryPrimitives.ReverseEndianness(*(short*)localKeyPtr);
                        localKeyPtr += 2;
                        break;
                    }
                default:
                    {
                        count = temp;
                        break;
                    }
            }
            while (*localKeyPtr == 0)
            {
                localKeyPtr++;
                count--;
            }
            byte[] buffer = new byte[count];
            fixed (byte* bufferPtr = buffer)
            {
                Unsafe.CopyBlock(bufferPtr, localKeyPtr, (uint)count);
                localKeyPtr += count;
            }
            *keyPtr = localKeyPtr;
            return buffer;
        }

        private static unsafe void CheckHeader(byte** keyPtr, byte toCheck)
        {
            byte* localPtr = *keyPtr;
            if (*localPtr++ == toCheck)
            {
                int add = *localPtr++ - 0x81;
                if ((uint)add <= 1)
                {
                    localPtr += add + 1;
                    *keyPtr = localPtr;
                    return;
                }
            }
            throw new ArgumentException("Invalid data found in privateKey.", nameof(keyPtr));
        }

        public static unsafe RSACryptoServiceProvider DecodePrivateKey(string privateKey)
        {
            fixed (byte* _keyPtr = Convert.FromBase64String(privateKey))
            {
                byte* keyPtr = _keyPtr;
                CheckHeader(&keyPtr, 0x30);
                if (*keyPtr++ == 0x02 && *keyPtr++ == 0x01 && *keyPtr++ == 0)
                {
                    RSAParameters rsaParams = new RSAParameters
                    {
                        Modulus = GetBytes(&keyPtr),
                        Exponent = GetBytes(&keyPtr),
                        D = GetBytes(&keyPtr),
                        P = GetBytes(&keyPtr),
                        Q = GetBytes(&keyPtr),
                        DP = GetBytes(&keyPtr),
                        DQ = GetBytes(&keyPtr),
                        InverseQ = GetBytes(&keyPtr)
                    };
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.ImportParameters(rsaParams);
                    return rsa;
                }
            }
            throw new ArgumentException("Invalid data found in privateKey.", nameof(privateKey));
        }

        public static unsafe RSACryptoServiceProvider DecodePublicKey(string publicKey)
        {
            fixed (byte* _keyBuffer = Convert.FromBase64String(publicKey))
            {
                byte* keyBuffer = _keyBuffer;
                CheckHeader(&keyBuffer, 0x30);
                byte* buffer = stackalloc byte[15];
                Unsafe.CopyBlock(buffer, keyBuffer, 15);
                if (OidSequence.SequenceEqual(new Span<byte>(buffer, 15)))
                {
                    keyBuffer += 15;
                    CheckHeader(&keyBuffer, 0x03);
                    if (*keyBuffer++ == 0)
                    {
                        CheckHeader(&keyBuffer, 0x30);
                        RSAParameters rsaParams = new RSAParameters()
                        {
                            Modulus = GetBytes(&keyBuffer),
                            Exponent = GetBytes(&keyBuffer)
                        };
                        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                        rsa.ImportParameters(rsaParams);
                        return rsa;
                    }
                }
            }
            throw new ArgumentException("Invalid data found in publicKey.", nameof(publicKey));
        }

        public static byte[] RsaDecode(string privateKey, byte[] toDecode)
        {
            using RSACryptoServiceProvider rsaProvider = DecodePrivateKey(privateKey);
            return RsaDecode(rsaProvider, toDecode);
        }

        public static byte[] RsaDecode(RSACryptoServiceProvider rsaProvider, byte[] toDecode)
        {
            int keySize = rsaProvider.KeySize / 8;
            if (toDecode.Length % keySize != 0)
            {
                throw new ArgumentException("Invalid length of encoded data.", nameof(toDecode));
            }
            int blocks = toDecode.Length / keySize,
                blockSize = keySize - 11;
            using MemoryStream ms = new MemoryStream(blockSize * blocks);
            byte[] currentBlock = new byte[keySize];
            int currentOffset = 0;
            for (int i = 0; i < blocks; i++)
            {
#if NET5_0_OR_GREATER
                Unsafe.CopyBlock(ref MemoryMarshal.GetArrayDataReference(currentBlock), ref Unsafe.AddByteOffset(ref MemoryMarshal.GetArrayDataReference(toDecode), (IntPtr)currentOffset), (uint)keySize);
#else
                Buffer.BlockCopy(toDecode, currentOffset, currentBlock, 0, keySize);
#endif
                currentOffset += keySize;
                byte[] decoded = rsaProvider.Decrypt(currentBlock, false);
                ms.Write(decoded, 0, decoded.Length);
            }
            return ms.ToArray();
        }

        public static byte[] RsaEncode(string publicKey, byte[] toEncode)
        {
            if (toEncode.Length == 0)
            {
                throw new ArgumentException("At least 1 byte must be provided.", nameof(toEncode));
            }
            using RSACryptoServiceProvider rsaProvider = DecodePublicKey(publicKey);
            return RsaEncode(rsaProvider, toEncode);
        }

        public static byte[] RsaEncode(RSACryptoServiceProvider rsaProvider, byte[] toEncode)
        {
            if (toEncode.Length == 0)
            {
                throw new ArgumentException("At least 1 byte must be provided.", nameof(toEncode));
            }
            int blockSize = rsaProvider.KeySize / 8,
                rawSize = blockSize - 11,
                blocks = (toEncode.Length + rawSize - 1) / rawSize;
            byte[] encoded = new byte[blockSize * blocks];
            byte[] currentBlock = new byte[rawSize];
            for (int i = 0, inputOffset = 0, encodedOffset = 0; ; inputOffset += rawSize, encodedOffset += blockSize)
            {
                if (++i == blocks)
                {
                    int lastSize = toEncode.Length - inputOffset;
                    if (lastSize < blockSize)
                    {
                        currentBlock = new byte[lastSize];
                    }
                }
#if NET5_0_OR_GREATER
                Unsafe.CopyBlock(ref MemoryMarshal.GetArrayDataReference(currentBlock), ref Unsafe.AddByteOffset(ref MemoryMarshal.GetArrayDataReference(toEncode), (IntPtr)inputOffset), (uint)currentBlock.Length);
                byte[] currentEncoded = rsaProvider.Encrypt(currentBlock, false);
                Unsafe.CopyBlock(ref Unsafe.AddByteOffset(ref MemoryMarshal.GetArrayDataReference(encoded), (IntPtr)encodedOffset), ref MemoryMarshal.GetArrayDataReference(currentEncoded), (uint)blockSize);
#else
                Unsafe.CopyBlock(ref currentBlock[0], ref toEncode[inputOffset], (uint)currentBlock.Length);
                byte[] currentEncoded = rsaProvider.Encrypt(currentBlock, false);
                Unsafe.CopyBlock(ref encoded[encodedOffset], ref currentEncoded[0], (uint)blockSize);
#endif
                if (i == blocks)
                {
                    break;
                }
            }
            return encoded;
        }
    }
}
