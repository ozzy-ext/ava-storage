using MyLab.ApiClient;

[Api("v1/ava/{id}")]
public interface IPublicContractV1
{
    [Get]
    Task<CallDetails<byte[]>> GetAsync
    (
        [Path("id")] string id,
        [Query("sz")] int? size,
        [Query("st")] string? subjectType
    );
}