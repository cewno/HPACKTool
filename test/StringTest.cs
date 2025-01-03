using Xunit;

namespace cewno.HPACKTool.Test;

public class StringTest
{
	[Fact]
	public void stringtestDecode()
	{
		byte[] d1 = { 10, 99, 117, 115, 116, 111, 109, 45, 107, 101, 121 };
		string s1 = "custom-key";

		byte[] d2 = { 13, 99, 117, 115, 116, 111, 109, 45, 104, 101, 97, 100, 101, 114 };
		string s2 = "custom-header";

		byte[] d3 = { 12, 47, 115, 97, 109, 112, 108, 101, 47, 112, 97, 116, 104 };
		string s3 = "/sample/path";

		//Huffman
		byte[] d4 = { 140, 241, 227, 194, 229, 242, 58, 107, 160, 171, 144, 244, 255 };
		string s4 = "www.example.com";
		//Huffman
		byte[] d5 = { 134, 168, 235, 16, 100, 156, 191 };
		string s5 = "no-cache";
		if ((StringTool.Decode(d1, 100000) != s1) |
		    (StringTool.Decode(d2, 100000) != s2) |
		    (StringTool.Decode(d3, 100000) != s3) |
		    (StringTool.Decode(d4, 100000) != s4) |
		    (StringTool.Decode(d5, 100000) != s5))
			throw new Exception("failed test");

		if (!VerifyArray(StringTool.EncoderToSource(s1), d1) |
		    !VerifyArray(StringTool.EncoderToSource(s2), d2) |
		    !VerifyArray(StringTool.EncoderToSource(s3), d3) |
		    !VerifyArray(StringTool.EncoderToHuffman(s4), d4) |
		    !VerifyArray(StringTool.EncoderToHuffman(s5), d5))
			throw new Exception("failed test");
	}


	private static bool VerifyArray(byte[]? data1, byte[]? data2)
	{
		Assert.Equal(data1, data2);

		return true;
	}
}