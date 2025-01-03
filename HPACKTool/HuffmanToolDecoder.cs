using System.Text;

namespace cewno.HPACKTool;

/// <summary>
///     这个类包含了一套用于解码和编码适用于 HTTP2 的 Huffman 压缩算法的工具
/// </summary>
public static partial class HuffmanTool
{
	// private static readonly byte[] bbbf = 
	// [
	//  0b_10000000,
	// 	0b_01000000,
	// 	0b_00100000,
	// 	0b_00010000,
	// 	0b_00001000,
	// 	0b_00000100,
	// 	0b_00000010, 
	// 	0b_00000001];

	/// <summary>
	///     用于与(&amp;) 运算提取位(bit) 数据的数组
	/// </summary>
	//倒着来，简化运算操作
	// ((nextz() & at) >> (7 - i)) 简化为 ((nextz() & data[i3]) >> i)
	// 减少一个减法操作
	private static readonly byte[] bbbf =
	{
		0b00000001,
		0b00000010,
		0b00000100,
		0b00001000,
		0b00010000,
		0b00100000,
		0b01000000,
		0b10000000
	};

	/// <summary>
	///     解码使用 HPACK 算法中的 Huffman 算法压缩的字符串并直接返回字符串
	/// </summary>
	/// <param name="data">已编码的数据</param>
	/// <returns>解码后的字符数组</returns>
	/// <exception cref="HuffmanForHPACKHaveEOSTagException">识别到不应出现的EOS标识</exception>
	/// <exception cref="HuffmanForHPACKPaddingInaccuracyException">填充格式不正确</exception>
	/// <returns>解码后的字符串</returns>
	public static string? DecoderToString(byte[] data)
	{
		Span<byte> buffer = stackalloc byte[(int)(data.Length * 1.7)];
		int length = Decoder(new Span<byte>(data, 0, data.Length), buffer);
		return length == 0 ? null : Encoding.ASCII.GetString(buffer.Slice(0, length));
	}

	/// <summary>
	///     解码使用 HPACK 算法中的 Huffman 算法压缩的字符串并直接返回字符串
	/// </summary>
	/// <param name="data">已编码的数据</param>
	/// <param name="offset">偏移量</param>
	/// <param name="length">数据长度</param>
	/// <exception cref="HuffmanForHPACKHaveEOSTagException">识别到不应出现的EOS标识</exception>
	/// <exception cref="HuffmanForHPACKPaddingInaccuracyException">填充格式不正确</exception>
	/// <returns>解码后的字符串</returns>
	public static string? DecoderToString(byte[] data, int offset, int length)
	{
		Span<byte> buffer = stackalloc byte[(int)(length * 1.6)];
		int olength = Decoder(new Span<byte>(data, offset, length), buffer);
		return olength == 0 ? null : Encoding.ASCII.GetString(buffer.Slice(0, olength));
	}

	/// <summary>
	///     解码使用 HPACK 算法中的 Huffman 算法压缩的字符串
	/// </summary>
	/// <param name="data">已编码的数据</param>
	/// <param name="buffer">输出缓冲区</param>
	/// <returns>解码后的字符数组</returns>
	/// <exception cref="HuffmanForHPACKHaveEOSTagException">识别到不应出现的EOS标识</exception>
	/// <exception cref="HuffmanForHPACKPaddingInaccuracyException">填充格式不正确</exception>
	public static int Decoder(byte[] data, Span<byte> buffer)
	{
		return Decoder(new Span<byte>(data, 0, data.Length), buffer);
	}

