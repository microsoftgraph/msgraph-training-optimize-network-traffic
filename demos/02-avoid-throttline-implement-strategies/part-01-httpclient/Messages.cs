// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Text.Json.Serialization;

namespace graphconsoleapp
{
  public class Messages
  {
    [JsonPropertyName("@odata.context")]
    public string ODataContext { get; set; } = string.Empty;
    [JsonPropertyName("@odata.nextLink")]
    public string ODataNextLink { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public Message[] Items { get; set; } = Array.Empty<Message>();
  }

  public class Message
  {
    [JsonPropertyName("@odata.etag")]
    public string ETag { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;
  }
}