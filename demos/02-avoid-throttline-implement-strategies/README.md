# Demo: Avoid throttling & implement throttling strategies

This completed project is the result of the lab exercise **Avoid throttling & implement throttling strategies** that is referenced in the [README](../../README.md) in this repo.

This demo consists of two smaller demos. Both demos do the same thing: implement a throttling strategy in a .NET Core console application. The first demo does this using the **HttpClient** object while the second demo uses the Microsoft Graph SDK.

The steps to run each demo are identical and you can use the same Azure AD application for both, so the following instructions can be used for each demo:

1. [Implement throttling strategy with HttpClient](./part-01-httpclient)
1. [Implement throttling strategy with the Microsoft Graph SDK](./part-02-graphclient)

## Prerequisites

- [Microsoft 365 tenant](https://developer.microsoft.com/office/dev-program?ocid=MSlearn)
- [.NET 5 or .NET 6 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio Code](https://code.visualstudio.com/)

## Run this Completed Project

- Create an Azure AD application by following the instructions in the lab exercise associated with this demo. In this step, you are instructed to collect these data elements:
  - tenantId
  - applicationId
- Rename the file **appsettings.json.example** to **appsettings.json**
- Update the properties in the **appsettings.json** with the values you collected in the last step.
- Build and run the application by following the instructions in the lab exercise associated with this demo.
