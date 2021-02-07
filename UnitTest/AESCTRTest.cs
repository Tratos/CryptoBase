using CryptoBase;
using CryptoBase.Abstractions.SymmetricCryptos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
	[TestClass]
	public class AESCTRTest
	{
		private static void Test(IStreamCrypto crypto, string hex, string hex2)
		{
			Assert.AreEqual(@"AES-CTR", crypto.Name);

			Span<byte> h1 = hex.FromHex();
			Span<byte> h2 = hex2.FromHex();

			Span<byte> o = stackalloc byte[h1.Length];

			crypto.Update(h1, o);
			Assert.IsTrue(o.SequenceEqual(h2));

			crypto.Reset();

			crypto.Update(h1, o);
			Assert.IsTrue(o.SequenceEqual(h2));

			crypto.Dispose();
		}

		/// <summary>
		/// https://gchq.github.io/CyberChef/#recipe=AES_Encrypt(%7B'option':'Hex','string':'00000000000000000000000000000000'%7D,%7B'option':'Hex','string':'00000000000000000000000000000000'%7D,'CTR','Hex','Hex','')
		/// </summary>
		[TestMethod]
		[DataRow(@"A0B0C0D0E0F1011121314151617181AA", @"fe3577795961e1fbbbb82528c74d2e99",
			@"4dfa5e481da23ea09a31022050859936da52fcee218005164f267cb65f5cfd7f2b4f97e0ff16924a52df269515110a07f9e460bc65ef95da58f740b7d1dbb0aada9c1581f429e0a00f7d67e23b730676783b262e8eb43a25f55fb90b3e753aef8c6713ec66c51881111593ccb3e8cb8f8de124080501eeeb389c4bcb6977cf950558abfe51a4f74a9df04396e93c8fe23588db2e81d4277acd2073c6196cbf120a5db00356a9fc4fa2f5489bee4194e73a8de03386d92c7fd22578cb1e71c4170f62b5085bae0154a7fa4da0f34699ec3f92e5388bde3184d72a7dd02376c91c0053a6f94c9ff24598eb3e91e4378add3083d6297ccf2275c81b6ec11467ba",
			@"9f81c160fce82ba391927f7882c162d7e98cc50cfca62a9a4756ddbd6deafef8e4f8332c979389a8065882aaec198efefa8120cf6efdbe821490738e3a0a904b7e8e35594e3a8ac4e88b6b03c4bd2ee6221390c9ea98027ea960643a34a2e0bec8f9c1f9b9fc8119b26a253bd578d3bcf0402a07cfa0eb85fdf9be8e5f341a7cd9c2e04e0f63121992e66cb32ad8d7afb640030570ba1fe20cc488513e5352b6c8d38a37ef9dfd3e693aa9f6f1b18311634cd45e19b3c73abfc279d6066dfb9defdf79c510e7ab3165c915f1b9a628b38be3c03b71dca6a146b3f27e2074370fe88a4f20ffc23161211bc0e132c2c673b35e23305d8d912b22a73284309218")]
		[DataRow(@"000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", @"00112233445566778899aabbccddeeff",
				@"2b13159cf228129b127df8799ab1b21882b00020dbfe3577795961e1fbbbb82528c74d2e9fa031d8a1f7e42c64a3f47f0e71ca172ca4ccf9af0ac9484550714afa9e04a0ba5a747a2ff4d14eebe0ad96b17684379f3a13588e2105b7f7b6b21d06b96e69ce2b2de07e64b76566cfc91e1859691393c77e1417b1a8c09b9b4675070bd39043c1f3ba47494fde4a3fae3c06cd3d9ec7ca5be4a14b0938c4848052d4ee4a64c25101c0f75dc55034dcfc1529f129fbb2f92b01aed7f279771ac9e75273f023030251fe8949ec35455d1e09f9c888ed5c2999c4b2ffdcafd714ba146051ef94faa056fc830506ee4734864897bc5b2d4b2fb32a74feb96739ea11",
				@"a5b1a256a34f5724f881b1e9d1f8d29174cf8ed2970fb9bd0054ccb3b5e6015e087c4b3dbf90edade1815d338b6cf8f1bf133b07eecfc4d1bb69a1a04e0221332bc390d36eac23997ed11cbab1981b69466c22ab23f94bc96e16419f63ba03cff717ce0615e162dac024fda8d042155605e005753abbb15c0d002c75d66f9f5369a51336a1d8f6d65b65f803c3f4c4e3afb3794aa261678b7e5bc5009daa1d4aeab301b801c9ab4e0edeb359f4ea4775995638c5fad6a9b3ef72ef94ea04879d728fd52b15650b2b6210c9d2ad880496220b8a963e9325593ec9eb7eaecab1e5805aa0a36a837f7fbac31ff504ec539a48cbc3ab3a361c12618b34237d64f2")]
		[DataRow(@"08090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", @"00112233445566778899aabbccddeeff",
				@"a5b1a256a34f5724f881b1e9d1f8d29174cf8ed2970fb9bd0054ccb3b5e6015e087c4b3dbf90edade1815d338b6cf8f1bf133b07eecfc4d1bb69a1a04e0221332bc390d36eac23997ed11cbab1981b69466c22ab23f94bc96e16419f63ba03cff717ce0615e162dac024fda8d042155605e005753abbb15c0d002c75d66f9f5369a51336a1d8f6d65b65f803c3f4c4e3afb3794aa261678b7e5bc5009daa1d4aeab301b801c9ab4e0edeb359f4ea4775995638c5fad6a9b3ef72ef94ea04879d728fd52b15650b2b6210c9d2ad880496220b8a963e9325593ec9eb7eaecab1e5805aa0a36a837f7fbac31ff504ec539a48cbc3ab3a361c12618b34237d64f2",
				@"2291802d076e5ab69f2514c51b45db293142f26fe9a9a702a097a5dc2b71806410dcc4816581fa1e1008dc2bfb9162300235de5e8fb5f99591237f9789952c42925c733868202803649ea4fff98854e65066af53f59c960d59522346bcc96202e9c3ea64bf9c9422f11f4869541c8b1c8b96f56ad6d7d54895171604dd5aeaffeb79070fee0cd53f4a536f9316ce25dec1fe381ad1dcf2d841be7521ef8b6780ed884cae8cb5a67464b9f23d42cf0a8ffd58596def2da0dcf839629435d7072f85504e76c7478a35a43da540ceef8aa3abd901ece95d9c5212c97c6c4535f2fcc3e305b6953270f416e66d569cf2582e8f233b116d0412eebe86e25e10cdd0")]
		public void Test(string keyHex, string ivHex, string hex, string hex2)
		{
			var key = keyHex.FromHex();
			var iv = ivHex.FromHex();
			Test(AESUtils.CreateCTR(key, iv), hex, hex2);
		}
	}
}
