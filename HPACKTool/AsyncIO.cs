namespace HPACKTool;

public interface AsyncIO
{
	public byte ReadOneByte();
	public int Read(byte[] buffer,int offset,int length);
	public void WriteByte(byte d);
	public void Write(byte[] buffer,int offset,int length);

}