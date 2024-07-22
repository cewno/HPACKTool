using System.Numerics;

namespace HPACKTool;

public static class IntegerTool
{
	private static readonly byte[] Nb = {0b_0,0b_00000001,0b_00000011,0b_00000111,0b_00001111,0b_00011111,0b_00111111,0b_01111111,0b_11111111};
	public static uint ReadUInt(byte n, AsyncIO asyncIo)
	{
		byte at = (byte)(asyncIo.ReadOneByte() & Nb[n]);
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if (at == Nb[n])
		{
			byte m = 0;
			uint i = 0;
			do
			{
				at = asyncIo.ReadOneByte();
				i |= (uint)((at & 0b_01111111) << m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return i + Nb[n];
		}
		else
		{
			return at;
		}

	}
	public static uint ReadUInt(byte n, Stream stream)
	{
		int at = stream.ReadByte() & Nb[n];
		if (at < 0)
		{
			throw new IOException();
		}
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			byte m = 0;
			uint i = 0;
			do
			{
				at = stream.ReadByte();
				if (at < 0)
				{
					throw new Exception();
				}
				i |= (uint)((at & 0b_01111111) << m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return i + Nb[n];
		}
		else
		{
			return (byte)at;
		}
	}
	public static ulong ReadULong(byte n, AsyncIO asyncIo)
	{
		byte at = (byte)(asyncIo.ReadOneByte() & Nb[n]);
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			ushort m = 0;
			ulong i = 0;
			do
			{
				at = asyncIo.ReadOneByte();
				i |= (ulong)((at & 0b_01111111) << m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return i + Nb[n];
		}
		else
		{
			return at;
		}
	}
	public static ulong ReadULong(byte n, Stream stream)
	{
		int at = stream.ReadByte() & Nb[n];
		if (at < 0)
		{
			throw new IOException();
		}
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			ushort m = 0;
			ulong i = 0;
			do
			{
				at = stream.ReadByte();
				if (at < 0)
				{
					throw new IOException();
				}
				i |= (ulong)((at & 0b_01111111) << m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return i + Nb[n];
		}
		else
		{
			return (byte)at;
		}
	}
	public static ushort ReadUShort(byte n, AsyncIO asyncIo)
	{
		byte at = (byte)(asyncIo.ReadOneByte() & Nb[n]);
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			byte m = 0;
			ushort i = 0;
			do
			{
				at = asyncIo.ReadOneByte();
				i |= (ushort)((at & 0b_01111111) << m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return (ushort)(i + Nb[n]);
		}
		else
		{
			return at;
		}
	}
	public static ushort ReadUShort(byte n, Stream stream)
	{
		int at = stream.ReadByte() & Nb[n];
		if (at < 0)
		{
			throw new IOException();
		}
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			byte m = 0;
			ushort i = 0;
			do
			{
				at = stream.ReadByte();
				if (at < 0)
				{
					throw new IOException();
				}
				i |= (ushort)((at & 0b_01111111) << m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return (ushort)(i + Nb[n]);
		}
		else
		{
			return (byte)at;
		}
	}
	public static byte ReadByte(byte n, AsyncIO asyncIo)
	{
		byte at = (byte)(asyncIo.ReadOneByte() & Nb[n]);
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if (at == Nb[n])
		{
			byte m = 0;
			byte i = 0;
			do
			{
				at = asyncIo.ReadOneByte();
				i |= (byte)((at & 0b_01111111) << m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return (byte)(i + Nb[n]);
		}
		else
		{
			return at;
		}
	}
	public static byte ReadByte(byte n, Stream stream)
	{
		int at = stream.ReadByte() & Nb[n];
		if (at < 0)
		{
			throw new IOException();
		}
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			byte m = 0;
			byte i = 0;
			do
			{
				at = stream.ReadByte();
				if (at < 0)
				{
					throw new IOException();
				}
				i |= (byte)((at & 0b_01111111) << m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return (byte)(i + Nb[n]);
		}
		else
		{
			return (byte)at;
		}
	}
#if NET7_0_OR_GREATER
	public static UInt128 ReadUInt128(byte n, AsyncIO asyncIo)
	{
		byte at = (byte)(asyncIo.ReadOneByte() & Nb[n]);
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			ushort m = 0;
			UInt128 i = 0;
			do
			{
				at = asyncIo.ReadOneByte();
				i |= ((UInt128)(at & 0b_01111111)) * (UInt128)(2^m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return i + Nb[n];
		}
		else
		{
			return at;
		}
	}
	public static UInt128 ReadUInt128(byte n, Stream stream)
	{
		int at = stream.ReadByte() & Nb[n];
		if (at < 0)
		{
			throw new IOException();
		}
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			ushort m = 0;
			UInt128 i = 0;
			do
			{
				at = stream.ReadByte();
				if (at < 0)
				{
					throw new IOException();
				}
				i |= ((UInt128)(at & 0b_01111111)) * (UInt128)(2^m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return i + Nb[n];
		}
		else
		{
			return (byte)at;
		}
	}
#endif

	public static BigInteger ReadBigInteger(byte n, AsyncIO asyncIo)
	{
		byte at = (byte)(asyncIo.ReadOneByte() & Nb[n]);
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			BigInteger m = 0;
			BigInteger i = 0;
			do
			{
				at = asyncIo.ReadOneByte();
				i |= (at & 0b_01111111) * (2^m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return i + Nb[n];
		}
		else
		{
			return at;
		}
	}
	public static BigInteger ReadBigInteger(byte n, Stream stream)
	{
		int at = stream.ReadByte() & Nb[n];
		if (at < 0)
		{
			throw new IOException();
		}
		//	比如 n = 5 时 下面这种情况会进入if
		//	  0   1   2   3   4   5   6   7
		//	+---+---+---+---+---+---+---+---+
		//	| ? | ? | ? | 1   1   1   1   1 |
		//	+---+---+---+-------------------+
		if ((Nb[n] & at) == Nb[n])
		{
			BigInteger m = 0;
			BigInteger i = 0;
			do
			{
				at = stream.ReadByte();
				if (at < 0)
				{
					throw new IOException();
				}
				i |= (at & 0b_01111111) * (2^m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return i + Nb[n];
		}
		else
		{
			return at;
		}
	}
	
    
}