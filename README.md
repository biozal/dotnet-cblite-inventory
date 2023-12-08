# Couchbase Lite 3.1 Inventory Demo Application

This is a mobile application that demonstrates the use of Couchbase Lite 3.1 in a .NET 8 Maui and .NET Uno application. It is a simple inventory application that allows you to add, edit, and delete items from an inventory audit. It also allows you to sync the inventory to Couchbase Capella App Services or Couchbase Server using Sync Gateway.

The standard solution file in the root folder is for the .NET Maui application.  The solution file in the InventoryUno folder is for the .NET Uno application.

The applications share a multi-platform class library called Dotnet.Cblite.Inventory.MPShared that contains the business logic, View Models, and Couchbase Lite code that is shared between the Maui and Uno application.  The .NET Maui and .NET Uno applications both reference this library.

For more information on configuring multi-target projects see [https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/configure-multi-targeting?view=net-maui-8.0](https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/configure-multi-targeting?view=net-maui-8.0).

Tests can be found in the Dotnet.Cblite.Inventory.Shared.Tests folder.