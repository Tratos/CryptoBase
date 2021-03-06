using CryptoBase.Abstractions.SymmetricCryptos;
using CryptoBase.SymmetricCryptos.BlockCryptoModes;
using CryptoBase.SymmetricCryptos.BlockCryptos.AES;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using Aes = System.Security.Cryptography.Aes;

namespace CryptoBase
{
	public static class AESUtils
	{
		public static bool IsSupportX86 => System.Runtime.Intrinsics.X86.Aes.IsSupported && Sse2.IsSupported;

		public static readonly Aes AesEcb;
		public static readonly Aes AesCbc;

		static AESUtils()
		{
			AesEcb = Aes.Create();
			AesEcb.Mode = CipherMode.ECB;
			AesEcb.Padding = PaddingMode.None;

			AesCbc = Aes.Create();
			AesCbc.Mode = CipherMode.CBC;
			AesCbc.Padding = PaddingMode.None;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AESCrypto CreateECB(byte[] key)
		{
			if (IsSupportX86)
			{
				return key.Length switch
				{
					16 => new Aes128CryptoX86(key),
					24 => new Aes192CryptoX86(key),
					32 => new Aes256CryptoX86(key),
					_ => throw new ArgumentOutOfRangeException(nameof(key))
				};
			}

			return new AESECBCrypto(key);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IBlockCrypto CreateCBC(byte[] key, byte[] iv)
		{
			if (IsSupportX86)
			{
				return new CBCBlockMode(CreateECB(key), iv);
			}

			return new AESCBCCrypto(key, iv);
		}
	}
}
