

namespace HPACKTool;


public class HuffmanTool
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
	/// 用于与（&）运算提取位（bit）数据的数组
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
	/// 解码HPACK算法中的Huffman算法压缩的字符串
	/// </summary>
	/// <param name="data">已编码的数据</param>
	/// <returns>解码后的字符数组</returns>
	/// <exception cref="HuffmanForHPACKHaveEOSTagException">识别到不应出现的EOS标识</exception>
	/// <exception cref="HuffmanForHPACKPaddingInaccuracyException">填充格式不正确</exception>
	public static char[]? Decoder(byte[] data)
	{
		if (data.Length == 0) return null;
		//当前处在在的位反过来的索引    真实值 = 7 - i
		int i = 8;
		//当前处在的byte的索引
		int i2 = 0;
		//当前处在的byet
		byte at = data[0];
		List<char> bc = new List<char>();
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
			if (i2 == data.Length - 1)
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
			if (i2 == data.Length - 1)
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
														bc.Add('0');
														break;
													}
													//00001
													case 1:
													{
														bc.Add('1');
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
														bc.Add('2');
														break;
													}
													//00011
													case 1:
													{
														bc.Add('a');
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
														bc.Add('c');
														break;
													}
													//00101
													case 1:
													{
														bc.Add('e');
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
														bc.Add('i');
														break;
													}
													//00111
													case 1:
													{
														bc.Add('o');
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
														bc.Add('s');
														break;
													}
													//01001
													case 1:
													{
														bc.Add('t');
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
																bc.Add(' ');
																break;
															}
															//0101001
															case 1:
															{
																bc.Add('%');
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
																bc.Add('-');
																break;
															}
															//010111
															case 1:
															{
																bc.Add('.');
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
																bc.Add('/');
																break;
															}
															//011001
															case 1:
															{
																bc.Add('3');
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
																bc.Add('4');
																break;
															}
															//011011
															case 1:
															{
																bc.Add('5');
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
																bc.Add('6');
																break;
															}
															//011101
															case 1:
															{
																bc.Add('7');
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
																bc.Add('8');
																break;
															}
															//011111
															case 1:
															{
																bc.Add('9');
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
																bc.Add('=');
																break;
															}
															//100001
															case 1:
															{
																bc.Add('A');
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
																bc.Add('_');
																break;
															}
															//100011
															case 1:
															{
																bc.Add('b');
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
																bc.Add('d');
																break;
															}
															//100101
															case 1:
															{
																bc.Add('f');
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
																bc.Add('g');
																break;
															}
															//100111
															case 1:
															{
																bc.Add('h');
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
																bc.Add('l');
																break;
															}
															//101001
															case 1:
															{
																bc.Add('m');
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
																bc.Add('n');
																break;
															}
															//101011
															case 1:
															{
																bc.Add('p');
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
																bc.Add('r');
																break;
															}
															//101101
															case 1:
															{
																bc.Add('u');
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
																		bc.Add(':');
																		break;
																	}
																	//1011101
																	case 1:
																	{
																		bc.Add('B');
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
																		bc.Add('C');
																		break;
																	}
																	//1011111
																	case 1:
																	{
																		bc.Add('D');
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
																		bc.Add('E');
																		break;
																	}
																	//1100001
																	case 1:
																	{
																		bc.Add('F');
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
																		bc.Add('G');
																		break;
																	}
																	//1100011
																	case 1:
																	{
																		bc.Add('H');
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
																		bc.Add('I');
																		break;
																	}
																	//110011
																	case 1:
																	{
																		bc.Add('J');
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
																		bc.Add('K');
																		break;
																	}
																	//1100111
																	case 1:
																	{
																		bc.Add('L');
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
																		bc.Add('M');
																		break;
																	}
																	//1101010
																	case 1:
																	{
																		bc.Add('N');
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
																		bc.Add('O');
																		break;
																	}
																	//1101011
																	case 1:
																	{
																		bc.Add('P');
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
																		bc.Add('Q');
																		break;
																	}
																	//1101101
																	case 1:
																	{
																		bc.Add('R');
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
																		bc.Add('S');
																		break;
																	}
																	//1101111
																	case 1:
																	{
																		bc.Add('T');
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
																		bc.Add('U');
																		break;
																	}
																	//1110001
																	case 1:
																	{
																		bc.Add('V');
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
																		bc.Add('W');
																		break;
																	}
																	//1110011
																	case 1:
																	{
																		bc.Add('Y');
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
																		bc.Add('j');
																		break;
																	}
																	//1110101
																	case 1:
																	{
																		bc.Add('k');
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
																		bc.Add('q');
																		break;
																	}
																	//1110111
																	case 1:
																	{
																		bc.Add('v');
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
																		bc.Add('w');
																		break;
																	}
																	//1111011
																	case 1:
																	{
																		bc.Add('x');
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
																		bc.Add('y');
																		break;
																	}
																	//1111011
																	case 1:
																	{
																		bc.Add('z');
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
																				bc.Add('&');
																				break;
																			}
																			//11111001
																			case 1:
																			{
																				bc.Add('*');
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
																				bc.Add(',');
																				break;
																			}
																			//11111011
																			case 1:
																			{
																				bc.Add(';');
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
																				bc.Add('X');
																				break;
																			}
																			//11111101
																			case 1:
																			{
																				bc.Add('Z');
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
																								bc.Add('!');
																								break;
																							}
																							//11111110|01
																							case 1:
																							{
																								bc.Add('"');
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
																								bc.Add('(');
																								break;
																							}
																							//11111110|11
																							case 1:
																							{
																								bc.Add(')');
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
																								bc.Add('?');
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
																										bc.Add('\'');
																										break;
																									}
																									//11111111|011
																									case 1:
																									{
																										bc.Add('+');
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
																										bc.Add('|');
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
																												bc.Add('#');
																												break;
																											}
																											//11111111|1011
																											case 1:
																											{
																												bc.Add('>');
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
																														bc.Add((char)0);
																														break;
																													}
																													//11111111|11001
																													case 1:
																													{
																														bc.Add('$');
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
																														bc.Add('@');
																														break;
																													}
																													//11111111|11011
																													case 1:
																													{
																														bc.Add('[');
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
																														bc.Add(']');
																														break;
																													}
																													//11111111|11101
																													case 1:
																													{
																														bc.Add('~');
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
																																bc.Add('^');
																																break;
																															}
																															//11111111|111101
																															case 1:
																															{
																																bc.Add('}');
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
																																		bc.Add('<');
																																		break;
																																	}
																																	//11111111|1111101
																																	case 1:
																																	{
																																		bc.Add('`');
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
																																		bc.Add('{');
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
																																										bc.Add('\\');
																																										break;
																																									}
																																									//11111111|11111110|001
																																									case 1:
																																									{
																																										bc.Add((char)195);
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
																																										bc.Add((char)208);
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
																																												bc.Add((char)128);
																																												break;
																																											}
																																											//11111111|11111110|0111
																																											case 1:
																																											{
																																												bc.Add((char)130);
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
																																												bc.Add((char)131);
																																												break;
																																											}
																																											//11111111|11111110|1001
																																											case 1:
																																											{
																																												bc.Add((char)162);
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
																																												bc.Add((char)184);
																																												break;
																																											}
																																											//11111111|11111110|1011
																																											case 1:
																																											{
																																												bc.Add((char)194);
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
																																												bc.Add((char)224);
																																												break;
																																											}
																																											//11111111|11111110|1101
																																											case 1:
																																											{
																																												bc.Add((char)226);
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
																																														bc.Add((char)153);
																																														break;
																																													}
																																													//11111111|11111110|11101
																																													case 1:
																																													{
																																														bc.Add((char)161);
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
																																														bc.Add((char)167);
																																														break;
																																													}
																																													//11111111|11111110|
																																													case 1:
																																													{
																																														bc.Add((char)172);
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
																																														bc.Add((char)176);
																																														break;
																																													}
																																													//11111111|11111111|00001
																																													case 1:
																																													{
																																														bc.Add((char)177);
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
																																														bc.Add((char)179);
																																														break;
																																													}
																																													//11111111|11111111|
																																													case 1:
																																													{
																																														bc.Add((char)209);
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
																																														bc.Add((char)216);
																																														break;
																																													}
																																													//11111111|11111111|
																																													case 1:
																																													{
																																														bc.Add((char)217);
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
																																														bc.Add((char)227);
																																														break;
																																													}
																																													//11111111|11111111|
																																													case 1:
																																													{
																																														bc.Add((char)229);
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
																																														bc.Add((char)230);
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
																																																bc.Add((char)129);
																																																break;
																																															}
																																															//11111111|11111111|010011
																																															case 1:
																																															{
																																																bc.Add((char)132);
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
																																																bc.Add((char)133);
																																																break;
																																															}
																																															//11111111|11111111|010101
																																															case 1:
																																															{
																																																bc.Add((char)134);
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
																																																bc.Add((char)136);
																																																break;
																																															}
																																															//11111111|11111111|010111
																																															case 1:
																																															{
																																																bc.Add((char)146);
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
																																																bc.Add((char)154);
																																																break;
																																															}
																																															//11111111|11111111|011001
																																															case 1:
																																															{
																																																bc.Add((char)156);
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
																																																bc.Add((char)160);
																																																break;
																																															}
																																															//11111111|11111111|011011
																																															case 1:
																																															{
																																																bc.Add((char)163);
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
																																																bc.Add((char)164);
																																																break;
																																															}
																																															//11111111|11111111|
																																															case 1:
																																															{
																																																bc.Add((char)169);
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
																																																bc.Add((char)170);
																																																break;
																																															}
																																															//11111111|11111111|011111
																																															case 1:
																																															{
																																																bc.Add((char)173);
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
																																																bc.Add((char)178);
																																																break;
																																															}
																																															//11111111|11111111|100001
																																															case 1:
																																															{
																																																bc.Add((char)181);
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
																																																bc.Add((char)185);
																																																break;
																																															}
																																															//11111111|11111111|100011
																																															case 1:
																																															{
																																																bc.Add((char)186);
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
																																																bc.Add((char)187);
																																																break;
																																															}
																																															//11111111|11111111|
																																															case 1:
																																															{
																																																bc.Add((char)189);
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
																																																bc.Add((char)190);
																																																break;
																																															}
																																															//11111111|11111111|100111
																																															case 1:
																																															{
																																																bc.Add((char)196);
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
																																																bc.Add((char)198);
																																																break;
																																															}
																																															//11111111|11111111|101001
																																															case 1:
																																															{
																																																bc.Add((char)228);
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
																																																bc.Add((char)232);
																																																break;
																																															}
																																															//11111111|11111111|101011
																																															case 1:
																																															{
																																																bc.Add((char)233);
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
																																																		bc.Add((char)1);
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		bc.Add((char)135);
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
																																																		bc.Add((char)137);
																																																		break;
																																																	}
																																																	//11111111|11111111|1011011
																																																	case 1:
																																																	{
																																																		bc.Add((char)138);
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
																																																		bc.Add((char)139);
																																																		break;
																																																	}
																																																	//11111111|11111111|1011101
																																																	case 1:
																																																	{
																																																		bc.Add((char)140);
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
																																																		bc.Add((char)141);
																																																		break;
																																																	}
																																																	//11111111|11111111|1011111
																																																	case 1:
																																																	{
																																																		bc.Add((char)143);
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
																																																		bc.Add((char)147);
																																																		break;
																																																	}
																																																	//11111111|11111111|1100001
																																																	case 1:
																																																	{
																																																		bc.Add((char)149);
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
																																																		bc.Add((char)150);
																																																		break;
																																																	}
																																																	//11111111|11111111|1100011
																																																	case 1:
																																																	{
																																																		bc.Add((char)151);
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
																																																		bc.Add((char)152);
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		bc.Add((char)155);
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
																																																		bc.Add((char)157);
																																																		break;
																																																	}
																																																	//11111111|11111111|1100111
																																																	case 1:
																																																	{
																																																		bc.Add((char)158);
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
																																																		bc.Add((char)165);
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		bc.Add((char)166);
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
																																																		bc.Add((char)168);
																																																		break;
																																																	}
																																																	//11111111|11111111|1101011
																																																	case 1:
																																																	{
																																																		bc.Add((char)174);
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
																																																		bc.Add((char)175);
																																																		break;
																																																	}
																																																	//11111111|11111111|11011101
																																																	case 1:
																																																	{
																																																		bc.Add((char)180);
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
																																																		bc.Add((char)182);
																																																		break;
																																																	}
																																																	//11111111|11111111|
																																																	case 1:
																																																	{
																																																		bc.Add((char)183);
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
																																																		bc.Add((char)188);
																																																		break;
																																																	}
																																																	//11111111|11111111|1110001
																																																	case 1:
																																																	{
																																																		bc.Add((char)191);
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
																																																		bc.Add((char)197);
																																																		break;
																																																	}
																																																	//11111111|11111111|1110011
																																																	case 1:
																																																	{
																																																		bc.Add((char)231);
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
																																																		bc.Add((char)239);
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
																																																				bc.Add((char)9);
																																																				break;
																																																			}
																																																			//11111111|11111111|11101011
																																																			case 1:
																																																			{
																																																				bc.Add((char)142);
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
																																																				bc.Add((char)144);
																																																				break;
																																																			}
																																																			//11111111|11111111|
																																																			case 1:
																																																			{
																																																				bc.Add((char)145);
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
																																																				bc.Add((char)148);
																																																				break;
																																																			}
																																																			//11111111|11111111|11101111
																																																			case 1:
																																																			{
																																																				bc.Add((char)159);
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
																																																				bc.Add((char)171);
																																																				break;
																																																			}
																																																			//11111111|11111111|
																																																			case 1:
																																																			{
																																																				bc.Add((char)206);
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
																																																				bc.Add((char)215);
																																																				break;
																																																			}
																																																			//11111111|11111111|1111011
																																																			case 1:
																																																			{
																																																				bc.Add((char)225);
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
																																																				bc.Add((char)236);
																																																				break;
																																																			}
																																																			//11111111|11111111|11110101
																																																			case 1:
																																																			{
																																																				bc.Add((char)237);
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
																																																						bc.Add((char)199);
																																																						break;
																																																					}
																																																					//11111111|11111111|
																																																					case 1:
																																																					{
																																																						bc.Add((char)207);
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
																																																						bc.Add((char)234);
																																																						break;
																																																					}
																																																					//11111111|11111111|111101111|1
																																																					case 1:
																																																					{
																																																						bc.Add((char)235);
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
																																																								bc.Add((char)192);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111000|01
																																																							case 1:
																																																							{
																																																								bc.Add((char)193);
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
																																																								bc.Add((char)200);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111000|11
																																																							case 1:
																																																							{
																																																								bc.Add((char)201);
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
																																																								bc.Add((char)202);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111001|01
																																																							case 1:
																																																							{
																																																								bc.Add((char)205);
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
																																																								bc.Add((char)210);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111001|11
																																																							case 1:
																																																							{
																																																								bc.Add((char)213);
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
																																																								bc.Add((char)218);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111010|01
																																																							case 1:
																																																							{
																																																								bc.Add((char)219);
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
																																																								bc.Add((char)238);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111010|11
																																																							case 1:
																																																							{
																																																								bc.Add((char)240);
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
																																																								bc.Add((char)242);
																																																								break;
																																																							}
																																																							//11111111|11111111|11111011|01
																																																							case 1:
																																																							{
																																																								bc.Add((char)243);
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
																																																								bc.Add((char)255);
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
																																																										bc.Add((char)203);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111011|111
																																																									case 1:
																																																									{
																																																										bc.Add((char)204);
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
																																																										bc.Add((char)211);
																																																										break;
																																																									}
																																																									//11111111|11111111|111111
																																																									case 1:
																																																									{
																																																										bc.Add((char)212);
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
																																																										bc.Add((char)214);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111100|011
																																																									case 1:
																																																									{
																																																										bc.Add((char)221);
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
																																																										bc.Add((char)222);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111100|101
																																																									case 1:
																																																									{
																																																										bc.Add((char)223);
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
																																																										bc.Add((char)241);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111100|111
																																																									case 1:
																																																									{
																																																										bc.Add((char)244);
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
																																																										bc.Add((char)245);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|001
																																																									case 1:
																																																									{
																																																										bc.Add((char)246);
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
																																																										bc.Add((char)247);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|011
																																																									case 1:
																																																									{
																																																										bc.Add((char)248);
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
																																																										bc.Add((char)250);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|101
																																																									case 1:
																																																									{
																																																										bc.Add((char)251);
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
																																																										bc.Add((char)252);
																																																										break;
																																																									}
																																																									//11111111|11111111|11111101|111
																																																									case 1:
																																																									{
																																																										bc.Add((char)253);
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
																																																										bc.Add((char)254);
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
																																																												bc.Add((char)2);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|0011
																																																											case 1:
																																																											{
																																																												bc.Add((char)3);
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
																																																												bc.Add((char)4);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|0101
																																																											case 1:
																																																											{
																																																												bc.Add((char)5);
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
																																																												bc.Add((char)6);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|0111
																																																											case 1:
																																																											{
																																																												bc.Add((char)7);
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
																																																												bc.Add((char)8);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1001
																																																											case 1:
																																																											{
																																																												bc.Add((char)11);
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
																																																											case 0:
																																																											{
																																																												bc.Add((char)12);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1011
																																																											case 1:
																																																											{
																																																												bc.Add((char)14);
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
																																																												bc.Add((char)15);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1101
																																																											case 1:
																																																											{
																																																												bc.Add((char)16);
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
																																																												bc.Add((char)17);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111110|1111
																																																											case 1:
																																																											{
																																																												bc.Add((char)18);
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
																																																												bc.Add((char)19);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0001
																																																											case 1:
																																																											{
																																																												bc.Add((char)20);
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
																																																												bc.Add((char)21);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0011
																																																											case 1:
																																																											{
																																																												bc.Add((char)23);
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
																																																												bc.Add((char)24);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0101
																																																											case 1:
																																																											{
																																																												bc.Add((char)25);
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
																																																												bc.Add((char)26);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|0111
																																																											case 1:
																																																											{
																																																												bc.Add((char)27);
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
																																																												bc.Add((char)28);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1001
																																																											case 1:
																																																											{
																																																												bc.Add((char)29);
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
																																																												bc.Add((char)30);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1011
																																																											case 1:
																																																											{
																																																												bc.Add((char)31);
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
																																																												bc.Add((char)127);
																																																												break;
																																																											}
																																																											//11111111|11111111|11111111|1101
																																																											case 1:
																																																											{
																																																												bc.Add((char)220);
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
																																																												bc.Add((char)249);
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
																																																																bc.Add((char)10);
																																																																break;
																																																															}
																																																															//11111111|11111111|11111111|111101
																																																															case 1:
																																																															{
																																																																bc.Add((char)13);
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
																																																																bc.Add((char)22);
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
	

	// public static byte[] Encoder(char[] data)
	// {
	// 	
	// }
}