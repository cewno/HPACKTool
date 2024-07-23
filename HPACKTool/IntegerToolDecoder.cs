using System.Numerics;

namespace HPACKTool;

public static class IntegerTool
{
	private static readonly byte[] Nb = new byte[]
	{
		0b_0,        //index 0
		0b_00000001, //index 1
		0b_00000011, //index 2
		0b_00000111, //index 3
		0b_00001111, //index 4
		0b_00011111, //index 5
		0b_00111111, //index 6
		0b_01111111, //index 7
		0b_11111111  //index 8
	};

	#region rUInt
	
	/// <summary>
	/// 从双线程异步io中读取数字并解码
	/// ！！！方法不提供大小检查，请确保使用的返回类型的最大值大于所设定最大值
	/// </summary>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="asyncIo">异步io</param>
	/// <returns>解码后的数字</returns>
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
					throw new IOException();
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

	public static uint ReadUInt(byte n, byte[] buffer, int offset = 0)
	{
		int index = offset;
		byte at = (byte)(buffer[index++] & Nb[n]);
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
				at = buffer[index++];
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
	#endregion

	#region rULong

	/// <summary>
	/// 从双线程异步io中读取数字并解码
	/// ！！！方法不提供大小检查，请确保使用的返回类型的最大值大于所设定最大值
	/// </summary>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="asyncIo">异步io</param>
	/// <returns>解码后的数字</returns>
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
	
	public static ulong ReadULong(byte n, byte[] buffer, int offset = 0)
	{
		int index = offset;
		byte at = (byte)(buffer[index++] & Nb[n]);
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
				at = buffer[index++];
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
	#endregion
	
	#region rUShort
	
	/// <summary>
	/// 从双线程异步io中读取数字并解码
	/// ！！！方法不提供大小检查，请确保使用的返回类型的最大值大于所设定最大值
	/// </summary>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="asyncIo">异步io</param>
	/// <returns>解码后的数字</returns>
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
	public static ushort ReadUShort(byte n, byte[] buffer, int offset = 0)
	{
		int index = offset;
		byte at = (byte)(buffer[index++] & Nb[n]);
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
				at = buffer[index++];
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
	
	#endregion

	#region rByte

	/// <summary>
	/// 从双线程异步io中读取数字并解码
	/// ！！！方法不提供大小检查，请确保使用的返回类型的最大值大于所设定最大值
	/// </summary>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="asyncIo">异步io</param>
	/// <returns>解码后的数字</returns>
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
	public static byte ReadByte(byte n, byte[] buffer, int offset = 0)
	{
		int index = offset;
		byte at = (byte)(buffer[index++] & Nb[n]);
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
				at = buffer[index++];
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
	
	#endregion

	#region RUint128
	
	
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
	
	#endregion

	#region RBigint


	/// <summary>
	/// 从双线程异步io中读取数字并解码
	/// ！！！方法不提供大小检查，请确保使用的返回类型的最大值大于所设定最大值
	/// </summary>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="asyncIo">异步io</param>
	/// <returns>解码后的数字</returns>
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

	public static BigInteger ReadBigInteger(byte n, byte[] buffer, int offset = 0)
	{
		int index = offset;
		byte at = (byte)(buffer[index++] & Nb[n]);
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
				at = buffer[index++];
				i |= (at & 0b_01111111) * (2 ^ m);
				m += 7;
			} while ((at & 0b_10000000) == 0b_10000000);

			return i + Nb[n];
		}
		else
		{
			return at;
		}
	}

	#endregion
	
    
}