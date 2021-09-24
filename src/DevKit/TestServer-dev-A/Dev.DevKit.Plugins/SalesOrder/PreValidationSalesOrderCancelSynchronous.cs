using Dev.DevKit.Shared;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using System;

namespace Dev.DevKit.Plugins.SalesOrder
{
    [CrmPluginRegistration("Cancel", "salesorder", StageEnum.PreValidation, ExecutionModeEnum.Synchronous, "",
    "Dev.DevKit.Plugins.SalesOrder.PreValidationSalesOrderCancelSynchronous", 1/*ExecutionOrder*/, IsolationModeEnum.Sandbox, PluginType = PluginType.Plugin,
    Image1Name = "", Image1Alias = "", Image1Type = ImageTypeEnum.PreImage, Image1Attributes = "")]
    public class PreValidationSalesOrderCancelSynchronous : IPlugin
    {
        /*
          InputParameters:
              OrderClose    Microsoft.Xrm.Sdk.Entity - require
              Status        Microsoft.Xrm.Sdk.OptionSetValue - require
           OutputParameters:
        */

        //private readonly string unSecureConfiguration = null;
        //private readonly string secureConfiguration = null;

        //public PreValidationSalesOrderCancelSynchronous(string unSecureConfiguration, string secureConfiguration)
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
            if (context.Stage != (int)StageEnum.PreValidation) throw new InvalidPluginExecutionException("Stage does not equals PreValidation");
            if (context.PrimaryEntityName.ToLower() != "salesorder".ToLower()) throw new InvalidPluginExecutionException("PrimaryEntityName does not equals salesorder");
            if (context.MessageName.ToLower() != "Cancel".ToLower()) throw new InvalidPluginExecutionException("MessageName does not equals Cancel");
            if (context.Mode != (int)ExecutionModeEnum.Synchronous) throw new InvalidPluginExecutionException("Execution does not equals Synchronous");

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
