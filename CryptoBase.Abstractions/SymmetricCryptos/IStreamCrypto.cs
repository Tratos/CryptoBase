using System;

namespace CryptoBase.Abstractions.SymmetricCryptos
{
	public interface IStreamCrypto : ISymmetricCrypto
	{
		/// <summary>
		/// 加密/解密
		/// </summary>
		void Update(ReadOnlySpan<byte> source, Span<byte> destination);
	}
}
