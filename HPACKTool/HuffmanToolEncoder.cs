namespace HPACKTool;

public static partial class HuffmanTool
{
	public static byte[]? Encoder(string data)
	{
		return Encoder(System.Text.Encoding.UTF8.GetBytes(data));
	}

	public static byte[]? Encoder(byte[] data)
	{
		if (data.Length == 0) return null;
		List<byte> tdata = new List<byte>();
		byte i = 0;
		byte last = 0;

		//ASCll
		void write(byte bitl, byte datal, params byte[] data)
		{
			if (i == 0)
			{
				if (bitl % 8 == 0)
				{
					for (int j = 0; j < datal; j++) tdata.Add(data[j]);
				}
				else
				{
					int j = 0;
					for (; j < datal - 1; j++) tdata.Add(data[j]);

					last = data[j];
					i = (byte)(bitl % 8);
				}
			}
			else
			{
				byte alll = (byte)(i + bitl);
				byte alldl = (byte)(alll / 8);
				if (alll % 8 == 0)
				{
					byte nlast = last;
					for (int j = 0; j < alldl; j++)
					{
						tdata.Add((byte)(nlast | (data[j] >> i)));
						nlast = (byte)(data[j] << (8 - i));
					}

					i = 0;
				}
				else
				{
					byte nlast = last;
					if (alldl == 0)
					{
						last |= (byte)(data[0] >> i);
						i = (byte)(i + bitl);
						return;
					}
					else
					{
						int j = 0;
						for (; j < alldl; j++)
						{
							tdata.Add((byte)(nlast | (data[j] >> i)));
							nlast = (byte)(data[j] << (8 - i));
						}

						if (datal > alldl)
						{
							nlast |= (byte)(data[j] >> i);
						}
					}

					last = nlast;
					i = (byte)(alll % 8);
				}
			}
		}

		void down()
		{
			switch (i)
			{
				case 1:
					tdata.Add((byte)(last | 0b_01111111));
					break;
				case 2:
					tdata.Add((byte)(last | 0b_00111111));
					break;
				case 3:
					tdata.Add((byte)(last | 0b_00011111));
					break;
				case 4:
					tdata.Add((byte)(last | 0b_00001111));
					break;
				case 5:
					tdata.Add((byte)(last | 0b_00000111));
					break;
				case 6:
					tdata.Add((byte)(last | 0b_00000011));
					break;
				case 7:
					tdata.Add((byte)(last | 0b_00000001));
					break;
			}
		}

		foreach (byte c in data)
			switch (c)
			{
				case (byte)'0':
					write(5, 1, 0b_00000000);
					break;
				case (byte)'1':
					write(5, 1, 0b_00001000);
					break;
				case (byte)'2':
					write(5, 1, 0b_00010000);
					break;
				case (byte)'a':
					write(5, 1, 0b_00011000);
					break;
				case (byte)'c':
					write(5, 1, 0b_00100000);
					break;
				case (byte)'e':
					write(5, 1, 0b_00101000);
					break;
				case (byte)'i':
					write(5, 1, 0b_00110000);
					break;
				case (byte)'o':
					write(5, 1, 0b_00111000);
					break;
				case (byte)'s':
					write(5, 1, 0b_01000000);
					break;
				case (byte)'t':
					write(5, 1, 0b_01001000);
					break;
				case (byte)' ':
					write(6, 1, 0b_01010000);
					break;
				case (byte)'%':
					write(6, 1, 0b_01010100);
					break;
				case (byte)'-':
					write(6, 1, 0b_01011000);
					break;
				case (byte)'.':
					write(6, 1, 0b_01011100);
					break;
				case (byte)'/':
					write(6, 1, 0b_01100000);
					break;
				case (byte)'3':
					write(6, 1, 0b_01100100);
					break;
				case (byte)'4':
					write(6, 1, 0b_01101000);
					break;
				case (byte)'5':
					write(6, 1, 0b_01101100);
					break;
				case (byte)'6':
					write(6, 1, 0b_01110000);
					break;
				case (byte)'7':
					write(6, 1, 0b_01110100);
					break;
				case (byte)'8':
					write(6, 1, 0b_01111000);
					break;
				case (byte)'9':
					write(6, 1, 0b_01111100);
					break;
				case (byte)'=':
					write(6, 1, 0b_10000000);
					break;
				case (byte)'A':
					write(6, 1, 0b_10000100);
					break;
				case (byte)'_':
					write(6, 1, 0b_10001000);
					break;
				case (byte)'b':
					write(6, 1, 0b_10001100);
					break;
				case (byte)'d':
					write(6, 1, 0b_10010000);
					break;
				case (byte)'f':
					write(6, 1, 0b_10010100);
					break;
				case (byte)'g':
					write(6, 1, 0b_10011000);
					break;
				case (byte)'h':
					write(6, 1, 0b_10011100);
					break;
				case (byte)'l':
					write(6, 1, 0b_10100000);
					break;
				case (byte)'m':
					write(6, 1, 0b_10100100);
					break;
				case (byte)'n':
					write(6, 1, 0b_10101000);
					break;
				case (byte)'p':
					write(6, 1, 0b_10101100);
					break;
				case (byte)'r':
					write(6, 1, 0b_10110000);
					break;
				case (byte)'u':
					write(6, 1, 0b_10110100);
					break;
				case (byte)':':
					write(7, 1, 0b_10111000);
					break;
				case (byte)'B':
					write(7, 1, 0b_10111010);
					break;
				case (byte)'C':
					write(7, 1, 0b_10111100);
					break;
				case (byte)'D':
					write(7, 1, 0b_10111110);
					break;
				case (byte)'E':
					write(7, 1, 0b_11000000);
					break;
				case (byte)'F':
					write(7, 1, 0b_11000010);
					break;
				case (byte)'G':
					write(7, 1, 0b_11000100);
					break;
				case (byte)'H':
					write(7, 1, 0b_11000110);
					break;
				case (byte)'I':
					write(7, 1, 0b_11001000);
					break;
				case (byte)'J':
					write(7, 1, 0b_11001010);
					break;
				case (byte)'K':
					write(7, 1, 0b_11001100);
					break;
				case (byte)'L':
					write(7, 1, 0b_11001110);
					break;
				case (byte)'M':
					write(7, 1, 0b_11010000);
					break;
				case (byte)'N':
					write(7, 1, 0b_11010010);
					break;
				case (byte)'O':
					write(7, 1, 0b_11010100);
					break;
				case (byte)'P':
					write(7, 1, 0b_11010110);
					break;
				case (byte)'Q':
					write(7, 1, 0b_11011000);
					break;
				case (byte)'R':
					write(7, 1, 0b_11011010);
					break;
				case (byte)'S':
					write(7, 1, 0b_11011100);
					break;
				case (byte)'T':
					write(7, 1, 0b_11011110);
					break;
				case (byte)'U':
					write(7, 1, 0b_11100000);
					break;
				case (byte)'V':
					write(7, 1, 0b_11100010);
					break;
				case (byte)'W':
					write(7, 1, 0b_11100100);
					break;
				case (byte)'Y':
					write(7, 1, 0b_11100110);
					break;
				case (byte)'j':
					write(7, 1, 0b_11101000);
					break;
				case (byte)'k':
					write(7, 1, 0b_11101010);
					break;
				case (byte)'q':
					write(7, 1, 0b_11101100);
					break;
				case (byte)'v':
					write(7, 1, 0b_11101110);
					break;
				case (byte)'w':
					write(7, 1, 0b_11110000);
					break;
				case (byte)'x':
					write(7, 1, 0b_11110010);
					break;
				case (byte)'y':
					write(7, 1, 0b_11110100);
					break;
				case (byte)'z':
					write(7, 1, 0b_11110110);
					break;
				case (byte)'&':
					write(8, 1, 0b_11111000);
					break;
				case (byte)'*':
					write(8, 1, 0b_11111001);
					break;
				case (byte)',':
					write(8, 1, 0b_11111010);
					break;
				case (byte)';':
					write(8, 1, 0b_11111011);
					break;
				case (byte)'X':
					write(8, 1, 0b_11111100);
					break;
				case (byte)'Z':
					write(8, 1, 0b_11111101);
					break;
				case (byte)'!':
					write(10, 2, 0b_11111110, 0b_00000000);
					break;
				case (byte)'"':
					write(10, 2, 0b_11111110, 0b_01000000);
					break;
				case (byte)'(':
					write(10, 2, 0b_11111110, 0b_10000000);
					break;
				case (byte)')':
					write(10, 2, 0b_11111110, 0b_11000000);
					break;
				case (byte)'?':
					write(10, 2, 0b_11111111, 0b_00000000);
					break;
				case (byte)'\'':
					write(11, 2, 0b_11111111, 0b_01000000);
					break;
				case (byte)'+':
					write(11, 2, 0b_11111111, 0b_01100000);
					break;
				case (byte)'|':
					write(11, 2, 0b_11111111, 0b_10000000);
					break;
				case (byte)'#':
					write(12, 2, 0b_11111111, 0b_10100000);
					break;
				case (byte)'>':
					write(12, 2, 0b_11111111, 0b_10110000);
					break;
				case 0:
					write(13, 2, 0b_11111111, 0b_11000000);
					break;
				case (byte)'$':
					write(13, 2, 0b_11111111, 0b_11001000);
					break;
				case (byte)'@':
					write(13, 2, 0b_11111111, 0b_11010000);
					break;
				case (byte)'[':
					write(13, 2, 0b_11111111, 0b_11011000);
					break;
				case (byte)']':
					write(13, 2, 0b_11111111, 0b_11100000);
					break;
				case (byte)'~':
					write(13, 2, 0b_11111111, 0b_11101000);
					break;
				case (byte)'^':
					write(14, 2, 0b_11111111, 0b_11110000);
					break;
				case (byte)'}':
					write(14, 2, 0b_11111111, 0b_11110100);
					break;
				case (byte)'<':
					write(15, 2, 0b_11111111, 0b_11111000);
					break;
				case (byte)'`':
					write(15, 2, 0b_11111111, 0b_11111010);
					break;
				case (byte)'{':
					write(15, 2, 0b_11111111, 0b_11111100);
					break;
				case (byte)'\\':
					write(19, 3, 0b_11111111, 0b_11111110, 0b_00000000);
					break;
				case 195:
					write(19, 3, 0b_11111111, 0b_11111110, 0b_00100000);
					break;
				case 208:
					write(19, 3, 0b_11111111, 0b_11111110, 0b_01000000);
					break;
				case 128:
					write(20, 3, 0b_11111111, 0b_11111110, 0b_01100000);
					break;
				case 130:
					write(20, 3, 0b_11111111, 0b_11111110, 0b_01110000);
					break;
				case 131:
					write(20, 3, 0b_11111111, 0b_11111110, 0b_10000000);
					break;
				case 162:
					write(20, 3, 0b_11111111, 0b_11111110, 0b_10010000);
					break;
				case 184:
					write(20, 3, 0b_11111111, 0b_11111110, 0b_10100000);
					break;
				case 194:
					write(20, 3, 0b_11111111, 0b_11111110, 0b_10110000);
					break;
				case 224:
					write(20, 3, 0b_11111111, 0b_11111110, 0b_11000000);
					break;
				case 226:
					write(20, 3, 0b_11111111, 0b_11111110, 0b_11010000);
					break;
				case 153:
					write(21, 3, 0b_11111111, 0b_11111110, 0b_11100000);
					break;
				case 161:
					write(21, 3, 0b_11111111, 0b_11111110, 0b_11101000);
					break;
				case 167:
					write(21, 3, 0b_11111111, 0b_11111110, 0b_11110000);
					break;
				case 172:
					write(21, 3, 0b_11111111, 0b_11111110, 0b_11111000);
					break;
				case 176:
					write(21, 3, 0b_11111111, 0b_11111111, 0b_00000000);
					break;
				case 177:
					write(21, 3, 0b_11111111, 0b_11111111, 0b_00001000);
					break;
				case 179:
					write(21, 3, 0b_11111111, 0b_11111111, 0b_00010000);
					break;
				case 209:
					write(21, 3, 0b_11111111, 0b_11111111, 0b_00011000);
					break;
				case 216:
					write(21, 3, 0b_11111111, 0b_11111111, 0b_00100000);
					break;
				case 217:
					write(21, 3, 0b_11111111, 0b_11111111, 0b_00101000);
					break;
				case 227:
					write(21, 3, 0b_11111111, 0b_11111111, 0b_00110000);
					break;
				case 229:
					write(21, 3, 0b_11111111, 0b_11111111, 0b_00111000);
					break;
				case 230:
					write(21, 3, 0b_11111111, 0b_11111111, 0b_01000000);
					break;
				case 129:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01001000);
					break;
				case 132:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01001100);
					break;
				case 133:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01010000);
					break;
				case 134:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01010100);
					break;
				case 136:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01011000);
					break;
				case 146:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01011100);
					break;
				case 154:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01100000);
					break;
				case 156:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01100100);
					break;
				case 160:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01101000);
					break;
				case 163:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01101100);
					break;
				case 164:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01110000);
					break;
				case 169:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01110100);
					break;
				case 170:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01111000);
					break;
				case 173:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_01111100);
					break;
				case 178:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10000000);
					break;
				case 181:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10000100);
					break;
				case 185:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10001000);
					break;
				case 186:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10001100);
					break;
				case 187:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10010000);
					break;
				case 189:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10010100);
					break;
				case 190:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10011000);
					break;
				case 196:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10011100);
					break;
				case 198:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10100000);
					break;
				case 228:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10100100);
					break;
				case 232:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10101000);
					break;
				case 233:
					write(22, 3, 0b_11111111, 0b_11111111, 0b_10101100);
					break;
				case 1:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_10110000);
					break;
				case 135:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_10110010);
					break;
				case 137:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_10110100);
					break;
				case 138:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_10110110);
					break;
				case 139:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_10111000);
					break;
				case 140:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_10111010);
					break;
				case 141:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_10111100);
					break;
				case 143:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_10111110);
					break;
				case 147:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11000000);
					break;
				case 149:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11000010);
					break;
				case 150:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11000100);
					break;
				case 151:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11000110);
					break;
				case 152:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11001000);
					break;
				case 155:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11001010);
					break;
				case 157:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11001100);
					break;
				case 158:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11001110);
					break;
				case 165:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11010000);
					break;
				case 166:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11010010);
					break;
				case 168:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11010100);
					break;
				case 174:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11010110);
					break;
				case 175:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11011000);
					break;
				case 180:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11011010);
					break;
				case 182:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11011100);
					break;
				case 183:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11011110);
					break;
				case 188:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11100000);
					break;
				case 191:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11100010);
					break;
				case 197:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11100100);
					break;
				case 231:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11100110);
					break;
				case 239:
					write(23, 3, 0b_11111111, 0b_11111111, 0b_11101000);
					break;
				case 9:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11101010);
					break;
				case 142:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11101011);
					break;
				case 144:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11101100);
					break;
				case 145:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11101101);
					break;
				case 148:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11101110);
					break;
				case 159:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11101111);
					break;
				case 171:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11110000);
					break;
				case 206:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11110001);
					break;
				case 215:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11110010);
					break;
				case 225:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11110011);
					break;
				case 236:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11110100);
					break;
				case 237:
					write(24, 3, 0b_11111111, 0b_11111111, 0b_11110101);
					break;
				case 199:
					write(25, 4, 0b_11111111, 0b_11111111, 0b_11110110, 0b_00000000);
					break;
				case 207:
					write(25, 4, 0b_11111111, 0b_11111111, 0b_11110110, 0b_10000000);
					break;
				case 234:
					write(25, 4, 0b_11111111, 0b_11111111, 0b_11110111, 0b_00000000);
					break;
				case 235:
					write(25, 4, 0b_11111111, 0b_11111111, 0b_11110111, 0b_10000000);
					break;
				case 192:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111000, 0b_00000000);
					break;
				case 193:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111000, 0b_01000000);
					break;
				case 200:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111000, 0b_10000000);
					break;
				case 201:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111000, 0b_11000000);
					break;
				case 202:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111001, 0b_00000000);
					break;
				case 205:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111001, 0b_01000000);
					break;
				case 210:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111001, 0b_10000000);
					break;
				case 213:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111001, 0b_11000000);
					break;
				case 218:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111010, 0b_00000000);
					break;
				case 219:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111010, 0b_01000000);
					break;
				case 238:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111010, 0b_10000000);
					break;
				case 240:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111010, 0b_11000000);
					break;
				case 242:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111011, 0b_00000000);
					break;
				case 243:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111011, 0b_01000000);
					break;
				case 255:
					write(26, 4, 0b_11111111, 0b_11111111, 0b_11111011, 0b_10000000);
					break;
				case 203:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111011, 0b_11000000);
					break;
				case 204:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111011, 0b_11100000);
					break;
				case 211:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111100, 0b_00000000);
					break;
				case 212:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111100, 0b_00100000);
					break;
				case 214:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111100, 0b_01000000);
					break;
				case 221:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111100, 0b_01100000);
					break;
				case 222:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111100, 0b_10000000);
					break;
				case 223:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111100, 0b_10100000);
					break;
				case 241:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111100, 0b_11000000);
					break;
				case 244:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111100, 0b_11100000);
					break;
				case 245:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111101, 0b_00000000);
					break;
				case 246:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111101, 0b_00100000);
					break;
				case 247:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111101, 0b_01000000);
					break;
				case 248:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111101, 0b_01100000);
					break;
				case 250:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111101, 0b_10000000);
					break;
				case 251:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111101, 0b_10100000);
					break;
				case 252:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111101, 0b_11000000);
					break;
				case 253:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111101, 0b_11100000);
					break;
				case 254:
					write(27, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_00000000);
					break;
				case 2:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_00100000);
					break;
				case 3:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_00110000);
					break;
				case 4:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_01000000);
					break;
				case 5:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_01010000);
					break;
				case 6:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_01100000);
					break;
				case 7:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_01110000);
					break;
				case 8:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_10000000);
					break;
				case 11:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_10010000);
					break;
				case 12:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_10100000);
					break;
				case 14:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_10110000);
					break;
				case 15:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_11000000);
					break;
				case 16:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_11010000);
					break;
				case 17:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_11100000);
					break;
				case 18:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111110, 0b_11110000);
					break;
				case 19:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_00000000);
					break;
				case 20:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_00010000);
					break;
				case 21:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_00100000);
					break;
				case 23:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_00110000);
					break;
				case 24:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_01000000);
					break;
				case 25:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_01010000);
					break;
				case 26:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_01100000);
					break;
				case 27:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_01110000);
					break;
				case 28:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_10000000);
					break;
				case 29:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_10010000);
					break;
				case 30:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_10100000);
					break;
				case 31:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_10110000);
					break;
				case 127:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_11000000);
					break;
				case 220:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_11010000);
					break;
				case 249:
					write(28, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_11100000);
					break;
				case 10:
					write(30, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_11110000);
					break;
				case 13:
					write(30, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_11110100);
					break;
				case 22:
					write(30, 4, 0b_11111111, 0b_11111111, 0b_11111111, 0b_11111000);
					break;
			}

		down();
		return tdata.ToArray();
	}
}