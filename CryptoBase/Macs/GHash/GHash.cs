using CryptoBase.Abstractions.SymmetricCryptos;
using System;

namespace CryptoBase.Macs.GHash
{
	public abstract class GHash : IMac
	{
		public string Name => @"GHash";

		public const int BlockSize = 16;
		public const int TagSize = 16;

		protected GHash(byte[] key)
		{
			if (key.Length < BlockSize)
			{
				throw new ArgumentException(@"Key length must be 16 bytes", nameof(key));
			}
		}

		protected abstract void GFMul(ReadOnlySpan<byte> x);

		public void Update(ReadOnlySpan<byte> source)
		{
			while (source.Length >= BlockSize)
			{
				GFMul(source);
				source = source.Slice(BlockSize);
			}

			if (source.IsEmpty)
			{
				return;
			}

			Span<byte> block = stackalloc byte[BlockSize];
			source.CopyTo(block);
			GFMul(block);
		}

		public abstract void GetMac(Span<byte> destination);

		public virtual void Dispose() { }

		public abstract void Reset();
	}
}