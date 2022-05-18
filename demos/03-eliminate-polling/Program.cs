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
    private static object? _deltaLink = null;
    private static IUserDeltaCollectionPage? _previousPage = null;

    public static void Main(string[] args)
    {
      var config = LoadAppSettings();
      if (config == null)
      {
        Console.WriteLine("Invalid appsettings.json file.");
        return;
      }

      Console.WriteLine("All users in tenant:");
      CheckForUpdates(config);
      Console.WriteLine();
      while (true)
      {
        Console.WriteLine("... sleeping for 10s - press CTRL+C to terminate");
        System.Threading.Thread.Sleep(10 * 1000);
        Console.WriteLine("> Checking for new/updated users since last query...");
        CheckForUpdates(config);
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

    private static void OutputUsers(IUserDeltaCollectionPage users)
    {
      foreach (var user in users)
      {
        Console.WriteLine($"User: {user.Id}, {user.GivenName} {user.Surname}");
      }
    }

    private static IUserDeltaCollectionPage GetUsers(GraphServiceClient graphClient, object? deltaLink)
    {
      IUserDeltaCollectionPage page;

      // IF this is the first request, then request all users
      //    and include Delta() to request a delta link to be included in the
      //    last page of data
      if (_previousPage == null || deltaLink == null)
      {
        page = graphClient.Users
                          .Delta()
                          .Request()
                          .Select("Id,GivenName,Surname")
                          .GetAsync()
                          .Result;
      }
      // ELSE, not the first page so get the next page of users
      else
      {
        _previousPage.InitializeNextPageRequest(graphClient, deltaLink.ToString());
        page = _previousPage.NextPageRequest.GetAsync().Result;
      }

      _previousPage = page;
      return page;
    }

    private static void CheckForUpdates(IConfigurationRoot config)
    {
      var graphClient = GetAuthenticatedGraphClient(config);

      // get a page of users
      var users = GetUsers(graphClient, _deltaLink);

      OutputUsers(users);

      // go through all of the pages so that we can get the delta link on the last page.
      while (users.NextPageRequest != null)
      {
        users = users.NextPageRequest.GetAsync().Result;
        OutputUsers(users);
      }

      object? deltaLink;

      if (users.AdditionalData.TryGetValue("@odata.deltaLink", out deltaLink))
      {
        _deltaLink = deltaLink;
      }
    }
  }
}