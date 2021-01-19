using BenchmarkDotNet.Running;
using CryptoBase.Benchmark;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
	[TestClass]
	public class BenchmarkTest
	{
		[TestMethod]
		public void MD5Benchmark()
		{
			var _ = BenchmarkRunner.Run<MD5Benchmark>();
		}

		[TestMethod]
		public void SodiumIncrementBenchmark()
		{
			var _ = BenchmarkRunner.Run<SodiumIncrementBenchmark>();
		}

		[TestMethod]
		public void SHA1Benchmark()
		{
			var _ = BenchmarkRunner.Run<SHA1Benchmark>();
		}

		[TestMethod]
		public void SM3Benchmark()
		{
			var _ = BenchmarkRunner.Run<SM3Benchmark>();
		}

		[TestMethod]
		public void RC4Benchmark()
		{
			var _ = BenchmarkRunner.Run<RC4Benchmark>();
		}
	}
}
