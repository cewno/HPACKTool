using System.Text;

namespace cewno.HPACKTool;

public static partial class HuffmanTool
{
	private static readonly byte[] AllBytes =
		new byte[]
		{
			0b_00000000, 0b_00001000, 0b_00010000, 0b_00011000, 0b_00100000, 0b_00101000, 0b_00110000, 0b_00111000,
			0b_01000000, 0b_01001000, 0b_01010000, 0b_01010100, 0b_01011000, 0b_01011100, 0b_01100000, 0b_01100100,
			0b_01101000, 0b_01101100, 0b_01110000, 0b_01110100, 0b_01111000, 0b_01111100, 0b_10000000, 0b_10000100,
			0b_10001000, 0b_10001100, 0b_10010000, 0b_10010100, 0b_10011000, 0b_10011100, 0b_10100000, 0b_10100100,
			0b_10101000, 0b_10101100, 0b_10110000, 0b_10110100, 0b_10111000, 0b_10111010, 0b_10111100, 0b_10111110,
			0b_11000000, 0b_11000010, 0b_11000100, 0b_11000110, 0b_11001000, 0b_11001010, 0b_11001100, 0b_11001110,
			0b_11010000, 0b_11010010, 0b_11010100, 0b_11010110, 0b_11011000, 0b_11011010, 0b_11011100, 0b_11011110,
			0b_11100000, 0b_11100010, 0b_11100100, 0b_11100110, 0b_11101000, 0b_11101010, 0b_11101100, 0b_11101110,
			0b_11110000, 0b_11110010, 0b_11110100, 0b_11110110, 0b_11111000, 0b_11111001, 0b_11111010, 0b_11111011,
			0b_11111100, 0b_11111101, 0b_11111110, 0b_00000000, 0b_11111110, 0b_01000000, 0b_11111110, 0b_10000000,
			0b_11111110, 0b_11000000, 0b_11111111, 0b_00000000, 0b_11111111, 0b_01000000, 0b_11111111, 0b_01100000,
			0b_11111111, 0b_10000000, 0b_11111111, 0b_10100000, 0b_11111111, 0b_10110000, 0b_11111111, 0b_11000000,
			0b_11111111, 0b_11001000, 0b_11111111, 0b_11010000, 0b_11111111, 0b_11011000, 0b_11111111, 0b_11100000,
			0b_11111111, 0b_11101000, 0b_11111111, 0b_11110000, 0b_11111111, 0b_11110100, 0b_11111111, 0b_11111000,
			0b_11111111, 0b_11111010, 0b_11111111, 0b_11111100, 0b_11111111, 0b_11111110, 0b_00000000, 0b_11111111,
			0b_11111110, 0b_00100000, 0b_11111111, 0b_11111110, 0b_01000000, 0b_11111111, 0b_11111110, 0b_01100000,
			0b_11111111, 0b_11111110, 0b_01110000, 0b_11111111, 0b_11111110, 0b_10000000, 0b_11111111, 0b_11111110,
			0b_10010000, 0b_11111111, 0b_11111110, 0b_10100000, 0b_11111111, 0b_11111110, 0b_10110000, 0b_11111111,
			0b_11111110, 0b_11000000, 0b_11111111, 0b_11111110, 0b_11010000, 0b_11111111, 0b_11111110, 0b_11100000,
			0b_11111111, 0b_11111110, 0b_11101000, 0b_11111111, 0b_11111110, 0b_11110000, 0b_11111111, 0b_11111110,
			0b_11111000, 0b_11111111, 0b_11111111, 0b_00000000, 0b_11111111, 0b_11111111, 0b_00001000, 0b_11111111,
			0b_11111111, 0b_00010000, 0b_11111111, 0b_11111111, 0b_00011000, 0b_11111111, 0b_11111111, 0b_00100000,
			0b_11111111, 0b_11111111, 0b_00101000, 0b_11111111, 0b_11111111, 0b_00110000, 0b_11111111, 0b_11111111,
			0b_00111000, 0b_11111111, 0b_11111111, 0b_01000000, 0b_11111111, 0b_11111111, 0b_01001000, 0b_11111111,
			0b_11111111, 0b_01001100, 0b_11111111, 0b_11111111, 0b_01010000, 0b_11111111, 0b_11111111, 0b_01010100,
			0b_11111111, 0b_11111111, 0b_01011000, 0b_11111111, 0b_11111111, 0b_01011100, 0b_11111111, 0b_11111111,
			0b_01100000, 0b_11111111, 0b_11111111, 0b_01100100, 0b_11111111, 0b_11111111, 0b_01101000, 0b_11111111,
			0b_11111111, 0b_01101100, 0b_11111111, 0b_11111111, 0b_01110000, 0b_11111111, 0b_11111111, 0b_01110100,
			0b_11111111, 0b_11111111, 0b_01111000, 0b_11111111, 0b_11111111, 0b_01111100, 0b_11111111, 0b_11111111,
			0b_10000000, 0b_11111111, 0b_11111111, 0b_10000100, 0b_11111111, 0b_11111111, 0b_10001000, 0b_11111111,
			0b_11111111, 0b_10001100, 0b_11111111, 0b_11111111, 0b_10010000, 0b_11111111, 0b_11111111, 0b_10010100,
			0b_11111111, 0b_11111111, 0b_10011000, 0b_11111111, 0b_11111111, 0b_10011100, 0b_11111111, 0b_11111111,
			0b_10100000, 0b_11111111, 0b_11111111, 0b_10100100, 0b_11111111, 0b_11111111, 0b_10101000, 0b_11111111,
			0b_11111111, 0b_10101100, 0b_11111111, 0b_11111111, 0b_10110000, 0b_11111111, 0b_11111111, 0b_10110010,
			0b_11111111, 0b_11111111, 0b_10110100, 0b_11111111, 0b_11111111, 0b_10110110, 0b_11111111, 0b_11111111,
			0b_10111000, 0b_11111111, 0b_11111111, 0b_10111010, 0b_11111111, 0b_11111111, 0b_10111100, 0b_11111111,
			0b_11111111, 0b_10111110, 0b_11111111, 0b_11111111, 0b_11000000, 0b_11111111, 0b_11111111, 0b_11000010,
			0b_11111111, 0b_11111111, 0b_11000100, 0b_11111111, 0b_11111111, 0b_11000110, 0b_11111111, 0b_11111111,
			0b_11001000, 0b_11111111, 0b_11111111, 0b_11001010, 0b_11111111, 0b_11111111, 0b_11001100, 0b_11111111,
			0b_11111111, 0b_11001110, 0b_11111111, 0b_11111111, 0b_11010000, 0b_11111111, 0b_11111111, 0b_11010010,
			0b_11111111, 0b_11111111, 0b_11010100, 0b_11111111, 0b_11111111, 0b_11010110, 0b_11111111, 0b_11111111,
			0b_11011000, 0b_11111111, 0b_11111111, 0b_11011010, 0b_11111111, 0b_11111111, 0b_11011100, 0b_11111111,
			0b_11111111, 0b_11011110, 0b_11111111, 0b_11111111, 0b_11100000, 0b_11111111, 0b_11111111, 0b_11100010,
			0b_11111111, 0b_11111111, 0b_11100100, 0b_11111111, 0b_11111111, 0b_11100110, 0b_11111111, 0b_11111111,
			0b_11101000, 0b_11111111, 0b_11111111, 0b_11101010, 0b_11111111, 0b_11111111, 0b_11101011, 0b_11111111,
			0b_11111111, 0b_11101100, 0b_11111111, 0b_11111111, 0b_11101101, 0b_11111111, 0b_11111111, 0b_11101110,
			0b_11111111, 0b_11111111, 0b_11101111, 0b_11111111, 0b_11111111, 0b_11110000, 0b_11111111, 0b_11111111,
			0b_11110001, 0b_11111111, 0b_11111111, 0b_11110010, 0b_11111111, 0b_11111111, 0b_11110011, 0b_11111111,
			0b_11111111, 0b_11110100, 0b_11111111, 0b_11111111, 0b_11110101, 0b_11111111, 0b_11111111, 0b_11110110,
			0b_00000000, 0b_11111111, 0b_11111111, 0b_11110110, 0b_10000000, 0b_11111111, 0b_11111111, 0b_11110111,
			0b_00000000, 0b_11111111, 0b_11111111, 0b_11110111, 0b_10000000, 0b_11111111, 0b_11111111, 0b_11111000,
			0b_00000000, 0b_11111111, 0b_11111111, 0b_11111000, 0b_01000000, 0b_11111111, 0b_11111111, 0b_11111000,
			0b_10000000, 0b_11111111, 0b_11111111, 0b_11111000, 0b_11000000, 0b_11111111, 0b_11111111, 0b_11111001,
			0b_00000000, 0b_11111111, 0b_11111111, 0b_11111001, 0b_01000000, 0b_11111111, 0b_11111111, 0b_11111001,
			0b_10000000, 0b_11111111, 0b_11111111, 0b_11111001, 0b_11000000, 0b_11111111, 0b_11111111, 0b_11111010,
			0b_00000000, 0b_11111111, 0b_11111111, 0b_11111010, 0b_01000000, 0b_11111111, 0b_11111111, 0b_11111010,
			0b_10000000, 0b_11111111, 0b_11111111, 0b_11111010, 0b_11000000, 0b_11111111, 0b_11111111, 0b_11111011,
			0b_00000000, 0b_11111111, 0b_11111111, 0b_11111011, 0b_01000000, 0b_11111111, 0b_11111111, 0b_11111011,
			0b_10000000, 0b_11111111, 0b_11111111, 0b_11111011, 0b_11000000, 0b_11111111, 0b_11111111, 0b_11111011,
			0b_11100000, 0b_11111111, 0b_11111111, 0b_11111100, 0b_00000000, 0b_11111111, 0b_11111111, 0b_11111100,
			0b_00100000, 0b_11111111, 0b_11111111, 0b_11111100, 0b_01000000, 0b_11111111, 0b_11111111, 0b_11111100,
			0b_01100000, 0b_11111111, 0b_11111111, 0b_11111100, 0b_10000000, 0b_11111111, 0b_11111111, 0b_11111100,
			0b_10100000, 0b_11111111, 0b_11111111, 0b_11111100, 0b_11000000, 0b_11111111, 0b_11111111, 0b_11111100,
			0b_11100000, 0b_11111111, 0b_11111111, 0b_11111101, 0b_00000000, 0b_11111111, 0b_11111111, 0b_11111101,
			0b_00100000, 0b_11111111, 0b_11111111, 0b_11111101, 0b_01000000, 0b_11111111, 0b_11111111, 0b_11111101,
			0b_01100000, 0b_11111111, 0b_11111111, 0b_11111101, 0b_10000000, 0b_11111111, 0b_11111111, 0b_11111101,
			0b_10100000, 0b_11111111, 0b_11111111, 0b_11111101, 0b_11000000, 0b_11111111, 0b_11111111, 0b_11111101,
			0b_11100000, 0b_11111111, 0b_11111111, 0b_11111110, 0b_00000000, 0b_11111111, 0b_11111111, 0b_11111110,
			0b_00100000, 0b_11111111, 0b_11111111, 0b_11111110, 0b_00110000, 0b_11111111, 0b_11111111, 0b_11111110,
			0b_01000000, 0b_11111111, 0b_11111111, 0b_11111110, 0b_01010000, 0b_11111111, 0b_11111111, 0b_11111110,
			0b_01100000, 0b_11111111, 0b_11111111, 0b_11111110, 0b_01110000, 0b_11111111, 0b_11111111, 0b_11111110,
			0b_10000000, 0b_11111111, 0b_11111111, 0b_11111110, 0b_10010000, 0b_11111111, 0b_11111111, 0b_11111110,
			0b_10100000, 0b_11111111, 0b_11111111, 0b_11111110, 0b_10110000, 0b_11111111, 0b_11111111, 0b_11111110,
			0b_11000000, 0b_11111111, 0b_11111111, 0b_11111110, 0b_11010000, 0b_11111111, 0b_11111111, 0b_11111110,
			0b_11100000, 0b_11111111, 0b_11111111, 0b_11111110, 0b_11110000, 0b_11111111, 0b_11111111, 0b_11111111,
			0b_00000000, 0b_11111111, 0b_11111111, 0b_11111111, 0b_00010000, 0b_11111111, 0b_11111111, 0b_11111111,
			0b_00100000, 0b_11111111, 0b_11111111, 0b_11111111, 0b_00110000, 0b_11111111, 0b_11111111, 0b_11111111,
			0b_01000000, 0b_11111111, 0b_11111111, 0b_11111111, 0b_01010000, 0b_11111111, 0b_11111111, 0b_11111111,
			0b_01100000, 0b_11111111, 0b_11111111, 0b_11111111, 0b_01110000, 0b_11111111, 0b_11111111, 0b_11111111,
			0b_10000000, 0b_11111111, 0b_11111111, 0b_11111111, 0b_10010000, 0b_11111111, 0b_11111111, 0b_11111111,
			0b_10100000, 0b_11111111, 0b_11111111, 0b_11111111, 0b_10110000, 0b_11111111, 0b_11111111, 0b_11111111,
			0b_11000000, 0b_11111111, 0b_11111111, 0b_11111111, 0b_11010000, 0b_11111111, 0b_11111111, 0b_11111111,
			0b_11100000, 0b_11111111, 0b_11111111, 0b_11111111, 0b_11110000, 0b_11111111, 0b_11111111, 0b_11111111,
			0b_11110100, 0b_11111111, 0b_11111111, 0b_11111111, 0b_11111000
		};

