namespace AvaStorage.Domain.PictureAddressing;

public class DefaultPicWithSizeAddrProvider : IPictureAddressProvider
{
    private readonly int _size;

    public DefaultPicWithSizeAddrProvider(int size)
    {
        _size = size;
    }

    public string ProvideAddress()
    {
        return Path.Combine(PictureAddressConst.DefaultPath, _size + ".png");
    }
}