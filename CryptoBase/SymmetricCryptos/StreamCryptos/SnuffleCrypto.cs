using CryptoBase.Abstractions;
using CryptoBase.Abstractions.SymmetricCryptos;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace CryptoBase.SymmetricCryptos.StreamCryptos
{
	public abstract class SnuffleCrypto : SnuffleCryptoBase, IIntrinsics
	{
		public abstract bool IsSupport { get; }

		/// <summary>
		/// expand 16-byte k
		/// </summary>
		protected static readonly uint[] Sigma16 = { 0x61707865, 0x3120646e, 0x79622d36, 0x6b206574 };

		/// <summary>
		/// expand 32-byte k
		/// </summary>
		protected static readonly uint[] Sigma32 = { 0x61707865, 0x3320646e, 0x79622d32, 0x6b206574 };

		protected byte Rounds { get; init; } = 20;

		protected readonly uint[] State;
		protected readonly byte[] KeyStream;

		protected readonly ReadOnlyMemory<byte> Key;
		protected readonly ReadOnlyMemory<byte> Iv;

		protected int Index;

		protected SnuffleCrypto(byte[] key, byte[] iv) : base(key, iv)
		{
			Key = key;
			Iv = iv;

			State = ArrayPool<uint>.Shared.Rent(StateSize);
			KeyStream = ArrayPool<byte>.Shared.Rent(StateSize * sizeof(uint));
		}

		public override unsafe void Update(ReadOnlySpan<byte> source, Span<byte> destination)
		{
			base.Update(source, destination);

			var length = source.Length;
			fixed (uint* pState = State)
			fixed (byte* pStream = KeyStream)
			fixed (byte* pSource = source)
			fixed (byte* pDestination = destination)
			{
				Update(length, pState, pStream, pSource, pDestination);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void Update(int length, uint* state, byte* stream, byte* source, byte* destination)
		{
			while (length > 0)
			{
				if (Index == 0)
				{
					if (IsSupport)
					{
						if (Avx.IsSupported && Avx2.IsSupported)
						{
							if (length >= 512)
							{
								SnuffleCore512(state, ref source, ref destination, ref length);
							}

							while (length >= 128)
							{
								SnuffleCore128(state, source, destination);

								source += 128;
								destination += 128;
								length -= 128;
							}
						}

						if (Sse2.IsSupported)
						{
							if (length >= 256)
							{
								SnuffleCore256(state, ref source, ref destination, ref length);
							}

							while (length >= 64)
							{
								SnuffleCore64(state, source, destination);

								source += 64;
								destination += 64;
								length -= 64;
							}
						}

						if (length == 0)
						{
							break;
						}
					}

					UpdateKeyStream();
					IncrementCounter(state);
				}

				var r = 64 - Index;

				if (IsSupport)
				{
					IntrinsicsUtils.Xor(stream + Index, source, destination, Math.Min(r, length));
				}
				else
				{
					IntrinsicsUtils.XorSoftwareFallback(stream + Index, source, destination, Math.Min(r, length));
				}

				if (length < r)
				{
					Index += length;
					return;
				}

				Index = 0;
				length -= r;
				source += r;
				destination += r;
			}
		}

		protected abstract unsafe void SnuffleCore64(uint* state, byte* source, byte* destination);
		protected abstract unsafe void SnuffleCore128(uint* state, byte* source, byte* destination);
		protected abstract unsafe void SnuffleCore256(uint* state, ref byte* source, ref byte* destination, ref int length);
		protected abstract unsafe void SnuffleCore512(uint* state, ref byte* source, ref byte* destination, ref int length);

		protected abstract unsafe void IncrementCounter(uint* state);
		protected abstract void UpdateKeyStream();

		public override void Dispose()
		{
			base.Dispose();

			ArrayPool<uint>.Shared.Return(State);
			ArrayPool<byte>.Shared.Return(KeyStream);
		}
	}
}
