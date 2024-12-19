using MyLab.ApiClient;

namespace AvaStorage.Tests
{
    [Api("v1/ava/{id}")]
    public interface IAvaStorageContractV1
    {
        [Put]
        Task<CallDetails> PutAsync
        (
            [Path("id")] string id,
            [BinContent] byte[] picture
        );

        [Get]
        Task<CallDetails<byte[]>> GetAsync
        (
            [Path("id")] string id,
            [Query("sz")] int? size,
            [Query("st")] string? subjectType
        );
    }
}
