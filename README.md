# Currency Converter

A web application that converts USD amounts into words in English or German, using ASP.NET Core (.NET 10), React, TypeScript, and Bootstrap.

## Build and Run

Requires .NET 10 SDK and Node.js/npm.

From the project root, start the backend:

```bash
dotnet run --project src/CurrencyConverter.Api
```

In a separate terminal, start the frontend:

```bash
cd src/CurrencyConverter.Web
npm install
npm run dev
```

Open `http://localhost:5173` in your browser.

The frontend is configured to communicate with the API at `http://localhost:5209`.

## Troubleshooting

The frontend runs on port `5173` and the backend on port `5209` by default.

If you need to change the frontend port, update:

- `server.port` in `src/CurrencyConverter.Web/vite.config.ts`
- `WithOrigins()` in `src/CurrencyConverter.Api/Program.cs`

If the frontend opens but conversion fails, check that the backend is running on port `5209`.

If you need to change the backend port, update:

- `applicationUrl` in `src/CurrencyConverter.Api/Properties/launchSettings.json` (HTTP profile)
- `VITE_API_BASE_URL` in `src/CurrencyConverter.Web/.env`

Restart the corresponding servers after changing these settings.

## Design Decisions

The currency conversion logic runs on the backend. The frontend handles user input, formatting, and displaying the result.

I used separate converter classes for English and German because they have different rules for forming numbers. Language-specific words are stored in `.resx` resource files.

## Assumptions

- Only USD amounts up to `999999999,99` are supported.
- A comma is used as the decimal separator, with up to two decimal places.
- One decimal digit represents tenths (e.g., `25,1` means 25 dollars and 10 cents).
- Spaces are used as thousands separators in the UI but removed before sending the amount to the API.

## Testing

Run the unit tests from the project root:

```bash
dotnet test
```

## Project Structure

```text
CurrencyConverter/
├── CurrencyConverter.slnx
├── README.md
├── src/
│   ├── CurrencyConverter.Api/
│   │   ├── Controllers/
│   │   │   └── CurrencyController.cs       # REST API endpoint
│   │   ├── Models/
│   │   │   └── ConvertRequest.cs           # Request model
│   │   └── Program.cs                      # API and CORS configuration
│   ├── CurrencyConverter.Core/
│   │   ├── Converters/
│   │   │   ├── EnglishNumberConverter.cs   # English number conversion
│   │   │   ├── GermanNumberConverter.cs    # German number conversion
│   │   │   └── INumberConverter.cs         # Converter interface
│   │   ├── CurrencyConverter.cs            # Input validation and currency conversion
│   │   └── Resources/
│   │       ├── CurrencyWords.de.resx       # German vocabulary
│   │       └── CurrencyWords.resx          # English vocabulary
│   └── CurrencyConverter.Web/
│       ├── src/
│       │   ├── App.tsx
│       │   └── components/
│       │       └── CurrencyConverter.tsx   # UI and API requests
│       └── vite.config.ts
└── tests/
    └── CurrencyConverter.Tests/
        └── UnitTest1.cs                   # xUnit tests
```



## References and AI Usage

**Reference:**

- [Humanizer](https://github.com/Humanizr/Humanizer) — Referenced while researching English and German number-to-words conversion.

**AI Usage:**

I used ChatGPT as a supporting tool during development, mainly for:

- Helping fill in repetitive English and German vocabulary entries in the `.resx` resource files.
- Getting help with specific React/TypeScript code (input validation, thousands separator formatting, and error handling).
- Troubleshooting errors (Vite environment variables, import paths, API errors, and CORS).
- Polishing the English wording and formatting of the README.