using System.Numerics;

namespace cewno.HPACKTool;

public static partial class IntegerTool
{
	#region wUInt

	/// <summary>
	///     写入整数到普通io
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="stream">普通io</param>
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

	/// <summary>
	///     写入整数到缓冲区
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="buffer">缓冲区</param>
	/// <param name="offset">缓冲区偏移量</param>
	/// <returns>写入的数据长度</returns>
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

	/// <summary>
	///     写入整数到一个新的缓冲区，基于<see cref="List{T}" />实现
	///     这个方法比较慢
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <returns>新建立的缓冲区</returns>
	public static byte[] WriteUInteger(uint data, byte n, byte head)
	{
		if (data < Nb[n]) return new[] { (byte)(head | (byte)data) };

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

	/// <summary>
	///     写入整数到普通io
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="stream">普通io</param>
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

	/// <summary>
	///     写入整数到缓冲区
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="buffer">缓冲区</param>
	/// <param name="offset">缓冲区偏移量</param>
	/// <returns>写入的数据长度</returns>
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

	/// <summary>
	///     写入整数到一个新的缓冲区，基于<see cref="List{T}" />实现
	///     这个方法比较慢
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <returns>新建立的缓冲区</returns>
	public static byte[] WriteUInteger(ulong data, byte n, byte head)
	{
		if (data < Nb[n]) return new[] { (byte)(head | (byte)data) };

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

	/// <summary>
	///     写入整数到普通io
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="stream">普通io</param>
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

	/// <summary>
	///     写入整数到缓冲区
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="buffer">缓冲区</param>
	/// <param name="offset">缓冲区偏移量</param>
	/// <returns>写入的数据长度</returns>
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

	/// <summary>
	///     写入整数到一个新的缓冲区，基于<see cref="List{T}" />实现
	///     这个方法比较慢
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <returns>新建立的缓冲区</returns>
	public static byte[] WriteUInteger(ushort data, byte n, byte head)
	{
		if (data < Nb[n]) return new[] { (byte)(head | (byte)data) };

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

	/// <summary>
	///     写入整数到普通io
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="stream">普通io</param>
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

	/// <summary>
	///     写入整数到缓冲区
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="buffer">缓冲区</param>
	/// <param name="offset">缓冲区偏移量</param>
	/// <returns>写入的数据长度</returns>
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

	/// <summary>
	///     写入整数到一个新的缓冲区，基于<see cref="List{T}" />实现
	///     这个方法比较慢
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <returns>新建立的缓冲区</returns>
	public static byte[] WriteUInteger(byte data, byte n, byte head)
	{
		if (data < Nb[n]) return new[] { (byte)(head | data) };

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

	/// <summary>
	///     写入整数到普通io
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="stream">普通io</param>
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

	/// <summary>
	///     写入整数到缓冲区
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="buffer">缓冲区</param>
	/// <param name="offset">缓冲区偏移量</param>
	/// <returns>写入的数据长度</returns>
	public static int WriteUInteger(UInt128 data, byte n, byte head, byte[] buffer, int offset)
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

	/// <summary>
	///     写入整数到一个新的缓冲区，基于<see cref="List{T}" />实现
	///     这个方法比较慢
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <returns>新建立的缓冲区</returns>
	public static byte[] WriteUInteger(UInt128 data, byte n, byte head)
	{
		if (data < Nb[n]) return new[] { (byte)(head | (byte)data) };

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
#endif

	#endregion

	#region wBigInteger

	/// <summary>
	///     写入整数到普通io
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="stream">普通io</param>
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

	/// <summary>
	///     写入整数到缓冲区
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <param name="buffer">缓冲区</param>
	/// <param name="offset">缓冲区偏移量</param>
	/// <returns>写入的数据长度</returns>
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

	/// <summary>
	///     写入整数到一个新的缓冲区，基于<see cref="List{T}" />实现
	///     这个方法比较慢
	/// </summary>
	/// <param name="data">整数</param>
	/// <param name="n">前缀长度,参见 <a href="https://www.rfc-editor.org/rfc/rfc7541.html#section-5.1/">RFC7541 第5.1节</a></param>
	/// <param name="head">首位头数据</param>
	/// <returns>新建立的缓冲区</returns>
	public static byte[] WriteUInteger(BigInteger data, byte n, byte head)
	{
		if (data < Nb[n]) return new[] { (byte)(head | (byte)data) };

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