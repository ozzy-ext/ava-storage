namespace AvaStorage.Domain.PictureAddressing;

public class DefaultPicAddrProvider : IPictureAddressProvider
{
    public string ProvideAddress()
    {
        return Path.Combine(PictureAddressConst.DefaultPath, PictureAddressConst.DefaultFilename);
    }
}