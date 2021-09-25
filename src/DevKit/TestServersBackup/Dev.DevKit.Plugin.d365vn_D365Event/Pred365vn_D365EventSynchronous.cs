using Dev.DevKit.Shared;
using Dev.DevKit.Shared.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using System;

namespace Dev.DevKit.Plugind365vn_D365Event
{
    [CrmPluginRegistration("Create", "d365vn_d365event", StageEnum.PreOperation, ExecutionModeEnum.Synchronous, "",
    "Dev.DevKit.Plugind365vn_D365Event.Pred365vn_D365EventCreateSynchronous", 1/*ExecutionOrder*/, IsolationModeEnum.Sandbox, PluginType = PluginType.Plugin,
    Image1Name = "", Image1Alias = "", Image1Type = ImageTypeEnum.PreImage, Image1Attributes = "")]
    [CrmPluginRegistration("Update", "d365vn_d365event", StageEnum.PreOperation, ExecutionModeEnum.Synchronous, "d365vn_value1,d365vn_value2,d365vn_value3",
    "Dev.DevKit.Plugind365vn_D365Event.Pred365vn_D365EventUpdateSynchronous", 1/*ExecutionOrder*/, IsolationModeEnum.Sandbox, PluginType = PluginType.Plugin,
    Image1Name = "PreImage", Image1Alias = "PreImage", Image1Type = ImageTypeEnum.PreImage, Image1Attributes = "d365vn_value1,d365vn_value2,d365vn_value3")]
    public class Pred365vn_D365EventSynchronous : IPlugin
    {
        /*
          InputParameters:
              Target                             Microsoft.Xrm.Sdk.Entity - require
              SuppressDuplicateDetection         System.Boolean
              CalculateMatchCodeSynchronously    System.Boolean
              SolutionUniqueName                 System.String
              MaintainLegacyAppServerBehavior    System.Boolean
              ReturnRowVersion                   System.Boolean
           OutputParameters:
              id                                 System.Guid - require
        */

        //private readonly string unSecureConfiguration = null;
        //private readonly string secureConfiguration = null;

        //public Pred365vn_D365EventSynchronous(string unSecureConfiguration, string secureConfiguration)
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
            if (context.Stage != (int)StageEnum.PreOperation) throw new InvalidPluginExecutionException("Stage does not equals PreOperation");
            if (context.PrimaryEntityName.ToLower() != "d365vn_d365event".ToLower()) throw new InvalidPluginExecutionException("PrimaryEntityName does not equals d365vn_d365event");
            //if (context.MessageName.ToLower() != "Create".ToLower()) throw new InvalidPluginExecutionException("MessageName does not equals Create");
            if (context.Mode != (int)ExecutionModeEnum.Synchronous) throw new InvalidPluginExecutionException("Execution does not equals Synchronous");

            //tracing.DebugContext(context);

            ExecutePlugin(context, serviceFactory, service, tracing);
        }

        private void ExecutePlugin(IPluginExecutionContext context, IOrganizationServiceFactory serviceFactory, IOrganizationService service, ITracingService tracing)
        {
            var target = context.InputParameterOrDefault<Entity>("Target");
            var preEntity = context.MessageName.ToLower() == "Create".ToLower() ? new Entity() : context?.PreEntityImages?["PreImage"];
            //var postEntity = (Entity)context?.PostEntityImages?["???"];
            //YOUR PLUGIN-CODE GO HERE
            var merged = new d365vn_D365Event(preEntity, target);
            var result = new d365vn_D365Event(target);
            result.d365vn_ValueSum = (merged.d365vn_Value1 ?? 0) + (merged.d365vn_Value2 ?? 0) + (merged.d365vn_Value3 ?? 0);
        }
    }
}
