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
    }
}
