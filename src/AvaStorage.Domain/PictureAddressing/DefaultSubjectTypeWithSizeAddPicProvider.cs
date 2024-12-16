using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.PictureAddressing;

public class DefaultSubjectTypeWithSizeAddPicProvider : IPictureAddressProvider
{
    private readonly SubjectType _subjectType;
    private readonly int _size;

    public DefaultSubjectTypeWithSizeAddPicProvider(SubjectType subjectType, int size)
    {
        _subjectType = subjectType;
        _size = size;
    }
    public string ProvideAddress()
    {
        return Path.Combine(PictureAddressConst.SubjectPath, _subjectType.Value, _size.ToString());
    }
}