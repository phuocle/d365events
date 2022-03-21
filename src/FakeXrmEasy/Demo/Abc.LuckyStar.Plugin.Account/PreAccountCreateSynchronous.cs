using Abc.LuckyStar.Shared;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Abc.LuckyStar.PluginAccount
{
    [CrmPluginRegistration("Create", "account", StageEnum.PreOperation, ExecutionModeEnum.Synchronous, "",
    "Abc.LuckyStar.PluginAccount.PreAccountCreateSynchronous", 1/*ExecutionOrder*/, IsolationModeEnum.Sandbox, PluginType = PluginType.Plugin,
    Image1Name = "", Image1Alias = "", Image1Type = ImageTypeEnum.PreImage, Image1Attributes = "")]
    public class PreAccountCreateSynchronous : IPlugin
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

        //public PreAccountCreateSynchronous(string unSecureConfiguration, string secureConfiguration)
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
            if (context.PrimaryEntityName.ToLower() != "account".ToLower()) throw new InvalidPluginExecutionException("PrimaryEntityName does not equals account");
            if (context.MessageName.ToLower() != "Create".ToLower()) throw new InvalidPluginExecutionException("MessageName does not equals Create");
            if (context.Mode != (int)ExecutionModeEnum.Synchronous) throw new InvalidPluginExecutionException("Execution does not equals Synchronous");

            tracing.DebugContext(context);

            ExecutePlugin(context, serviceFactory, service, tracing);
        }

        private void ExecutePlugin(IPluginExecutionContext context, IOrganizationServiceFactory serviceFactory, IOrganizationService service, ITracingService tracing)
        {
            var target = context.InputParameterOrDefault<Entity>("Target");
            //var preEntity = (Entity)context?.PreEntityImages?["???"];
            //var postEntity = (Entity)context?.PostEntityImages?["???"];
            //YOUR PLUGIN-CODE GO HERE
            SetAccountNumber(service, tracing, target);
        }

        private void SetAccountNumber(IOrganizationService service, ITracingService tracing, Entity target)
        {
            var accountNumber = GetLatestAccountNumber(service);
            if (target.Contains("Abc")) {
                target["accountnumber"] = $"ACC-{(accountNumber + 2).ToString("00000")}"; //ACC-00001 -> ... -> ACC-00003 -> ACC-00004 -> .. -> ACC-00005
            }
        }

        private int GetLatestAccountNumber(IOrganizationService service)
        {
            var fetchXml = $@"
<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='account'>
    <attribute name='accountnumber'/>
    <order attribute='accountnumber' descending='true'/>
    <filter type='and'>
      <condition attribute='accountnumber' operator='not-null'/>
    </filter>
  </entity>
</fetch>
";
            var rows = service.RetrieveMultiple(new FetchExpression(fetchXml));
            if (rows.Entities.Count == 0) return 0;
            var firstRow = rows.Entities[0];
            var accountNumber = firstRow.GetAttributeValue<string>("accountnumber");
            return int.Parse(accountNumber.Substring("ACC-".Length));
        }
    }
}
