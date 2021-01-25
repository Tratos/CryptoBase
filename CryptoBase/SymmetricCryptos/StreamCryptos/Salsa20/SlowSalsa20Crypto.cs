namespace CryptoBase.SymmetricCryptos.StreamCryptos.Salsa20
{
	public class SlowSalsa20Crypto : FastSalsa20Crypto
	{
		public override bool IsSupport => false;

		public SlowSalsa20Crypto(byte[] key, byte[] iv) : base(key, iv) { }

		protected override void UpdateKeyStream()
		{
			Salsa20Utils.UpdateKeyStream(Rounds, State, KeyStream);
		}
	}
}
