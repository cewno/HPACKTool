using System.Numerics;

namespace HPACKTool;

public static partial class IntegerTool
{
	#region wUInt

	public static void WriteUInteger(uint data, byte n, byte head, IAsyncIO asyncIO)
	{
		if (data < Nb[n])
		{
			asyncIO.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			asyncIO.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				asyncIO.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}

			asyncIO.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static void WriteUInteger(uint data, byte n, byte head, Stream stream)
	{
		if (data < Nb[n])
		{
			stream.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			stream.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				stream.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}

			stream.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static int WriteUInteger(uint data, byte n, byte head, byte[] buffer, int offset)
	{
		int index = offset;
		if (data < Nb[n])
		{
			buffer[index++] = (byte)(head | (byte)data);
		}
		else
		{
			data -= Nb[n];
			buffer[index++] = (byte)(head | Nb[n]);
			while (data > 0b_01111111)
			{
				buffer[index++] = (byte)((data & 0b_01111111) | 0b_10000000);
				data >>= 7;
			}

			buffer[index++] = (byte)(data & 0b_01111111);
		}

		return index - offset;
	}

	public static byte[] WriteUInteger(uint data, byte n, byte head)
	{
		if (data < Nb[n])
		{
			return new[] { (byte)(head | (byte)data) };
		}

		List<byte> bytes = new List<byte>();
		data -= Nb[n];
		bytes.Add((byte)(head | Nb[n]));
		while (data > 0b_01111111)
		{
			bytes.Add((byte)((data & 0b_01111111) | 0b_10000000));
			data >>= 7;
		}

		bytes.Add((byte)(data & 0b_01111111));
		return bytes.ToArray();
	}

	#endregion

	#region wULong

	public static void WriteUInteger(ulong data, byte n, byte head, IAsyncIO asyncIO)
	{
		if (data < Nb[n])
		{
			asyncIO.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			asyncIO.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				asyncIO.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}

			asyncIO.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static void WriteUInteger(ulong data, byte n, byte head, Stream stream)
	{
		if (data < Nb[n])
		{
			stream.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			stream.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				stream.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}

			stream.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static int WriteUInteger(ulong data, byte n, byte head, byte[] buffer, int offset)
	{
		int index = offset;
		if (data < Nb[n])
		{
			buffer[index++] = (byte)(head | (byte)data);
		}
		else
		{
			data -= Nb[n];
			buffer[index++] = (byte)(head | Nb[n]);
			while (data > 0b_01111111)
			{
				buffer[index++] = (byte)((data & 0b_01111111) | 0b_10000000);
				data >>= 7;
			}

			buffer[index++] = (byte)(data & 0b_01111111);
		}

		return index - offset;
	}

	public static byte[] WriteUInteger(ulong data, byte n, byte head)
	{
		if (data < Nb[n])
		{
			return new[] { (byte)(head | (byte)data) };
		}

		List<byte> bytes = new List<byte>();
		data -= Nb[n];
		bytes.Add((byte)(head | Nb[n]));
		while (data > 0b_01111111)
		{
			bytes.Add((byte)((data & 0b_01111111) | 0b_10000000));
			data >>= 7;
		}

		bytes.Add((byte)(data & 0b_01111111));
		return bytes.ToArray();
	}

	#endregion

	#region wUShort

	public static void WriteUInteger(ushort data, byte n, byte head, IAsyncIO asyncIO)
	{
		if (data < Nb[n])
		{
			asyncIO.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			asyncIO.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				asyncIO.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}

			asyncIO.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static void WriteUInteger(ushort data, byte n, byte head, Stream stream)
	{
		if (data < Nb[n])
		{
			stream.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			stream.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				stream.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}

			stream.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static int WriteUInteger(ushort data, byte n, byte head, byte[] buffer, int offset)
	{
		int index = offset;
		if (data < Nb[n])
		{
			buffer[index++] = (byte)(head | (byte)data);
		}
		else
		{
			data -= Nb[n];
			buffer[index++] = (byte)(head | Nb[n]);
			while (data > 0b_01111111)
			{
				buffer[index++] = (byte)((data & 0b_01111111) | 0b_10000000);
				data >>= 7;
			}

			buffer[index++] = (byte)(data & 0b_01111111);
		}

		return index - offset;
	}

	public static byte[] WriteUInteger(ushort data, byte n, byte head)
	{
		if (data < Nb[n])
		{
			return new[] { (byte)(head | (byte)data) };
		}

		List<byte> bytes = new List<byte>();
		data -= Nb[n];
		bytes.Add((byte)(head | Nb[n]));
		while (data > 0b_01111111)
		{
			bytes.Add((byte)((data & 0b_01111111) | 0b_10000000));
			data >>= 7;
		}

		bytes.Add((byte)(data & 0b_01111111));
		return bytes.ToArray();
	}

	#endregion


	#region wByte

	public static void WriteUInteger(byte data, byte n, byte head, IAsyncIO asyncIO)
	{
		if (data < Nb[n])
		{
			asyncIO.WriteByte((byte)(head | data));
		}
		else
		{
			data -= Nb[n];
			// asyncIO.WriteByte((byte)(head | Nb[n]));// =false
			// while (data > 0b_01111111)
			// {
			// 	asyncIO.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
			// 	data >>= 7;
			// }
			asyncIO.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static void WriteUInteger(byte data, byte n, byte head, Stream stream)
	{
		if (data < Nb[n])
		{
			stream.WriteByte((byte)(head | data));
		}
		else
		{
			data -= Nb[n];
			// stream.WriteByte((byte)(head | Nb[n]));// = false
			// while (data > 0b_01111111)
			// {
			// 	stream.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
			// 	data >>= 7;
			// }
			stream.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static int WriteUInteger(byte data, byte n, byte head, byte[] buffer, int offset)
	{
		int index = offset;
		if (data < Nb[n])
		{
			buffer[index++] = (byte)(head | data);
		}
		else
		{
			data -= Nb[n];
			buffer[index++] = (byte)(head | Nb[n]);
			// while (data > 0b_01111111)// = false
			// {
			// 	buffer[index++] = (byte)((data & 0b_01111111) | 0b_10000000);
			// 	data >>= 7;
			// }
			buffer[index++] = (byte)(data & 0b_01111111);
		}

		return index - offset;
	}

	public static byte[] WriteUInteger(byte data, byte n, byte head)
	{
		if (data < Nb[n])
		{
			return new[] { (byte)(head | data) };
		}

		List<byte> bytes = new List<byte>();
		data -= Nb[n];
		bytes.Add((byte)(head | Nb[n]));
		while (data > 0b_01111111)
		{
			bytes.Add((byte)((data & 0b_01111111) | 0b_10000000));
			data >>= 7;
		}

		bytes.Add((byte)(data & 0b_01111111));
		return bytes.ToArray();
	}

	#endregion

	#region wUInt128

#if NET7_0_OR_GREATER
	public static void WriteUInteger(UInt128 data, byte n, byte head, IAsyncIO asyncIO)
	{
		if (data < Nb[n])
		{
			asyncIO.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			asyncIO.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				asyncIO.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}
			asyncIO.WriteByte((byte)(data & 0b_01111111));
		}
	}
	public static void WriteUInteger(UInt128 data, byte n, byte head, Stream stream)
	{
		if (data < Nb[n])
		{
			stream.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			stream.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				stream.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}
			stream.WriteByte((byte)(data & 0b_01111111));
		}
	}
	public static int WriteUInteger(UInt128 data,byte n, byte head, byte[] buffer,int offset)
	{
		int index = offset;
		if (data < Nb[n])
		{
			buffer[index++] = (byte)(head | (byte)data);
		}
		else
		{
			data -= Nb[n];
			buffer[index++] = (byte)(head | Nb[n]);
			while (data > 0b_01111111)
			{
				buffer[index++] = (byte)((data & 0b_01111111) | 0b_10000000);
				data >>= 7;
			}
			buffer[index++] = (byte)(data & 0b_01111111);
		}

		return index - offset;
	}
	public static byte[] WriteUInteger(UInt128 data, byte n, byte head)
	{
		if (data < Nb[n])
		{
			return new[] { (byte)(head | (byte)data) };
		}
		else
		{
			List<byte> bytes = new List<byte>();
			data -= Nb[n];
			bytes.Add((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				bytes.Add((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}
			bytes.Add((byte)(data & 0b_01111111));
			return bytes.ToArray();
		}
	}
#endif

	#endregion

	#region wBigInteger

	public static void WriteUInteger(BigInteger data, byte n, byte head, IAsyncIO asyncIO)
	{
		if (data < Nb[n])
		{
			asyncIO.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			asyncIO.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				asyncIO.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}

			asyncIO.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static void WriteUInteger(BigInteger data, byte n, byte head, Stream stream)
	{
		if (data < Nb[n])
		{
			stream.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			stream.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_01111111)
			{
				stream.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}

			stream.WriteByte((byte)(data & 0b_01111111));
		}
	}

	public static int WriteUInteger(BigInteger data, byte n, byte head, byte[] buffer, int offset)
	{
		int index = offset;
		if (data < Nb[n])
		{
			buffer[index++] = (byte)(head | (byte)data);
		}
		else
		{
			data -= Nb[n];
			buffer[index++] = (byte)(head | Nb[n]);
			while (data > 0b_01111111)
			{
				buffer[index++] = (byte)((data & 0b_01111111) | 0b_10000000);
				data >>= 7;
			}

			buffer[index++] = (byte)(data & 0b_01111111);
		}

		return index - offset;
	}

	public static byte[] WriteUInteger(BigInteger data, byte n, byte head)
	{
		if (data < Nb[n])
		{
			return new[] { (byte)(head | (byte)data) };
		}

		List<byte> bytes = new List<byte>();
		data -= Nb[n];
		bytes.Add((byte)(head | Nb[n]));
		while (data > 0b_01111111)
		{
			bytes.Add((byte)((data & 0b_01111111) | 0b_10000000));
			data >>= 7;
		}

		bytes.Add((byte)(data & 0b_01111111));
		return bytes.ToArray();
	}

	#endregion
}