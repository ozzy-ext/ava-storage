using System.Threading.Tasks;
using MyLab.ApiClient;

namespace MyLab.AvaStorage
{
    [Api("v1/ava/{id}", Key = "ava-storage")]
    public interface IAvaStorageV1
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
