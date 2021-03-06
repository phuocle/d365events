﻿//---------------------------------------------------------------------------------------------------
// <auto-generated>
//		Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
//		Generated by DynamicsCrm.DevKit - https://github.com/phuocle/Dynamics-Crm-DevKit
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using Microsoft.Xrm.Sdk;
using System;
using System.Diagnostics;

namespace VirtualTable.Shared.Entities.d365vn_sqldatasourceOptionSets
{

}

namespace VirtualTable.Shared.Entities
{
    public partial class d365vn_sqldatasource : EntityBase
    {
        public struct Fields
        {
            public const string d365vn_ExternalCreatedOnField = "d365vn_externalcreatedonfield";
            public const string d365vn_ExternalModifiedOnField = "d365vn_externalmodifiedonfield";
            public const string d365vn_name = "d365vn_name";
            public const string d365vn_SqlConnectionString = "d365vn_sqlconnectionstring";
            public const string d365vn_sqldatasourceId = "d365vn_sqldatasourceid";
        }

        public const string EntityLogicalName = "d365vn_sqldatasource";

        public const int EntityTypeCode = 10150;

        [DebuggerNonUserCode()]
        public d365vn_sqldatasource()
        {
            Entity = new Entity(EntityLogicalName);
            PreEntity = CloneThisEntity(Entity);
        }

        [DebuggerNonUserCode()]
        public d365vn_sqldatasource(Guid d365vn_sqldatasourceId)
        {
            Entity = new Entity(EntityLogicalName, d365vn_sqldatasourceId);
            PreEntity = CloneThisEntity(Entity);
        }

        [DebuggerNonUserCode()]
        public d365vn_sqldatasource(string keyName, object keyValue)
        {
            Entity = new Entity(EntityLogicalName, keyName, keyValue);
            PreEntity = CloneThisEntity(Entity);
        }

        [DebuggerNonUserCode()]
        public d365vn_sqldatasource(Entity entity)
        {
            Entity = entity;
            PreEntity = CloneThisEntity(Entity);
        }

        [DebuggerNonUserCode()]
        public d365vn_sqldatasource(Entity entity, Entity merge)
        {
            Entity = entity;
            foreach (var property in merge?.Attributes)
            {
                var key = property.Key;
                var value = property.Value;
                Entity[key] = value;
            }
            PreEntity = CloneThisEntity(Entity);
        }

        [DebuggerNonUserCode()]
        public d365vn_sqldatasource(KeyAttributeCollection keys)
        {
            Entity = new Entity(EntityLogicalName, keys);
            PreEntity = CloneThisEntity(Entity);
        }

        /// <summary>
        /// <para>String - MaxLength: 100</para>
        /// <para>External Created On Field</para>
        /// </summary>
        [DebuggerNonUserCode()]
        public string d365vn_ExternalCreatedOnField
        {
            get { return Entity.GetAttributeValue<string>(Fields.d365vn_ExternalCreatedOnField); }
            set { Entity.Attributes[Fields.d365vn_ExternalCreatedOnField] = value; }
        }

        /// <summary>
        /// <para>String - MaxLength: 100</para>
        /// <para>External Modified On Field</para>
        /// </summary>
        [DebuggerNonUserCode()]
        public string d365vn_ExternalModifiedOnField
        {
            get { return Entity.GetAttributeValue<string>(Fields.d365vn_ExternalModifiedOnField); }
            set { Entity.Attributes[Fields.d365vn_ExternalModifiedOnField] = value; }
        }

        /// <summary>
        /// <para>String - MaxLength: 100</para>
        /// <para>Sql DataSource</para>
        /// </summary>
        [DebuggerNonUserCode()]
        public string d365vn_name
        {
            get { return Entity.GetAttributeValue<string>(Fields.d365vn_name); }
            set { Entity.Attributes[Fields.d365vn_name] = value; }
        }

        /// <summary>
        /// <para>Required - String - MaxLength: 1000</para>
        /// <para>Sql Connection String</para>
        /// </summary>
        [DebuggerNonUserCode()]
        public string d365vn_SqlConnectionString
        {
            get { return Entity.GetAttributeValue<string>(Fields.d365vn_SqlConnectionString); }
            set { Entity.Attributes[Fields.d365vn_SqlConnectionString] = value; }
        }

        /// <summary>
        /// <para>Unique identifier for entity instances</para>
        /// <para>Primary Key - Uniqueidentifier</para>
        /// <para>Sql DataSource</para>
        /// </summary>
        [DebuggerNonUserCode()]
        public Guid d365vn_sqldatasourceId
        {
            get { return Id; }
            set
            {
                Entity.Attributes[Fields.d365vn_sqldatasourceId] = value;
                Entity.Id = value;
            }
        }
    }
}

