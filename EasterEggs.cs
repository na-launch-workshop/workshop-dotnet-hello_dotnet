// Workshop extras for the .NET translation service.
//
// Everything interesting in here is encoded on purpose. Yes, you could sit
// down with `base64 -d` and crack it open in about thirty seconds -- but
// where is the fun in that? Go poke at the running service instead. Start
// with:
//
//     curl http://localhost:8080/eggs

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

internal static class EasterEggs
{
    public const string HintHeader = "X-Easter-Eggs";

    private static readonly Dictionary<string, string> Vault = new()
    {
        ["hint"] =
            "MSBlZ2cgaGlkZGVuIGluIHRoaXMgc2VydmljZS4gU3RhcnQgd2l0aDogR0VUIC9lZ2dz",
        ["greet"] =
            "eyJQSVJBVEUiOiAiYWhveSB3b3JsZCwgeWUgc2N1cnZ5IGRldiEiLCAiTEVFVCI6ICJo" +
            "M2xsMCB3MHJsZCIsICJFTU9KSSI6ICLwn5GLIPCfjI0iLCAiQklOQVJZIjogIjAxMTAx" +
            "MDAwIDAxMTAxMDAxIn0=",
        ["riddle"] =
            "8J+lmiBFYXN0ZXIgRWdnIEh1bnQgLSAuTkVUIFRyYW5zbGF0aW9uIFNlcnZpY2UKPT09" +
            "PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PQoKT25lIGVn" +
            "ZyBpcyBoaWRkZW4gaW4gdGhpcyBzZXJ2aWNlLgoKICAiVGhlIHRyYW5zbGF0aW9uIGZp" +
            "bGUgaG9sZHMgODUgZ3JlZXRpbmdzLCBidXQgdGhlIHNlcnZpY2Uga25vd3MKICAgYSBm" +
            "ZXcgaXQgbmV2ZXIgd3JvdGUgZG93bi4gUGlyYXRlcyBjYW4gcmVhZCB0aGVtLiIKCkhp" +
            "bnQ6IG9uZSBlbnZpcm9ubWVudCB2YXJpYWJsZSBhbHJlYWR5IGRlY2lkZXMgd2hpY2gg" +
            "Z3JlZXRpbmcgeW91CmdldCBiYWNrLCBhbmQgcmVzb3VyY2VzLyBpcyBub3QgdGhlIG9u" +
            "bHkgcGxhY2UgYSBncmVldGluZyBjYW4gbGl2ZS4KCkdvb2QgbHVjay4g8J+Qowo=",
        ["found"] =
            "CgogICAgICAgICBfX19fX19fX19fXwogICAgICAgICcuXz09Xz09Xz1fLicKICAgICAg" +
            "ICAuLVw6ICAgICAgLy0uCiAgICAgICB8ICh8Oi4gICAgIHwpIHwKICAgICAgICAnLXw6" +
            "LiAgICAgfC0nCiAgICAgICAgICBcOjouICAgIC8KICAgICAgICAgICAnOjouIC4nCiAg" +
            "ICAgICAgICAgICApICgKICAgICAgICAgICBfLicgJy5fCiAgICAgICAgICBgIiIiIiIi" +
            "ImAKCvCfj4YgRUdHIEZPVU5EISDwn4+GCgpZb3UgdGF1Z2h0IHRoaXMgc2VydmljZSB0" +
            "byBzcGVhayBhIGxhbmd1YWdlIHRoYXQgd2FzIG5ldmVyIGluCnJlc291cmNlcy90cmFu" +
            "c2xhdGlvbnMuanNvbi4gR28gdGVsbCB0aGUgd29ya3Nob3AgZmFjaWxpdGF0b3IuIPCf" +
            "kKM=",
    };

    private static readonly Dictionary<string, string> Unwrapped = new();

    private static readonly Lazy<Dictionary<string, string>> Greetings = new(() =>
        JsonSerializer.Deserialize<Dictionary<string, string>>(Unwrap("greet")) ?? new());

    public static string HintMessage => Unwrap("hint");

    private static string Unwrap(string key)
    {
        lock (Unwrapped)
        {
            if (!Unwrapped.TryGetValue(key, out var value))
            {
                value = Encoding.UTF8.GetString(Convert.FromBase64String(Vault[key]));
                Unwrapped[key] = value;
            }

            return value;
        }
    }

    /// <summary>A greeting the translation file has never heard of, if there is one.</summary>
    public static string? AlternateGreeting(string language) =>
        Greetings.Value.TryGetValue(language.ToUpperInvariant(), out var greeting) ? greeting : null;

    /// <summary>The "you found it" banner, appended to the greeting.</summary>
    public static string Claim() => Unwrap("found");

    /// <summary>The riddle, served to anyone who asks.</summary>
    public static string Riddle() => Unwrap("riddle");
}
