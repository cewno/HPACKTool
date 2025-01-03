namespace cewno.HPACKTool;

/// <summary>
///     适用于HPACK压缩算法的Huffman压缩算法在解码时出现错误
/// </summary>
public class HuffmanForHPACKDecodingException : Exception
{
}

/// <summary>
///     适用于HPACK压缩算法的Huffman压缩算法的数据已结束
/// </summary>
public class HuffmanForHPACKEOF : HuffmanForHPACKDecodingException
{
}

/// <summary>
///     适用于HPACK压缩算法的Huffman压缩算法的数据有EOS标记
/// </summary>
public class HuffmanForHPACKHaveEOSTagException : HuffmanForHPACKDecodingException
{
}

/// <summary>
///     适用于HPACK压缩算法的Huffman压缩算法的数据填充格式错误
/// </summary>
public class HuffmanForHPACKPaddingInaccuracyException : HuffmanForHPACKDecodingException
{
}