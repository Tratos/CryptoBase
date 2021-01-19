using BenchmarkDotNet.Attributes;
using CryptoBase.Abstractions.SymmetricCryptos;
using CryptoBase.SymmetricCryptos.StreamCryptos.RC4;
using System;

namespace CryptoBase.Benchmark
{
	[MemoryDiagnoser]
	public class RC4Benchmark
	{
		[Params(32, 114514)]
		public int ByteLength { get; set; }

		private Memory<byte> _randombytes;
		private byte[] _randomKey = null!;

		[GlobalSetup]
		public void Setup()
		{
			_randombytes = Utils.RandBytes(ByteLength).ToArray();
			_randomKey = Utils.RandBytes(16).ToArray();
		}

		private static void Test(ISymmetricCrypto crypto, Span<byte> origin)
		{
			Span<byte> o = stackalloc byte[origin.Length];
			crypto.Encrypt(origin, o);
		}

		[Benchmark(Baseline = true)]
		public void BouncyCastle()
		{
			Test(new BcRC4Crypto(_randomKey), _randombytes.Span);
		}

		[Benchmark]
		public void Slow()
		{
			Test(new SlowRC4Crypto(_randomKey), _randombytes.Span);
		}
	}
}
