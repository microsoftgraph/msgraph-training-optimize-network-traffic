// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace graphconsoleapp
{
  public class Messages {
    [JsonPropertyName("@odata.context")]
    public string ODataContext {get; set;}
    [JsonPropertyName("@odata.nextLink")]
    public string ODataNextLink {get; set;}

    [JsonPropertyName("value")]
    public Message[] Items {get; set;}
  }

  public class Message {
    [JsonPropertyName("@odata.etag")]
    public string ETag {get; set;}

    [JsonPropertyName("id")]
    public string Id {get; set;}

    [JsonPropertyName("subject")]
    public string Subject {get; set;}
  }
}