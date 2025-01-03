using Xunit;
using Xunit.Abstractions;

namespace cewno.HPACKTool.Test;

public class HuffmanEncoderTest
{
	private readonly ITestOutputHelper OutputHelper;

	public HuffmanEncoderTest(ITestOutputHelper testOutputHelper)
	{
		OutputHelper = testOutputHelper;
	}


	[Fact]
	public void Test1()
	{
		byte[] data = { 0b_10101110, 0b_11000011, 0b_01110111, 0b_00011010, 0b_01001011 };
		string str = "private";
		long start;
		byte[]? outd;
		long end;
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		outd = HuffmanTool.Encoder(str);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Assert.Equal(data, outd);
		OutputHelper.WriteLine((end - start) + "ms " + "ok");
	}
	[Fact]
	public void Test2()
	{
		byte[] data =
		{
			0b_11010000, 0b_01111010, 0b_10111110, 0b_10010100, 0b_00010000, 0b_01010100, 0b_11010100, 0b_01000100,
			0b_10101000, 0b_00100000, 0b_00000101, 0b_10010101, 0b_00000100, 0b_00001011, 0b_10000001, 0b_01100110,
			0b_11100000, 0b_10000010, 0b_10100110, 0b_00101101, 0b_00011011, 0b_11111111
		};
		string str = "Mon, 21 Oct 2013 20:13:21 GMT";
		long start;
		byte[]? outd;
		long end;
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		outd = HuffmanTool.Encoder(str);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Assert.Equal(data, outd);
		OutputHelper.WriteLine((end - start) + "ms " + "ok");
	}
	[Fact]
	public void Test3()
	{
		byte[] data = { 0b11111100, 0b11111101 };
		string str = "XZ";
		long start;
		byte[]? outd;
		long end;
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		outd = HuffmanTool.Encoder(str);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Assert.Equal(data, outd);
		OutputHelper.WriteLine((end - start) + "ms " + "ok");
	}
	[Fact]
	public void Test4()
	{
		byte[] data = { 0b11111101, 0b11111100 };
		string str = "ZX";
		long start;
		byte[]? outd;
		long end;
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		outd = HuffmanTool.Encoder(str);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Assert.Equal(data, outd);
		OutputHelper.WriteLine((end - start) + "ms " + "ok");
	}
	[Fact]
	public void Test5()
	{
		byte[] data =
		{
			0b_10010100, 0b_11100111, 0b_10000010, 0b_00011101, 0b_11010111, 0b_11110010, 0b_11100110, 0b_11000111,
			0b_10110011, 0b_00110101, 0b_11011111, 0b_11011111, 0b_11001101, 0b_01011011, 0b_00111001, 0b_01100000,
			0b_11010101, 0b_10101111, 0b_00100111, 0b_00001000, 0b_01111111, 0b_00110110, 0b_01110010, 0b_11000001,
			0b_10101011, 0b_00100111, 0b_00001111, 0b_10110101, 0b_00101001, 0b_00011111, 0b_10010101, 0b_10000111,
			0b_00110001, 0b_01100000, 0b_01100101, 0b_11000000, 0b_00000011, 0b_11101101, 0b_01001110, 0b_11100101,
			0b_10110001, 0b_00000110, 0b_00111101, 0b_01010000, 0b_00000111

		};
		string str = "foo=ASDJKHQKBZXOQWEOPIUAXQWEOIU; max-age=3600; version=1";

		long start;
		byte[]? outd;
		long end;
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		outd = HuffmanTool.Encoder(str);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Assert.Equal(data, outd);
		OutputHelper.WriteLine((end - start) + "ms " + "ok");
	}
}