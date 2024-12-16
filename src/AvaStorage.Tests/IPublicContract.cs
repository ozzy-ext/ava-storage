using MyLab.ApiClient;

[Api("v1/ava")]
public interface IPublicContractV1
{
    [Get]
    Task<CallDetails<byte[]>> GetAsync
    (
        [Query("id")] string id,
        [Query("sz")] int? size,
        [Query("st")] string? subjectType
    );
}