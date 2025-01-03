#define maxint
using System.Text;

namespace cewno.HPACKTool;

/// <summary>
///     用于解码string的工具类
/// </summary>
public static partial class StringTool
{
	/// <summary>
	///     从缓冲区解码string
	/// </summary>
	/// <param name="buffer">缓冲区</param>
	/// <param name="Maxl">最大长度</param>
	/// <param name="offset">缓冲区偏移量</param>
	/// <returns>解码后的<see cref="string" /></returns>
	/// <exception cref="StringDecodingException">字符串超长了</exception>
	public static string? Decode(byte[] buffer, int Maxl, int offset = 0)
	{
		// H = 1 (Huffman enable)
		uint l = IntegerTool.ReadUInt(7, buffer, out int rl, offset);
		if (l > (uint)Maxl) throw new StringDecodingException();


		return (buffer[offset] & 0b_10000000) == 0b_10000000 ? HuffmanTool.DecoderToString(buffer, rl, (int)l) : Encoding.ASCII.GetString(buffer, rl, (int)l);
	}
}