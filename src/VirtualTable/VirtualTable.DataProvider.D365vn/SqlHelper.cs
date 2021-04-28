using MarkMpn.Sql4Cds.Engine;
using MarkMpn.Sql4Cds.Engine.FetchXml;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using System;
using System.Collections.Generic;
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
            EntityCollection collection;
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

        internal static void Update(d365vn_sqldatasource setting, IPluginExecutionContext context, IOrganizationService service, ITracingService tracing)
        {
            tracing.DebugMessage("BEGIN Update");

            if (setting.d365vn_SqlConnectionString == null)
                throw new InvalidPluginExecutionException("SqlConnectionString is null");

            var mapper = new Mapper(context, service, tracing);
            var mappings = mapper.GetCustomMappings();
            var entity = context.InputParameterOrDefault<Entity>("Target");
            string sql = $"UPDATE {mappings[context.PrimaryEntityName]} SET {{0}} WHERE {mappings[mapper.PrimaryEntityMetadata.PrimaryIdAttribute]} = '{context.PrimaryEntityId}'";
            using (SqlConnection sqlConnection = new SqlConnection(setting.d365vn_SqlConnectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    List<string> setList = new List<string>();
                    foreach (var attribute in entity.Attributes)
                    {
                        if (attribute.Key == mapper.PrimaryEntityMetadata.PrimaryIdAttribute) continue;
                        if (mappings[attribute.Key] == setting.d365vn_ExternalCreatedOnField || mappings[attribute.Key] == setting.d365vn_ExternalModifiedOnField) continue;
                        command.Parameters.AddWithValue($"@{mappings[attribute.Key]}", GetValueOfAttribute(attribute.Value));
                        setList.Add($"{mappings[attribute.Key]}=@{mappings[attribute.Key]}");
                    }
                    if (setting.d365vn_ExternalModifiedOnField != null)
                    {
                        setList.Add($"{setting.d365vn_ExternalModifiedOnField}=@{setting.d365vn_ExternalModifiedOnField}");
                        command.Parameters.AddWithValue($"@{setting.d365vn_ExternalModifiedOnField}", DateTime.UtcNow);
                    }
                    sql = string.Format(sql, string.Join(", ", setList));
                    command.CommandText = sql;

                    tracing.DebugMessage(ConvertSqlCommandToSql(command));

                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }

            tracing.DebugMessage("END Update");
        }

        internal static void Delete(d365vn_sqldatasource setting, IPluginExecutionContext context, IOrganizationService service, ITracingService tracing)
        {
            tracing.DebugMessage("BEGIN Delete");

            if (setting.d365vn_SqlConnectionString == null)
                throw new InvalidPluginExecutionException("SqlConnectionString is null");

            var mapper = new Mapper(context, service, tracing);
            var mappings = mapper.GetCustomMappings();
            string sql = $"DELETE {mappings[context.PrimaryEntityName]} WHERE {mappings[mapper.PrimaryEntityMetadata.PrimaryIdAttribute]} = '{context.PrimaryEntityId}'";

            tracing.DebugMessage(sql);

            using (SqlConnection sqlConnection = new SqlConnection(setting.d365vn_SqlConnectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandText = sql;
                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }

            tracing.DebugMessage("END Delete");
        }

        internal static object Create(d365vn_sqldatasource setting, IPluginExecutionContext context, IOrganizationService service, ITracingService tracing)
        {
            tracing.DebugMessage("BEGIN Create");

            if (setting.d365vn_SqlConnectionString == null)
                throw new InvalidPluginExecutionException("SqlConnectionString is null");

            var mapper = new Mapper(context, service, tracing);
            var mappings = mapper.GetCustomMappings();
            var entity = context.InputParameterOrDefault<Entity>("Target");
            string sql = $"INSERT INTO {mappings[context.PrimaryEntityName]}({{0}}) VALUES ({{1}})";
            var id = Guid.NewGuid();

            using (SqlConnection sqlConnection = new SqlConnection(setting.d365vn_SqlConnectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    List<string> columns = new List<string>();
                    List<string> values = new List<string>();
                    foreach (var attribute in entity.Attributes)
                    {
                        if (mappings[attribute.Key] == setting.d365vn_ExternalCreatedOnField || mappings[attribute.Key] == setting.d365vn_ExternalModifiedOnField)
                            continue;
                        columns.Add($"{mappings[attribute.Key]}");
                        values.Add($"@{mappings[attribute.Key]}");
                        command.Parameters.AddWithValue($"@{mappings[attribute.Key]}", GetValueOfAttribute(attribute.Value));
                    }
                    columns.Add($"{mappings[mapper.PrimaryEntityMetadata.PrimaryIdAttribute]}");
                    values.Add($"@{mappings[mapper.PrimaryEntityMetadata.PrimaryIdAttribute]}");
                    command.Parameters.AddWithValue($"@{mappings[mapper.PrimaryEntityMetadata.PrimaryIdAttribute]}", id);
                    if (setting.d365vn_ExternalCreatedOnField != null)
                    {
                        columns.Add($"{setting.d365vn_ExternalCreatedOnField}");
                        values.Add($"@{setting.d365vn_ExternalCreatedOnField}");
                        command.Parameters.AddWithValue($"@{setting.d365vn_ExternalCreatedOnField}", DateTime.UtcNow);
                    }
                    if (setting.d365vn_ExternalModifiedOnField != null)
                    {
                        columns.Add($"{setting.d365vn_ExternalModifiedOnField}");
                        values.Add($"@{setting.d365vn_ExternalModifiedOnField}");
                        command.Parameters.AddWithValue($"@{setting.d365vn_ExternalModifiedOnField}", DateTime.UtcNow);
                    }
                    sql = string.Format(sql, string.Join(", ", columns), string.Join(", ", values));

                    command.CommandText = sql;

                    tracing.DebugMessage(ConvertSqlCommandToSql(command));

                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }

            tracing.DebugMessage("END Create");

            return id;
        }



        internal static Entity Retrieve(d365vn_sqldatasource setting, IPluginExecutionContext context, IOrganizationService service, ITracingService tracing)
        {
            tracing.DebugMessage("BEGIN Retrieve");

            if (setting.d365vn_SqlConnectionString == null)
                throw new InvalidPluginExecutionException("SqlConnectionString is null");

            var entity = new Entity(context.PrimaryEntityName, context.PrimaryEntityId);
            var mapper = new Mapper(context, service, tracing);
            string sql = $"SELECT * FROM {context.PrimaryEntityName} WITH(NOLOCK) WHERE {mapper.PrimaryEntityMetadata.PrimaryIdAttribute} = '{mapper.MapToVirtualEntityValue(mapper.PrimaryEntityMetadata.PrimaryIdAttribute, context.PrimaryEntityId)}'";
            sql = mapper.MapVirtualEntityAttributes(sql);

            tracing.DebugMessage(sql);

            var entities = GetEntitiesFromSql(context, mapper, setting.d365vn_SqlConnectionString, sql, 1, 1);
            if (entities.Entities != null && entities.Entities.Count > 0)
            {
                entity = entities.Entities[0];
                entity.Id = context.PrimaryEntityId;
                entity.LogicalName = context.PrimaryEntityName;
            }

            tracing.DebugMessage("END Retrieve");

            return entity;
        }

        private static object GetValueOfAttribute(object value)
        {
            if (value is AliasedValue)
                return GetValueOfAttribute(((AliasedValue)value).Value);
            else if (value is EntityReference)
                return ((EntityReference)value).Id;
            else if (value is OptionSetValue)
                return ((OptionSetValue)value).Value;
            else if (value is Money)
                return ((Money)value).Value;
            if (value != null)
                return value;
            else
                return DBNull.Value;
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

        private static string ConvertSqlCommandToSql(SqlCommand command)
        {
            var sql = command.CommandText;
            foreach (SqlParameter parameter in command.Parameters)
            {
                sql = sql.Replace(parameter?.ParameterName, parameter?.Value?.ToString());
            }
            return sql;
        }
    }
}
