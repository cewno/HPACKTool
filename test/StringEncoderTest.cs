using Xunit;

namespace cewno.HPACKTool.Test;

public class StringEncodeTest
{
	// huffman false
	[Fact]
	public void Test1()
	{
		byte[] encoded = { 10, 99, 117, 115, 116, 111, 109, 45, 107, 101, 121 };
		string source = "custom-key";
		byte[]? encoderToHuffman = StringTool.EncoderToSource(source);
		Assert.Equal(encoded, encoderToHuffman);
	}
	// huffman false
	[Fact]
	public void Test2()
	{
		byte[] encoded = { 13, 99, 117, 115, 116, 111, 109, 45, 104, 101, 97, 100, 101, 114 };
		string source = "custom-header";
		byte[]? encoderToHuffman = StringTool.EncoderToSource(source);
		Assert.Equal(encoded, encoderToHuffman);
	}
	// huffman false
	[Fact]
	public void Test3()
	{
		byte[] encoded = { 12, 47, 115, 97, 109, 112, 108, 101, 47, 112, 97, 116, 104 };
		string source = "/sample/path";
		byte[]? encoderToHuffman = StringTool.EncoderToSource(source);
		Assert.Equal(encoded, encoderToHuffman);
	}
	// huffman true
	[Fact]
	public void Test4()
	{
		byte[] encoded = { 140, 241, 227, 194, 229, 242, 58, 107, 160, 171, 144, 244, 255 };
		string source = "www.example.com";
		byte[]? encoderToHuffman = StringTool.EncoderToHuffman(source);
		Assert.Equal(encoded, encoderToHuffman);
	}
	// huffman true
	[Fact]
	public void Test5()
	{
		byte[] encoded = { 134, 168, 235, 16, 100, 156, 191 };
		string source = "no-cache";
		byte[]? encoderToHuffman = StringTool.EncoderToHuffman(source);
		Assert.Equal(encoded, encoderToHuffman);
	}
}