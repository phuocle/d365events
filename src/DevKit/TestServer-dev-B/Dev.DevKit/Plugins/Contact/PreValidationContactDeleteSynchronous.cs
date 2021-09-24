using Dev.DevKit.Shared;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using System;

namespace Dev.DevKit.Plugins.Contact
{
    [CrmPluginRegistration("Delete", "contact", StageEnum.PreValidation, ExecutionModeEnum.Synchronous, "",
    "Dev.DevKit.Plugins.Contact.PreValidationContactDeleteSynchronous", 1/*ExecutionOrder*/, IsolationModeEnum.Sandbox, PluginType = PluginType.Plugin,
    Image1Name = "", Image1Alias = "", Image1Type = ImageTypeEnum.PreImage, Image1Attributes = "")]
    public class PreValidationContactDeleteSynchronous : IPlugin
    {
        /*
          InputParameters:
              Target                 Microsoft.Xrm.Sdk.EntityReference - require
              SolutionUniqueName     System.String
              ConcurrencyBehavior    Microsoft.Xrm.Sdk.ConcurrencyBehavior
           OutputParameters:
        */

        //private readonly string unSecureConfiguration = null;
        //private readonly string secureConfiguration = null;

        //public PreValidationContactDeleteSynchronous(string unSecureConfiguration, string secureConfiguration)
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
            if (context.PrimaryEntityName.ToLower() != "contact".ToLower()) throw new InvalidPluginExecutionException("PrimaryEntityName does not equals contact");
            if (context.MessageName.ToLower() != "Delete".ToLower()) throw new InvalidPluginExecutionException("MessageName does not equals Delete");
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
