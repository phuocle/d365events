﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using System;
using VirtualTable.Shared;
using VirtualTable.Shared.Entities;

namespace VirtualTable.DataProvider.D365vn
{
    [CrmPluginRegistration("VirtualTable.DataProvider.D365vn.Create", VirtualTablePlugin.Create)]
    public class Create : IPlugin
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

        //private readonly string _unsecureString = null;
        //private readonly string _secureString = null;

        //public Create(string unsecureString, string secureString)
        //{
        //    if (!string.IsNullOrWhiteSpace(unsecureString)) _unsecureString = unsecureString;
        //    if (!string.IsNullOrWhiteSpace(secureString)) _secureString = secureString;
        //}

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var tracing = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var retriever = serviceProvider.Get<IEntityDataSourceRetrieverService>();
            var dataSource = retriever.RetrieveEntityDataSource();

            //tracing.DebugMessage("Begin Data Provider: VirtualTable.DataProvider.D365vn.Create");
            //tracing.DebugContext(context);

            ExecutePlugin(context, serviceFactory, service, tracing, dataSource);

            //tracing.DebugMessage("End Data Provider: VirtualTable.DataProvider.D365vn.Create");
        }

        private void ExecutePlugin(IPluginExecutionContext context, IOrganizationServiceFactory serviceFactory, IOrganizationService service, ITracingService tracing, Entity dataSource)
        {
            //Get Parameter from DataSource
            //var ??? = dataSource.GetAttributeValue<string>("???");
            //var ??? = dataSource.GetAttributeValue<int>("???");

            //var target = context.InputParameterOrDefault<Entity>("Target");
            //var id = Guid.NewGuid();

            var setting = new d365vn_sqldatasource(dataSource);
            context.OutputParameters["id"] = SqlHelper.Create(setting, context, service, tracing);
        }
    }
}
