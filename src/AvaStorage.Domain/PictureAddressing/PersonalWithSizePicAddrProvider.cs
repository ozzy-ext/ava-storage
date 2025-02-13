using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.PictureAddressing;

public class PersonalWithSizePicAddrProvider : IPictureAddressProvider
{
    private readonly AvatarId _avaId;
    private readonly int _size;

    public PersonalWithSizePicAddrProvider(AvatarId avaId, int size)
    {
        _avaId = avaId;
        _size = size;
    }
    public string ProvideAddress()
    {
        return Path.Combine(PictureAddressConst.PersonalPath, _avaId.Value, _size + ".png");
    }
}