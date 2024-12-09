using MyLab.ApiClient;

namespace AvaStorage.Tests
{
    [Api("v1/ava")]
    public interface IAdminContractV1
    {
        [Put]
        Task<CallDetails> PutAsync
        (
            [Query("id")] string id,
            [BinContent] byte[] picture
        );

        [Get]
        Task<CallDetails<byte[]>> GetAsync
        (
            [Query("id")] string id,
            [Query("sz")] int? size,
            [Query("st")] string? subjectType
        );
    }
}
