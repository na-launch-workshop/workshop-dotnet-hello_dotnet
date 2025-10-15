# .NET Hello World Translation Service

This project reimplements the Quarkus-based hello world service from `workshop-quarkus-hello_quarkus` using ASP.NET Core minimal APIs. It exposes a single endpoint that returns a localized greeting with an ISO-8601 timestamp appended, mirroring the original behaviour.

## Prerequisites
- .NET SDK 8.0 or newer

## Restore Dependencies
```bash
dotnet restore
```

## Running the Tests
```bash
dotnet test
```

## Starting the Service
```bash
dotnet run
```
The server listens on port `8080` by default. Override with the `PORT` environment variable if needed.

## HTTP Endpoint
- `GET /` – Responds with a greeting string such as `hello world @ 2024-06-01T12:34:56.000Z`.

## Configuration
Environment variables provide the same configurability as the Quarkus and Node.js versions:
- `TRANSLATION_DEFAULT_LANGUAGE` (default: `EN`) – Two-letter code used to choose the greeting.
- `TRANSLATION_FILE` (default: `translations.json`) – File located in the `resources/` directory that contains the greeting map.
- `PORT` (default: `8080`) – Port for the HTTP server.

All translations are stored in `resources/translations.json`, copied verbatim from the Quarkus project.