	/// <summary>
	///     使用适用于 HTTP2 的 Huffman 压缩算法将 <see cref="string" /> 解码
	/// </summary>
	/// <param name="data">编码前的数据</param>
	/// <returns>编码后的缓冲区</returns>
	public static byte[] Encoder(string data)
	{
		return Encoder(Encoding.ASCII.GetBytes(data)).ToArray();
	}

	/// <summary>
	///     使用适用于 HTTP2 的 Huffman 压缩算法将缓冲区内的 ascii 文本解码到缓冲区
	/// </summary>
	/// <param name="data">已编码数据</param>
	/// <returns>包含结果的<see cref="Memory{T}" /></returns>
	public static Memory<byte> Encoder(byte[] data)
	{
		byte[] bytes = new byte[(int)(data.Length * 3.75)];
		int encoder = Encoder(data, bytes);
		return new Memory<byte>(bytes, 0, encoder);
	}


	/// <summary>
	///     使用适用于 HTTP2 的 Huffman 压缩算法将缓冲区内的 ascii 文本解码到缓冲区
	/// </summary>
	/// <param name="data">已编码数据</param>
	/// <param name="re">结果缓冲区</param>
	/// <returns>结果实际占用的长度</returns>
	public static unsafe int Encoder(Span<byte> data, Span<byte> re)
	{
		fixed (byte* ptrd = data)
		{
			fixed (byte* ptrr = re)
			{
				return Encoder(ptrd, data.Length, ptrr, re.Length);
			}
		}
	}


