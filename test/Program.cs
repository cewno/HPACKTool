using HPACKTool;

namespace test;

class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("Hello, World!");
		byte[] test1data = {0b_10101110, 0b_11000011, 0b_01110111, 0b_00011010, 0b_01001011 };
		string test1s = "private";
		byte[] test2data =
		{
			0b_11010000, 0b_01111010, 0b_10111110, 0b_10010100, 0b_00010000, 0b_01010100, 0b_11010100, 0b_01000100,
			0b_10101000, 0b_00100000, 0b_00000101, 0b_10010101, 0b_00000100, 0b_00001011, 0b_10000001, 0b_01100110,
			0b_11100000, 0b_10000010, 0b_10100110, 0b_00101101, 0b_00011011, 0b_11111111
		};
		string test2s = "Mon, 21 Oct 2013 20:13:21 GMT";
		byte[] test3data = { 0b11111100, 0b11111101 };
		string test3s = "XZ";
		byte[] test4data = { 0b11111101,0b11111100 };
		string test4s = "ZX";
		long start;
		long end;
		char[]? decoder;
		decoder = HuffmanTool.Decoder(test1data);
		string outd;
		//test1
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		decoder = HuffmanTool.Decoder(test1data);
		outd = new string(decoder);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Console.WriteLine("1:" + (end - start) + "ms " + (outd.Equals(test1s) ? "ok" : "error"));
		//test2
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		decoder = HuffmanTool.Decoder(test2data);
		outd = new string(decoder);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Console.WriteLine("2:" + (end - start) + "ms " + (outd.Equals(test2s) ? "ok" : "error"));
		//test3
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		decoder = HuffmanTool.Decoder(test3data);
		outd = new string(decoder);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Console.WriteLine("3:" + (end - start) + "ms " + (outd.Equals(test3s) ? "ok" : "error"));
		//test4
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		decoder = HuffmanTool.Decoder(test4data);
		outd = new string(decoder);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Console.WriteLine("4:" + (end - start) + "ms " + (outd.Equals(test4s) ? "ok" : "error"));
	}
}