using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.PictureAddressing;

public class DefaultSubjectTypePicAddrProvider : IPictureAddressProvider
{
    private readonly SubjectType _subjectType;

    public DefaultSubjectTypePicAddrProvider(SubjectType subjectType)
    {
        _subjectType = subjectType;
    }
    public string ProvideAddress()
    {
        return Path.Combine(PictureAddressConst.SubjectPath, _subjectType.Value, PictureAddressConst.DefaultFilename);
    }
}