	/// <summary>
	///     使用适用于 HTTP2 的 Huffman 压缩算法将缓冲区内的 ascii 文本解码到缓冲区
	/// </summary>
	/// <param name="data">已编码数据</param>
	/// <param name="dataLength">已编码数据长度</param>
	/// <param name="result">结果缓冲区</param>
	/// <param name="resultLength">结果缓冲区最大长度</param>
	/// <returns>结果实际占用的长度</returns>
	/// <exception cref="IndexOutOfRangeException">当缓冲区太小时抛出</exception>
	public static unsafe int Encoder(byte* data, int dataLength, byte* result, int resultLength)
	{
		if (dataLength == 0 || resultLength == 0) return -1;
		//NoStackallocList tdata = new NoStackallocList(result);
		//List<byte> tdata = new List<byte>();
		//last 位长度
		int lastOffset = 0;
		//上一个未写入完成的 bit
		byte last = 0;
		//结果写入索引
		int wi = 0;

		//ASCll

		for (int j = 0; j < dataLength; j++)
		{
			byte at = data[j];
			int starti = 0;
			byte bytesl = 0;
			byte bitl = 0;

			#region switch

			switch (at)
			{
				case (byte)'0':
					starti = 0;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)'1':
					starti = 1;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)'2':
					starti = 2;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)'a':
					starti = 3;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)'c':
					starti = 4;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)'e':
					starti = 5;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)'i':
					starti = 6;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)'o':
					starti = 7;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)'s':
					starti = 8;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)'t':
					starti = 9;
					bytesl = 1;
					bitl = 5;
					break;
				case (byte)' ':
					starti = 10;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'%':
					starti = 11;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'-':
					starti = 12;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'.':
					starti = 13;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'/':
					starti = 14;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'3':
					starti = 15;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'4':
					starti = 16;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'5':
					starti = 17;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'6':
					starti = 18;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'7':
					starti = 19;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'8':
					starti = 20;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'9':
					starti = 21;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'=':
					starti = 22;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'A':
					starti = 23;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'_':
					starti = 24;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'b':
					starti = 25;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'d':
					starti = 26;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'f':
					starti = 27;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'g':
					starti = 28;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'h':
					starti = 29;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'l':
					starti = 30;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'m':
					starti = 31;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'n':
					starti = 32;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'p':
					starti = 33;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'r':
					starti = 34;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)'u':
					starti = 35;
					bytesl = 1;
					bitl = 6;
					break;
				case (byte)':':
					starti = 36;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'B':
					starti = 37;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'C':
					starti = 38;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'D':
					starti = 39;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'E':
					starti = 40;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'F':
					starti = 41;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'G':
					starti = 42;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'H':
					starti = 43;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'I':
					starti = 44;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'J':
					starti = 45;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'K':
					starti = 46;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'L':
					starti = 47;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'M':
					starti = 48;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'N':
					starti = 49;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'O':
					starti = 50;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'P':
					starti = 51;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'Q':
					starti = 52;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'R':
					starti = 53;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'S':
					starti = 54;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'T':
					starti = 55;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'U':
					starti = 56;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'V':
					starti = 57;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'W':
					starti = 58;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'Y':
					starti = 59;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'j':
					starti = 60;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'k':
					starti = 61;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'q':
					starti = 62;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'v':
					starti = 63;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'w':
					starti = 64;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'x':
					starti = 65;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'y':
					starti = 66;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'z':
					starti = 67;
					bytesl = 1;
					bitl = 7;
					break;
				case (byte)'&':
					starti = 68;
					bytesl = 1;
					bitl = 8;
					break;
				case (byte)'*':
					starti = 69;
					bytesl = 1;
					bitl = 8;
					break;
				case (byte)',':
					starti = 70;
					bytesl = 1;
					bitl = 8;
					break;
				case (byte)';':
					starti = 71;
					bytesl = 1;
					bitl = 8;
					break;
				case (byte)'X':
					starti = 72;
					bytesl = 1;
					bitl = 8;
					break;
				case (byte)'Z':
					starti = 73;
					bytesl = 1;
					bitl = 8;
					break;
				case (byte)'!':
					starti = 74;
					bytesl = 2;
					bitl = 10;
					break;
				case (byte)'"':
					starti = 76;
					bytesl = 2;
					bitl = 10;
					break;
				case (byte)'(':
					starti = 78;
					bytesl = 2;
					bitl = 10;
					break;
				case (byte)')':
					starti = 80;
					bytesl = 2;
					bitl = 10;
					break;
				case (byte)'?':
					starti = 82;
					bytesl = 2;
					bitl = 10;
					break;
				case (byte)'\'':
					starti = 84;
					bytesl = 2;
					bitl = 11;
					break;
				case (byte)'+':
					starti = 86;
					bytesl = 2;
					bitl = 11;
					break;
				case (byte)'|':
					starti = 88;
					bytesl = 2;
					bitl = 11;
					break;
				case (byte)'#':
					starti = 90;
					bytesl = 2;
					bitl = 12;
					break;
				case (byte)'>':
					starti = 92;
					bytesl = 2;
					bitl = 12;
					break;
				case 0:
					starti = 94;
					bytesl = 2;
					bitl = 13;
					break;
				case (byte)'$':
					starti = 96;
					bytesl = 2;
					bitl = 13;
					break;
				case (byte)'@':
					starti = 98;
					bytesl = 2;
					bitl = 13;
					break;
				case (byte)'[':
					starti = 100;
					bytesl = 2;
					bitl = 13;
					break;
				case (byte)']':
					starti = 102;
					bytesl = 2;
					bitl = 13;
					break;
				case (byte)'~':
					starti = 104;
					bytesl = 2;
					bitl = 13;
					break;
				case (byte)'^':
					starti = 106;
					bytesl = 2;
					bitl = 14;
					break;
				case (byte)'}':
					starti = 108;
					bytesl = 2;
					bitl = 14;
					break;
				case (byte)'<':
					starti = 110;
					bytesl = 2;
					bitl = 15;
					break;
				case (byte)'`':
					starti = 112;
					bytesl = 2;
					bitl = 15;
					break;
				case (byte)'{':
					starti = 114;
					bytesl = 2;
					bitl = 15;
					break;
				case (byte)'\\':
					starti = 116;
					bytesl = 3;
					bitl = 19;
					break;
				case 195:
					starti = 119;
					bytesl = 3;
					bitl = 19;
					break;
				case 208:
					starti = 122;
					bytesl = 3;
					bitl = 19;
					break;
				case 128:
					starti = 125;
					bytesl = 3;
					bitl = 20;
					break;
				case 130:
					starti = 128;
					bytesl = 3;
					bitl = 20;
					break;
				case 131:
					starti = 131;
					bytesl = 3;
					bitl = 20;
					break;
				case 162:
					starti = 134;
					bytesl = 3;
					bitl = 20;
					break;
				case 184:
					starti = 137;
					bytesl = 3;
					bitl = 20;
					break;
				case 194:
					starti = 140;
					bytesl = 3;
					bitl = 20;
					break;
				case 224:
					starti = 143;
					bytesl = 3;
					bitl = 20;
					break;
				case 226:
					starti = 146;
					bytesl = 3;
					bitl = 20;
					break;
				case 153:
					starti = 149;
					bytesl = 3;
					bitl = 21;
					break;
				case 161:
					starti = 152;
					bytesl = 3;
					bitl = 21;
					break;
				case 167:
					starti = 155;
					bytesl = 3;
					bitl = 21;
					break;
				case 172:
					starti = 158;
					bytesl = 3;
					bitl = 21;
					break;
				case 176:
					starti = 161;
					bytesl = 3;
					bitl = 21;
					break;
				case 177:
					starti = 164;
					bytesl = 3;
					bitl = 21;
					break;
				case 179:
					starti = 167;
					bytesl = 3;
					bitl = 21;
					break;
				case 209:
					starti = 170;
					bytesl = 3;
					bitl = 21;
					break;
				case 216:
					starti = 173;
					bytesl = 3;
					bitl = 21;
					break;
				case 217:
					starti = 176;
					bytesl = 3;
					bitl = 21;
					break;
				case 227:
					starti = 179;
					bytesl = 3;
					bitl = 21;
					break;
				case 229:
					starti = 182;
					bytesl = 3;
					bitl = 21;
					break;
				case 230:
					starti = 185;
					bytesl = 3;
					bitl = 21;
					break;
				case 129:
					starti = 188;
					bytesl = 3;
					bitl = 22;
					break;
				case 132:
					starti = 191;
					bytesl = 3;
					bitl = 22;
					break;
				case 133:
					starti = 194;
					bytesl = 3;
					bitl = 22;
					break;
				case 134:
					starti = 197;
					bytesl = 3;
					bitl = 22;
					break;
				case 136:
					starti = 200;
					bytesl = 3;
					bitl = 22;
					break;
				case 146:
					starti = 203;
					bytesl = 3;
					bitl = 22;
					break;
				case 154:
					starti = 206;
					bytesl = 3;
					bitl = 22;
					break;
				case 156:
					starti = 209;
					bytesl = 3;
					bitl = 22;
					break;
				case 160:
					starti = 212;
					bytesl = 3;
					bitl = 22;
					break;
				case 163:
					starti = 215;
					bytesl = 3;
					bitl = 22;
					break;
				case 164:
					starti = 218;
					bytesl = 3;
					bitl = 22;
					break;
				case 169:
					starti = 221;
					bytesl = 3;
					bitl = 22;
					break;
				case 170:
					starti = 224;
					bytesl = 3;
					bitl = 22;
					break;
				case 173:
					starti = 227;
					bytesl = 3;
					bitl = 22;
					break;
				case 178:
					starti = 230;
					bytesl = 3;
					bitl = 22;
					break;
				case 181:
					starti = 233;
					bytesl = 3;
					bitl = 22;
					break;
				case 185:
					starti = 236;
					bytesl = 3;
					bitl = 22;
					break;
				case 186:
					starti = 239;
					bytesl = 3;
					bitl = 22;
					break;
				case 187:
					starti = 242;
					bytesl = 3;
					bitl = 22;
					break;
				case 189:
					starti = 245;
					bytesl = 3;
					bitl = 22;
					break;
				case 190:
					starti = 248;
					bytesl = 3;
					bitl = 22;
					break;
				case 196:
					starti = 251;
					bytesl = 3;
					bitl = 22;
					break;
				case 198:
					starti = 254;
					bytesl = 3;
					bitl = 22;
					break;
				case 228:
					starti = 257;
					bytesl = 3;
					bitl = 22;
					break;
				case 232:
					starti = 260;
					bytesl = 3;
					bitl = 22;
					break;
				case 233:
					starti = 263;
					bytesl = 3;
					bitl = 22;
					break;
				case 1:
					starti = 266;
					bytesl = 3;
					bitl = 23;
					break;
				case 135:
					starti = 269;
					bytesl = 3;
					bitl = 23;
					break;
				case 137:
					starti = 272;
					bytesl = 3;
					bitl = 23;
					break;
				case 138:
					starti = 275;
					bytesl = 3;
					bitl = 23;
					break;
				case 139:
					starti = 278;
					bytesl = 3;
					bitl = 23;
					break;
				case 140:
					starti = 281;
					bytesl = 3;
					bitl = 23;
					break;
				case 141:
					starti = 284;
					bytesl = 3;
					bitl = 23;
					break;
				case 143:
					starti = 287;
					bytesl = 3;
					bitl = 23;
					break;
				case 147:
					starti = 290;
					bytesl = 3;
					bitl = 23;
					break;
				case 149:
					starti = 293;
					bytesl = 3;
					bitl = 23;
					break;
				case 150:
					starti = 296;
					bytesl = 3;
					bitl = 23;
					break;
				case 151:
					starti = 299;
					bytesl = 3;
					bitl = 23;
					break;
				case 152:
					starti = 302;
					bytesl = 3;
					bitl = 23;
					break;
				case 155:
					starti = 305;
					bytesl = 3;
					bitl = 23;
					break;
				case 157:
					starti = 308;
					bytesl = 3;
					bitl = 23;
					break;
				case 158:
					starti = 311;
					bytesl = 3;
					bitl = 23;
					break;
				case 165:
					starti = 314;
					bytesl = 3;
					bitl = 23;
					break;
				case 166:
					starti = 317;
					bytesl = 3;
					bitl = 23;
					break;
				case 168:
					starti = 320;
					bytesl = 3;
					bitl = 23;
					break;
				case 174:
					starti = 323;
					bytesl = 3;
					bitl = 23;
					break;
				case 175:
					starti = 326;
					bytesl = 3;
					bitl = 23;
					break;
				case 180:
					starti = 329;
					bytesl = 3;
					bitl = 23;
					break;
				case 182:
					starti = 332;
					bytesl = 3;
					bitl = 23;
					break;
				case 183:
					starti = 335;
					bytesl = 3;
					bitl = 23;
					break;
				case 188:
					starti = 338;
					bytesl = 3;
					bitl = 23;
					break;
				case 191:
					starti = 341;
					bytesl = 3;
					bitl = 23;
					break;
				case 197:
					starti = 344;
					bytesl = 3;
					bitl = 23;
					break;
				case 231:
					starti = 347;
					bytesl = 3;
					bitl = 23;
					break;
				case 239:
					starti = 350;
					bytesl = 3;
					bitl = 23;
					break;
				case 9:
					starti = 353;
					bytesl = 3;
					bitl = 24;
					break;
				case 142:
					starti = 356;
					bytesl = 3;
					bitl = 24;
					break;
				case 144:
					starti = 359;
					bytesl = 3;
					bitl = 24;
					break;
				case 145:
					starti = 362;
					bytesl = 3;
					bitl = 24;
					break;
				case 148:
					starti = 365;
					bytesl = 3;
					bitl = 24;
					break;
				case 159:
					starti = 368;
					bytesl = 3;
					bitl = 24;
					break;
				case 171:
					starti = 371;
					bytesl = 3;
					bitl = 24;
					break;
				case 206:
					starti = 374;
					bytesl = 3;
					bitl = 24;
					break;
				case 215:
					starti = 377;
					bytesl = 3;
					bitl = 24;
					break;
				case 225:
					starti = 380;
					bytesl = 3;
					bitl = 24;
					break;
				case 236:
					starti = 383;
					bytesl = 3;
					bitl = 24;
					break;
				case 237:
					starti = 386;
					bytesl = 3;
					bitl = 24;
					break;
				case 199:
					starti = 389;
					bytesl = 4;
					bitl = 25;
					break;
				case 207:
					starti = 393;
					bytesl = 4;
					bitl = 25;
					break;
				case 234:
					starti = 397;
					bytesl = 4;
					bitl = 25;
					break;
				case 235:
					starti = 401;
					bytesl = 4;
					bitl = 25;
					break;
				case 192:
					starti = 405;
					bytesl = 4;
					bitl = 26;
					break;
				case 193:
					starti = 409;
					bytesl = 4;
					bitl = 26;
					break;
				case 200:
					starti = 413;
					bytesl = 4;
					bitl = 26;
					break;
				case 201:
					starti = 417;
					bytesl = 4;
					bitl = 26;
					break;
				case 202:
					starti = 421;
					bytesl = 4;
					bitl = 26;
					break;
				case 205:
					starti = 425;
					bytesl = 4;
					bitl = 26;
					break;
				case 210:
					starti = 429;
					bytesl = 4;
					bitl = 26;
					break;
				case 213:
					starti = 433;
					bytesl = 4;
					bitl = 26;
					break;
				case 218:
					starti = 437;
					bytesl = 4;
					bitl = 26;
					break;
				case 219:
					starti = 441;
					bytesl = 4;
					bitl = 26;
					break;
				case 238:
					starti = 445;
					bytesl = 4;
					bitl = 26;
					break;
				case 240:
					starti = 449;
					bytesl = 4;
					bitl = 26;
					break;
				case 242:
					starti = 453;
					bytesl = 4;
					bitl = 26;
					break;
				case 243:
					starti = 457;
					bytesl = 4;
					bitl = 26;
					break;
				case 255:
					starti = 461;
					bytesl = 4;
					bitl = 26;
					break;
				case 203:
					starti = 465;
					bytesl = 4;
					bitl = 27;
					break;
				case 204:
					starti = 469;
					bytesl = 4;
					bitl = 27;
					break;
				case 211:
					starti = 473;
					bytesl = 4;
					bitl = 27;
					break;
				case 212:
					starti = 477;
					bytesl = 4;
					bitl = 27;
					break;
				case 214:
					starti = 481;
					bytesl = 4;
					bitl = 27;
					break;
				case 221:
					starti = 485;
					bytesl = 4;
					bitl = 27;
					break;
				case 222:
					starti = 489;
					bytesl = 4;
					bitl = 27;
					break;
				case 223:
					starti = 493;
					bytesl = 4;
					bitl = 27;
					break;
				case 241:
					starti = 497;
					bytesl = 4;
					bitl = 27;
					break;
				case 244:
					starti = 501;
					bytesl = 4;
					bitl = 27;
					break;
				case 245:
					starti = 505;
					bytesl = 4;
					bitl = 27;
					break;
				case 246:
					starti = 509;
					bytesl = 4;
					bitl = 27;
					break;
				case 247:
					starti = 513;
					bytesl = 4;
					bitl = 27;
					break;
				case 248:
					starti = 517;
					bytesl = 4;
					bitl = 27;
					break;
				case 250:
					starti = 521;
					bytesl = 4;
					bitl = 27;
					break;
				case 251:
					starti = 525;
					bytesl = 4;
					bitl = 27;
					break;
				case 252:
					starti = 529;
					bytesl = 4;
					bitl = 27;
					break;
				case 253:
					starti = 533;
					bytesl = 4;
					bitl = 27;
					break;
				case 254:
					starti = 537;
					bytesl = 4;
					bitl = 27;
					break;
				case 2:
					starti = 541;
					bytesl = 4;
					bitl = 28;
					break;
				case 3:
					starti = 545;
					bytesl = 4;
					bitl = 28;
					break;
				case 4:
					starti = 549;
					bytesl = 4;
					bitl = 28;
					break;
				case 5:
					starti = 553;
					bytesl = 4;
					bitl = 28;
					break;
				case 6:
					starti = 557;
					bytesl = 4;
					bitl = 28;
					break;
				case 7:
					starti = 561;
					bytesl = 4;
					bitl = 28;
					break;
				case 8:
					starti = 565;
					bytesl = 4;
					bitl = 28;
					break;
				case 11:
					starti = 569;
					bytesl = 4;
					bitl = 28;
					break;
				case 12:
					starti = 573;
					bytesl = 4;
					bitl = 28;
					break;
				case 14:
					starti = 577;
					bytesl = 4;
					bitl = 28;
					break;
				case 15:
					starti = 581;
					bytesl = 4;
					bitl = 28;
					break;
				case 16:
					starti = 585;
					bytesl = 4;
					bitl = 28;
					break;
				case 17:
					starti = 589;
					bytesl = 4;
					bitl = 28;
					break;
				case 18:
					starti = 593;
					bytesl = 4;
					bitl = 28;
					break;
				case 19:
					starti = 597;
					bytesl = 4;
					bitl = 28;
					break;
				case 20:
					starti = 601;
					bytesl = 4;
					bitl = 28;
					break;
				case 21:
					starti = 605;
					bytesl = 4;
					bitl = 28;
					break;
				case 23:
					starti = 609;
					bytesl = 4;
					bitl = 28;
					break;
				case 24:
					starti = 613;
					bytesl = 4;
					bitl = 28;
					break;
				case 25:
					starti = 617;
					bytesl = 4;
					bitl = 28;
					break;
				case 26:
					starti = 621;
					bytesl = 4;
					bitl = 28;
					break;
				case 27:
					starti = 625;
					bytesl = 4;
					bitl = 28;
					break;
				case 28:
					starti = 629;
					bytesl = 4;
					bitl = 28;
					break;
				case 29:
					starti = 633;
					bytesl = 4;
					bitl = 28;
					break;
				case 30:
					starti = 637;
					bytesl = 4;
					bitl = 28;
					break;
				case 31:
					starti = 641;
					bytesl = 4;
					bitl = 28;
					break;
				case 127:
					starti = 645;
					bytesl = 4;
					bitl = 28;
					break;
				case 220:
					starti = 649;
					bytesl = 4;
					bitl = 28;
					break;
				case 249:
					starti = 653;
					bytesl = 4;
					bitl = 28;
					break;
				case 10:
					starti = 657;
					bytesl = 4;
					bitl = 30;
					break;
				case 13:
					starti = 661;
					bytesl = 4;
					bitl = 30;
					break;
				case 22:
					starti = 665;
					bytesl = 4;
					bitl = 30;
					break;
			}

			#endregion

			//无未写入完的位
			if (lastOffset == 0)
			{
				//这次写入后无剩余位
				if (bitl % 8 == 0)
				{
					for (int k = 0; k < bytesl; k++)
					{
						if (wi == resultLength) throw new IndexOutOfRangeException();

						result[wi++] = AllBytes[starti++];
					}
				}
				//这次写入后有剩余位
				else
				{
					for (int k = 0; k < bytesl - 1; k++)
					{
						if (wi == resultLength) throw new IndexOutOfRangeException();

						result[wi++] = AllBytes[starti++];
					}

					last = AllBytes[starti];
					lastOffset = bitl % 8;
				}
			}
			//上一次写入有未写入完的位
			else
			{
				byte localLast = last;
				int alstlength1 = lastOffset + bitl;

				//这次写入后无剩余位
				if (alstlength1 % 8 == 0)
				{
					for (int i = 0; i < alstlength1 / 8; i++)
					{
						if (wi == resultLength) throw new IndexOutOfRangeException();

						byte atbyte = AllBytes[starti++];
						result[wi++] = (byte)(localLast | (atbyte >> lastOffset));
						localLast = (byte)(atbyte << (8 - lastOffset));
					}

					lastOffset = 0;
				}
				//这次写入后有剩余位
				else
				{
					//与上一次写入的剩余位合并后不足一个字节
					if (alstlength1 < 8)
					{
						last = (byte)(last | (AllBytes[starti] >> lastOffset));
						lastOffset = alstlength1;
					}
					//与上一次写入的剩余位合并后至少有一个字节且有剩余位     无剩余位会在`这次写入后无剩余位`中被匹配
					else
					{
						for (int i = 0; i < alstlength1 / 8; i++)
						{
							if (wi == resultLength) throw new IndexOutOfRangeException();

							byte atbyte = AllBytes[starti++];
							result[wi++] = (byte)(localLast | (atbyte >> lastOffset));
							localLast = (byte)(atbyte << (8 - lastOffset));
						}

						last = localLast;
						lastOffset = alstlength1 % 8;
					}
				}
			}


			// Span<byte> data1 = new Span<byte>(allbytes, starti, bytesl);
			// if (lastOffset == 0)
			// {
			// 	if (bitl % 8 == 0)
			// 	{
			// 		for (int j1 = 0; j1 < bytesl; j1++) tdata.Add(data1[j1]);
			// 	}
			// 	else
			// 	{
			// 		int j2 = 0;
			// 		for (; j2 < bytesl - 1; j2++) tdata.Add(data1[j2]);
			//
			// 		last = data1[j2];
			// 		lastOffset = (byte)(bitl % 8);
			// 	}
			// }
			// else
			// {
			// 	byte alll = (byte)(lastOffset + bitl);
			// 	byte alldl = (byte)(alll / 8);
			// 	if (alll % 8 == 0)
			// 	{
			// 		byte nlast = last;
			// 		for (int j3 = 0; j3 < alldl; j3++)
			// 		{
			// 			tdata.Add((byte)(nlast | (data1[j3] >> lastOffset)));
			// 			nlast = (byte)(data1[j3] << (8 - lastOffset));
			// 		}
			//
			// 		lastOffset = 0;
			// 	}
			// 	else
			// 	{
			// 		byte nlast = last;
			// 		if (alldl == 0)
			// 		{
			// 			last |= (byte)(data1[0] >> lastOffset);
			// 			lastOffset = (byte)(lastOffset + bitl);
			// 		}
			// 		else
			// 		{
			// 			int j4 = 0;
			// 			for (; j4 < alldl; j4++)
			// 			{
			// 				tdata.Add((byte)(nlast | (data1[j4] >> lastOffset)));
			// 				nlast = (byte)(data1[j4] << (8 - lastOffset));
			// 			}
			//
			// 			// if (bytesl > alldl)
			// 			// {
			// 			// 	nlast |= (byte)(data1[j4] >> lastOffset);
			// 			// }
			// 			
			// 			last = nlast;
			// 			lastOffset = (byte)(alll % 8);
			// 		}
			//
			// 	}
			// }
		}

		if (lastOffset != 0)
		{
			if (wi == resultLength) throw new IndexOutOfRangeException();

#pragma warning disable CS8509 // switch 表达式不处理其输入类型的所有可能的值(它不是穷举)。例如，模式“0”未包含在内。
			byte b = lastOffset switch
			{
				1 => (byte)(last | 0b_01111111),
				2 => (byte)(last | 0b_00111111),
				3 => (byte)(last | 0b_00011111),
				4 => (byte)(last | 0b_00001111),
				5 => (byte)(last | 0b_00000111),
				6 => (byte)(last | 0b_00000011),
				7 => (byte)(last | 0b_00000001)
			};
#pragma warning restore CS8509 // switch 表达式不处理其输入类型的所有可能的值(它不是穷举)。例如，模式“0”未包含在内。
			//tdata.Add(b);
			result[wi++] = b;
		}


		return wi;
	}
}