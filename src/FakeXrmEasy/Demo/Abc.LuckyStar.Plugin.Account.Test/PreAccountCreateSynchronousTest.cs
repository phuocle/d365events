using Abc.LuckyStar.ProxyTypes;
using Abc.LuckyStar.Shared;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Reflection;

namespace Abc.LuckyStar.PluginAccount.Test
{
    [TestClass]
    public class PreAccountCreateSynchronousTest
    {
        public static XrmFakedContext Context { get; set; }
        public static XrmFakedPluginExecutionContext Plugin { get; set; }
        private static string PrimaryEntityName { get; set; } = "account";
        private static string MessageName { get; set; } = "create";

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            Context = new XrmFakedContext();
            Context.ProxyTypesAssembly = Assembly.GetAssembly(typeof(ProxyTypesAssembly));
            Plugin = Context.GetDefaultPluginContext();
            Plugin.PrimaryEntityName = PrimaryEntityName;
            Plugin.MessageName = MessageName;
            Plugin.Stage = (int)StageEnum.PreOperation;
            Plugin.Mode = (int)ExecutionModeEnum.Synchronous;
        }

        /*
        [TestMethod]
        public void _00_Check_UnsecureString_And_SecureString()
        {
            var target = new Entity(PrimaryEntityName)
            {
                [$"{PrimaryEntityName}id"] = Guid.NewGuid()
            };
            Plugin.InputParameters["Target"] = target;
            var unsecureString = "UnsecureString";
            var secureString = "SecureString";
            Context.ExecutePluginWithConfigurations<PreAccountCreateSynchronous>(Plugin, unsecureString, secureString);
            Assert.IsTrue(target != null);
        }
        */

        [TestMethod]
        public void _01_Check_Stage()
        {
            var context = new XrmFakedContext();
            var plugin = context.GetDefaultPluginContext();
            plugin.Stage = -1;
            Assert.ThrowsException<InvalidPluginExecutionException>(() =>
            {
                context.ExecutePluginWith<PreAccountCreateSynchronous>(plugin);
            }, "Stage does not equals PreOperation");
        }

        [TestMethod]
        public void _02_Check_PrimaryEntityName()
        {
            var context = new XrmFakedContext();
            var plugin = context.GetDefaultPluginContext();
            plugin.Stage = (int)StageEnum.PreOperation;
            plugin.PrimaryEntityName = "abcd";
            Assert.ThrowsException<InvalidPluginExecutionException>(() =>
            {
                context.ExecutePluginWith<PreAccountCreateSynchronous>(plugin);
            }, $"PrimaryEntityName does not equals {PrimaryEntityName}");
        }

        [TestMethod]
        public void _03_Check_MessageName()
        {
            var context = new XrmFakedContext();
            var plugin = context.GetDefaultPluginContext();
            plugin.Stage = (int)StageEnum.PreOperation;
            plugin.PrimaryEntityName = PrimaryEntityName;
            plugin.MessageName = "abcd";
            Assert.ThrowsException<InvalidPluginExecutionException>(() =>
            {
                context.ExecutePluginWith<PreAccountCreateSynchronous>(plugin);
            }, $"MessageName does not equals {MessageName}");
        }

        [TestMethod]
        public void _04_Check_Mode()
        {
            var context = new XrmFakedContext();
            var plugin = context.GetDefaultPluginContext();
            plugin.Stage = (int)StageEnum.PreOperation;
            plugin.PrimaryEntityName = PrimaryEntityName;
            plugin.MessageName = MessageName;
            plugin.Mode = -1;
            Assert.ThrowsException<InvalidPluginExecutionException>(() =>
            {
                context.ExecutePluginWith<PreAccountCreateSynchronous>(plugin);
            }, "Execution does not equals Synchronous");
        }

        [TestMethod]
        public void _05_Check_CrmPluginRegistration()
        {
            var @class = new PreAccountCreateSynchronous();
            foreach (var attribute in System.Attribute.GetCustomAttributes(@class.GetType()))
            {
                if (attribute.GetType().Equals(typeof(CrmPluginRegistrationAttribute)))
                {
                    var check = attribute as CrmPluginRegistrationAttribute;
                    Assert.IsNotNull(check);
                }
                else
                    Assert.Fail();
            }
        }

