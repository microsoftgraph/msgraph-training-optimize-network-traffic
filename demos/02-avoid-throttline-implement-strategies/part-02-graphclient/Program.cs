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

      var client = GetAuthenticatedGraphClient(config);

      var profileResponse = client.Me.Request().GetAsync().Result;
      Console.WriteLine("Hello " + profileResponse.DisplayName);

      var stopwatch = new System.Diagnostics.Stopwatch();
      stopwatch.Start();

      var clientResponse = client.Me.Messages
                                    .Request()
                                    .Select(m => new { m.Id })
                                    .Top(100)
                                    .GetAsync()
                                    .Result;
      var items = clientResponse.CurrentPage;

      var tasks = new List<Task>();
      foreach (var graphMessage in items)
      {
        tasks.Add(Task.Run(() =>
        {

          Console.WriteLine("...retrieving message: {0}", graphMessage.Id);

          var messageDetail = GetMessageDetail(client, graphMessage.Id);

          if (messageDetail != null)
          {
            Console.WriteLine("SUBJECT: {0}", messageDetail.Subject);
          }

        }));
      }

      // do all work in parallel & wait for it to complete
      var allWork = Task.WhenAll(tasks);
      try
      {
        allWork.Wait();
      }
      catch { }

      stopwatch.Stop();
      Console.WriteLine();
      Console.WriteLine("Elapsed time: {0} seconds", stopwatch.Elapsed.Seconds);
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

    private static GraphServiceClient GetAuthenticatedGraphClient(IConfigurationRoot config)
    {
      var authenticationProvider = CreateAuthorizationProvider(config);
      var graphClient = new GraphServiceClient(authenticationProvider);
      return graphClient;
    }

    private static Message GetMessageDetail(GraphServiceClient client, string messageId)
    {
      // submit request to Microsoft Graph & wait to process response
      return client.Me.Messages[messageId].Request().GetAsync().Result;
    }
  }
}