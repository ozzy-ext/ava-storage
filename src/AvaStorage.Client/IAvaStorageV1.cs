using System;
using System.Threading.Tasks;
using MyLab.ApiClient;

namespace MyLab.AvaStorage
{
    [Api("v1/ava/{id}", Key = "ava-storage")]
    public interface IAvaStorageV1
    {
        [Put]
        Task<CallDetails> PutWrongFormatAsync
        (
            [Path("id")] string id,
            [BinContent("wrong/format")] byte[] picture
        );

        [Put]
        Task<CallDetails> PutPngAsync
        (
            [Path("id")] string id,
            [BinContent("image/png")] byte[] picture
        );

        [Put]
        Task<CallDetails> PutJpegAsync
        (
            [Path("id")] string id,
            [BinContent("image/jpeg")] byte[] picture
        );

        [Put]
        Task<CallDetails> PutGifAsync
        (
            [Path("id")] string id,
            [BinContent("image/gif")] byte[] picture
        );

        [Get]
        Task<CallDetails<byte[]>> GetAsync
        (
            [Path("id")] string id,
            [Query("sz")] int? size,
            [Query("st")] string? subjectType
        );

        [Get]
        Task<CallDetails<byte[]>> GetWithLastModifiedAsync
        (
            [Path("id")] string id,
            [Query("sz")] int? size,
            [Query("st")] string? subjectType,
            [Header("If-Modified-Since")] string ifLastModifiedSince
        );
    }
}
