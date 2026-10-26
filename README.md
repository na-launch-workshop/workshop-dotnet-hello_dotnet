# 🚀 **Module: .NET Hello World Translation Service**

**Technology Stack:**
  
- .NET

---

## 🎯 **Scenario**

This project reimplements the Quarkus-based hello world service from `workshop-quarkus-hello_quarkus` using ASP.NET Core minimal APIs. It exposes a single endpoint that returns a localized greeting with an ISO-8601 timestamp appended, mirroring the original behaviour.

---

### Prerequisites
- .NET SDK 8.0 or newer

---

## 🐾 **Guided Walkthrough**

1. Restore dependencies
```bash
dotnet restore
```

2. Run the tests
```bash
dotnet test
```

3. Start the service. The server listens on port `8080` by default. Override with the `PORT` environment variable if needed.
```bash
dotnet run
```

4. HTTP endpoint
- `GET /` – Responds with a greeting string such as `hello world @ 2024-06-01T12:34:56.000Z`.
- `GET /eggs` – 🥚 See below.

5. Configuration
Environment variables provide the same configurability as the Quarkus and Node.js versions:
- `TRANSLATION_DEFAULT_LANGUAGE` (default: `EN`) – Two-letter code used to choose the greeting.
- `TRANSLATION_FILE` (default: `translations.json`) – File located in the `resources/` directory that contains the greeting map.
- `PORT` (default: `8080`) – Port for the HTTP server.

All translations are stored in `resources/translations.json`, copied verbatim from the Quarkus project.

---

## 🥚 **Easter Eggs!**

- [ ] Find the easter egg riddle, steps below:

Start the service first with the **3. Start development server** task, then ask it for the riddle at `/eggs`.

Leave the server task running and open a second terminal (**Terminal** > **New Terminal**). That terminal runs in the same container as the server, so `localhost` works here:

```bash
curl http://localhost:8080/eggs
```

- [ ] 🥚 Find the four translation easter eggs

*"The translation file holds 85 greetings, but the service knows a few it never wrote down. Pirates can read them."*

You will know you found it when the service hands you a trophy. 🏆

**House rules:** reading `EasterEggs.cs` is cheating (and it is encoded anyway). The response headers, however, are fair game. 🐣

