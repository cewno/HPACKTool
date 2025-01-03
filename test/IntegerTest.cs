using Xunit;

namespace cewno.HPACKTool.Test;

public class IntegerTest
{
	private static readonly byte[] Nb =
	{
		0b_0, //index 0
		0b_00000001, //index 1
		0b_00000011, //index 2
		0b_00000111, //index 3
		0b_00001111, //index 4
		0b_00011111, //index 5
		0b_00111111, //index 6
		0b_01111111, //index 7
		0b_11111111 //index 8
	};

	[Fact]
	public void test()
	{
		ushort cs = 40000;
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger((uint)0, i, 0);

			if (IntegerTool.ReadUInt(i, buffer) != 0) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		for (ushort j = 0; j < cs; j++)
		{
			uint next = (uint)new Random().Next(int.MinValue + 1, int.MaxValue - 1);
			byte[] buffer = IntegerTool.WriteUInteger(next, i, 0);


			if (IntegerTool.ReadUInt(i, buffer) != next) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger((ulong)0, i, 0);


			if (IntegerTool.ReadULong(i, buffer) != 0) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		for (ushort j = 0; j < cs; j++)
		{
			ulong next = 5911071119582494064; //(ulong)new Random().NextInt64(long.MinValue, long.MaxValue);
			byte[] buffer = IntegerTool.WriteUInteger(next, i, 0);


			if (IntegerTool.ReadULong(i, buffer) != next) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger((ushort)0, i, 0);


			if (IntegerTool.ReadUShort(i, buffer) != 0) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		for (ushort j = 0; j < cs; j++)
		{
			ushort next = (ushort)new Random().Next(1, ushort.MaxValue - 1);
			byte[] buffer = IntegerTool.WriteUInteger(next, i, 0);


			if (IntegerTool.ReadUShort(i, buffer) != next) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(0, i, 0);


			if (IntegerTool.ReadByte(i, buffer) != 0) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		for (byte j = 0; j < byte.MaxValue; j++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(j, i, 0);


			if (IntegerTool.ReadByte(i, buffer) != j) throw new Exception();
		}
#if NET7_0_OR_GREATER
		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger((UInt128)0, i, 0);


			if (IntegerTool.ReadUInt128(i, buffer) != 0) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		for (ushort j = 0; j < cs; j++)
		{
			UInt128 next = new UInt128((ulong)new Random().NextInt64(long.MinValue, long.MaxValue),
				(ulong)new Random().NextInt64(long.MinValue + 1, long.MaxValue - 1));
			byte[] buffer = IntegerTool.WriteUInteger(next, i, 0);


			if (IntegerTool.ReadUInt128(i, buffer) != next) throw new Exception();
		}
#endif


		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(uint.MaxValue, i, 0);


			if (IntegerTool.ReadUInt(i, buffer) != uint.MaxValue) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(uint.MaxValue, i, 0);
			buffer[buffer.Length - 1] += 1;


			try
			{
				if (IntegerTool.ReadUInt(i, buffer) != uint.MaxValue) throw new Exception();
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok: ;
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(ulong.MaxValue, i, 0);


			if (IntegerTool.ReadULong(i, buffer) != ulong.MaxValue) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(ulong.MaxValue, i, 0);
			buffer[buffer.Length - 1] += 1;


			try
			{
				if (IntegerTool.ReadULong(i, buffer) != ulong.MaxValue) throw new Exception();
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok: ;
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(byte.MaxValue, i, 0);


			if (IntegerTool.ReadByte(i, buffer) != byte.MaxValue) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(byte.MaxValue, i, 0);
			buffer[buffer.Length - 1] += 1;


			try
			{
				if (IntegerTool.ReadByte(i, buffer) != byte.MaxValue) throw new Exception();
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok: ;
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(ushort.MaxValue, i, 0);


			if (IntegerTool.ReadUShort(i, buffer) != ushort.MaxValue) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(ushort.MaxValue, i, 0);
			buffer[buffer.Length - 1] += 1;


			try
			{
				if (IntegerTool.ReadUShort(i, buffer) != ushort.MaxValue) throw new Exception();
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok: ;
		}
#if NET7_0_OR_GREATER


		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(UInt128.MaxValue, i, 0);


			if (IntegerTool.ReadUInt128(i, buffer) != UInt128.MaxValue) throw new Exception();
		}

		for (byte i = 1; i <= 8; i++)
		{
			byte[] buffer = IntegerTool.WriteUInteger(UInt128.MaxValue, i, 0);
			buffer[buffer.Length - 1] += 1;


			try
			{
				if (IntegerTool.ReadUInt128(i, buffer) != UInt128.MaxValue) throw new Exception();
			}
			catch (OverflowException)
			{
				goto ok;
			}

			throw new Exception();
			ok: ;
		}
#endif
	}
}