        [TestMethod]
        public void _06_ExecutePlugin()
        {
            //setup
            var json = "{'BusinessUnitId':'e76d01d5-8497-ec11-b400-000d3a850537','CorrelationId':'0355e521-ebd8-4815-8e59-eb5d2bd9372d','Depth':1,'InitiatingUserId':'25b7f9da-8497-ec11-b400-000d3a850537','IsExecutingOffline':false,'IsInTransaction':true,'IsOfflinePlayback':false,'IsolationMode':2,'MessageName':'Create','Mode':0,'OperationCreatedOn':'2022-03-19T12:39:49Z','OperationId':'07f89c93-6a2c-44c7-b991-d35bed62250d','OrganizationId':'a606b758-8b73-459d-b532-ffdedf671283','OrganizationName':'unqa606b7588b73459db532ffdedf671','OwningExtension':{'EntityReferenceId':'667daab0-7ea7-ec11-b3fe-000d3a8572c9','LogicalName':'sdkmessageprocessingstep'},'PrimaryEntityId':'e4a6c7a9-81a7-ec11-b3fe-000d3a8572c9','PrimaryEntityName':'account','RequestId':'07f89c93-6a2c-44c7-b991-d35bed62250d','SecondaryEntityName':'none','SharedVariables':[{'Key':'IsAutoTransact','Value':true},{'Key':'x-ms-app-name','Value':'abc_LuckyStar'},{'Key':'DefaultsAddedFlag','Value':true}],'UserId':'25b7f9da-8497-ec11-b400-000d3a850537','InputParameters':{'Target':{'Attributes':[{'LogicalName':'territorycode','Type':'OptionSetValue','Value':1},{'LogicalName':'owningbusinessunit','Type':'EntityReference','Value':'e76d01d5-8497-ec11-b400-000d3a850537','EntityLogicalName':'businessunit'},{'LogicalName':'address2_shippingmethodcode','Type':'OptionSetValue','Value':1},{'LogicalName':'isprivate','Type':'bool','Value':false},{'LogicalName':'followemail','Type':'bool','Value':true},{'LogicalName':'donotbulkemail','Type':'bool','Value':false},{'LogicalName':'donotsendmm','Type':'bool','Value':false},{'LogicalName':'createdon','Type':'DateTime','Value':'2022-03-19T12:39:49Z'},{'LogicalName':'statecode','Type':'OptionSetValue','Value':0},{'LogicalName':'businesstypecode','Type':'OptionSetValue','Value':1},{'LogicalName':'donotpostalmail','Type':'bool','Value':false},{'LogicalName':'ownerid','Type':'EntityReference','Value':'25b7f9da-8497-ec11-b400-000d3a850537','EntityLogicalName':'systemuser'},{'LogicalName':'donotbulkpostalmail','Type':'bool','Value':false},{'LogicalName':'name','Type':'string','Value':'BBBBB'},{'LogicalName':'donotemail','Type':'bool','Value':false},{'LogicalName':'address2_addresstypecode','Type':'OptionSetValue','Value':1},{'LogicalName':'donotphone','Type':'bool','Value':false},{'LogicalName':'transactioncurrencyid','Type':'EntityReference','Value':'bd8cf96c-a497-ec11-b400-000d3a850537','EntityLogicalName':'transactioncurrency'},{'LogicalName':'modifiedby','Type':'EntityReference','Value':'25b7f9da-8497-ec11-b400-000d3a850537','EntityLogicalName':'systemuser'},{'LogicalName':'statuscode','Type':'OptionSetValue','Value':1},{'LogicalName':'preferredcontactmethodcode','Type':'OptionSetValue','Value':1},{'LogicalName':'accountratingcode','Type':'OptionSetValue','Value':1},{'LogicalName':'accountid','Type':'Guid','Value':'e4a6c7a9-81a7-ec11-b3fe-000d3a8572c9'},{'LogicalName':'donotfax','Type':'bool','Value':false},{'LogicalName':'merged','Type':'bool','Value':false},{'LogicalName':'createdby','Type':'EntityReference','Value':'25b7f9da-8497-ec11-b400-000d3a850537','EntityLogicalName':'systemuser'},{'LogicalName':'marketingonly','Type':'bool','Value':false},{'LogicalName':'customersizecode','Type':'OptionSetValue','Value':1},{'LogicalName':'shippingmethodcode','Type':'OptionSetValue','Value':1},{'LogicalName':'creditonhold','Type':'bool','Value':false},{'LogicalName':'modifiedon','Type':'DateTime','Value':'2022-03-19T12:39:49Z'},{'LogicalName':'participatesinworkflow','Type':'bool','Value':false},{'LogicalName':'accountclassificationcode','Type':'OptionSetValue','Value':1},{'LogicalName':'address2_freighttermscode','Type':'OptionSetValue','Value':1}],'FormattedValues':{'territorycode':'Default Value','address2_shippingmethodcode':'Default Value','isprivate':'No','followemail':'Allow','donotbulkemail':'Allow','donotsendmm':'Send','createdon':'2022-03-19T19:39:49+07:00','statecode':'Active','businesstypecode':'Default Value','donotpostalmail':'Allow','donotbulkpostalmail':'No','donotemail':'Allow','address2_addresstypecode':'Default Value','donotphone':'Allow','statuscode':'Active','preferredcontactmethodcode':'Any','accountratingcode':'Default Value','donotfax':'Allow','merged':'No','marketingonly':'No','customersizecode':'Default Value','shippingmethodcode':'Default Value','creditonhold':'No','modifiedon':'2022-03-19T19:39:49+07:00','participatesinworkflow':'No','accountclassificationcode':'Default Value','address2_freighttermscode':'Default Value'},'KeyAttributes':[],'EntityId':'e4a6c7a9-81a7-ec11-b3fe-000d3a8572c9','LogicalName':'account','RowVersion':null}},'OutputParameters':null,'PostEntityImages':null,'PreEntityImages':null}";
            var debugContext = Debug.JsonToDebugContext(json);
            Plugin.InputParameters["Target"] = (Entity)debugContext.InputParameters["Target"];

            //run
            Context.ExecutePluginWith<PreAccountCreateSynchronous>(Plugin);
            //result
            var target = (Entity)Plugin.InputParameters["Target"];
            Assert.IsTrue(target.Contains("accountnumber"));
            Assert.AreEqual(target.GetAttributeValue<string>("accountnumber"), "ACC-00002");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void _07_ExecutePlugin()
        {
            //setup
            var json = "{'BusinessUnitId':'e76d01d5-8497-ec11-b400-000d3a850537','CorrelationId':'0355e521-ebd8-4815-8e59-eb5d2bd9372d','Depth':1,'InitiatingUserId':'25b7f9da-8497-ec11-b400-000d3a850537','IsExecutingOffline':false,'IsInTransaction':true,'IsOfflinePlayback':false,'IsolationMode':2,'MessageName':'Create','Mode':0,'OperationCreatedOn':'2022-03-19T12:39:49Z','OperationId':'07f89c93-6a2c-44c7-b991-d35bed62250d','OrganizationId':'a606b758-8b73-459d-b532-ffdedf671283','OrganizationName':'unqa606b7588b73459db532ffdedf671','OwningExtension':{'EntityReferenceId':'667daab0-7ea7-ec11-b3fe-000d3a8572c9','LogicalName':'sdkmessageprocessingstep'},'PrimaryEntityId':'e4a6c7a9-81a7-ec11-b3fe-000d3a8572c9','PrimaryEntityName':'account','RequestId':'07f89c93-6a2c-44c7-b991-d35bed62250d','SecondaryEntityName':'none','SharedVariables':[{'Key':'IsAutoTransact','Value':true},{'Key':'x-ms-app-name','Value':'abc_LuckyStar'},{'Key':'DefaultsAddedFlag','Value':true}],'UserId':'25b7f9da-8497-ec11-b400-000d3a850537','InputParameters':{'Target':{'Attributes':[{'LogicalName':'territorycode','Type':'OptionSetValue','Value':1},{'LogicalName':'owningbusinessunit','Type':'EntityReference','Value':'e76d01d5-8497-ec11-b400-000d3a850537','EntityLogicalName':'businessunit'},{'LogicalName':'address2_shippingmethodcode','Type':'OptionSetValue','Value':1},{'LogicalName':'isprivate','Type':'bool','Value':false},{'LogicalName':'followemail','Type':'bool','Value':true},{'LogicalName':'donotbulkemail','Type':'bool','Value':false},{'LogicalName':'donotsendmm','Type':'bool','Value':false},{'LogicalName':'createdon','Type':'DateTime','Value':'2022-03-19T12:39:49Z'},{'LogicalName':'statecode','Type':'OptionSetValue','Value':0},{'LogicalName':'businesstypecode','Type':'OptionSetValue','Value':1},{'LogicalName':'donotpostalmail','Type':'bool','Value':false},{'LogicalName':'ownerid','Type':'EntityReference','Value':'25b7f9da-8497-ec11-b400-000d3a850537','EntityLogicalName':'systemuser'},{'LogicalName':'donotbulkpostalmail','Type':'bool','Value':false},{'LogicalName':'name','Type':'string','Value':'BBBBB'},{'LogicalName':'donotemail','Type':'bool','Value':false},{'LogicalName':'address2_addresstypecode','Type':'OptionSetValue','Value':1},{'LogicalName':'donotphone','Type':'bool','Value':false},{'LogicalName':'transactioncurrencyid','Type':'EntityReference','Value':'bd8cf96c-a497-ec11-b400-000d3a850537','EntityLogicalName':'transactioncurrency'},{'LogicalName':'modifiedby','Type':'EntityReference','Value':'25b7f9da-8497-ec11-b400-000d3a850537','EntityLogicalName':'systemuser'},{'LogicalName':'statuscode','Type':'OptionSetValue','Value':1},{'LogicalName':'preferredcontactmethodcode','Type':'OptionSetValue','Value':1},{'LogicalName':'accountratingcode','Type':'OptionSetValue','Value':1},{'LogicalName':'accountid','Type':'Guid','Value':'e4a6c7a9-81a7-ec11-b3fe-000d3a8572c9'},{'LogicalName':'donotfax','Type':'bool','Value':false},{'LogicalName':'merged','Type':'bool','Value':false},{'LogicalName':'createdby','Type':'EntityReference','Value':'25b7f9da-8497-ec11-b400-000d3a850537','EntityLogicalName':'systemuser'},{'LogicalName':'marketingonly','Type':'bool','Value':false},{'LogicalName':'customersizecode','Type':'OptionSetValue','Value':1},{'LogicalName':'shippingmethodcode','Type':'OptionSetValue','Value':1},{'LogicalName':'creditonhold','Type':'bool','Value':false},{'LogicalName':'modifiedon','Type':'DateTime','Value':'2022-03-19T12:39:49Z'},{'LogicalName':'participatesinworkflow','Type':'bool','Value':false},{'LogicalName':'accountclassificationcode','Type':'OptionSetValue','Value':1},{'LogicalName':'address2_freighttermscode','Type':'OptionSetValue','Value':1}],'FormattedValues':{'territorycode':'Default Value','address2_shippingmethodcode':'Default Value','isprivate':'No','followemail':'Allow','donotbulkemail':'Allow','donotsendmm':'Send','createdon':'2022-03-19T19:39:49+07:00','statecode':'Active','businesstypecode':'Default Value','donotpostalmail':'Allow','donotbulkpostalmail':'No','donotemail':'Allow','address2_addresstypecode':'Default Value','donotphone':'Allow','statuscode':'Active','preferredcontactmethodcode':'Any','accountratingcode':'Default Value','donotfax':'Allow','merged':'No','marketingonly':'No','customersizecode':'Default Value','shippingmethodcode':'Default Value','creditonhold':'No','modifiedon':'2022-03-19T19:39:49+07:00','participatesinworkflow':'No','accountclassificationcode':'Default Value','address2_freighttermscode':'Default Value'},'KeyAttributes':[],'EntityId':'e4a6c7a9-81a7-ec11-b3fe-000d3a8572c9','LogicalName':'account','RowVersion':null}},'OutputParameters':null,'PostEntityImages':null,'PreEntityImages':null}";
            var debugContext = Debug.JsonToDebugContext(json);
            Plugin.InputParameters["Target"] = (Entity)debugContext.InputParameters["Target"];

            var account1 = new Entity("account", Guid.NewGuid()) { ["accountnumber"] = "ACC-00003" };
            var account2 = new Entity("account", Guid.NewGuid()) { ["accountnumber"] = "ACC-00005" };
            Context.Data.Add("account", new System.Collections.Generic.Dictionary<Guid, Entity> {
                { account1.Id, account1 }, { account2.Id, account2 }
            });
            //run
            Context.ExecutePluginWith<PreAccountCreateSynchronous>(Plugin);
            //result
            var target = (Entity)Plugin.InputParameters["Target"];
            Assert.IsTrue(target.Contains("accountnumber"));
            Assert.AreEqual(target.GetAttributeValue<string>("accountnumber"), "ACC-00007");
            Assert.IsTrue(true);
        }
    }
}
