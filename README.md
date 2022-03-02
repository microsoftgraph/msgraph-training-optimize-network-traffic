# Microsoft Graph Training Module - Optimize Network Traffic with Microsoft Graph

Learn how Microsoft Graph uses throttling to maintain service health and how address this feature when creating applications.

> This module is also published as a Microsoft Learn module: [Optimize Network Traffic with Microsoft Graph](https://docs.microsoft.com/learn/modules/optimize-network-traffic/)

## Lab - Optimize Network Traffic with Microsoft Graph

The lab for this module is available in multiple units within the associated Microsoft Learn module. Use the following links to jump to the specific unit. Each Microsoft Learn unit represents a different lab exercise & demo in the presentation.

1. [Exercise: Understand throttling in Microsoft Graph](https://docs.microsoft.com/learn/modules/optimize-network-traffic/3-exercise-understand-throttling-microsoft-graph)

   In this exercise, you will create a new Azure AD web application registration using the Azure Active Directory admin center, a .NET Core console application and query the Microsoft Graph. You will issue many requests in parallel to trigger your requests to be throttled. This will allow you to see the response you will receive.

1. [Exercise: Avoid throttling & implement throttling strategies](https://docs.microsoft.com/learn/modules/optimize-network-traffic/5-exercise-avoid-throttling-implement-throttling-strategies)

   In this exercise, you'll use the Azure AD application and .NET console application you previously created and modify them to demonstrate two strategies to account for throttling in your application. One strategy used the **HttpClient** object but required you to implement the detect, delay and retry logic yourself when requests were throttled. The other strategy used the Microsoft Graph SDKs included support for handling this same scenario.

1. [Exercise: Eliminate polling Microsoft Graph with delta query](https://docs.microsoft.com/learn/modules/optimize-network-traffic/7-exercise-eliminate-polling-microsoft-graph-delta-query)

   In this exercise, you'll use the Graph Explorer to create and issue a single request that contains multiple child requests. This batching of requests enables developers to submit multiple requests in a single round-trip request to Microsoft Graph, creating more optimized queries.

## Demos

1. [Understand throttling in Microsoft Graph](./demos/01-understand-throttling)
1. [Avoid throttling & implement throttling strategies](./demos/02-avoid-throttline-implement-strategies)
1. [Eliminate polling Microsoft Graph with delta query](./demos/03-eliminate-polling)

## Watch the module

This module has been recorded and is available in the Office Development YouTube channel: [Optimize Network Traffic with Microsoft Graph](https://www.youtube.com/playlist?list=PLWZJrkeLOrbabcgkU-_DQJfEUbIw1qZeK)

## Contributors

| Roles                | Author(s)                                                                             |
| -------------------- | ------------------------------------------------------------------------------------- |
| Lab / Slides / Demos | Andrew Connell (Microsoft MVP, Voitanos) [@andrewconnell](//github.com/andrewconnell) |
| QA                   | Rob Windsor (Microsoft MVP, PAIT Group) [@rob-windsor](//github.com/rob-windsor)      |
| Sponsor / Support    | Jeremy Thake (Microsoft) [@jthake](//github.com/jthake)                               |

## Version history

| Version |        Date        |                                        Comments                                         |
| ------- | ------------------ | --------------------------------------------------------------------------------------- |
| 1.12    | March 1, 2022      | FY2022Q3 content refresh                                                                |
| 1.11    | December 8, 2021   | FY2022Q2 content refresh                                                                |
| 1.10    | September 13, 2021 | FY2022Q1 content refresh                                                                |
| 1.9     | May 26, 2021       | Refresh slides to new template                                                          |
| 1.8     | May 12, 2021       | FY2021Q3 content refresh                                                                |
| 1.7     | March 6, 2021      | FY2021Q3 content refresh                                                                |
| 1.6     | December 10, 2020  | FY2021Q2 content refresh                                                                |
| 1.5     | September 10, 2020 | FY2021Q1 content refresh                                                                |
| 1.4     | June 11, 2020      | FY2020Q4 content refresh                                                                |
| 1.3     | February 22, 2020  | FY2020Q3 content refresh                                                                |
| 1.2     | December 9, 2019   | FY2020Q2 content refresh                                                                |
| 1.1     | September 30, 2019 | Add: copyright statement to code; screencast link, exercise links, MS Learn module link |
| 1.0     | September 11, 2019 | New module published                                                                    |

## Disclaimer

**THIS CODE IS PROVIDED _AS IS_ WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.**

<img src="https://telemetry.sharepointpnp.com/msgraph-training-optimize-network-traffic" />
