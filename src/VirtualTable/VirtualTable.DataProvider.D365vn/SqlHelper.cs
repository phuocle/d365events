using MarkMpn.Sql4Cds.Engine;
using MarkMpn.Sql4Cds.Engine.FetchXml;
using Microsoft.Xrm.Sdk;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using VirtualTable.Shared.Entities;

namespace VirtualTable.DataProvider.D365vn
{
    public class SqlHelper
    {
        internal static EntityCollection RetrieveMultiple(string fetchXml, d365vn_sqldatasource setting, IPluginExecutionContext context, IOrganizationService service, ITracingService tracing)
        {
            tracing.DebugMessage("BEGIN RetrieveMultiple");

            if (setting.d365vn_SqlConnectionString == null)
                throw new InvalidPluginExecutionException("SqlConnectionString is null");
            var metadata = new AttributeMetadataCache(service);
            var fetch = Deserialize(fetchXml);
            var mapper = new Mapper(context, service, tracing);
            int page = -1;
            int count = -1;
            if (!string.IsNullOrEmpty(fetch.page))
            {
                page = Int32.Parse(fetch.page);
                fetch.page = string.Empty;
            }
            if (!string.IsNullOrEmpty(fetch.count))
            {
                count = Int32.Parse(fetch.count);
                fetch.count = string.Empty;
            }

            tracing.DebugMessage(fetchXml);

            var sql = FetchXml2Sql.Convert(service, metadata, fetch, new FetchXml2SqlOptions { PreserveFetchXmlOperatorsAsFunctions = false }, out _);

            tracing.DebugMessage(sql);

            sql = mapper.MapVirtualEntityAttributes(sql);
            EntityCollection collection = null;
            if (page != -1 && count != -1)
            {
                collection = GetEntitiesFromSql(context, mapper, setting.d365vn_SqlConnectionString, sql, count, page);
            }
            else
            {
                collection = GetEntitiesFromSql(context, mapper, setting.d365vn_SqlConnectionString, sql, -1, 1);
            }

            tracing.DebugMessage("END RetrieveMultiple");

            return collection;
        }

        private static EntityCollection GetEntitiesFromSql(IPluginExecutionContext context, Mapper mapper, string sqlConnectionString, string sql, int pageSize, int pageNumber)
        {
            var collection = new EntityCollection();
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, sqlConnection);
                DataSet dataSet = new DataSet();
                sqlConnection.Open();
                sqlDataAdapter.Fill(dataSet, "SqlData");
                sqlConnection.Close();
                collection = mapper.CreateEntities(dataSet, pageSize, pageNumber);
            }
            return collection;
        }

        private static FetchType Deserialize(string fetchXml)
        {
            var serializer = new XmlSerializer(typeof(FetchType));
            object result;
            using (TextReader reader = new StringReader(fetchXml))
            {
                result = serializer.Deserialize(reader);
            }
            return result as FetchType;
        }

    }
}
