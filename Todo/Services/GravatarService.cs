using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Todo.Models.Gravatar;

namespace Todo.Services
{
    public class GravatarService : IGravatarService
    {
        private readonly HttpClient _httpClient;

        public GravatarService(HttpClient httpClient, IOptions<GravatarOptions> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        public async Task<GravatarProfile> GetProfileAsync(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(emailAddress));

            var userHash = GravatarHashService.GetSha256Hash(emailAddress);

            // If we weren't rate limited then do the below
            //return await _httpClient.GetFromJsonAsync<GravatarProfile>($"profiles/{userHash}");

            var responseMessage = await _httpClient.GetAsync($"profiles/{userHash}");

            // Handle the case for too many requests don't want to throw here yet just fail gracefully
            if (responseMessage.StatusCode == HttpStatusCode.TooManyRequests)
            {
                return new GravatarProfile();
            }

            // Do throw if there was a legit error
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<GravatarProfile>();
        }
    }
}
