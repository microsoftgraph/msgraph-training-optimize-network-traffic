# Demo: Avoid throttling & implement throttling strategies

This completed project is the result of the lab exercise **Avoid throttling & implement throttling strategies** that is referenced in the [README](../../) in this repo.

This demo consists of two smaller demos. Both demos do the same thing: implement a throttling strategy in a .NET Core console application. The first demo does this using the **HttpClient** object while the second demo uses the Microsoft Graph SDK.

The steps to run each demo are identical and you can use the same Azure AD application for both, so the following instructions can be used for each demo:

1. [Implement throttling strategy with HttpClient](./part-01-httpclient)
1. [Implement throttling strategy with the Microsoft Graph SDK](./part-02-graphclient)

## Prerequisites

- Office 365 Tenancy
- [.NET Core 2.2 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio Code](https://code.visualstudio.com/)

### Demos

- Create an Azure AD application by following the instructions in the lab exercise associated with this demo. In this step, you are instructed to collect three data elements:
  - tenantId
  - applicationId
  - domain
- Update the properties in the [appsettings.json](./appsettings.json) with the values you collected in the last step.
- Build & run the application by following the instructions in the lab exercise associated with this demo.