
# Unit Conversion API

## Overview
An ASP.NET Core Web API for converting units of
length, mass, and temperature.

## Features
- Convert between supported measurement units.
- List conversion categories.
- List units within a category.
- Validate input and return useful HTTP errors.
- Use dependency injection and a separate conversion service.

## Technologies
- C#
- ASP.NET Core Web API
- Swagger / OpenAPI

## How to Run
1. Install a compatible .NET SDK.
2. Restore NuGet packages.
3. Build the solution.
4. Run the application in Visual Studio or use:
   dotnet run
5. Open the Swagger URL shown by the application.

## Endpoints
- GET /api/conversions/categories
- GET /api/conversions/units/{category}
- POST /api/conversions/convert

## Design Decisions
The conversion service uses a base unit for length
and mass, while temperature conversions use dedicated
formulas. The controller delegates calculations to a
service registered through dependency injection.
