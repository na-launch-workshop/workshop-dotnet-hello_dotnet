using System;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

const string PlainTextContentType = "text/plain; charset=utf-8";

var builder = WebApplication.CreateBuilder(args);

var portValue = Environment.GetEnvironmentVariable("PORT");
var port = 8080;

if (!string.IsNullOrWhiteSpace(portValue) && int.TryParse(portValue, out var parsedPort) && parsedPort > 0)
{
    port = parsedPort;
}

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(port);
});

var resourcesDirectory = Path.Combine(builder.Environment.ContentRootPath, "resources");

var app = builder.Build();

app.MapGet("/", async () =>
{
    var defaultLanguage = (Environment.GetEnvironmentVariable("TRANSLATION_DEFAULT_LANGUAGE") ?? "EN").ToUpperInvariant();
    var translationFile = Environment.GetEnvironmentVariable("TRANSLATION_FILE") ?? "translations.json";
    var translationFilePath = Path.Combine(resourcesDirectory, translationFile);

    try
    {
        await using var fileStream = File.OpenRead(translationFilePath);
        using var document = await JsonDocument.ParseAsync(fileStream);

        if (!document.RootElement.TryGetProperty("translations", out var translationsElement) ||
            translationsElement.ValueKind != JsonValueKind.Object)
        {
            return ErrorResponse($"Invalid JSON format in {translationFile}");
        }

        if (!TryGetTranslation(translationsElement, defaultLanguage, out var translation))
        {
            return ErrorResponse($"Invalid JSON format in {translationFile}");
        }

        var timestamp = DateTime.UtcNow.ToString("o");
        return Results.Text($"{translation} @ {timestamp}", PlainTextContentType);
    }
    catch (FileNotFoundException)
    {
        return ErrorResponse($"Could not find {translationFile} in resources.");
    }
    catch (DirectoryNotFoundException)
    {
        return ErrorResponse($"Could not find {translationFile} in resources.");
    }
    catch (JsonException)
    {
        return ErrorResponse($"Invalid JSON format in {translationFile}");
    }
    catch (Exception ex)
    {
        return ErrorResponse($"Error reading {translationFile}: {ex.Message}");
    }
});

app.Run();

static bool TryGetTranslation(JsonElement translationsElement, string language, out string? translation)
{
    translation = null;

    if (translationsElement.TryGetProperty(language, out var translationElement) &&
        translationElement.ValueKind == JsonValueKind.String)
    {
        translation = translationElement.GetString();
    }
    else if (translationsElement.TryGetProperty("EN", out var fallbackElement) &&
             fallbackElement.ValueKind == JsonValueKind.String)
    {
        translation = fallbackElement.GetString();
    }

    return !string.IsNullOrWhiteSpace(translation);
}

static IResult ErrorResponse(string message) =>
    Results.Text(message, PlainTextContentType, statusCode: StatusCodes.Status500InternalServerError);

public partial class Program { }
