// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Helpers;

namespace graphconsoleapp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var config = LoadAppSettings();
      if (config == null)
      {
        Console.WriteLine("Invalid appsettings.json file.");
        return;
      }

      var client = GetAuthenticatedHTTPClient(config);

      var profileResponse = client.GetAsync("https://graph.microsoft.com/v1.0/me").Result;
      var profileJson = profileResponse.Content.ReadAsStringAsync().Result;
      var profileObject = JsonDocument.Parse(profileJson);
      var displayName = profileObject.RootElement.GetProperty("displayName").GetString();
      Console.WriteLine("Hello " + displayName);

      var totalRequests = 100;
      var successRequests = 0;
      var tasks = new List<Task>();
      var failResponseCode = HttpStatusCode.OK;
      HttpResponseHeaders failedHeaders = null!;

      for (int i = 0; i < totalRequests; i++)
      {
        tasks.Add(Task.Run(() =>
        {
          var response = client.GetAsync("https://graph.microsoft.com/v1.0/me/messages").Result;
          Console.Write(".");
          if (response.StatusCode == HttpStatusCode.OK)
          {
            successRequests++;
          }
          else
          {
            Console.Write('X');
            failResponseCode = response.StatusCode;
            failedHeaders = response.Headers;
          }
        }));
      }

      var allWork = Task.WhenAll(tasks);
      try
      {
        allWork.Wait();
      }
      catch { }
      Console.WriteLine();
      Console.WriteLine("{0}/{1} requests succeeded.", successRequests, totalRequests);
      if (successRequests != totalRequests)
      {
        Console.WriteLine("Failed response code: {0}", failResponseCode.ToString());
        Console.WriteLine("Failed response headers: {0}", failedHeaders);
      }
    }

    private static IConfigurationRoot? LoadAppSettings()
    {
      try
      {
        var config = new ConfigurationBuilder()
                          .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", false, true)
                          .Build();

        if (string.IsNullOrEmpty(config["applicationId"]) ||
            string.IsNullOrEmpty(config["tenantId"]))
        {
          return null;
        }

        return config;
      }
      catch (System.IO.FileNotFoundException)
      {
        return null;
      }
    }

    private static IAuthenticationProvider CreateAuthorizationProvider(IConfigurationRoot config)
    {
      var clientId = config["applicationId"];
      var authority = $"https://login.microsoftonline.com/{config["tenantId"]}/v2.0";

      List<string> scopes = new List<string>();
      scopes.Add("https://graph.microsoft.com/.default");

      var cca = PublicClientApplicationBuilder.Create(clientId)
                                              .WithAuthority(authority)
                                              .WithDefaultRedirectUri()
                                              .Build();
      return MsalAuthenticationProvider.GetInstance(cca, scopes.ToArray());
    }

    private static HttpClient GetAuthenticatedHTTPClient(IConfigurationRoot config)
    {
      var authenticationProvider = CreateAuthorizationProvider(config);
      var httpClient = new HttpClient(new AuthHandler(authenticationProvider, new HttpClientHandler()));
      return httpClient;
    }
  }
}