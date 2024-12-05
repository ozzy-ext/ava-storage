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
            [Query("st")] string? subjectType,
            [BinContent] byte[] picture
        );
    }
}
