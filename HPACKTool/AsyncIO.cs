namespace HPACKTool;

public interface AsyncIO
{
	public byte ReadOneByte();
	public byte Read(byte[] buffer,int offset,int length);
	public void writeOneByte(byte d);
	public void write(byte[] buffer,int offset,int length);

}