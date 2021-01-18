using CryptoBase.Abstractions.Digests;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Buffers;
using System.Threading;

namespace CryptoBase.Digests.SHA1
{
	public class BcSHA1Digest : SHA1DigestBase
	{
		private static readonly ThreadLocal<Sha1Digest> Hasher = new(() => new Sha1Digest());

		public override void ComputeHash(in ReadOnlySpan<byte> origin, Span<byte> destination)
		{
			var buffer = ArrayPool<byte>.Shared.Rent(origin.Length);
			var outBuffer = ArrayPool<byte>.Shared.Rent(Length);
			try
			{
				origin.CopyTo(buffer);

				Hasher.Value!.Reset();
				Hasher.Value!.BlockUpdate(buffer, 0, origin.Length);
				Hasher.Value!.DoFinal(outBuffer, 0);

				outBuffer.AsSpan(0, Length).CopyTo(destination);
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(buffer);
				ArrayPool<byte>.Shared.Return(outBuffer);
			}
		}
	}
}
