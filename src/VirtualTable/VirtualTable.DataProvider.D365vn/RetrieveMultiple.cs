﻿using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Query;
using System;
using VirtualTable.Shared;
using VirtualTable.Shared.Entities;

namespace VirtualTable.DataProvider.D365vn
{
    [CrmPluginRegistration("VirtualTable.DataProvider.D365vn.RetrieveMultiple", VirtualTablePlugin.RetrieveMultiple)]
    public class RetrieveMultiple : IPlugin
    {
        /*
          InputParameters:
              Query                 Microsoft.Xrm.Sdk.Query.QueryBase - require
              AppModuleId           System.Guid
              IsAppModuleContext    System.Boolean
           OutputParameters:
              EntityCollection      Microsoft.Xrm.Sdk.EntityCollection - require
        */

        //private readonly string _unsecureString = null;
        //private readonly string _secureString = null;

        //public RetrieveMultiple(string unsecureString, string secureString)
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

            //tracing.DebugMessage("Begin Data Provider: VirtualTable.DataProvider.D365vn.RetrieveMultiple");
            //tracing.DebugContext(context);

            ExecutePlugin(context, serviceFactory, service, tracing, dataSource);

            //tracing.DebugMessage("End Data Provider: VirtualTable.DataProvider.D365vn.RetrieveMultiple");
        }

        private void ExecutePlugin(IPluginExecutionContext context, IOrganizationServiceFactory serviceFactory, IOrganizationService service, ITracingService tracing, Entity dataSource)
        {
            //Get Parameter from DataSource
            //var ??? = dataSource.GetAttributeValue<string>("???");
            //var ??? = dataSource.GetAttributeValue<int>("???");
            var setting = new d365vn_sqldatasource(dataSource);
            var query = context.InputParameters["Query"];
            var entities = new EntityCollection();
            entities.EntityName = context.PrimaryEntityName;
            string fetchXml;
            if (query is QueryExpression qe)
            {
                //UCI grid return QueryExpression
                var convertRequest = new QueryExpressionToFetchXmlRequest();
                convertRequest.Query = (QueryExpression)qe;
                var response = (QueryExpressionToFetchXmlResponse)service.Execute(convertRequest);
                fetchXml = response.FetchXml;
            }
            else if (query is FetchExpression fe)
            {
                //Advanced Find, Classic grid return FetchExpression
                fetchXml = fe.Query;
            }
            else
                throw new InvalidPluginExecutionException("Somthing wrong with Query");

            context.OutputParameters["BusinessEntityCollection"] = SqlHelper.RetrieveMultiple(fetchXml, setting, context, service, tracing);
        }
    }
}
