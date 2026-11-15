# CrystalImageHelper

ASP.NET 4.8 image proxy / local-cache service.

- Serves images using clean URLs whose **last two segments** are `{key}/{filename}`.
- Looks up `key` in `Web.config` (`Source:{key}`).
- If the file is missing locally, downloads it from Azure Blob Storage (optional SAS token).
- Optionally normalizes the image to 24-bit RGB (no alpha) so Crystal Reports can use it reliably.

See `_lookAtme.md` for coding rules, URL contract and configuration details.

## Visual Studio Solution

Open **`CrystalImageHelper.sln`** in Visual Studio.

```
CrystalImageHelper.sln
├── CrystalImageHelper/                 ← Web Application (.NET Framework 4.8)
│   ├── Handlers/
│   ├── Models/
│   ├── Services/
│   ├── Properties/
│   ├── Web.config
│   └── CrystalImageHelper.csproj
└── CrystalImageHelper.Tests/           ← NUnit test project
    └── CrystalImageHelper.Tests.csproj
```

## Quick start
1. Open `CrystalImageHelper.sln` in Visual Studio.
2. Edit `CrystalImageHelper/Web.config` appSettings for your local folders and Azure containers.
3. Build & run (or deploy to IIS targeting .NET Framework 4.8).
4. Request images as `/products/logo.png` or `/logos/company.png`.

## Activity Log
All actions are logged under `/activity/`. Read the latest file for status and next steps.
