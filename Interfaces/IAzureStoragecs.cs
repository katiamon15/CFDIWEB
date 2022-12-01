using CFDIWEB.Models.azure;

namespace CFDIWEB.Interfaces
{
        public interface IAzureStorage
        {
            /// <summary>
            /// This method uploads a file submitted with the request
            /// </summary>
            /// <param name="file">File for upload</param>
            /// <returns>Blob with status</returns>
            public Task<BlobResponseDto> UploadAsync(IFormFile file);

            /// <summary>
            /// This method downloads a file with the specified filename
            /// </summary>
            /// <param name="blobFilename">Filename</param>
            /// <returns>Blob</returns>
            public Task<BlobDto> DownloadAsync(string blobFilename);
        }



    
}