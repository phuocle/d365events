using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using System;
using VirtualTable.Shared;
using VirtualTable.Shared.Entities;

namespace VirtualTable.DataProvider.D365vn
{
    [CrmPluginRegistration("VirtualTable.DataProvider.D365vn.Update", "Update", PluginType.DataProvider, DataSource = "d365vn_sqldatasource")]
    public class Update : IPlugin
    {
        /*
          InputParameters:
              Target                             Microsoft.Xrm.Sdk.Entity - require
              SuppressDuplicateDetection         System.Boolean
              CalculateMatchCodeSynchronously    System.Boolean
              SolutionUniqueName                 System.String
              MaintainLegacyAppServerBehavior    System.Boolean
              ConcurrencyBehavior                Microsoft.Xrm.Sdk.ConcurrencyBehavior
              ReturnRowVersion                   System.Boolean
           OutputParameters:
        */

        //private readonly string unSecureConfiguration = null;
        //private readonly string secureConfiguration = null;

        //public Update(string unSecureConfiguration, string secureConfiguration)
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
            var retriever = serviceProvider.Get<IEntityDataSourceRetrieverService>();
            var dataSource = retriever.RetrieveEntityDataSource();

            tracing.DebugMessage("Begin Data Provider: VirtualTable.DataProvider.D365vn.Update");
            tracing.DebugContext(context);
            tracing.DebugMessage(dataSource.ToDebug());

            ExecutePlugin(context, serviceFactory, service, tracing, dataSource);

            tracing.DebugMessage("End Data Provider: VirtualTable.DataProvider.D365vn.Update");
        }

        private void ExecutePlugin(IPluginExecutionContext context, IOrganizationServiceFactory serviceFactory, IOrganizationService service, ITracingService tracing, Entity dataSource)
        {
            //Get Parameter from DataSource
            //var ??? = dataSource.GetAttributeValue<string>("???");
            //var ??? = dataSource.GetAttributeValue<int>("???");

            //var target = context.InputParameterOrDefault<Entity>("Target");

            //YOUR CODE ...

            var setting = new d365vn_sqldatasource(dataSource);
            SqlHelper.Update(setting, context, service, tracing);
        }
    }
}
