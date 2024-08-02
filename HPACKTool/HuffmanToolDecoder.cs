

using System.Diagnostics.CodeAnalysis;

namespace cewno.HPACKTool;

/// <summary>
/// 这个类包含了一套用于解码和编码适用于 HTTP2 的 Huffman 压缩算法的工具
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
	/// 用于与(&) 运算提取位(bit) 数据的数组
	/// </summary>
	//倒着来，简化运算操作
	// ((nextz() & at) >> (7 - i)) 简化为 ((nextz() & at) >> i)
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
	/// 解码使用 HPACK 算法中的 Huffman 算法压缩的字符串并直接返回字符串
	/// </summary>
	/// <param name="data">已编码的数据</param>
	/// <returns>解码后的字符数组</returns>
	/// <exception cref="HuffmanForHPACKHaveEOSTagException">识别到不应出现的EOS标识</exception>
	/// <exception cref="HuffmanForHPACKPaddingInaccuracyException">填充格式不正确</exception>
	/// <returns>解码后的字符串</returns>
	public static string? DecoderToString(byte[] data)
	{
		byte[]? decoder = Decoder(data, 0, data.Length);
		return decoder == null ? null : System.Text.Encoding.ASCII.GetString(decoder);
	}
	/// <summary>
	/// 解码使用 HPACK 算法中的 Huffman 算法压缩的字符串并直接返回字符串
	/// </summary>
	/// <param name="data">已编码的数据</param>
	/// <param name="offset">偏移量</param>
	/// <param name="length">数据长度</param>
	/// <returns>解码后的字符数组</returns>
	/// <exception cref="HuffmanForHPACKHaveEOSTagException">识别到不应出现的EOS标识</exception>
	/// <exception cref="HuffmanForHPACKPaddingInaccuracyException">填充格式不正确</exception>
	/// <returns>解码后的字符串</returns>
	public static string? DecoderToString(byte[] data, int offset, int length)
	{
		byte[]? decoder = Decoder(data, offset, length);
		return decoder == null ? null : System.Text.Encoding.ASCII.GetString(decoder);
	}

	/// <summary>
	/// 解码使用 HPACK 算法中的 Huffman 算法压缩的字符串
	/// </summary>
	/// <param name="data">已编码的数据</param>
	/// <returns>解码后的字符数组</returns>
	/// <exception cref="HuffmanForHPACKHaveEOSTagException">识别到不应出现的EOS标识</exception>
	/// <exception cref="HuffmanForHPACKPaddingInaccuracyException">填充格式不正确</exception>
	public static byte[]? Decoder(byte[] data)
	{
		return Decoder(data, 0, data.Length);
	}
	/// <summary>
	/// 解码使用 HPACK 算法中的 Huffman 算法压缩的字符串
	/// </summary>
	/// <param name="data">已编码的数据</param>
	/// <param name="offset">偏移量</param>
	/// <param name="length">数据长度</param>
	/// <returns>解码后的字符数组</returns>
	/// <exception cref="HuffmanForHPACKHaveEOSTagException">识别到不应出现的EOS标识</exception>
	/// <exception cref="HuffmanForHPACKPaddingInaccuracyException">填充格式不正确</exception>
	public static byte[]? Decoder(byte[] data, int offset, int length)
	{
		if (data.Length == 0) return null;
		//当前处在在的位反过来的索引    真实值 = 7 - i
		int i = 8;
		//当前处在的byte的索引
		int i2 = offset;
		//当前处在的byte
		byte at = data[offset];
		List<byte> bc = new List<byte>();
		//文档要求，末尾用1填充，若违反需要报错，这个变量是用来记录的
		//如果我理解错了，请向我提交issue
		bool have_zero = false;
		//还有数据
		bool h = true;
		//文档要求超出7位的填充要报错,这个变量是用来记录的
		bool oleight = false;
		//用于0的下一位
		byte nextz()
		{
			//标记有0
			have_zero = true;
			//检查是否已到末位
			if (i != 0) return bbbf[--i];
			i = 7;
			//检测是否还有数据
			if (i2 == length + offset - 1)
			{
				throw new HuffmanForHPACKEOF();
			}
			at = data[++i2];
			return bbbf[7];
		}
		//用于1的下一位
		byte nexto()
		{
			if (i != 0) return bbbf[--i];
			i = 7;
			//检测是否还有数据
			if (i2 == length + offset - 1)
			{
				throw new HuffmanForHPACKEOF();
			}
			at = data[++i2];
			return bbbf[7];
		}
		

		try
		{
			while (h)
			{
				
				//下面的内容超长，没耐心的请别展开，不过就算不展开也可能会很卡
				//这些都是我手写的，注释可能会写错，看到了的话可以提issue告诉我
				#region DecoderSwitch

				oleight = false;
				
				switch ((nexto() & at) >> i)
				{
					//0
					case 0:
					{
						switch ((nextz() & at) >> i)
						{
							//00
							case 0:
							{
								switch ((nextz() & at) >> i)
								{
									//000
									case 0:
									{
										switch ((nextz() & at) >> i)
										{
											//0000
											case 0:
											{
												switch ((nextz() & at) >> i)
												{
													//00000
													case 0:
													{
														bc.Add((byte)'0');
														break;
													}
													//00001
													case 1:
													{
														bc.Add((byte)'1');
														break;
													}
												}
												break;
											}
											//0001
											case 1:
											{
												switch ((nexto() & at) >> i)
												{
													//00010
													case 0:
													{
														bc.Add((byte)'2');
														break;
													}
													//00011
													case 1:
													{
														bc.Add((byte)'a');
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
										switch ((nexto() & at) >> i)
										{
											//0010
											case 0:
											{
												switch ((nextz() & at) >> i)
												{
													//00100
													case 0:
													{
														bc.Add((byte)'c');
														break;
													}
													//00101
													case 1:
													{
														bc.Add((byte)'e');
														break;
													}
												}
												break;
											}
											//0011
											case 1:
											{
												switch ((nexto() & at) >> i)
												{
													//00110
													case 0:
													{
														bc.Add((byte)'i');
														break;
													}
													//00111
													case 1:
													{
														bc.Add((byte)'o');
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
								switch ((nexto() & at) >> i)
								{
									//010
									case 0:
									{
										switch ((nextz() & at) >> i)
										{
											//0100
											case 0:
											{
												switch ((nextz() & at) >> i)
												{
													//01000
													case 0:
													{
														bc.Add((byte)'s');
														break;
													}
													//01001
													case 1:
													{
														bc.Add((byte)'t');
														break;
													}
												}
												break;
											}
											//0101
											case 1:
											{
												switch ((nexto() & at) >> i)
												{
													//01010
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//010100
															case 0:
															{
																bc.Add((byte)' ');
																break;
															}
															//0101001
															case 1:
															{
																bc.Add((byte)'%');
																break;
															}
														}
														break;
													}
													//01011
													case 1:
													{
														switch ((nexto() & at) >> i)
														{
															//010110
															case 0:
															{
																bc.Add((byte)'-');
																break;
															}
															//010111
															case 1:
															{
																bc.Add((byte)'.');
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
										switch ((nexto() & at) >> i)
										{
											//0110
											case 0:
											{
												switch ((nextz() & at) >> i)
												{
													//01100
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//011000
															case 0:
															{
																bc.Add((byte)'/');
																break;
															}
															//011001
															case 1:
															{
																bc.Add((byte)'3');
																break;
															}
														}
														break;
													}
													//01101
													case 1:
													{
														switch ((nexto() & at) >> i)
														{
															//011010
															case 0:
															{
																bc.Add((byte)'4');
																break;
															}
															//011011
															case 1:
															{
																bc.Add((byte)'5');
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
												switch ((nexto() & at) >> i)
												{
													//01110
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//011100
															case 0:
															{
																bc.Add((byte)'6');
																break;
															}
															//011101
															case 1:
															{
																bc.Add((byte)'7');
																break;
															}
														}
														break;
													}
													//01111
													case 1:
													{
														switch ((nexto() & at) >> i)
														{
															//011110
															case 0:
															{
																bc.Add((byte)'8');
																break;
															}
															//011111
															case 1:
															{
																bc.Add((byte)'9');
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
						switch ((nexto() & at) >> i)
						{
							//10
							case 0:
							{
								switch ((nextz() & at) >> i)
								{
									//100
									case 0:
									{
										switch ((nextz() & at) >> i)
										{
											//1000
											case 0:
											{
												switch ((nextz() & at) >> i)
												{
													//10000
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//100000
															case 0:
															{
																bc.Add((byte)'=');
																break;
															}
															//100001
															case 1:
															{
																bc.Add((byte)'A');
																break;
															}
														}
														break;
													}
													//10001
													case 1:
													{
														switch ((nexto() & at) >> i)
														{
															//100010
															case 0:
															{
																bc.Add((byte)'_');
																break;
															}
															//100011
															case 1:
															{
																bc.Add((byte)'b');
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
												switch ((nexto() & at) >> i)
												{
													//10010
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//100100
															case 0:
															{
																bc.Add((byte)'d');
																break;
															}
															//100101
															case 1:
															{
																bc.Add((byte)'f');
																break;
															}
														}
														break;
													}
													//10011
													case 1:
													{
														switch ((nexto() & at) >> i)
														{
															//100110
															case 0:
															{
																bc.Add((byte)'g');
																break;
															}
															//100111
															case 1:
															{
																bc.Add((byte)'h');
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
										switch ((nexto() & at) >> i)
										{
											//1010
											case 0:
											{
												switch ((nextz() & at) >> i)
												{
													//10100
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//101000
															case 0:
															{
																bc.Add((byte)'l');
																break;
															}
															//101001
															case 1:
															{
																bc.Add((byte)'m');
																break;
															}
														}
														break;
													}
													//10101
													case 1:
													{
														switch ((nexto() & at) >> i)
														{
															//101010
															case 0:
															{
																bc.Add((byte)'n');
																break;
															}
															//101011
															case 1:
															{
																bc.Add((byte)'p');
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
												switch ((nexto() & at) >> i)
												{
													//10110
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//101100
															case 0:
															{
																bc.Add((byte)'r');
																break;
															}
															//101101
															case 1:
															{
																bc.Add((byte)'u');
																break;
															}
														}
														break;
													}
													//10111
													case 1:
													{
														switch ((nexto() & at) >> i)
														{
															//101110
															case 0:
															{
																switch ((nextz() & at) >> i)
																{
																	//1011100
																	case 0:
																	{
																		bc.Add((byte)':');
																		break;
																	}
																	//1011101
																	case 1:
																	{
																		bc.Add((byte)'B');
																		break;
																	}
																}
																break;
															}
															//101111
															case 1:
															{
																switch ((nexto() & at) >> i)
																{
																	//1011110
																	case 0:
																	{
																		bc.Add((byte)'C');
																		break;
																	}
																	//1011111
																	case 1:
																	{
																		bc.Add((byte)'D');
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
								switch ((nexto() & at) >> i)
								{
									//110
									case 0:
									{
										switch ((nextz() & at) >> i)
										{
											//1100
											case 0:
											{
												switch ((nextz() & at) >> i)
												{
													//11000
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//110000
															case 0:
															{
																switch ((nextz() & at) >> i)
																{
																	//1100000
																	case 0:
																	{
																		bc.Add((byte)'E');
																		break;
																	}
																	//1100001
																	case 1:
																	{
																		bc.Add((byte)'F');
																		break;
																	}
																}
																break;
															}
															//110001
															case 1:
															{
																switch ((nexto() & at) >> i)
																{
																	//1100010
																	case 0:
																	{
																		bc.Add((byte)'G');
																		break;
																	}
																	//1100011
																	case 1:
																	{
																		bc.Add((byte)'H');
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
														switch ((nexto() & at) >> i)
														{
															//110010
															case 0:
															{
																switch ((nextz() & at) >> i)
																{
																	//110010
																	case 0:
																	{
																		bc.Add((byte)'I');
																		break;
																	}
																	//110011
																	case 1:
																	{
																		bc.Add((byte)'J');
																		break;
																	}
																}
																break;
															}
															//110011
															case 1:
															{
																switch ((nexto() & at) >> i)
																{
																	//1100110
																	case 0:
																	{
																		bc.Add((byte)'K');
																		break;
																	}
																	//1100111
																	case 1:
																	{
																		bc.Add((byte)'L');
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
												switch ((nexto() & at) >> i)
												{
													//11010
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//110100
															case 0:
															{
																switch ((nextz() & at) >> i)
																{
																	//1101000
																	case 0:
																	{
																		bc.Add((byte)'M');
																		break;
																	}
																	//1101010
																	case 1:
																	{
																		bc.Add((byte)'N');
																		break;
																	}
																}
																break;
															}
															//110101
															case 1:
															{
																switch ((nexto() & at) >> i)
																{
																	//1101010
																	case 0:
																	{
																		bc.Add((byte)'O');
																		break;
																	}
																	//1101011
																	case 1:
																	{
																		bc.Add((byte)'P');
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
														switch ((nexto() & at) >> i)
														{
															//110110
															case 0:
															{
																switch ((nextz() & at) >> i)
																{
																	//1101100
																	case 0:
																	{
																		bc.Add((byte)'Q');
																		break;
																	}
																	//1101101
																	case 1:
																	{
																		bc.Add((byte)'R');
																		break;
																	}
																}
																break;
															}
															//110111
															case 1:
															{
																switch ((nexto() & at) >> i)
																{
																	//1101110
																	case 0:
																	{
																		bc.Add((byte)'S');
																		break;
																	}
																	//1101111
																	case 1:
																	{
																		bc.Add((byte)'T');
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
										switch ((nexto() & at) >> i)
										{
											//1110
											case 0:
											{
												switch ((nextz() & at) >> i)
												{
													//11100
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//111000
															case 0:
															{
																switch ((nextz() & at) >> i)
																{
																	//1110000
																	case 0:
																	{
																		bc.Add((byte)'U');
																		break;
																	}
																	//1110001
																	case 1:
																	{
																		bc.Add((byte)'V');
																		break;
																	}
																}
																break;
															}
															//111001
															case 1:
															{
																switch ((nexto() & at) >> i)
																{
																	//1110010
																	case 0:
																	{
																		bc.Add((byte)'W');
																		break;
																	}
																	//1110011
																	case 1:
																	{
																		bc.Add((byte)'Y');
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
														switch ((nexto() & at) >> i)
														{
															//111010
															case 0:
															{
																switch ((nextz() & at) >> i)
																{
																	//1110100
																	case 0:
																	{
																		bc.Add((byte)'j');
																		break;
																	}
																	//1110101
																	case 1:
																	{
																		bc.Add((byte)'k');
																		break;
																	}
																}
																break;
															}
															//111011
															case 1:
															{
																switch ((nexto() & at) >> i)
																{
																	//1110110
																	case 0:
																	{
																		bc.Add((byte)'q');
																		break;
																	}
																	//1110111
																	case 1:
																	{
																		bc.Add((byte)'v');
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
												switch ((nexto() & at) >> i)
												{
													//11110
													case 0:
													{
														switch ((nextz() & at) >> i)
														{
															//111100
															case 0:
															{
																switch ((nextz() & at) >> i)
																{
																	//1111010
																	case 0:
																	{
																		bc.Add((byte)'w');
																		break;
																	}
																	//1111011
																	case 1:
																	{
																		bc.Add((byte)'x');
																		break;
																	}
																}
																break;
															}
															//111101
															case 1:
															{
																switch ((nexto() & at) >> i)
																{
																	//1111010
																	case 0:
																	{
																		bc.Add((byte)'y');
																		break;
																	}
																	//1111011
																	case 1:
																	{
																		bc.Add((byte)'z');
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
														switch ((nexto() & at) >> i)
														{
															//111110
															case 0:
															{
																switch ((nextz() & at) >> i)
																{
																	//1111100
																	case 0:
																	{
																		switch ((nextz() & at) >> i)
																		{
																			//11111000
																			case 0:
																			{
																				bc.Add((byte)'&');
																				break;
																			}
																			//11111001
																			case 1:
																			{
																				bc.Add((byte)'*');
																				break;
																			}
																		}
																		break;
																	}
																	//1111101
																	case 1:
																	{
																		switch ((nexto() & at) >> i)
																		{
																			//11111010
																			case 0:
																			{
																				bc.Add((byte)',');
																				break;
																			}
																			//11111011
																			case 1:
																			{
																				bc.Add((byte)';');
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
																switch ((nexto() & at) >> i)
																{
																	//1111110
																	case 0:
																	{
																		switch ((nextz() & at) >> i)
																		{
																			//11111100
																			case 0:
																			{
																				bc.Add((byte)'X');
																				break;
																			}
																			//11111101
																			case 1:
																			{
																				bc.Add((byte)'Z');
																				break;
																			}
																		}
																		break;
																	}
																	//1111111
																	case 1:
																	{
																		switch ((nexto() & at) >> i)
																		{
																			//11111110
																			case 0:
																			{
																				switch ((nextz() & at) >> i)
																				{
																					//11111110|0
																					case 0:
																					{
																						switch ((nextz() & at) >> i)
																						{
																							//11111110|00
																							case 0:
																							{
																								bc.Add((byte)'!');
																								break;
																							}
																							//11111110|01
																							case 1:
																							{
																								bc.Add((byte)'"');
																								break;
																							}
																						}
																						break;
																					}
																					//11111110|1
																					case 1:
																					{
																						switch ((nexto() & at) >> i)
																						{
																							//11111110|10
																							case 0:
																							{
																								bc.Add((byte)'(');
																								break;
																							}
																							//11111110|11
																							case 1:
																							{
																								bc.Add((byte)')');
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
																				switch ((nexto() & at) >> i)
																				{
																					//11111111|0
																					case 0:
																					{
																						switch ((nextz() & at) >> i)
																						{
																							//11111111|00
																							case 0:
																							{
																								bc.Add((byte)'?');
																								break;
																							}
																							//11111111|01
																							case 1:
																							{
																								switch ((nexto() & at) >> i)
																								{
																									//11111111|010
																									case 0:
																									{
																										bc.Add((byte)'\'');
																										break;
																									}
																									//11111111|011
																									case 1:
																									{
																										bc.Add((byte)'+');
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
																						switch ((nexto() & at) >> i)
																						{
																							//11111111|10
																							case 0:
																							{
																								switch ((nextz() & at) >> i)
																								{
																									//11111111|100
																									case 0:
																									{
																										bc.Add((byte)'|');
																										break;
																									}
																									//11111111|101
																									case 1:
																									{
																										switch ((nexto() & at) >> i)
																										{
																											//11111111|1010
																											case 0:
																											{
																												bc.Add((byte)'#');
																												break;
																											}
																											//11111111|1011
																											case 1:
																											{
																												bc.Add((byte)'>');
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
																								switch ((nexto() & at) >> i)
																								{
																									//11111111|110
																									case 0:
																									{
																										switch ((nextz() & at) >> i)
																										{
																											//11111111|1100
																											case 0:
																											{
																												switch ((nextz() & at) >> i)
																												{
																													//11111111|11000
																													case 0:
																													{
																														bc.Add(0);
																														break;
																													}
																													//11111111|11001
																													case 1:
																													{
																														bc.Add((byte)'$');
																														break;
																													}
																												}
																												break;
																											}
																											//11111111|1101
																											case 1:
																											{
																												switch ((nexto() & at) >> i)
																												{
																													//11111111|11010
																													case 0:
																													{
																														bc.Add((byte)'@');
																														break;
																													}
																													//11111111|11011
																													case 1:
																													{
																														bc.Add((byte)'[');
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
																										switch ((nexto() & at) >> i)
																										{
																											//11111111|1110
																											case 0:
																											{
																												switch ((nextz() & at) >> i)
																												{
																													//11111111|11100
																													case 0:
																													{
																														bc.Add((byte)']');
																														break;
																													}
																													//11111111|11101
																													case 1:
																													{
																														bc.Add((byte)'~');
																														break;
																													}
																												}
																												break;
																											}
																											//11111111|1111
																											case 1:
																											{
																												switch ((nexto() & at) >> i)
																												{
																													//11111111|11110
																													case 0:
																													{
																														switch ((nextz() & at) >> i)
																														{
																															//11111111|111100
																															case 0:
																															{
																																bc.Add((byte)'^');
																																break;
																															}
																															//11111111|111101
																															case 1:
																															{
																																bc.Add((byte)'}');
																																break;
																															}
																														}
																														break;
																													}
																													//11111111|11111
																													case 1:
																													{
																														switch ((nexto() & at) >> i)
																														{
																															//11111111|111110
																															case 0:
																															{
																																switch ((nextz() & at) >> i)
																																{
																																	//11111111|1111100
																																	case 0:
																																	{
																																		bc.Add((byte)'<');
																																		break;
																																	}
																																	//11111111|1111101
																																	case 1:
																																	{
																																		bc.Add((byte)'`');
																																		break;
																																	}
																																}
																																break;
																															}
																															//11111111|111111
																															case 1:
																															{
																																switch ((nexto() & at) >> i)
																																{
																																	//11111111|1111110
																																	case 0:
																																	{
																																		bc.Add((byte)'{');
																																		break;
																																	}
																																	//11111111|1111111
																																	case 1:
																																	{
																																		switch ((nexto() & at) >> i)
																																		{
																																			//11111111|11111110|
																																			case 0:
																																			{
																																				switch ((nextz() & at) >> i)
																																				{
																																					//11111111|11111110|0
																																					case 0:
																																					{
																																						switch ((nextz() & at) >> i)
																																						{
																																							//11111111|11111110|00
																																							case 0:
																																							{
																																								switch ((nextz() & at) >> i)
																																								{
																																									//11111111|11111110|000
																																									case 0:
																																									{
																																										bc.Add((byte)'\\');
																																										break;
																																									}
																																									//11111111|11111110|001
																																									case 1:
																																									{
																																										bc.Add(195);
																																										break;
																																									}
																																								}
																																								break;
																																							}
																																							//11111111|11111110|01
																																							case 1:
																																							{
																																								switch ((nexto() & at) >> i)
																																								{
																																									//11111111|11111110|010
																																									case 0:
																																									{
																																										bc.Add(208);
																																										break;
																																									}
																																									//11111111|11111110|011
																																									case 1:
																																									{
																																										switch ((nexto() & at) >> i)
																																										{
																																											//11111111|11111110|0110
																																											case 0:
																																											{
																																												bc.Add(128);
																																												break;
																																											}
																																											//11111111|11111110|0111
																																											case 1:
																																											{
																																												bc.Add(130);
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
																																						switch ((nexto() & at) >> i)
																																						{
																																							//11111111|11111110|10
																																							case 0:
																																							{
																																								switch ((nextz() & at) >> i)
																																								{
																																									//11111111|11111110|100
																																									case 0:
																																									{
																																										switch ((nextz() & at) >> i)
																																										{
																																											//11111111|11111110|1000
																																											case 0:
																																											{
																																												bc.Add(131);
																																												break;
																																											}
																																											//11111111|11111110|1001
																																											case 1:
																																											{
																																												bc.Add(162);
																																												break;
																																											}
																																										}
																																										break;
																																									}
																																									//11111111|11111110|101
																																									case 1:
																																									{
																																										switch ((nexto() & at) >> i)
																																										{
																																											//11111111|111111110|1010
																																											case 0:
																																											{
																																												bc.Add(184);
																																												break;
																																											}
																																											//11111111|11111110|1011
																																											case 1:
																																											{
																																												bc.Add(194);
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
																																								switch ((nexto() & at) >> i)
																																								{
																																									//11111111|11111110|110
																																									case 0:
																																									{
																																										switch ((nextz() & at) >> i)
																																										{
																																											//11111111|11111110|1100
																																											case 0:
																																											{
																																												bc.Add(224);
																																												break;
																																											}
																																											//11111111|11111110|1101
																																											case 1:
																																											{
																																												bc.Add(226);
																																												break;
																																											}
																																										}
																																										break;
																																									}
																																									//11111111|11111110|111
																																									case 1:
																																									{
																																										switch ((nexto() & at) >> i)
																																										{
																																											//11111111|11111110|1110
																																											case 0:
																																											{
																																												switch ((nextz() & at) >> i)
																																												{
																																													//11111111|11111110|11100
																																													case 0:
																																													{
																																														bc.Add(153);
																																														break;
																																													}
																																													//11111111|11111110|11101
																																													case 1:
																																													{
																																														bc.Add(161);
																																														break;
																																													}
																																												}
																																												break;
																																											}
																																											//11111111|11111110|1111
																																											case 1:
																																											{
																																												switch ((nexto() & at) >> i)
																																												{
																																													//11111111|11111110|11110
																																													case 0:
																																													{
																																														bc.Add(167);
																																														break;
																																													}
																																													//11111111|11111110|
																																													case 1:
																																													{
																																														bc.Add(172);
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
																																				switch ((nexto() & at) >> i)
																																				{
																																					//11111111|11111111|0
																																					case 0:
																																					{
																																						switch ((nextz() & at) >> i)
																																						{
																																							//11111111|11111111|00
																																							case 0:
																																							{
																																								switch ((nextz() & at) >> i)
																																								{
																																									//11111111|11111111|000
																																									case 0:
																																									{
																																										switch ((nextz() & at) >> i)
																																										{
																																											//11111111|11111111|0000
																																											case 0:
																																											{
																																												switch ((nextz() & at) >> i)
																																												{
																																													//11111111|11111111|00000
																																													case 0:
																																													{
																																														bc.Add(176);
																																														break;
																																													}
																																													//11111111|11111111|00001
																																													case 1:
																																													{
																																														bc.Add(177);
																																														break;
																																													}
																																												}
																																												break;
																																											}
																																											//11111111|11111111|0001
																																											case 1:
																																											{
																																												switch ((nexto() & at) >> i)
																																												{
																																													//11111111|11111111|00010
																																													case 0:
																																													{
																																														bc.Add(179);
																																														break;
																																													}
																																													//11111111|11111111|
																																													case 1:
																																													{
																																														bc.Add(209);
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
																																										switch ((nexto() & at) >> i)
																																										{
																																											//11111111|11111111|0010
																																											case 0:
																																											{
																																												switch ((nextz() & at) >> i)
																																												{
																																													//11111111|11111111|00100
																																													case 0:
																																													{
																																														bc.Add(216);
																																														break;
																																													}
																																													//11111111|11111111|
																																													case 1:
																																													{
																																														bc.Add(217);
																																														break;
																																													}
																																												}
																																												break;
																																											}
																																											//11111111|11111111|0011
																																											case 1:
																																											{
																																												switch ((nexto() & at) >> i)
																																												{
																																													//11111111|11111111|00110
																																													case 0:
																																													{
																																														bc.Add(227);
																																														break;
																																													}
																																													//11111111|11111111|
																																													case 1:
																																													{
																																														bc.Add(229);
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
																																								switch ((nexto() & at) >> i)
																																								{
																																									//11111111|11111111|010
																																									case 0:
																																									{
																																										switch ((nextz() & at) >> i)
																																										{
																																											//11111111|11111111|0100
																																											case 0:
																																											{
																																												switch ((nextz() & at) >> i)
																																												{
																																													//11111111|11111111|01000
																																													case 0:
																																													{
																																														bc.Add(230);
																																														break;
																																													}
																																													//11111111|11111111|01001
																																													case 1:
																																													{
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|010010
																																															case 0:
																																															{
																																																bc.Add(129);
																																																break;
																																															}
																																															//11111111|11111111|010011
																																															case 1:
																																															{
																																																bc.Add(132);
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
																																												switch ((nexto() & at) >> i)
																																												{
																																													//11111111|11111111|01010
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|010100
																																															case 0:
																																															{
																																																bc.Add(133);
																																																break;
																																															}
																																															//11111111|11111111|010101
																																															case 1:
																																															{
																																																bc.Add(134);
																																																break;
																																															}
																																														}
																																														break;
																																													}
																																													//11111111|11111111|01011
																																													case 1:
																																													{
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|010110
																																															case 0:
																																															{
																																																bc.Add(136);
																																																break;
																																															}
																																															//11111111|11111111|010111
																																															case 1:
																																															{
																																																bc.Add(146);
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
																																										switch ((nexto() & at) >> i)
																																										{
																																											//11111111|11111111|0110
																																											case 0:
																																											{
																																												switch ((nextz() & at) >> i)
																																												{
																																													//11111111|11111111|01100
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|011000
																																															case 0:
																																															{
																																																bc.Add(154);
																																																break;
																																															}
																																															//11111111|11111111|011001
																																															case 1:
																																															{
																																																bc.Add(156);
																																																break;
																																															}
																																														}
																																														break;
																																													}
																																													//11111111|11111111|01101
																																													case 1:
																																													{
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|011010
																																															case 0:
																																															{
																																																bc.Add(160);
																																																break;
																																															}
																																															//11111111|11111111|011011
																																															case 1:
																																															{
																																																bc.Add(163);
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
																																												switch ((nexto() & at) >> i)
																																												{
																																													//11111111|11111111|01110
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|011100
																																															case 0:
																																															{
																																																bc.Add(164);
																																																break;
																																															}
																																															//11111111|11111111|
																																															case 1:
																																															{
																																																bc.Add(169);
																																																break;
																																															}
																																														}
																																														break;
																																													}
																																													//11111111|11111111|01111
																																													case 1:
																																													{
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|011110
																																															case 0:
																																															{
																																																bc.Add(170);
																																																break;
																																															}
																																															//11111111|11111111|011111
																																															case 1:
																																															{
																																																bc.Add(173);
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
																																						switch ((nexto() & at) >> i)
																																						{
																																							//11111111|11111111|10
																																							case 0:
																																							{
																																								switch ((nextz() & at) >> i)
																																								{
																																									//11111111|11111111|100
																																									case 0:
																																									{
																																										switch ((nextz() & at) >> i)
																																										{
																																											//11111111|11111111|1000
																																											case 0:
																																											{
																																												switch ((nextz() & at) >> i)
																																												{
																																													//11111111|11111111|10000
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|100000
																																															case 0:
																																															{
																																																bc.Add(178);
																																																break;
																																															}
																																															//11111111|11111111|100001
																																															case 1:
																																															{
																																																bc.Add(181);
																																																break;
																																															}
																																														}
																																														break;
																																													}
																																													//11111111|11111111|10001
																																													case 1:
																																													{
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|100010
																																															case 0:
																																															{
																																																bc.Add(185);
																																																break;
																																															}
																																															//11111111|11111111|100011
																																															case 1:
																																															{
																																																bc.Add(186);
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
																																												switch ((nexto() & at) >> i)
																																												{
																																													//11111111|11111111|10010
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|100100
																																															case 0:
																																															{
																																																bc.Add(187);
																																																break;
																																															}
																																															//11111111|11111111|
																																															case 1:
																																															{
																																																bc.Add(189);
																																																break;
																																															}
																																														}
																																														break;
																																													}
																																													//11111111|11111111|10011
																																													case 1:
																																													{
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|100110
																																															case 0:
																																															{
																																																bc.Add(190);
																																																break;
																																															}
																																															//11111111|11111111|100111
																																															case 1:
																																															{
																																																bc.Add(196);
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
																																										switch ((nexto() & at) >> i)
																																										{
																																											//11111111|11111111|1010
																																											case 0:
																																											{
																																												switch ((nextz() & at) >> i)
																																												{
																																													//11111111|11111111|10100
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|101000
																																															case 0:
																																															{
																																																bc.Add(198);
																																																break;
																																															}
																																															//11111111|11111111|101001
																																															case 1:
																																															{
																																																bc.Add(228);
																																																break;
																																															}
																																														}
																																														break;
																																													}
																																													//11111111|11111111|10101
																																													case 1:
																																													{
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|101010
																																															case 0:
																																															{
																																																bc.Add(232);
																																																break;
																																															}
																																															//11111111|11111111|101011
																																															case 1:
																																															{
																																																bc.Add(233);
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
																																												switch ((nexto() & at) >> i)
																																												{
																																													//11111111|11111111|10110
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|101100
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1011000
																																																	case 0:
																																																	{
																																																		bc.Add(1);
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		bc.Add(135);
																																																		break;
																																																	}
																																																}
																																																break;
																																															}
																																															//11111111|11111111|101101
																																															case 1:
																																															{
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1011010
																																																	case 0:
																																																	{
																																																		bc.Add(137);
																																																		break;
																																																	}
																																																	//11111111|11111111|1011011
																																																	case 1:
																																																	{
																																																		bc.Add(138);
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
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|101110
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1011100
																																																	case 0:
																																																	{
																																																		bc.Add(139);
																																																		break;
																																																	}
																																																	//11111111|11111111|1011101
																																																	case 1:
																																																	{
																																																		bc.Add(140);
																																																		break;
																																																	}
																																																}
																																																break;
																																															}
																																															//11111111|11111111|101111
																																															case 1:
																																															{
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1011110
																																																	case 0:
																																																	{
																																																		bc.Add(141);
																																																		break;
																																																	}
																																																	//11111111|11111111|1011111
																																																	case 1:
																																																	{
																																																		bc.Add(143);
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
																																								switch ((nexto() & at) >> i)
																																								{
																																									//11111111|11111111|110
																																									case 0:
																																									{
																																										switch ((nextz() & at) >> i)
																																										{
																																											//11111111|11111111|1100
																																											case 0:
																																											{
																																												switch ((nextz() & at) >> i)
																																												{
																																													//11111111|11111111|11000
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|110000
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1100000
																																																	case 0:
																																																	{
																																																		bc.Add(147);
																																																		break;
																																																	}
																																																	//11111111|11111111|1100001
																																																	case 1:
																																																	{
																																																		bc.Add(149);
																																																		break;
																																																	}
																																																}
																																																break;
																																															}
																																															//11111111|11111111|110001
																																															case 1:
																																															{
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1100010
																																																	case 0:
																																																	{
																																																		bc.Add(150);
																																																		break;
																																																	}
																																																	//11111111|11111111|1100011
																																																	case 1:
																																																	{
																																																		bc.Add(151);
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
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|110010
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1100100
																																																	case 0:
																																																	{
																																																		bc.Add(152);
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		bc.Add(155);
																																																		break;
																																																	}
																																																}
																																																break;
																																															}
																																															//11111111|11111111|110011
																																															case 1:
																																															{
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1100110
																																																	case 0:
																																																	{
																																																		bc.Add(157);
																																																		break;
																																																	}
																																																	//11111111|11111111|1100111
																																																	case 1:
																																																	{
																																																		bc.Add(158);
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
																																												switch ((nexto() & at) >> i)
																																												{
																																													//11111111|11111111|11010
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|110100
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1101000
																																																	case 0:
																																																	{
																																																		bc.Add(165);
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		bc.Add(166);
																																																		break;
																																																	}
																																																}
																																																break;
																																															}
																																															//11111111|11111111|110101
																																															case 1:
																																															{
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1101010
																																																	case 0:
																																																	{
																																																		bc.Add(168);
																																																		break;
																																																	}
																																																	//11111111|11111111|1101011
																																																	case 1:
																																																	{
																																																		bc.Add(174);
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
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|110110
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1101100
																																																	case 0:
																																																	{
																																																		bc.Add(175);
																																																		break;
																																																	}
																																																	//11111111|11111111|11011101
																																																	case 1:
																																																	{
																																																		bc.Add(180);
																																																		break;
																																																	}
																																																}
																																																break;
																																															}
																																															//11111111|11111111|110111
																																															case 1:
																																															{
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1101110
																																																	case 0:
																																																	{
																																																		bc.Add(182);
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		bc.Add(183);
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
																																										switch ((nexto() & at) >> i)
																																										{
																																											//11111111|11111111|1110
																																											case 0:
																																											{
																																												switch ((nextz() & at) >> i)
																																												{
																																													//11111111|11111111|11100
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|111000
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1110000
																																																	case 0:
																																																	{
																																																		bc.Add(188);
																																																		break;
																																																	}
																																																	//11111111|11111111|1110001
																																																	case 1:
																																																	{
																																																		bc.Add(191);
																																																		break;
																																																	}
																																																}
																																																break;
																																															}
																																															//11111111|11111111|111001
																																															case 1:
																																															{
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1110010
																																																	case 0:
																																																	{
																																																		bc.Add(197);
																																																		break;
																																																	}
																																																	//11111111|11111111|1110011
																																																	case 1:
																																																	{
																																																		bc.Add(231);
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
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|111010
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1110100
																																																	case 0:
																																																	{
																																																		bc.Add(239);
																																																		break;
																																																	}
																																																	//11111111|11111111|1110101
																																																	case 1:
																																																	{
																																																		switch ((nexto() & at) >> i)
																																																		{
																																																			//11111111|11111111|11101010
																																																			case 0:
																																																			{
																																																				bc.Add(9);
																																																				break;
																																																			}
																																																			//11111111|11111111|11101011
																																																			case 1:
																																																			{
																																																				bc.Add(142);
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
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1110110
																																																	case 0:
																																																	{
																																																		switch ((nextz() & at) >> i)
																																																		{
																																																			//11111111|11111111|11101100
																																																			case 0:
																																																			{
																																																				bc.Add(144);
																																																				break;
																																																			}
																																																			//11111111|11111111|
																																																			case 1:
																																																			{
																																																				bc.Add(145);
																																																				break;
																																																			}
																																																		}
																																																		break;
																																																	}
																																																	//11111111|11111111|1110111
																																																	case 1:
																																																	{
																																																		switch ((nexto() & at) >> i)
																																																		{
																																																			//11111111|11111111|11101110
																																																			case 0:
																																																			{
																																																				bc.Add(148);
																																																				break;
																																																			}
																																																			//11111111|11111111|11101111
																																																			case 1:
																																																			{
																																																				bc.Add(159);
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
																																												switch ((nexto() & at) >> i)
																																												{
																																													//11111111|11111111|11110
																																													case 0:
																																													{
																																														switch ((nextz() & at) >> i)
																																														{
																																															//11111111|11111111|111100
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1111000
																																																	case 0:
																																																	{
																																																		switch ((nextz() & at) >> i)
																																																		{
																																																			//11111111|11111111|11110000
																																																			case 0:
																																																			{
																																																				bc.Add(171);
																																																				break;
																																																			}
																																																			//11111111|11111111|
																																																			case 1:
																																																			{
																																																				bc.Add(206);
																																																				break;
																																																			}
																																																		}
																																																		break;
																																																	}
																																																	//11111111|11111111|1111001
																																																	case 1:
																																																	{
																																																		switch ((nexto() & at) >> i)
																																																		{
																																																			//11111111|11111111|11110010
																																																			case 0:
																																																			{
																																																				bc.Add(215);
																																																				break;
																																																			}
																																																			//11111111|11111111|1111011
																																																			case 1:
																																																			{
																																																				bc.Add(225);
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
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1111010
																																																	case 0:
																																																	{
																																																		switch ((nextz() & at) >> i)
																																																		{
																																																			//11111111|11111111|11110100
																																																			case 0:
																																																			{
																																																				bc.Add(236);
																																																				break;
																																																			}
																																																			//11111111|11111111|11110101
																																																			case 1:
																																																			{
																																																				bc.Add(237);
																																																				break;
																																																			}
																																																		}
																																																		break;
																																																	}
																																																	//11111111|11111111|1111011
																																																	case 1:
																																																	{
																																																		switch ((nexto() & at) >> i)
																																																		{
																																																			//11111111|11111111|11110110
																																																			case 0:
																																																			{
																																																				switch ((nextz() & at) >> i)
																																																				{
																																																					//11111111|11111111|11110110|0
																																																					case 0:
																																																					{
																																																						bc.Add(199);
																																																						break;
																																																					}
																																																					//11111111|11111111|
																																																					case 1:
																																																					{
																																																						bc.Add(207);
																																																						break;
																																																					}
																																																				}
																																																				break;
																																																			}
																																																			//11111111|11111111|11110111
																																																			case 1:
																																																			{
																																																				switch ((nexto() & at) >> i)
																																																				{
																																																					//11111111|11111111|11110111|0
																																																					case 0:
																																																					{
																																																						bc.Add(234);
																																																						break;
																																																					}
																																																					//11111111|11111111|111101111|1
																																																					case 1:
																																																					{
																																																						bc.Add(235);
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
																																														switch ((nexto() & at) >> i)
																																														{
																																															//11111111|11111111|111110
																																															case 0:
																																															{
																																																switch ((nextz() & at) >> i)
																																																{
																																																	//11111111|11111111|1111100
																																																	case 0:
																																																	{
																																																		switch ((nextz() & at) >> i)
																																																		{
																																																			//11111111|11111111|11111000
																																																			case 0:
																																																			{
																																																				switch ((nextz() & at) >> i)
																																																				{
																																																					//11111111|11111111|11111000|0
																																																					case 0:
																																																					{
																																																						switch ((nextz() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111000|00
																																																							case 0:
																																																							{
																																																								bc.Add(192);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111000|01
																																																							case 1:
																																																							{
																																																								bc.Add(193);
																																																								break;
																																																							}
																																																						}
																																																						break;
																																																					}
																																																					//11111111|11111111|11111000|1
																																																					case 1:
																																																					{
																																																						switch ((nexto() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111000|10
																																																							case 0:
																																																							{
																																																								bc.Add(200);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111000|11
																																																							case 1:
																																																							{
																																																								bc.Add(201);
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
																																																				switch ((nexto() & at) >> i)
																																																				{
																																																					//11111111|11111111|11111001|0
																																																					case 0:
																																																					{
																																																						switch ((nextz() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111001|00
																																																							case 0:
																																																							{
																																																								bc.Add(202);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111001|01
																																																							case 1:
																																																							{
																																																								bc.Add(205);
																																																								break;
																																																							}
																																																						}
																																																						break;
																																																					}
																																																					//11111111|11111111|111111001|1
																																																					case 1:
																																																					{
																																																						switch ((nexto() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111001|10
																																																							case 0:
																																																							{
																																																								bc.Add(210);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111001|11
																																																							case 1:
																																																							{
																																																								bc.Add(213);
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
																																																		switch ((nexto() & at) >> i)
																																																		{
																																																			//11111111|11111111|11111010
																																																			case 0:
																																																			{
																																																				switch ((nextz() & at) >> i)
																																																				{
																																																					//11111111|11111111|11111010|0
																																																					case 0:
																																																					{
																																																						switch ((nextz() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111010|00
																																																							case 0:
																																																							{
																																																								bc.Add(218);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111010|01
																																																							case 1:
																																																							{
																																																								bc.Add(219);
																																																								break;
																																																							}
																																																						}	
																																																						break;
																																																					}
																																																					//11111111|11111111|11111010|1
																																																					case 1:
																																																					{
																																																						switch ((nexto() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111010|10
																																																							case 0:
																																																							{
																																																								bc.Add(238);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111010|11
																																																							case 1:
																																																							{
																																																								bc.Add(240);
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
																																																				switch ((nexto() & at) >> i)
																																																				{
																																																					//11111111|11111111|11111011|0
																																																					case 0:
																																																					{
																																																						switch ((nextz() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111011|00
																																																							case 0:
																																																							{
																																																								bc.Add(242);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111011|01
																																																							case 1:
																																																							{
																																																								bc.Add(243);
																																																								break;
																																																							}
																																																						}	
																																																						break;
																																																					}
																																																					//11111111|11111111|11111011|1
																																																					case 1:
																																																					{
																																																						switch ((nexto() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111011|10
																																																							case 0:
																																																							{
																																																								bc.Add(255);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111011|11
																																																							case 1:
																																																							{
																																																								switch ((nexto() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111011|110
																																																									case 0:
																																																									{
																																																										bc.Add(203);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111011|111
																																																									case 1:
																																																									{
																																																										bc.Add(204);
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
																																																switch ((nexto() & at) >> i)
																																																{
																																																	//11111111|11111111|1111110
																																																	case 0:
																																																	{
																																																		switch ((nextz() & at) >> i)
																																																		{
																																																			//11111111|11111111|11111100
																																																			case 0:
																																																			{
																																																				switch ((nextz() & at) >> i)
																																																				{
																																																					//11111111|11111111|11111100|0
																																																					case 0:
																																																					{
																																																						switch ((nextz() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111100|00
																																																							case 0:
																																																							{
																																																								switch ((nextz() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111100|000
																																																									case 0:
																																																									{
																																																										bc.Add(211);
																																																										break;
																																																									}
																																																									//11111111|11111111|111111
																																																									case 1:
																																																									{
																																																										bc.Add(212);
																																																										break;
																																																									}
																																																								}	
																																																								break;
																																																							}
																																																							//11111111|11111111|11111100|01
																																																							case 1:
																																																							{
																																																								switch ((nexto() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111100|010
																																																									case 0:
																																																									{
																																																										bc.Add(214);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111100|011
																																																									case 1:
																																																									{
																																																										bc.Add(221);
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
																																																						switch ((nexto() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111100|10
																																																							case 0:
																																																							{
																																																								switch ((nextz() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111100|100
																																																									case 0:
																																																									{
																																																										bc.Add(222);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111100|101
																																																									case 1:
																																																									{
																																																										bc.Add(223);
																																																										break;
																																																									}
																																																								}	
																																																								break;
																																																							}
																																																							//11111111|11111111|11111100|11
																																																							case 1:
																																																							{
																																																								switch ((nexto() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111100|110
																																																									case 0:
																																																									{
																																																										bc.Add(241);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111100|111
																																																									case 1:
																																																									{
																																																										bc.Add(244);
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
																																																				switch ((nexto() & at) >> i)
																																																				{
																																																					//11111111|11111111|11111101|0
																																																					case 0:
																																																					{
																																																						switch ((nextz() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111101|00
																																																							case 0:
																																																							{
																																																								switch ((nextz() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111101|000
																																																									case 0:
																																																									{
																																																										bc.Add(245);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|001
																																																									case 1:
																																																									{
																																																										bc.Add(246);
																																																										break;
																																																									}
																																																								}	
																																																								break;
																																																							}
																																																							//11111111|11111111|11111101|01
																																																							case 1:
																																																							{
																																																								switch ((nexto() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111101|010
																																																									case 0:
																																																									{
																																																										bc.Add(247);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|011
																																																									case 1:
																																																									{
																																																										bc.Add(248);
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
																																																						switch ((nexto() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111101|10
																																																							case 0:
																																																							{
																																																								switch ((nextz() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111101|100
																																																									case 0:
																																																									{
																																																										bc.Add(250);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|101
																																																									case 1:
																																																									{
																																																										bc.Add(251);
																																																										break;
																																																									}
																																																								}	
																																																								break;
																																																							}
																																																							//11111111|11111111|11111101|11
																																																							case 1:
																																																							{
																																																								switch ((nexto() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111101|110
																																																									case 0:
																																																									{
																																																										bc.Add(252);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|111
																																																									case 1:
																																																									{
																																																										bc.Add(253);
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
																																																		switch ((nexto() & at) >> i)
																																																		{
																																																			//11111111|11111111|11111110
																																																			case 0:
																																																			{
																																																				switch ((nextz() & at) >> i)
																																																				{
																																																					//11111111|11111111|11111110|0
																																																					case 0:
																																																					{
																																																						switch ((nextz() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111110|00
																																																							case 0:
																																																							{
																																																								switch ((nextz() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111110|000
																																																									case 0:
																																																									{
																																																										bc.Add(254);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111110|001
																																																									case 1:
																																																									{
																																																										switch ((nexto() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111110|0010
																																																											case 0:
																																																											{
																																																												bc.Add(2);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|0011
																																																											case 1:
																																																											{
																																																												bc.Add(3);
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
																																																								switch ((nexto() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111110|010
																																																									case 0:
																																																									{
																																																										switch ((nextz() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111110|0100
																																																											case 0:
																																																											{
																																																												bc.Add(4);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|0101
																																																											case 1:
																																																											{
																																																												bc.Add(5);
																																																												break;
																																																											}
																																																										}
																																																										break;
																																																									}
																																																									//11111111|11111111|11111110|011
																																																									case 1:
																																																									{
																																																										switch ((nexto() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111110|0110
																																																											case 0:
																																																											{
																																																												bc.Add(6);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|0111
																																																											case 1:
																																																											{
																																																												bc.Add(7);
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
																																																						switch ((nexto() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111110|10
																																																							case 0:
																																																							{
																																																								switch ((nextz() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111110|100
																																																									case 0:
																																																									{
																																																										switch ((nextz() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111110|1000
																																																											case 0:
																																																											{
																																																												bc.Add(8);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1001
																																																											case 1:
																																																											{
																																																												bc.Add(11);
																																																												break;
																																																											}
																																																										}
																																																										break;
																																																									}
																																																									//11111111|11111111|11111110|101
																																																									case 1:
																																																									{
																																																										switch ((nexto() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111110|1010
																																																											//11111111|11111111|11111110|000
																																																											case 0:
																																																											{
																																																												bc.Add(12);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1011
																																																											case 1:
																																																											{
																																																												bc.Add(14);
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
																																																								switch ((nexto() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111110|110
																																																									case 0:
																																																									{
																																																										switch ((nextz() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111110|1100
																																																											case 0:
																																																											{
																																																												bc.Add(15);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1101
																																																											case 1:
																																																											{
																																																												bc.Add(16);
																																																												break;
																																																											}
																																																										}
																																																										break;
																																																									}
																																																									//11111111|11111111|11111110|111
																																																									case 1:
																																																									{
																																																										switch ((nexto() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111110|1110
																																																											case 0:
																																																											{
																																																												bc.Add(17);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1111
																																																											case 1:
																																																											{
																																																												bc.Add(18);
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
																																																				switch ((nexto() & at) >> i)
																																																				{
																																																					//11111111|11111111|11111111|0
																																																					case 0:
																																																					{
																																																						switch ((nextz() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111111|00
																																																							case 0:
																																																							{
																																																								switch ((nextz() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111111|000
																																																									case 0:
																																																									{
																																																										switch ((nextz() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111111|0000
																																																											case 0:
																																																											{
																																																												bc.Add(19);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0001
																																																											case 1:
																																																											{
																																																												bc.Add(20);
																																																												break;
																																																											}
																																																										}
																																																										break;
																																																									}
																																																									//11111111|11111111|11111111|001
																																																									case 1:
																																																									{
																																																										switch ((nexto() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111111|0010
																																																											case 0:
																																																											{
																																																												bc.Add(21);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0011
																																																											case 1:
																																																											{
																																																												bc.Add(23);
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
																																																								switch ((nexto() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111111|010
																																																									case 0:
																																																									{
																																																										switch ((nextz() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111111|0100
																																																											case 0:
																																																											{
																																																												bc.Add(24);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0101
																																																											case 1:
																																																											{
																																																												bc.Add(25);
																																																												break;
																																																											}
																																																										}
																																																										break;
																																																									}
																																																									//11111111|11111111|11111111|011
																																																									case 1:
																																																									{
																																																										switch ((nexto() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111111|0110
																																																											case 0:
																																																											{
																																																												bc.Add(26);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0111
																																																											case 1:
																																																											{
																																																												bc.Add(27);
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
																																																						switch ((nexto() & at) >> i)
																																																						{
																																																							//11111111|11111111|11111111|10
																																																							case 0:
																																																							{
																																																								switch ((nextz() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111111|100
																																																									case 0:
																																																									{
																																																										switch ((nextz() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111111|1000
																																																											case 0:
																																																											{
																																																												bc.Add(28);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1001
																																																											case 1:
																																																											{
																																																												bc.Add(29);
																																																												break;
																																																											}
																																																										}
																																																										break;
																																																									}
																																																									//11111111|11111111|11111111|101
																																																									case 1:
																																																									{
																																																										switch ((nexto() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111111|1010
																																																											case 0:
																																																											{
																																																												bc.Add(30);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1011
																																																											case 1:
																																																											{
																																																												bc.Add(31);
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
																																																								switch ((nexto() & at) >> i)
																																																								{
																																																									//11111111|11111111|11111111|110
																																																									case 0:
																																																									{
																																																										switch ((nextz() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111111|1100
																																																											case 0:
																																																											{
																																																												bc.Add(127);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1101
																																																											case 1:
																																																											{
																																																												bc.Add(220);
																																																												break;
																																																											}
																																																										}
																																																										break;
																																																									}
																																																									//11111111|11111111|11111111|111
																																																									case 1:
																																																									{
																																																										switch ((nexto() & at) >> i)
																																																										{
																																																											//11111111|11111111|11111111|1110
																																																											case 0:
																																																											{
																																																												bc.Add(249);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1111
																																																											case 1:
																																																											{
																																																												switch ((nexto() & at) >> i)
																																																												{
																																																													//11111111|11111111|11111111|11110
																																																													case 0:
																																																													{
																																																														switch ((nextz() & at) >> i)
																																																														{
																																																															//11111111|11111111|11111111|111100
																																																															case 0:
																																																															{
																																																																bc.Add(10);
																																																																break;
																																																															}
																																																															//11111111|11111111|11111111|111101
																																																															case 1:
																																																															{
																																																																bc.Add(13);
																																																																break;
																																																															}
																																																														}
																																																														break;
																																																													}
																																																													//11111111|11111111|11111111|11111
																																																													case 1:
																																																													{
																																																														switch ((nexto() & at) >> i)
																																																														{
																																																															//11111111|11111111|11111111|111110
																																																															case 0:
																																																															{
																																																																bc.Add(22);
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
				if (i2 == data.Length - 1 && i == 0) break;
			}
		}
		catch(HuffmanForHPACKEOF){
			if (oleight)
			{
				throw new HuffmanForHPACKPaddingInaccuracyException();
			}
		}

		if (have_zero)
		{
			throw new HuffmanForHPACKPaddingInaccuracyException();
		}

		return bc.ToArray();
	}
	
}