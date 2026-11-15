# CrystalImageHelper

ASP.NET 4.8 image proxy / local-cache service.

- Serves images using clean URLs whose **last two segments** are `{key}/{filename}`.
- Looks up `key` in `Web.config` (`Source:{key}`).
- If the file is missing locally, downloads it from Azure Blob Storage (optional SAS token).
- Optionally normalizes the image to 24-bit RGB (no alpha) so Crystal Reports can use it reliably.

See `_lookAtme.md` for coding rules, URL contract and configuration details.

## Quick start
1. Edit `Web.config` appSettings for your local folders and Azure containers.
2. Deploy as an IIS application targeting .NET Framework 4.8.
3. Request images as `/products/logo.png` or `/logos/company.png`.

## Structure
- `App_Code/` – handler + pure helpers (small single-purpose classes)
- `Tests/` – NUnit tests for the pure logic
- `activity/` – session logs (read the latest file for status / next steps)
