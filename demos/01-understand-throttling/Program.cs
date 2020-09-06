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
using Helpers;

namespace graphconsoleapp
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");

      var config = LoadAppSettings();
      if (config == null)
      {
        Console.WriteLine("Invalid appsettings.json file.");
        return;
      }

      var userName = ReadUsername();
      var userPassword = ReadPassword();

      var client = GetAuthenticatedHTTPClient(config, userName, userPassword);

      var totalRequests = 100;
      var successRequests = 0;
      var tasks = new List<Task>();
      var failResponseCode = HttpStatusCode.OK;
      HttpResponseHeaders failedHeaders = null;

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

    private static IConfigurationRoot LoadAppSettings()
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

    private static IAuthenticationProvider CreateAuthorizationProvider(IConfigurationRoot config, string userName, SecureString userPassword)
    {
      var clientId = config["applicationId"];
      var authority = $"https://login.microsoftonline.com/{config["tenantId"]}/v2.0";

      List<string> scopes = new List<string>();
      scopes.Add("User.Read");
      scopes.Add("Mail.Read");

      var cca = PublicClientApplicationBuilder.Create(clientId)
                                              .WithAuthority(authority)
                                              .Build();
      return MsalAuthenticationProvider.GetInstance(cca, scopes.ToArray(), userName, userPassword);
    }

    private static HttpClient GetAuthenticatedHTTPClient(IConfigurationRoot config, string userName, SecureString userPassword)
    {
      var authenticationProvider = CreateAuthorizationProvider(config, userName, userPassword);
      var httpClient = new HttpClient(new AuthHandler(authenticationProvider, new HttpClientHandler()));
      return httpClient;
    }

    private static SecureString ReadPassword()
    {
      Console.WriteLine("Enter your password");
      SecureString password = new SecureString();
      while (true)
      {
        ConsoleKeyInfo c = Console.ReadKey(true);
        if (c.Key == ConsoleKey.Enter)
        {
          break;
        }
        password.AppendChar(c.KeyChar);
        Console.Write("*");
      }
      Console.WriteLine();
      return password;
    }

    private static string ReadUsername()
    {
      string username;
      Console.WriteLine("Enter your username");
      username = Console.ReadLine();
      return username;
    }
  }

}
