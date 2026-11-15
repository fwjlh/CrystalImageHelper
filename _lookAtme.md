# CrystalImageHelper

**User:** fwjlh  
**Repo path:** fwjlh/CrystalImageHelper  

## Purpose
ASP.NET 4.8 image proxy / cache service.  
Serves images from a local folder. If missing, downloads from Azure Blob Storage (using optional SAS token) and caches locally.  
Optionally normalizes images to 24-bit RGB (no alpha) so they work reliably with Crystal Reports.

## URL Format
Last two URL segments are treated as `{key}/{filename}`:

```
https://yourhost/products/logo.png
https://yourhost/logos/company.png
```

`key` is looked up in `Web.config` under `Source:{key}`.

## Configuration (Web.config appSettings)
```
Format: LocalFolder | AzureBaseUrl | AzureSasToken (optional) | NormalizeForCrystal (true/false)
```

Example:
```xml
<add key="Source:products" value="C:\ReportImages\Products|https://yourstorageaccount.blob.core.windows.net/products|sv=...|true" />
```

## Coding Rules (must follow)
- Source file preferably ≤15–20KB (hard limit ~25KB)
- Max 2 nesting levels
- Nested block >5 lines → extract to named function
- Prefer small, single-purpose named functions (named over arrow for simple cases)
- Prefer immutability and pure functions
- Max 4 parameters; otherwise use an options/params type
- Explicit types on all parameters. Explicit return types on all exported functions
- Prefer early return
- Names must reveal intent
- No dead code
- No side effects in constructors or property getters
- Avoid null — prefer Option/Result or careful nullable reference types
- Keep public APIs small and stable
- Comments only for “why”, never for “what”
- 100% test coverage on logic + meaningful edge cases
- Test names must be clear and descriptive

## Activity Log
All actions are logged under `/activity/`.  
Next session should read the latest activity file for status and next step.
