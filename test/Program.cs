using HPACKTool;

namespace test;

internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Hello, World!");
		byte[] test1data = { 0b_10101110, 0b_11000011, 0b_01110111, 0b_00011010, 0b_01001011 };
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
		byte[] test4data = { 0b11111101, 0b11111100 };
		string test4s = "ZX";
		byte[] test5data =
		{
			0b_10010100, 0b_11100111, 0b_10000010, 0b_00011101, 0b_11010111, 0b_11110010, 0b_11100110, 0b_11000111,
			0b_10110011, 0b_00110101, 0b_11011111, 0b_11011111, 0b_11001101, 0b_01011011, 0b_00111001, 0b_01100000,
			0b_11010101, 0b_10101111, 0b_00100111, 0b_00001000, 0b_01111111, 0b_00110110, 0b_01110010, 0b_11000001,
			0b_10101011, 0b_00100111, 0b_00001111, 0b_10110101, 0b_00101001, 0b_00011111, 0b_10010101, 0b_10000111,
			0b_00110001, 0b_01100000, 0b_01100101, 0b_11000000, 0b_00000011, 0b_11101101, 0b_01001110, 0b_11100101,
			0b_10110001, 0b_00000110, 0b_00111101, 0b_01010000, 0b_00000111

		};
		string test5s = "foo=ASDJKHQKBZXOQWEOPIUAXQWEOIU; max-age=3600; version=1";
		long start;
		long end;
		byte[]? decoder;
		decoder = HuffmanTool.Decoder(test1data);
		string outd;
		//test1.1
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		decoder = HuffmanTool.Decoder(test1data);
		outd = System.Text.Encoding.ASCII.GetString(decoder);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Console.WriteLine("1.1:" + (end - start) + "ms " + (outd.Equals(test1s) ? "ok" : "error"));
		//test1.2
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		decoder = HuffmanTool.Decoder(test2data);
		outd = System.Text.Encoding.ASCII.GetString(decoder);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Console.WriteLine("1.2:" + (end - start) + "ms " + (outd.Equals(test2s) ? "ok" : "error"));
		//test1.3
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		decoder = HuffmanTool.Decoder(test3data);
		outd = System.Text.Encoding.ASCII.GetString(decoder);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Console.WriteLine("1.3:" + (end - start) + "ms " + (outd.Equals(test3s) ? "ok" : "error"));
		//test1.4
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		decoder = HuffmanTool.Decoder(test4data);
		outd = System.Text.Encoding.ASCII.GetString(decoder);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Console.WriteLine("1.4:" + (end - start) + "ms " + (outd.Equals(test4s) ? "ok" : "error"));
		//test1.5
		start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		decoder = HuffmanTool.Decoder(test5data);
		outd = System.Text.Encoding.ASCII.GetString(decoder);
		end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		Console.WriteLine("1.5:" + (end - start) + "ms " + (outd.Equals(test5s) ? "ok" : "error"));
	
		

		{
			ushort eandd = 40000;
			int lastl = 0;
			long mine = 0;
			long maxe = 0;
			long mind = 0;
			long maxd = 0;
			long alle = 0;
			long alld = 0;
			long nl;
			for (ushort i = 0; i < eandd; i++)
			{
				for (int j = 0; j < lastl; j++)
				{
					Console.Write('\r');
				}
				lastl = i.ToString().Length;
				Console.Write(i);
				
				int l = new Random().Next(1,1000);
				byte[] buff = new byte[l];
				new Random().NextBytes(buff);
				start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				byte[]? encoder = HuffmanTool.Encoder(buff);
				end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				nl = end - start;
				mine = Math.Min(mine, nl);
				maxe = Math.Max(maxe, nl);
				alle += nl;
				
				
				start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				byte[]? bytes = HuffmanTool.Decoder(encoder);
				end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				nl = end - start;
				mind = Math.Min(mind, nl);
				maxd = Math.Max(maxd, nl);
				alld += nl;
				if (buff.Length != bytes.Length)
				{
					Console.WriteLine("error in test5");
					return;
				}

				for (int j = 0; j < buff.Length; j++)
				{
					if (buff[j] != bytes[j])
					{
						Console.WriteLine("error in test5");
						return;
					}
				}
			}
			for (int j = 0; j < lastl; j++)
			{
				Console.Write('\r');
			}
			Console.WriteLine($"测试2完成 共测试{eandd}组 长度：随机1~1000 编码共用时{alle}ms 平均用时{alle / (double)eandd}ms 最快{mine}ms 最慢{maxe}ms    解码共用时{alld}ms 平均用时{alld / (double)eandd}ms 最快{mind}ms 最慢{maxd}ms");
			
		}
	}
}