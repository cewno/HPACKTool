namespace cewno.HPACKTool;

/// <summary>
/// 表示一个异步io
/// </summary>
public interface IAsyncIO
{
	/// <summary>
	/// 读取一个字节
	/// </summary>
	/// <returns></returns>
	public byte ReadOneByte();
	/// <summary>
	/// 读取一个字节但不移动至下一位
	/// </summary>
	/// <returns></returns>
	public byte ReadOneByteNoNext();
	/// <summary>
	/// 将数据读取到缓冲区
	/// </summary>
	/// <param name="buffer">目标缓冲区</param>
	/// <param name="offset">偏移量</param>
	/// <param name="length">读取长度</param>
	/// <returns>读取的长度</returns>
	public int Read(byte[] buffer,int offset,int length);
	/// <summary>
	/// 写入一个字节到流里
	/// </summary>
	/// <param name="data">要写入的数据</param>
	public void WriteByte(byte data);
	/// <summary>
	/// 将缓冲区里的数据写入到流里
	/// </summary>
	/// <param name="buffer">缓冲区</param>
	/// <param name="offset">偏移量</param>
	/// <param name="length">写入长度</param>
	public void Write(byte[] buffer,int offset,int length);

}