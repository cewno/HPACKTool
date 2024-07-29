using Xunit;
using cewno.HPACKTool;

namespace HPACKTool.Test;

public class IntegerTest
{
	private static readonly byte[] Nb =
	{
		0b_0,        //index 0
		0b_00000001, //index 1
		0b_00000011, //index 2
		0b_00000111, //index 3
		0b_00001111, //index 4
		0b_00011111, //index 5
		0b_00111111, //index 6
		0b_01111111, //index 7
		0b_11111111  //index 8
	};
	
	[Fact]
	public void test()
	{
		ushort cs = 40000;
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger((uint)0,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadUInt(i,(IAsyncIO)testReadIo) != 0)
			{
				throw new Exception();
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			for (ushort j = 0; j < cs; j++)
			{
				uint next = (uint)new Random().Next(int.MinValue + 1, int.MaxValue - 1);
				byte[] buffer = IntegerTool.WriteUInteger(next,i,0);
				TestReadIO testReadIo = new TestReadIO(buffer);
				
				if (IntegerTool.ReadUInt(i,(IAsyncIO)testReadIo) != next)
				{
					throw new Exception();
				}
			}
		}
		
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger((ulong)0,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadULong(i,(IAsyncIO)testReadIo) != 0)
			{
				throw new Exception();
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			for (ushort j = 0; j < cs; j++)
			{
				ulong next = (ulong)new Random().NextInt64(long.MinValue + 1, long.MaxValue - 1);
				byte[] buffer = IntegerTool.WriteUInteger(next,i,0);
				TestReadIO testReadIo = new TestReadIO(buffer);
				
				if (IntegerTool.ReadULong(i,(IAsyncIO)testReadIo) != next)
				{
					throw new Exception();
				}
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger((ushort)0,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadUShort(i,(IAsyncIO)testReadIo) != 0)
			{
				throw new Exception();
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			for (ushort j = 0; j < cs; j++)
			{
				ushort next = (ushort)new Random().Next(1, ushort.MaxValue - 1);
				byte[] buffer = IntegerTool.WriteUInteger(next,i,0);
				TestReadIO testReadIo = new TestReadIO(buffer);
				
				if (IntegerTool.ReadUShort(i,(IAsyncIO)testReadIo) != next)
				{
					throw new Exception();
				}
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger((byte)0,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadByte(i,(IAsyncIO)testReadIo) != 0)
			{
				throw new Exception();
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			for (byte j = 0; j < byte.MaxValue; j++)
			{
				byte[] buffer = IntegerTool.WriteUInteger(j,i,0);
				TestReadIO testReadIo = new TestReadIO(buffer);
				
				if (IntegerTool.ReadByte(i,(IAsyncIO)testReadIo) != j)
				{
					throw new Exception();
				}
			}
		}
#if NET7_0_OR_GREATER
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger((UInt128)0,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadUInt128(i,(IAsyncIO)testReadIo) != 0)
			{
				throw new Exception();
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			for (ushort j = 0; j < cs; j++)
			{
				UInt128 next = new UInt128((ulong)new Random().NextInt64(long.MinValue, long.MaxValue),
											(ulong)new Random().NextInt64(long.MinValue + 1, long.MaxValue - 1));
				byte[] buffer = IntegerTool.WriteUInteger(next,i,0);
				TestReadIO testReadIo = new TestReadIO(buffer);
				
				if (IntegerTool.ReadUInt128(i,(IAsyncIO)testReadIo) != next)
				{
					throw new Exception();
				}
			}
		}	
#endif
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(uint.MaxValue,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadUInt(i,(IAsyncIO)testReadIo) != uint.MaxValue)
			{
				throw new Exception();
			}
		}
		
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(uint.MaxValue,i,0);
			buffer[buffer.Length - 1] += 1;
			TestReadIO testReadIo = new TestReadIO(buffer);

			try
			{
				if (IntegerTool.ReadUInt(i,(IAsyncIO)testReadIo) != uint.MaxValue)
				{
					throw new Exception();
				}
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok:
			;

		}
		
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(ulong.MaxValue,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadULong(i,(IAsyncIO)testReadIo) != ulong.MaxValue)
			{
				throw new Exception();
			}
		}
		
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(ulong.MaxValue,i,0);
			buffer[buffer.Length - 1] += 1;
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			try
			{
				if (IntegerTool.ReadULong(i, (IAsyncIO)testReadIo) != ulong.MaxValue)
				{
					throw new Exception();
				}
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok:
			;
			
		}
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(byte.MaxValue,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadByte(i,(IAsyncIO)testReadIo) != byte.MaxValue)
			{
				throw new Exception();
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(byte.MaxValue,i,0);
			buffer[buffer.Length - 1] += 1;
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			try
			{
				if (IntegerTool.ReadByte(i, (IAsyncIO)testReadIo) != byte.MaxValue)
				{
					throw new Exception();
				}
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok:
			;
		}
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(ushort.MaxValue,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadUShort(i,(IAsyncIO)testReadIo) != ushort.MaxValue)
			{
				throw new Exception();
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(ushort.MaxValue,i,0);
			buffer[buffer.Length - 1] += 1;
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			try
			{
				if (IntegerTool.ReadUShort(i, (IAsyncIO)testReadIo) != ushort.MaxValue)
				{
					throw new Exception();
				}
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok:
			;
		}
#if NET7_0_OR_GREATER
		

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(UInt128.MaxValue,i,0);
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			if (IntegerTool.ReadUInt128(i,(IAsyncIO)testReadIo) != UInt128.MaxValue)
			{
				throw new Exception();
			}
		}
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(UInt128.MaxValue,i,0);
			buffer[buffer.Length - 1] += 1;
			TestReadIO testReadIo = new TestReadIO(buffer);
				
			try
			{
				if (IntegerTool.ReadUInt128(i, (IAsyncIO)testReadIo) != UInt128.MaxValue)
				{
					throw new Exception();
				}
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok:
			;
		}
#endif
	}
}

internal class TestReadIO : Stream, IAsyncIO
{
	public override void Flush()
	{
		throw new NotImplementedException();
	}

	private byte[] buffer;
	internal TestReadIO(byte[] buffer)
	{
		this.buffer = buffer;
	}

	private int i;
	public byte ReadOneByte()
	{
		return buffer[i++];
	}

	public byte ReadOneByteNoNext()
	{
		return buffer[i];
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotImplementedException();
	}

	public void ReadOnlyLength(byte[] buffer, int offset, int length)
	{
		throw new NotImplementedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotImplementedException();
	}

	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotImplementedException();
	}

	public override bool CanRead { get; }
	public override bool CanSeek { get; }
	public override bool CanWrite { get; }
	public override long Length { get; }
	public override long Position { get; set; }
}