	/// <summary>
	///     解码使用 HPACK 算法中的 Huffman 算法压缩的字符串
	/// </summary>
	/// <param name="data">已编码的数据</param>
	/// <param name="resultBuffer">结果缓冲区，其长度应为输入长度的1.6倍及以上</param>
	/// <returns>解码后的字符数组</returns>
	/// <exception cref="HuffmanForHPACKHaveEOSTagException">识别到不应出现的EOS标识</exception>
	/// <exception cref="HuffmanForHPACKPaddingInaccuracyException">填充格式不正确</exception>
	public static int Decoder(ReadOnlySpan<byte> data, Span<byte> resultBuffer)
	{
		if (data.Length == 0) return 0;

		int resultBufferLength = resultBuffer.Length;
		int length = data.Length;
		//当前处在在的位反过来的索引    真实值 = 7 - i
		int i = 8;
		//当前处在的byte的索引
		int i2 = 0;

		int wi = 0;


		//文档要求，末尾用1填充，若违反需要报错，这个变量是用来记录的
		//如果我理解错了，请向我提交issue
		bool have_zero = false;
		//还有数据
		bool h = true;
		//文档要求超出7位的填充要报错,这个变量是用来记录的
		bool oleight = false;

		int i3 = 0;
		int maxIndex = length - 1;

		//用于0的下一位
		short nextz()
		{
			//标记有0
			have_zero = true;
			//检查是否已到末位
			if (i != 0) return bbbf[--i];
			//检测是否还有数据
			if (i2 == maxIndex)
			{
				h = false;
				return -1;
			}

			i = 7;
			i3 = ++i2;
			return bbbf[7];
		}

		//用于1的下一位
		short nexto()
		{
			if (i != 0) return bbbf[--i];
			//检测是否还有数据
			if (i2 == maxIndex)
			{
				h = false;
				return -1;
			}

			i = 7;
			i3 = ++i2;
			return bbbf[7];
		}

		void chken()
		{
			if (wi == resultBufferLength) throw new IndexOutOfRangeException();
		}

		int lastwi = wi;


		{
			while (h)
			{
				//下面的内容超长，没耐心的请别展开，不过就算不展开也可能会很卡
				//这些都是我手写的，注释可能会写错，看到了的话可以提issue告诉我

				#region DecoderSwitch

				oleight = false;
				short t;
				t = nexto();
				if (t == -1) break;
				switch ((t & data[i3]) >> i)
				{
					//0
					case 0:
					{
						t = nextz();
						if (t == -1) break;
						switch ((t & data[i3]) >> i)
						{
							//00
							case 0:
							{
								t = nextz();
								if (t == -1) break;
								switch ((t & data[i3]) >> i)
								{
									//000
									case 0:
									{
										t = nextz();
										if (t == -1) break;
										switch ((t & data[i3]) >> i)
										{
											//0000
											case 0:
											{
												t = nextz();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//00000
													case 0:
													{
														chken();
														resultBuffer[wi++] = (byte)'0';
														break;
													}
													//00001
													case 1:
													{
														chken();
														resultBuffer[wi++] = (byte)'1';
														break;
													}
												}

												break;
											}
											//0001
											case 1:
											{
												t = nexto();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//00010
													case 0:
													{
														chken();
														resultBuffer[wi++] = (byte)'2';
														break;
													}
													//00011
													case 1:
													{
														chken();
														resultBuffer[wi++] = (byte)'a';
														break;
													}
												}

												break;
											}
										}

										break;
									}
									//001
									case 1:
									{
										t = nexto();
										if (t == -1) break;
										switch ((t & data[i3]) >> i)
										{
											//0010
											case 0:
											{
												t = nextz();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//00100
													case 0:
													{
														chken();
														resultBuffer[wi++] = (byte)'c';
														break;
													}
													//00101
													case 1:
													{
														chken();
														resultBuffer[wi++] = (byte)'e';
														break;
													}
												}

												break;
											}
											//0011
											case 1:
											{
												t = nexto();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//00110
													case 0:
													{
														chken();
														resultBuffer[wi++] = (byte)'i';
														break;
													}
													//00111
													case 1:
													{
														chken();
														resultBuffer[wi++] = (byte)'o';
														break;
													}
												}

												break;
											}
										}

										break;
									}
								}

								break;
							}
							//01
							case 1:
							{
								t = nexto();
								if (t == -1) break;
								switch ((t & data[i3]) >> i)
								{
									//010
									case 0:
									{
										t = nextz();
										if (t == -1) break;
										switch ((t & data[i3]) >> i)
										{
											//0100
											case 0:
											{
												t = nextz();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//01000
													case 0:
													{
														chken();
														resultBuffer[wi++] = (byte)'s';
														break;
													}
													//01001
													case 1:
													{
														chken();
														resultBuffer[wi++] = (byte)'t';
														break;
													}
												}

												break;
											}
											//0101
											case 1:
											{
												t = nexto();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//01010
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//010100
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)' ';
																break;
															}
															//0101001
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'%';
																break;
															}
														}

														break;
													}
													//01011
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//010110
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'-';
																break;
															}
															//010111
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'.';
																break;
															}
														}

														break;
													}
												}

												break;
											}
										}

										break;
									}
									//011
									case 1:
									{
										t = nexto();
										if (t == -1) break;
										switch ((t & data[i3]) >> i)
										{
											//0110
											case 0:
											{
												t = nextz();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//01100
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//011000
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'/';
																break;
															}
															//011001
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'3';
																break;
															}
														}

														break;
													}
													//01101
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//011010
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'4';
																break;
															}
															//011011
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'5';
																break;
															}
														}

														break;
													}
												}

												break;
											}
											//0111
											case 1:
											{
												t = nexto();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//01110
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//011100
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'6';
																break;
															}
															//011101
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'7';
																break;
															}
														}

														break;
													}
													//01111
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//011110
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'8';
																break;
															}
															//011111
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'9';
																break;
															}
														}

														break;
													}
												}

												break;
											}
										}

										break;
									}
								}

								break;
							}
						}

						break;
					}
					//1
					case 1:
					{
						t = nexto();
						if (t == -1) break;
						switch ((t & data[i3]) >> i)
						{
							//10
							case 0:
							{
								t = nextz();
								if (t == -1) break;
								switch ((t & data[i3]) >> i)
								{
									//100
									case 0:
									{
										t = nextz();
										if (t == -1) break;
										switch ((t & data[i3]) >> i)
										{
											//1000
											case 0:
											{
												t = nextz();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//10000
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//100000
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'=';
																break;
															}
															//100001
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'A';
																break;
															}
														}

														break;
													}
													//10001
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//100010
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'_';
																break;
															}
															//100011
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'b';
																break;
															}
														}

														break;
													}
												}

												break;
											}
											//1001
											case 1:
											{
												t = nexto();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//10010
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//100100
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'d';
																break;
															}
															//100101
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'f';
																break;
															}
														}

														break;
													}
													//10011
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//100110
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'g';
																break;
															}
															//100111
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'h';
																break;
															}
														}

														break;
													}
												}

												break;
											}
										}

										break;
									}
									//101
									case 1:
									{
										t = nexto();
										if (t == -1) break;
										switch ((t & data[i3]) >> i)
										{
											//1010
											case 0:
											{
												t = nextz();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//10100
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//101000
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'l';
																break;
															}
															//101001
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'m';
																break;
															}
														}

														break;
													}
													//10101
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//101010
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'n';
																break;
															}
															//101011
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'p';
																break;
															}
														}

														break;
													}
												}

												break;
											}
											//1011
											case 1:
											{
												t = nexto();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//10110
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//101100
															case 0:
															{
																chken();
																resultBuffer[wi++] = (byte)'r';
																break;
															}
															//101101
															case 1:
															{
																chken();
																resultBuffer[wi++] = (byte)'u';
																break;
															}
														}

														break;
													}
													//10111
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//101110
															case 0:
															{
																t = nextz();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1011100
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)':';
																		break;
																	}
																	//1011101
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'B';
																		break;
																	}
																}

																break;
															}
															//101111
															case 1:
															{
																t = nexto();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1011110
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'C';
																		break;
																	}
																	//1011111
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'D';
																		break;
																	}
																}

																break;
															}
														}

														break;
													}
												}

												break;
											}
										}

										break;
									}
								}

								break;
							}
							//11
							case 1:
							{
								t = nexto();
								if (t == -1) break;
								switch ((t & data[i3]) >> i)
								{
									//110
									case 0:
									{
										t = nextz();
										if (t == -1) break;
										switch ((t & data[i3]) >> i)
										{
											//1100
											case 0:
											{
												t = nextz();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//11000
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//110000
															case 0:
															{
																t = nextz();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1100000
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'E';
																		break;
																	}
																	//1100001
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'F';
																		break;
																	}
																}

																break;
															}
															//110001
															case 1:
															{
																t = nexto();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1100010
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'G';
																		break;
																	}
																	//1100011
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'H';
																		break;
																	}
																}

																break;
															}
														}

														break;
													}
													//11001
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//110010
															case 0:
															{
																t = nextz();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//110010
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'I';
																		break;
																	}
																	//110011
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'J';
																		break;
																	}
																}

																break;
															}
															//110011
															case 1:
															{
																t = nexto();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1100110
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'K';
																		break;
																	}
																	//1100111
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'L';
																		break;
																	}
																}

																break;
															}
														}

														break;
													}
												}

												break;
											}
											//1101
											case 1:
											{
												t = nexto();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//11010
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//110100
															case 0:
															{
																t = nextz();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1101000
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'M';
																		break;
																	}
																	//1101010
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'N';
																		break;
																	}
																}

																break;
															}
															//110101
															case 1:
															{
																t = nexto();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1101010
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'O';
																		break;
																	}
																	//1101011
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'P';
																		break;
																	}
																}

																break;
															}
														}

														break;
													}
													//11011
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//110110
															case 0:
															{
																t = nextz();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1101100
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'Q';
																		break;
																	}
																	//1101101
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'R';
																		break;
																	}
																}

																break;
															}
															//110111
															case 1:
															{
																t = nexto();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1101110
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'S';
																		break;
																	}
																	//1101111
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'T';
																		break;
																	}
																}

																break;
															}
														}

														break;
													}
												}

												break;
											}
										}

										break;
									}
									//111
									case 1:
									{
										t = nexto();
										if (t == -1) break;
										switch ((t & data[i3]) >> i)
										{
											//1110
											case 0:
											{
												t = nextz();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//11100
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//111000
															case 0:
															{
																t = nextz();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1110000
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'U';
																		break;
																	}
																	//1110001
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'V';
																		break;
																	}
																}

																break;
															}
															//111001
															case 1:
															{
																t = nexto();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1110010
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'W';
																		break;
																	}
																	//1110011
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'Y';
																		break;
																	}
																}

																break;
															}
														}

														break;
													}
													//11101
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//111010
															case 0:
															{
																t = nextz();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1110100
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'j';
																		break;
																	}
																	//1110101
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'k';
																		break;
																	}
																}

																break;
															}
															//111011
															case 1:
															{
																t = nexto();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1110110
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'q';
																		break;
																	}
																	//1110111
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'v';
																		break;
																	}
																}

																break;
															}
														}

														break;
													}
												}

												break;
											}
											//1111
											case 1:
											{
												t = nexto();
												if (t == -1) break;
												switch ((t & data[i3]) >> i)
												{
													//11110
													case 0:
													{
														t = nextz();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//111100
															case 0:
															{
																t = nextz();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1111010
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'w';
																		break;
																	}
																	//1111011
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'x';
																		break;
																	}
																}

																break;
															}
															//111101
															case 1:
															{
																t = nexto();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1111010
																	case 0:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'y';
																		break;
																	}
																	//1111011
																	case 1:
																	{
																		chken();
																		resultBuffer[wi++] = (byte)'z';
																		break;
																	}
																}

																break;
															}
														}

														break;
													}
													//11111
													case 1:
													{
														t = nexto();
														if (t == -1) break;
														switch ((t & data[i3]) >> i)
														{
															//111110
															case 0:
															{
																t = nextz();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1111100
																	case 0:
																	{
																		t = nextz();
																		if (t == -1) break;
																		switch ((t & data[i3]) >> i)
																		{
																			//11111000
																			case 0:
																			{
																				chken();
																				resultBuffer[wi++] = (byte)'&';
																				break;
																			}
																			//11111001
																			case 1:
																			{
																				chken();
																				resultBuffer[wi++] = (byte)'*';
																				break;
																			}
																		}

																		break;
																	}
																	//1111101
																	case 1:
																	{
																		t = nexto();
																		if (t == -1) break;
																		switch ((t & data[i3]) >> i)
																		{
																			//11111010
																			case 0:
																			{
																				chken();
																				resultBuffer[wi++] = (byte)',';
																				break;
																			}
																			//11111011
																			case 1:
																			{
																				chken();
																				resultBuffer[wi++] = (byte)';';
																				break;
																			}
																		}

																		break;
																	}
																}

																break;
															}
															//111111
															case 1:
															{
																t = nexto();
																if (t == -1) break;
																switch ((t & data[i3]) >> i)
																{
																	//1111110
																	case 0:
																	{
																		t = nextz();
																		if (t == -1) break;
																		switch ((t & data[i3]) >> i)
																		{
																			//11111100
																			case 0:
																			{
																				chken();
																				resultBuffer[wi++] = (byte)'X';
																				break;
																			}
																			//11111101
																			case 1:
																			{
																				chken();
																				resultBuffer[wi++] = (byte)'Z';
																				break;
																			}
																		}

																		break;
																	}
																	//1111111
																	case 1:
																	{
																		t = nexto();
																		if (t == -1) break;
																		switch ((t & data[i3]) >> i)
																		{
																			//11111110
																			case 0:
																			{
																				t = nextz();
																				if (t == -1) break;
																				switch ((t & data[i3]) >> i)
																				{
																					//11111110|0
																					case 0:
																					{
																						t = nextz();
																						if (t == -1) break;
																						switch ((t & data[i3]) >> i)
																						{
																							//11111110|00
																							case 0:
																							{
																								chken();
																								resultBuffer[wi++] = (byte)'!';
																								break;
																							}
																							//11111110|01
																							case 1:
																							{
																								chken();
																								resultBuffer[wi++] = (byte)'"';
																								break;
																							}
																						}

																						break;
																					}
																					//11111110|1
																					case 1:
																					{
																						t = nexto();
																						if (t == -1) break;
																						switch ((t & data[i3]) >> i)
																						{
																							//11111110|10
																							case 0:
																							{
																								chken();
																								resultBuffer[wi++] = (byte)'(';
																								break;
																							}
																							//11111110|11
																							case 1:
																							{
																								chken();
																								resultBuffer[wi++] = (byte)')';
																								break;
																							}
																						}

																						break;
																					}
																				}

																				break;
																			}
																			//11111111
																			case 1:
																			{
																				oleight = true;
																				t = nexto();
																				if (t == -1) break;
																				switch ((t & data[i3]) >> i)
																				{
																					//11111111|0
																					case 0:
																					{
																						t = nextz();
																						if (t == -1) break;
																						switch ((t & data[i3]) >> i)
																						{
																							//11111111|00
																							case 0:
																							{
																								chken();
																								resultBuffer[wi++] = (byte)'?';
																								break;
																							}
																							//11111111|01
																							case 1:
																							{
																								t = nexto();
																								if (t == -1) break;
																								switch ((t & data[i3]) >> i)
																								{
																									//11111111|010
																									case 0:
																									{
																										chken();
																										resultBuffer[wi++] = (byte)'\'';
																										break;
																									}
																									//11111111|011
																									case 1:
																									{
																										chken();
																										resultBuffer[wi++] = (byte)'+';
																										break;
																									}
																								}

																								break;
																							}
																						}

																						break;
																					}
																					//11111111|1
																					case 1:
																					{
																						t = nexto();
																						if (t == -1) break;
																						switch ((t & data[i3]) >> i)
																						{
																							//11111111|10
																							case 0:
																							{
																								t = nextz();
																								if (t == -1) break;
																								switch ((t & data[i3]) >> i)
																								{
																									//11111111|100
																									case 0:
																									{
																										chken();
																										resultBuffer[wi++] = (byte)'|';
																										break;
																									}
																									//11111111|101
																									case 1:
																									{
																										t = nexto();
																										if (t == -1) break;
																										switch ((t & data[i3]) >> i)
																										{
																											//11111111|1010
																											case 0:
																											{
																												chken();
																												resultBuffer[wi++] = (byte)'#';
																												break;
																											}
																											//11111111|1011
																											case 1:
																											{
																												chken();
																												resultBuffer[wi++] = (byte)'>';
																												break;
																											}
																										}

																										break;
																									}
																								}

																								break;
																							}
																							//11111111|11
																							case 1:
																							{
																								t = nexto();
																								if (t == -1) break;
																								switch ((t & data[i3]) >> i)
																								{
																									//11111111|110
																									case 0:
																									{
																										t = nextz();
																										if (t == -1) break;
																										switch ((t & data[i3]) >> i)
																										{
																											//11111111|1100
																											case 0:
																											{
																												t = nextz();
																												if (t == -1) break;
																												switch ((t & data[i3]) >> i)
																												{
																													//11111111|11000
																													case 0:
																													{
																														chken();
																														resultBuffer[wi++] = 0;
																														break;
																													}
																													//11111111|11001
																													case 1:
																													{
																														chken();
																														resultBuffer[wi++] = (byte)'$';
																														break;
																													}
																												}

																												break;
																											}
																											//11111111|1101
																											case 1:
																											{
																												t = nexto();
																												if (t == -1) break;
																												switch ((t & data[i3]) >> i)
																												{
																													//11111111|11010
																													case 0:
																													{
																														chken();
																														resultBuffer[wi++] = (byte)'@';
																														break;
																													}
																													//11111111|11011
																													case 1:
																													{
																														chken();
																														resultBuffer[wi++] = (byte)'[';
																														break;
																													}
																												}

																												break;
																											}
																										}

																										break;
																									}
																									//11111111|111
																									case 1:
																									{
																										t = nexto();
																										if (t == -1) break;
																										switch ((t & data[i3]) >> i)
																										{
																											//11111111|1110
																											case 0:
																											{
																												t = nextz();
																												if (t == -1) break;
																												switch ((t & data[i3]) >> i)
																												{
																													//11111111|11100
																													case 0:
																													{
																														chken();
																														resultBuffer[wi++] = (byte)']';
																														break;
																													}
																													//11111111|11101
																													case 1:
																													{
																														chken();
																														resultBuffer[wi++] = (byte)'~';
																														break;
																													}
																												}

																												break;
																											}
																											//11111111|1111
																											case 1:
																											{
																												t = nexto();
																												if (t == -1) break;
																												switch ((t & data[i3]) >> i)
																												{
																													//11111111|11110
																													case 0:
																													{
																														t = nextz();
																														if (t == -1) break;
																														switch ((t & data[i3]) >> i)
																														{
																															//11111111|111100
																															case 0:
																															{
																																chken();
																																resultBuffer[wi++] = (byte)'^';
																																break;
																															}
																															//11111111|111101
																															case 1:
																															{
																																chken();
																																resultBuffer[wi++] = (byte)'}';
																																break;
																															}
																														}

																														break;
																													}
																													//11111111|11111
																													case 1:
																													{
																														t = nexto();
																														if (t == -1) break;
																														switch ((t & data[i3]) >> i)
																														{
																															//11111111|111110
																															case 0:
																															{
																																t = nextz();
																																if (t == -1) break;
																																switch ((t & data[i3]) >> i)
																																{
																																	//11111111|1111100
																																	case 0:
																																	{
																																		chken();
																																		resultBuffer[wi++] = (byte)'<';
																																		break;
																																	}
																																	//11111111|1111101
																																	case 1:
																																	{
																																		chken();
																																		resultBuffer[wi++] = (byte)'`';
																																		break;
																																	}
																																}

																																break;
																															}
																															//11111111|111111
																															case 1:
																															{
																																t = nexto();
																																if (t == -1) break;
																																switch ((t & data[i3]) >> i)
																																{
																																	//11111111|1111110
																																	case 0:
																																	{
																																		chken();
																																		resultBuffer[wi++] = (byte)'{';
																																		break;
																																	}
																																	//11111111|1111111
																																	case 1:
																																	{
																																		t = nexto();
																																		if (t == -1) break;
																																		switch ((t & data[i3]) >> i)
																																		{
																																			//11111111|11111110|
																																			case 0:
																																			{
																																				t = nextz();
																																				if (t == -1) break;
																																				switch ((t & data[i3]) >> i)
																																				{
																																					//11111111|11111110|0
																																					case 0:
																																					{
																																						t = nextz();
																																						if (t == -1) break;
																																						switch ((t & data[i3]) >> i)
																																						{
																																							//11111111|11111110|00
																																							case 0:
																																							{
																																								t = nextz();
																																								if (t == -1) break;
																																								switch ((t & data[i3]) >> i)
																																								{
																																									//11111111|11111110|000
																																									case 0:
																																									{
																																										chken();
																																										resultBuffer[wi++] = (byte)'\\';
																																										break;
																																									}
																																									//11111111|11111110|001
																																									case 1:
																																									{
																																										chken();
																																										resultBuffer[wi++] = 195;
																																										break;
																																									}
																																								}

																																								break;
																																							}
																																							//11111111|11111110|01
																																							case 1:
																																							{
																																								t = nexto();
																																								if (t == -1) break;
																																								switch ((t & data[i3]) >> i)
																																								{
																																									//11111111|11111110|010
																																									case 0:
																																									{
																																										chken();
																																										resultBuffer[wi++] = 208;
																																										break;
																																									}
																																									//11111111|11111110|011
																																									case 1:
																																									{
																																										t = nexto();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111110|0110
																																											case 0:
																																											{
																																												chken();
																																												resultBuffer[wi++] = 128;
																																												break;
																																											}
																																											//11111111|11111110|0111
																																											case 1:
																																											{
																																												chken();
																																												resultBuffer[wi++] = 130;
																																												break;
																																											}
																																										}

																																										break;
																																									}
																																								}

																																								break;
																																							}
																																						}

																																						break;
																																					}
																																					//11111111|11111110|1
																																					case 1:
																																					{
																																						t = nexto();
																																						if (t == -1) break;
																																						switch ((t & data[i3]) >> i)
																																						{
																																							//11111111|11111110|10
																																							case 0:
																																							{
																																								t = nextz();
																																								if (t == -1) break;
																																								switch ((t & data[i3]) >> i)
																																								{
																																									//11111111|11111110|100
																																									case 0:
																																									{
																																										t = nextz();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111110|1000
																																											case 0:
																																											{
																																												chken();
																																												resultBuffer[wi++] = 131;
																																												break;
																																											}
																																											//11111111|11111110|1001
																																											case 1:
																																											{
																																												chken();
																																												resultBuffer[wi++] = 162;
																																												break;
																																											}
																																										}

																																										break;
																																									}
																																									//11111111|11111110|101
																																									case 1:
																																									{
																																										t = nexto();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|111111110|1010
																																											case 0:
																																											{
																																												chken();
																																												resultBuffer[wi++] = 184;
																																												break;
																																											}
																																											//11111111|11111110|1011
																																											case 1:
																																											{
																																												chken();
																																												resultBuffer[wi++] = 194;
																																												break;
																																											}
																																										}

																																										break;
																																									}
																																								}

																																								break;
																																							}
																																							//11111111|11111110|11
																																							case 1:
																																							{
																																								t = nexto();
																																								if (t == -1) break;
																																								switch ((t & data[i3]) >> i)
																																								{
																																									//11111111|11111110|110
																																									case 0:
																																									{
																																										t = nextz();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111110|1100
																																											case 0:
																																											{
																																												chken();
																																												resultBuffer[wi++] = 224;
																																												break;
																																											}
																																											//11111111|11111110|1101
																																											case 1:
																																											{
																																												chken();
																																												resultBuffer[wi++] = 226;
																																												break;
																																											}
																																										}

																																										break;
																																									}
																																									//11111111|11111110|111
																																									case 1:
																																									{
																																										t = nexto();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111110|1110
																																											case 0:
																																											{
																																												t = nextz();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111110|11100
																																													case 0:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 153;
																																														break;
																																													}
																																													//11111111|11111110|11101
																																													case 1:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 161;
																																														break;
																																													}
																																												}

																																												break;
																																											}
																																											//11111111|11111110|1111
																																											case 1:
																																											{
																																												t = nexto();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111110|11110
																																													case 0:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 167;
																																														break;
																																													}
																																													//11111111|11111110|
																																													case 1:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 172;
																																														break;
																																													}
																																												}

																																												break;
																																											}
																																										}

																																										break;
																																									}
																																								}

																																								break;
																																							}
																																						}

																																						break;
																																					}
																																				}

																																				break;
																																			}
																																			//11111111|11111111
																																			case 1:
																																			{
																																				t = nexto();
																																				if (t == -1) break;
																																				switch ((t & data[i3]) >> i)
																																				{
																																					//11111111|11111111|0
																																					case 0:
																																					{
																																						t = nextz();
																																						if (t == -1) break;
																																						switch ((t & data[i3]) >> i)
																																						{
																																							//11111111|11111111|00
																																							case 0:
																																							{
																																								t = nextz();
																																								if (t == -1) break;
																																								switch ((t & data[i3]) >> i)
																																								{
																																									//11111111|11111111|000
																																									case 0:
																																									{
																																										t = nextz();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111111|0000
																																											case 0:
																																											{
																																												t = nextz();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|00000
																																													case 0:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 176;
																																														break;
																																													}
																																													//11111111|11111111|00001
																																													case 1:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 177;
																																														break;
																																													}
																																												}

																																												break;
																																											}
																																											//11111111|11111111|0001
																																											case 1:
																																											{
																																												t = nexto();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|00010
																																													case 0:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 179;
																																														break;
																																													}
																																													//11111111|11111111|
																																													case 1:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 209;
																																														break;
																																													}
																																												}

																																												break;
																																											}
																																										}

																																										break;
																																									}
																																									//11111111|11111111|001
																																									case 1:
																																									{
																																										t = nexto();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111111|0010
																																											case 0:
																																											{
																																												t = nextz();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|00100
																																													case 0:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 216;
																																														break;
																																													}
																																													//11111111|11111111|
																																													case 1:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 217;
																																														break;
																																													}
																																												}

																																												break;
																																											}
																																											//11111111|11111111|0011
																																											case 1:
																																											{
																																												t = nexto();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|00110
																																													case 0:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 227;
																																														break;
																																													}
																																													//11111111|11111111|
																																													case 1:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 229;
																																														break;
																																													}
																																												}

																																												break;
																																											}
																																										}

																																										break;
																																									}
																																								}

																																								break;
																																							}
																																							//11111111|11111111|01
																																							case 1:
																																							{
																																								t = nexto();
																																								if (t == -1) break;
																																								switch ((t & data[i3]) >> i)
																																								{
																																									//11111111|11111111|010
																																									case 0:
																																									{
																																										t = nextz();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111111|0100
																																											case 0:
																																											{
																																												t = nextz();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|01000
																																													case 0:
																																													{
																																														chken();
																																														resultBuffer[wi++] = 230;
																																														break;
																																													}
																																													//11111111|11111111|01001
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|010010
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 129;
																																																break;
																																															}
																																															//11111111|11111111|010011
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 132;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																											//11111111|11111111|0101
																																											case 1:
																																											{
																																												t = nexto();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|01010
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|010100
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 133;
																																																break;
																																															}
																																															//11111111|11111111|010101
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 134;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|01011
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|010110
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 136;
																																																break;
																																															}
																																															//11111111|11111111|010111
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 146;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																										}

																																										break;
																																									}
																																									//11111111|11111111|011
																																									case 1:
																																									{
																																										t = nexto();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111111|0110
																																											case 0:
																																											{
																																												t = nextz();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|01100
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|011000
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 154;
																																																break;
																																															}
																																															//11111111|11111111|011001
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 156;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|01101
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|011010
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 160;
																																																break;
																																															}
																																															//11111111|11111111|011011
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 163;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																											//11111111|11111111|0111
																																											case 1:
																																											{
																																												t = nexto();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|01110
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|011100
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 164;
																																																break;
																																															}
																																															//11111111|11111111|
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 169;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|01111
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|011110
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 170;
																																																break;
																																															}
																																															//11111111|11111111|011111
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 173;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																										}

																																										break;
																																									}
																																								}

																																								break;
																																							}
																																						}

																																						break;
																																					}
																																					//11111111|11111111|1
																																					case 1:
																																					{
																																						t = nexto();
																																						if (t == -1) break;
																																						switch ((t & data[i3]) >> i)
																																						{
																																							//11111111|11111111|10
																																							case 0:
																																							{
																																								t = nextz();
																																								if (t == -1) break;
																																								switch ((t & data[i3]) >> i)
																																								{
																																									//11111111|11111111|100
																																									case 0:
																																									{
																																										t = nextz();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111111|1000
																																											case 0:
																																											{
																																												t = nextz();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|10000
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|100000
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 178;
																																																break;
																																															}
																																															//11111111|11111111|100001
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 181;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|10001
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|100010
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 185;
																																																break;
																																															}
																																															//11111111|11111111|100011
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 186;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																											//11111111|11111111|1001
																																											case 1:
																																											{
																																												t = nexto();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|10010
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|100100
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 187;
																																																break;
																																															}
																																															//11111111|11111111|
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 189;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|10011
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|100110
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 190;
																																																break;
																																															}
																																															//11111111|11111111|100111
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 196;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																										}

																																										break;
																																									}
																																									//11111111|11111111|101
																																									case 1:
																																									{
																																										t = nexto();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111111|1010
																																											case 0:
																																											{
																																												t = nextz();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|10100
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|101000
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 198;
																																																break;
																																															}
																																															//11111111|11111111|101001
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 228;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|10101
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|101010
																																															case 0:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 232;
																																																break;
																																															}
																																															//11111111|11111111|101011
																																															case 1:
																																															{
																																																chken();
																																																resultBuffer[wi++] = 233;
																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																											//11111111|11111111|1011
																																											case 1:
																																											{
																																												t = nexto();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|10110
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|101100
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1011000
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 1;
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 135;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|101101
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1011010
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 137;
																																																		break;
																																																	}
																																																	//11111111|11111111|1011011
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 138;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|10111
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|101110
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1011100
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 139;
																																																		break;
																																																	}
																																																	//11111111|11111111|1011101
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 140;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|101111
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1011110
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 141;
																																																		break;
																																																	}
																																																	//11111111|11111111|1011111
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 143;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																										}

																																										break;
																																									}
																																								}

																																								break;
																																							}
																																							//11111111|11111111|11
																																							case 1:
																																							{
																																								t = nexto();
																																								if (t == -1) break;
																																								switch ((t & data[i3]) >> i)
																																								{
																																									//11111111|11111111|110
																																									case 0:
																																									{
																																										t = nextz();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111111|1100
																																											case 0:
																																											{
																																												t = nextz();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|11000
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|110000
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1100000
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 147;
																																																		break;
																																																	}
																																																	//11111111|11111111|1100001
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 149;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|110001
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1100010
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 150;
																																																		break;
																																																	}
																																																	//11111111|11111111|1100011
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 151;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|11001
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|110010
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1100100
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 152;
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 155;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|110011
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1100110
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 157;
																																																		break;
																																																	}
																																																	//11111111|11111111|1100111
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 158;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																											//11111111|11111111|1101
																																											case 1:
																																											{
																																												t = nexto();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|11010
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|110100
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1101000
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 165;
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 166;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|110101
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1101010
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 168;
																																																		break;
																																																	}
																																																	//11111111|11111111|1101011
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 174;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|11011
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|110110
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1101100
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 175;
																																																		break;
																																																	}
																																																	//11111111|11111111|11011101
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 180;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|110111
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1101110
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 182;
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 183;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																										}

																																										break;
																																									}
																																									//11111111|11111111|111
																																									case 1:
																																									{
																																										t = nexto();
																																										if (t == -1) break;
																																										switch ((t & data[i3]) >> i)
																																										{
																																											//11111111|11111111|1110
																																											case 0:
																																											{
																																												t = nextz();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|11100
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|111000
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1110000
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 188;
																																																		break;
																																																	}
																																																	//11111111|11111111|1110001
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 191;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|111001
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1110010
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 197;
																																																		break;
																																																	}
																																																	//11111111|11111111|1110011
																																																	case 1:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 231;
																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|11101
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|111010
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1110100
																																																	case 0:
																																																	{
																																																		chken();
																																																		resultBuffer[wi++] = 239;
																																																		break;
																																																	}
																																																	//11111111|11111111|1110101
																																																	case 1:
																																																	{
																																																		t = nexto();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11101010
																																																			case 0:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 9;
																																																				break;
																																																			}
																																																			//11111111|11111111|11101011
																																																			case 1:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 142;
																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|111011
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1110110
																																																	case 0:
																																																	{
																																																		t = nextz();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11101100
																																																			case 0:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 144;
																																																				break;
																																																			}
																																																			//11111111|11111111|
																																																			case 1:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 145;
																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																	//11111111|11111111|1110111
																																																	case 1:
																																																	{
																																																		t = nexto();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11101110
																																																			case 0:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 148;
																																																				break;
																																																			}
																																																			//11111111|11111111|11101111
																																																			case 1:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 159;
																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																											//11111111|11111111|1111
																																											case 1:
																																											{
																																												t = nexto();
																																												if (t == -1) break;
																																												switch ((t & data[i3]) >> i)
																																												{
																																													//11111111|11111111|11110
																																													case 0:
																																													{
																																														t = nextz();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|111100
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1111000
																																																	case 0:
																																																	{
																																																		t = nextz();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11110000
																																																			case 0:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 171;
																																																				break;
																																																			}
																																																			//11111111|11111111|
																																																			case 1:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 206;
																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																	//11111111|11111111|1111001
																																																	case 1:
																																																	{
																																																		t = nexto();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11110010
																																																			case 0:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 215;
																																																				break;
																																																			}
																																																			//11111111|11111111|1111011
																																																			case 1:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 225;
																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|111101
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1111010
																																																	case 0:
																																																	{
																																																		t = nextz();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11110100
																																																			case 0:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 236;
																																																				break;
																																																			}
																																																			//11111111|11111111|11110101
																																																			case 1:
																																																			{
																																																				chken();
																																																				resultBuffer[wi++] = 237;
																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																	//11111111|11111111|1111011
																																																	case 1:
																																																	{
																																																		t = nexto();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11110110
																																																			case 0:
																																																			{
																																																				t = nextz();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11110110|0
																																																					case 0:
																																																					{
																																																						chken();
																																																						resultBuffer[wi++] = 199;
																																																						break;
																																																					}
																																																					//11111111|11111111|
																																																					case 1:
																																																					{
																																																						chken();
																																																						resultBuffer[wi++] = 207;
																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																			//11111111|11111111|11110111
																																																			case 1:
																																																			{
																																																				t = nexto();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11110111|0
																																																					case 0:
																																																					{
																																																						chken();
																																																						resultBuffer[wi++] = 234;
																																																						break;
																																																					}
																																																					//11111111|11111111|111101111|1
																																																					case 1:
																																																					{
																																																						chken();
																																																						resultBuffer[wi++] = 235;
																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																													//11111111|11111111|11111
																																													case 1:
																																													{
																																														t = nexto();
																																														if (t == -1) break;
																																														switch ((t & data[i3]) >> i)
																																														{
																																															//11111111|11111111|111110
																																															case 0:
																																															{
																																																t = nextz();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1111100
																																																	case 0:
																																																	{
																																																		t = nextz();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11111000
																																																			case 0:
																																																			{
																																																				t = nextz();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11111000|0
																																																					case 0:
																																																					{
																																																						t = nextz();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111000|00
																																																							case 0:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 192;
																																																								break;
																																																							}
																																																							//11111111|11111111|11111000|01
																																																							case 1:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 193;
																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																					//11111111|11111111|11111000|1
																																																					case 1:
																																																					{
																																																						t = nexto();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111000|10
																																																							case 0:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 200;
																																																								break;
																																																							}
																																																							//11111111|11111111|11111000|11
																																																							case 1:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 201;
																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																			//11111111|11111111|11111001
																																																			case 1:
																																																			{
																																																				t = nexto();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11111001|0
																																																					case 0:
																																																					{
																																																						t = nextz();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111001|00
																																																							case 0:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 202;
																																																								break;
																																																							}
																																																							//11111111|11111111|11111001|01
																																																							case 1:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 205;
																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																					//11111111|11111111|111111001|1
																																																					case 1:
																																																					{
																																																						t = nexto();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111001|10
																																																							case 0:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 210;
																																																								break;
																																																							}
																																																							//11111111|11111111|11111001|11
																																																							case 1:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 213;
																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																	//11111111|11111111|1111101
																																																	case 1:
																																																	{
																																																		t = nexto();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11111010
																																																			case 0:
																																																			{
																																																				t = nextz();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11111010|0
																																																					case 0:
																																																					{
																																																						t = nextz();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111010|00
																																																							case 0:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 218;
																																																								break;
																																																							}
																																																							//11111111|11111111|11111010|01
																																																							case 1:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 219;
																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																					//11111111|11111111|11111010|1
																																																					case 1:
																																																					{
																																																						t = nexto();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111010|10
																																																							case 0:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 238;
																																																								break;
																																																							}
																																																							//11111111|11111111|11111010|11
																																																							case 1:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 240;
																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																			//11111111|11111111|11111011
																																																			case 1:
																																																			{
																																																				t = nexto();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11111011|0
																																																					case 0:
																																																					{
																																																						t = nextz();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111011|00
																																																							case 0:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 242;
																																																								break;
																																																							}
																																																							//11111111|11111111|11111011|01
																																																							case 1:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 243;
																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																					//11111111|11111111|11111011|1
																																																					case 1:
																																																					{
																																																						t = nexto();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111011|10
																																																							case 0:
																																																							{
																																																								chken();
																																																								resultBuffer[wi++] = 255;
																																																								break;
																																																							}
																																																							//11111111|11111111|11111011|11
																																																							case 1:
																																																							{
																																																								t = nexto();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111011|110
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 203;
																																																										break;
																																																									}
																																																									//11111111|11111111|11111011|111
																																																									case 1:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 204;
																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																															//11111111|11111111|111111
																																															case 1:
																																															{
																																																t = nexto();
																																																if (t == -1) break;
																																																switch ((t & data[i3]) >> i)
																																																{
																																																	//11111111|11111111|1111110
																																																	case 0:
																																																	{
																																																		t = nextz();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11111100
																																																			case 0:
																																																			{
																																																				t = nextz();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11111100|0
																																																					case 0:
																																																					{
																																																						t = nextz();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111100|00
																																																							case 0:
																																																							{
																																																								t = nextz();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111100|000
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 211;
																																																										break;
																																																									}
																																																									//11111111|11111111|111111
																																																									case 1:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 212;
																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																							//11111111|11111111|11111100|01
																																																							case 1:
																																																							{
																																																								t = nexto();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111100|010
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 214;
																																																										break;
																																																									}
																																																									//11111111|11111111|11111100|011
																																																									case 1:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 221;
																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																					//11111111|11111111|11111100|1
																																																					case 1:
																																																					{
																																																						t = nexto();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111100|10
																																																							case 0:
																																																							{
																																																								t = nextz();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111100|100
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 222;
																																																										break;
																																																									}
																																																									//11111111|11111111|11111100|101
																																																									case 1:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 223;
																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																							//11111111|11111111|11111100|11
																																																							case 1:
																																																							{
																																																								t = nexto();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111100|110
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 241;
																																																										break;
																																																									}
																																																									//11111111|11111111|11111100|111
																																																									case 1:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 244;
																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																			//11111111|11111111|11111101
																																																			case 1:
																																																			{
																																																				t = nexto();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11111101|0
																																																					case 0:
																																																					{
																																																						t = nextz();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111101|00
																																																							case 0:
																																																							{
																																																								t = nextz();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111101|000
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 245;
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|001
																																																									case 1:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 246;
																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																							//11111111|11111111|11111101|01
																																																							case 1:
																																																							{
																																																								t = nexto();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111101|010
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 247;
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|011
																																																									case 1:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 248;
																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																					//11111111|11111111|11111101|1
																																																					case 1:
																																																					{
																																																						t = nexto();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111101|10
																																																							case 0:
																																																							{
																																																								t = nextz();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111101|100
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 250;
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|101
																																																									case 1:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 251;
																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																							//11111111|11111111|11111101|11
																																																							case 1:
																																																							{
																																																								t = nexto();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111101|110
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 252;
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|111
																																																									case 1:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 253;
																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																	//11111111|11111111|1111111
																																																	case 1:
																																																	{
																																																		t = nexto();
																																																		if (t == -1) break;
																																																		switch ((t & data[i3]) >> i)
																																																		{
																																																			//11111111|11111111|11111110
																																																			case 0:
																																																			{
																																																				t = nextz();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11111110|0
																																																					case 0:
																																																					{
																																																						t = nextz();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111110|00
																																																							case 0:
																																																							{
																																																								t = nextz();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111110|000
																																																									case 0:
																																																									{
																																																										chken();
																																																										resultBuffer[wi++] = 254;
																																																										break;
																																																									}
																																																									//11111111|11111111|11111110|001
																																																									case 1:
																																																									{
																																																										t = nexto();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111110|0010
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 2;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|0011
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 3;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																							//11111111|11111111|11111110|01
																																																							case 1:
																																																							{
																																																								t = nexto();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111110|010
																																																									case 0:
																																																									{
																																																										t = nextz();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111110|0100
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 4;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|0101
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 5;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																									//11111111|11111111|11111110|011
																																																									case 1:
																																																									{
																																																										t = nexto();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111110|0110
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 6;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|0111
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 7;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																					//11111111|11111111|111111110|1
																																																					case 1:
																																																					{
																																																						t = nexto();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111110|10
																																																							case 0:
																																																							{
																																																								t = nextz();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111110|100
																																																									case 0:
																																																									{
																																																										t = nextz();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111110|1000
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 8;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1001
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 11;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																									//11111111|11111111|11111110|101
																																																									case 1:
																																																									{
																																																										t = nexto();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111110|1010
																																																											//11111111|11111111|11111110|000
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 12;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1011
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 14;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																							//11111111|11111111|11111110|11
																																																							case 1:
																																																							{
																																																								t = nexto();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111110|110
																																																									case 0:
																																																									{
																																																										t = nextz();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111110|1100
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 15;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1101
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 16;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																									//11111111|11111111|11111110|111
																																																									case 1:
																																																									{
																																																										t = nexto();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111110|1110
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 17;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1111
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 18;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																			//11111111|11111111|11111111
																																																			case 1:
																																																			{
																																																				t = nexto();
																																																				if (t == -1) break;
																																																				switch ((t & data[i3]) >> i)
																																																				{
																																																					//11111111|11111111|11111111|0
																																																					case 0:
																																																					{
																																																						t = nextz();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111111|00
																																																							case 0:
																																																							{
																																																								t = nextz();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111111|000
																																																									case 0:
																																																									{
																																																										t = nextz();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111111|0000
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 19;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0001
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 20;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																									//11111111|11111111|11111111|001
																																																									case 1:
																																																									{
																																																										t = nexto();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111111|0010
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 21;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0011
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 23;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																							//11111111|11111111|11111111|01
																																																							case 1:
																																																							{
																																																								t = nexto();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111111|010
																																																									case 0:
																																																									{
																																																										t = nextz();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111111|0100
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 24;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0101
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 25;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																									//11111111|11111111|11111111|011
																																																									case 1:
																																																									{
																																																										t = nexto();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111111|0110
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 26;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0111
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 27;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																					//11111111|11111111|11111111|1
																																																					case 1:
																																																					{
																																																						t = nexto();
																																																						if (t == -1) break;
																																																						switch ((t & data[i3]) >> i)
																																																						{
																																																							//11111111|11111111|11111111|10
																																																							case 0:
																																																							{
																																																								t = nextz();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111111|100
																																																									case 0:
																																																									{
																																																										t = nextz();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111111|1000
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 28;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1001
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 29;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																									//11111111|11111111|11111111|101
																																																									case 1:
																																																									{
																																																										t = nexto();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111111|1010
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 30;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1011
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 31;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																							//11111111|11111111|11111111|11
																																																							case 1:
																																																							{
																																																								t = nexto();
																																																								if (t == -1) break;
																																																								switch ((t & data[i3]) >> i)
																																																								{
																																																									//11111111|11111111|11111111|110
																																																									case 0:
																																																									{
																																																										t = nextz();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111111|1100
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 127;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1101
																																																											case 1:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 220;
																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																									//11111111|11111111|11111111|111
																																																									case 1:
																																																									{
																																																										t = nexto();
																																																										if (t == -1) break;
																																																										switch ((t & data[i3]) >> i)
																																																										{
																																																											//11111111|11111111|11111111|1110
																																																											case 0:
																																																											{
																																																												chken();
																																																												resultBuffer[wi++] = 249;
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1111
																																																											case 1:
																																																											{
																																																												t = nexto();
																																																												if (t == -1) break;
																																																												switch ((t & data[i3]) >> i)
																																																												{
																																																													//11111111|11111111|11111111|11110
																																																													case 0:
																																																													{
																																																														t = nextz();
																																																														if (t == -1) break;
																																																														switch ((t & data[i3]) >> i)
																																																														{
																																																															//11111111|11111111|11111111|111100
																																																															case 0:
																																																															{
																																																																chken();
																																																																resultBuffer[wi++] = 10;
																																																																break;
																																																															}
																																																															//11111111|11111111|11111111|111101
																																																															case 1:
																																																															{
																																																																chken();
																																																																resultBuffer[wi++] = 13;
																																																																break;
																																																															}
																																																														}

																																																														break;
																																																													}
																																																													//11111111|11111111|11111111|11111
																																																													case 1:
																																																													{
																																																														t = nexto();
																																																														if (t == -1) break;
																																																														switch ((t & data[i3]) >> i)
																																																														{
																																																															//11111111|11111111|11111111|111110
																																																															case 0:
																																																															{
																																																																chken();
																																																																resultBuffer[wi++] = 22;
																																																																break;
																																																															}
																																																															//11111111|11111111|11111111|111111
																																																															case 1:
																																																															{
																																																																throw new HuffmanForHPACKHaveEOSTagException();
																																																															}
																																																														}

																																																														break;
																																																													}
																																																												}

																																																												break;
																																																											}
																																																										}

																																																										break;
																																																									}
																																																								}

																																																								break;
																																																							}
																																																						}

																																																						break;
																																																					}
																																																				}

																																																				break;
																																																			}
																																																		}

																																																		break;
																																																	}
																																																}

																																																break;
																																															}
																																														}

																																														break;
																																													}
																																												}

																																												break;
																																											}
																																										}

																																										break;
																																									}
																																								}

																																								break;
																																							}
																																						}

																																						break;
																																					}
																																				}

																																				break;
																																			}
																																		}

																																		break;
																																	}
																																}

																																break;
																															}
																														}

																														break;
																													}
																												}

																												break;
																											}
																										}

																										break;
																									}
																								}

																								break;
																							}
																						}

																						break;
																					}
																				}

																				break;
																			}
																		}

																		break;
																	}
																}

																break;
															}
														}

														break;
													}
												}

												break;
											}
										}

										break;
									}
								}

								break;
							}
						}

						break;
					}
				}

				#endregion

				have_zero = false;
				if (t == -1)
					if (oleight)
						throw new HuffmanForHPACKPaddingInaccuracyException();

				//if (i2 == data.Length - 1) break;
			}
		}
		// catch(HuffmanForHPACKEOF){
		// 	if (oleight)
		// 	{
		// 		throw new HuffmanForHPACKPaddingInaccuracyException();
		// 	}
		// }

		if (have_zero) throw new HuffmanForHPACKPaddingInaccuracyException();

		return wi;
	}


	private ref struct StackallocList
	{
		private readonly Span<byte> _buffer;

		public StackallocList(Span<byte> buffer)
		{
			_buffer = buffer;
		}

		private int _position = 0;
		public int length = 0;


		public void Add(byte value)
		{
			checked
			{
				_buffer[_position++] = value;
				length++;
			}
		}
	}
}