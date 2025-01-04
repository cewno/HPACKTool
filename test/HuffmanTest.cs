using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace cewno.HPACKTool.Test;

public class HuffmanTest
{
	private readonly ITestOutputHelper OutputHelper;

	public HuffmanTest(ITestOutputHelper testOutputHelper)
	{
		OutputHelper = testOutputHelper;
	}
	
	[Fact]
	public void RandomTest()
	{
		int maxLength = 2000;
		int count = 2000;
		byte[] source = new byte[maxLength];
		byte[] encodeOut = new byte[(int)(maxLength * 3.75) + 1];
		byte[] decodeOut = new byte[maxLength];
		Random random = new Random();
		OutputHelper.WriteLine($"随机测试次数：{count} 长度：随机 1 ~ {maxLength}");
		for (int i = 0; i < count; i++)
		{
			int length = random.Next(maxLength) + 1;
			Memory<byte> localSource = new Memory<byte>(source, 0, length);
			
			random.NextBytes(localSource.Span);
			int encodeOutLength = HuffmanTool.Encoder(localSource.Span, encodeOut);
			int decodeOutLength = HuffmanTool.Decoder( new Span<byte>(encodeOut, 0, encodeOutLength), decodeOut);
			//Assert.Equal(source.Length, decodeOutLength);
			try
			{
				Assert.Equal(localSource, new Memory<byte>(decodeOut, 0, decodeOutLength));
			}
			catch (EqualException)
			{
				string r1 = "[";
				for (int j = 0; j < source.Length; j++)
				{
					r1 += source[j] + ", ";
				}

				r1 += ']';
				
				string r2 = "[";
				for (int j = 0; j < encodeOutLength; j++)
				{
					r2 += encodeOut[j] + ", ";
				}
				r2 += ']';
				
				string r3 = "[";
				for (int j = 0; j < decodeOutLength; j++)
				{
					r3 += decodeOut[j] + ", ";
				}
				r3 += ']';
				OutputHelper.WriteLine($"{r1}\n\n {r2}\n\n {r3}");
				Thread.Sleep(1000);
				throw;
			}

		}
	}
}