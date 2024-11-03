using System.Net.Http.Json;
using Todo.WebApi.FunctionalTests.Contracts;

namespace Todo.WebApi.FunctionalTests.Extensions;

internal static class HttpResponseMessageExtensions
{
    internal static async Task<CustomProblemDetails> GetProblemDetails(
        this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Successful response");
        }

        CustomProblemDetails? problemDetails = await response
            .Content
            .ReadFromJsonAsync<CustomProblemDetails>();

        Ensure.NotNull(problemDetails);

        return problemDetails;
    }
}
