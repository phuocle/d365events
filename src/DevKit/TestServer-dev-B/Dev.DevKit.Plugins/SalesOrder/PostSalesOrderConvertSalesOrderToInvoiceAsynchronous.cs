using Dev.DevKit.Shared;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using System;

namespace Dev.DevKit.Plugins.SalesOrder
{
    [CrmPluginRegistration("ConvertSalesOrderToInvoice", "salesorder", StageEnum.PostOperation, ExecutionModeEnum.Asynchronous, "",
    "Dev.DevKit.Plugins.SalesOrder.PostSalesOrderConvertSalesOrderToInvoiceAsynchronous", 1/*ExecutionOrder*/, IsolationModeEnum.Sandbox, PluginType = PluginType.Plugin, DeleteAsyncOperation = true,
    Image1Name = "", Image1Alias = "", Image1Type = ImageTypeEnum.PreImage, Image1Attributes = "")]
    public class PostSalesOrderConvertSalesOrderToInvoiceAsynchronous : IPlugin
    {
        /*
          InputParameters:
              SalesOrderId         System.Guid - require
              ColumnSet            Microsoft.Xrm.Sdk.Query.ColumnSet
              ProcessInstanceId    Microsoft.Xrm.Sdk.EntityReference
           OutputParameters:
              Entity               Microsoft.Xrm.Sdk.Entity - require
        */

        //private readonly string unSecureConfiguration = null;
        //private readonly string secureConfiguration = null;

        //public PostSalesOrderConvertSalesOrderToInvoiceAsynchronous(string unSecureConfiguration, string secureConfiguration)
        //{
        //    this.unSecureConfiguration = unSecureConfiguration;
        //    this.secureConfiguration = secureConfiguration;
        //}

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var tracing = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            if (context.Stage != (int)StageEnum.PostOperation) throw new InvalidPluginExecutionException("Stage does not equals PostOperation");
            if (context.PrimaryEntityName.ToLower() != "salesorder".ToLower()) throw new InvalidPluginExecutionException("PrimaryEntityName does not equals salesorder");
            if (context.MessageName.ToLower() != "ConvertSalesOrderToInvoice".ToLower()) throw new InvalidPluginExecutionException("MessageName does not equals ConvertSalesOrderToInvoice");
            if (context.Mode != (int)ExecutionModeEnum.Asynchronous) throw new InvalidPluginExecutionException("Execution does not equals Asynchronous");

            //tracing.DebugContext(context);

            ExecutePlugin(context, serviceFactory, service, tracing);
        }

        private void ExecutePlugin(IPluginExecutionContext context, IOrganizationServiceFactory serviceFactory, IOrganizationService service, ITracingService tracing)
        {
            //var target = context.InputParameterOrDefault<???>("???");
            //var preEntity = (Entity)context?.PreEntityImages?["???"];
            //var postEntity = (Entity)context?.PostEntityImages?["???"];
            //YOUR PLUGIN-CODE GO HERE

        }
    }
}
