using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.PictureAddressing;

public class OriginalPersonalPicAddrProvider : IPictureAddressProvider
{
    private readonly AvatarId _avaId;

    public OriginalPersonalPicAddrProvider(AvatarId avaId)
    {
        _avaId = avaId;
    }
    public string ProvideAddress()
    {
        return Path.Combine(PictureAddressConst.PersonalPath, _avaId.Value, PictureAddressConst.OriginFilename);
    }
}