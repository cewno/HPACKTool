using System.Buffers;

namespace cewno.HPACKTool;

public static partial class StringTool
{
	/// <summary>
	/// 将字符串写入到缓冲区
	/// </summary>
	/// <param name="data">字符串</param>
	/// <returns>缓冲区</returns>
	public static byte[]? EncoderToSource(string data)
	{
		return EncoderToSource(System.Text.Encoding.ASCII.GetBytes(data));
	}
	/// <summary>
	/// 将二进制数据写入到缓冲区
	/// </summary>
	/// <param name="data">二进制数据</param>
	/// <returns>缓冲区</returns>
	public static byte[]? EncoderToSource(byte[] data)
	{
		byte[] lengthdata = IntegerTool.WriteUInteger((uint)data.Length, 7, 0b_00000000);
		byte[] alldata = new byte[data.Length + lengthdata.Length];
		Array.Copy(lengthdata, 0, alldata, 0, lengthdata.Length);
		Array.Copy(data,0,alldata,lengthdata.Length,data.Length);
		return alldata;
	}
	
	
	
	//Huffman
	
	
	/// <summary>
	/// 将<see cref="string"/>使用 Huffman 压缩后编码写入缓冲区
	/// </summary>
	/// <param name="data">字符串</param>
	/// <returns>缓冲区</returns>
	public static byte[]? EncoderToHuffman(string data)
	{
		return EncoderToHuffman(System.Text.Encoding.ASCII.GetBytes(data));
	}

	/// <summary>
	/// 将二进制数据使用 Huffman 压缩后编码写入缓冲区
	/// </summary>
	/// <param name="data"></param>
	/// <returns>缓冲区</returns>
	public static byte[]? EncoderToHuffman(byte[] data)
	{
		byte[] bytes = ArrayPool<byte>.Shared.Rent((int)(data.Length * 3.75));
		int encoder = HuffmanTool.Encoder(data, bytes);
		if (encoder <= 0)
		{
			return null;
		}
		byte[] lengthdata = IntegerTool.WriteUInteger((uint)encoder, 7, 0b_10000000);
		byte[] alldata = new byte[encoder + lengthdata.Length];
		Array.Copy(lengthdata, 0, alldata, 0, lengthdata.Length);
		Array.Copy(bytes,0,alldata,lengthdata.Length,encoder);
		return alldata;
	}



	
	
    
}