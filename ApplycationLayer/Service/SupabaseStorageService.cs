using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.DTOs.Supabase;
using DomainLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ApplicationLayer.Service
{
    public interface ISupabaseStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }

    public class SupabaseStorageService : ISupabaseStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseSettings _settings;

        public SupabaseStorageService(HttpClient httpClient, IOptions<SupabaseSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            var sanitizedFileName = FileHelper.SanitizeFileName(file.FileName);

            var filePath = $"{Guid.NewGuid()}_{file.FileName}";

            using var content = new StreamContent(file.OpenReadStream());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_settings.Url}/storage/v1/object/{_settings.Bucket}/{filePath}")
            {
                Content = content
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ServiceRoleKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Upload failed: {error}");
            }

            // Nếu bucket public:
            var publicUrl = $"{_settings.Url}/storage/v1/object/public/{_settings.Bucket}/{filePath}";
            return publicUrl;
        }
    }
}
