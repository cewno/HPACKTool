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
	/// 将字符串使用 Huffman 编码到缓冲区
	/// </summary>
	/// <param name="data">字符串</param>
	/// <returns>缓冲区</returns>
	public static byte[]? EncoderToHuffman(string data)
	{
		return EncoderToHuffman(System.Text.Encoding.ASCII.GetBytes(data));
	}

	/// <summary>
	/// 将二进制数据使用 Huffman 编码到缓冲区
	/// </summary>
	/// <param name="data"></param>
	/// <returns>缓冲区</returns>
	public static byte[]? EncoderToHuffman(byte[] data)
	{
		data = HuffmanTool.Encoder(data);
		if (data == null)
		{
			return null;
		}
		byte[] lengthdata = IntegerTool.WriteUInteger((uint)data.Length, 7, 0b_10000000);
		byte[] alldata = new byte[data.Length + lengthdata.Length];
		Array.Copy(lengthdata, 0, alldata, 0, lengthdata.Length);
		Array.Copy(data,0,alldata,lengthdata.Length,data.Length);
		return alldata;
	}



	
	
    
}