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
	/// <summary>
	/// 将字符串写入到异步流
	/// </summary>
	/// <param name="data">字符串</param>
	/// <param name="asyncIo">异步流</param>
	public static void EncoderToSource(string data, IAsyncIO asyncIo)
	{
		byte[] bytes = System.Text.Encoding.ASCII.GetBytes(data);
		IntegerTool.WriteUInteger((uint)bytes.Length, 7, 0b_00000000, asyncIo);
		asyncIo.Write(bytes,0,data.Length);
	}
	/// <summary>
	/// 将二进制数据写入到异步流
	/// </summary>
	/// <param name="data">二进制数据</param>
	/// <param name="asyncIo">异步流</param>
	public static void EncoderToSource(byte[] data,IAsyncIO asyncIo)
	{
		IntegerTool.WriteUInteger((uint)data.Length, 7, 0b_00000000, asyncIo);
		asyncIo.Write(data,0,data.Length);
	}
	/// <summary>
	/// 将字符串写入到普通流
	/// </summary>
	/// <param name="data">字符串</param>
	/// <param name="stream">普通流</param>
	public static void EncoderToSource(string data, Stream stream)
	{
		byte[] bytes = System.Text.Encoding.ASCII.GetBytes(data);
		IntegerTool.WriteUInteger((uint)bytes.Length, 7, 0b_00000000, stream);
		stream.Write(bytes,0,data.Length);
	}
	/// <summary>
	/// 将二进制数据写入到普通流
	/// </summary>
	/// <param name="data">二进制数据</param>
	/// <param name="stream">普通流</param>
	public static void EncoderToSource(byte[] data,Stream stream)
	{
		IntegerTool.WriteUInteger((uint)data.Length, 7, 0b_00000000, stream);
		stream.Write(data,0,data.Length);
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
	/// <summary>
	/// 将字符串使用 Huffman 编码到异步流
	/// </summary>
	/// <param name="data">字符串</param>
	/// <param name="asyncIo">异步流</param>
	public static void EncoderToHuffman(string data, IAsyncIO asyncIo)
	{
		byte[] bytes = HuffmanTool.Encoder(data);
		if (bytes == null)
		{
			return;
		}
		IntegerTool.WriteUInteger((uint)bytes.Length, 7, 0b_10000000, asyncIo);
		asyncIo.Write(bytes,0,data.Length);
	}

	/// <summary>
	/// 将二进制数据使用 Huffman 编码到异步流
	/// </summary>
	/// <param name="data"></param>
	/// <param name="asyncIo">异步流</param>
	public static void EncoderToHuffman(byte[] data,IAsyncIO asyncIo)
	{
		data = HuffmanTool.Encoder(data);
		if (data == null)
		{
			return;
		}
		IntegerTool.WriteUInteger((uint)data.Length, 7, 0b_10000000, asyncIo);
		asyncIo.Write(data,0,data.Length);
	}
	
	
	/// <summary>
	/// 将字符串使用 Huffman 编码到普通流
	/// </summary>
	/// <param name="data">字符串</param>
	/// <param name="stream">普通流</param>
	public static void EncoderToHuffman(string data, Stream stream)
	{
		byte[] bytes = HuffmanTool.Encoder(data);
		if (bytes == null)
		{
			return;
		}
		IntegerTool.WriteUInteger((uint)bytes.Length, 7, 0b_10000000, stream);
		stream.Write(bytes,0,data.Length);
	}

	/// <summary>
	/// 将二进制数据使用 Huffman 编码到普通流
	/// </summary>
	/// <param name="data"></param>
	/// <param name="stream">普通流</param>
	public static void EncoderToHuffman(byte[] data,Stream stream)
	{
		data = HuffmanTool.Encoder(data);
		if (data == null)
		{
			return;
		}
		IntegerTool.WriteUInteger((uint)data.Length, 7, 0b_10000000, stream);
		stream.Write(data,0,data.Length);
	}
    
}