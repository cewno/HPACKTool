namespace HPACKTool;

public static partial class IntegerTool
{
	public static void writeUInt(uint data, byte n, byte head, AsyncIO asyncIO)
	{
		if (data < Nb[n])
		{
			asyncIO.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			asyncIO.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_011111111)
			{
				asyncIO.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}
			asyncIO.WriteByte((byte)(data & 0b_01111111));
		}
	}
	public static void writeUInt(uint data, byte n, byte head, Stream stream)
	{
		if (data < Nb[n])
		{
			stream.WriteByte((byte)(head | (byte)data));
		}
		else
		{
			data -= Nb[n];
			stream.WriteByte((byte)(head | Nb[n]));
			while (data > 0b_011111111)
			{
				stream.WriteByte((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}
			stream.WriteByte((byte)(data & 0b_01111111));
		}
	}
	public static int writeUInt(uint data,byte n, byte head, byte[] buffer,int offset)
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
			while (data > 0b_011111111)
			{
				buffer[index++] = (byte)((data & 0b_01111111) | 0b_10000000);
				data >>= 7;
			}
			buffer[index++] = (byte)(data & 0b_01111111);
		}

		return index - offset;
	}
	public static byte[] writeUInt(uint data, byte n, byte head)
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
			while (data > 0b_011111111)
			{
				bytes.Add((byte)((data & 0b_01111111) | 0b_10000000));
				data >>= 7;
			}
			bytes.Add((byte)(data & 0b_01111111));
			return bytes.ToArray();
		}
	}
}