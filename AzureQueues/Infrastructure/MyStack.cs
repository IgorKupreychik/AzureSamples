using Pulumi;
using Pulumi.Azure.Core;
using Pulumi.Azure.Storage;

class MyStack : Stack
{
    public MyStack()
    {
        // For existing RG
        //var resourceGroup = Output.Create(GetResourceGroup.InvokeAsync(
        //    new GetResourceGroupArgs
        //    {
        //        Name = "igor-kupreychik-rg"
        //    }));


        //// Create an Azure Storage Account
        //var storageAccount = new Account("storage", new AccountArgs
        //{
        //    ResourceGroupName = resourceGroup.Apply(r => r.Name),
        //    AccountReplicationType = "LRS",
        //    AccountTier = "Standard"
        //});

        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup("queue-test-rg");

        // Create an Azure Storage Account
        var storageAccount = new Account("igkqueuestorage", new AccountArgs
        {
            Name = "igkqueuestorage",
            ResourceGroupName = resourceGroup.Name,
            AccountReplicationType = "LRS",
            AccountTier = "Standard"
        });

        var serviceBusNamespace = new Pulumi.Azure.ServiceBus.Namespace("igkServiceBusNamespace", new Pulumi.Azure.ServiceBus.NamespaceArgs
        {
            Name = "igkServiceBusNamespace",
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
            Sku = "Standard"
        });

        var serviceBusQueue = new Pulumi.Azure.ServiceBus.Queue("igkServiceBusQueue", new Pulumi.Azure.ServiceBus.QueueArgs
        {
            Name = "igkServiceBusQueue",
            ResourceGroupName = resourceGroup.Name,
            NamespaceName = serviceBusNamespace.Name,
            EnablePartitioning = true,
        });
    }
}
