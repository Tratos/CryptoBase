using CryptoBase.Abstractions.Digests;
using System;
using System.Threading;

namespace CryptoBase.Digests.SM3
{
	public static class SM3Utils
	{
		private static readonly ThreadLocal<SM3DigestBase> Bc = new(() => new BcSM3Digest());
		private static readonly ThreadLocal<SM3DigestBase> Slow = new(() => new SlowSM3Digest());

		public static void BouncyCastle(in ReadOnlySpan<byte> origin, Span<byte> destination)
		{
			Bc.Value!.ComputeHash(origin, destination);
		}

		public static void MayFast(in ReadOnlySpan<byte> origin, Span<byte> destination)
		{
			Slow.Value!.ComputeHash(origin, destination);
		}
	}
}