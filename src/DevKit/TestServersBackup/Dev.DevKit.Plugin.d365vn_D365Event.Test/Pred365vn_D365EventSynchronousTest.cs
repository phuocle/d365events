using Dev.DevKit.ProxyTypes;
using Dev.DevKit.Shared;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Reflection;

namespace Dev.DevKit.Plugind365vn_D365Event.Test
{
    [TestClass]
    public class Pred365vn_D365EventSynchronousTest
    {
        public static XrmFakedContext Context { get; set; }
        public static XrmFakedPluginExecutionContext Plugin { get; set; }
        private static string PrimaryEntityName { get; set; } = "d365vn_d365event";
        private static string MessageName { get; set; } = "Update";

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
            Context.ExecutePluginWithConfigurations<Pred365vn_D365EventSynchronous>(Plugin, unsecureString, secureString);
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
                context.ExecutePluginWith<Pred365vn_D365EventSynchronous>(plugin);
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
                context.ExecutePluginWith<Pred365vn_D365EventSynchronous>(plugin);
            }, $"PrimaryEntityName does not equals {PrimaryEntityName}");
        }

        //[TestMethod]
        //public void _03_Check_MessageName()
        //{
        //    var context = new XrmFakedContext();
        //    var plugin = context.GetDefaultPluginContext();
        //    plugin.Stage = (int)StageEnum.PreOperation;
        //    plugin.PrimaryEntityName = PrimaryEntityName;
        //    plugin.MessageName = "abcd";
        //    Assert.ThrowsException<InvalidPluginExecutionException>(() =>
        //    {
        //        context.ExecutePluginWith<Pred365vn_D365EventSynchronous>(plugin);
        //    }, $"MessageName does not equals {MessageName}");
        //}

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
                context.ExecutePluginWith<Pred365vn_D365EventSynchronous>(plugin);
            }, "Execution does not equals Synchronous");
        }

        [TestMethod]
        public void _05_Check_CrmPluginRegistration()
        {
            var @class = new Pred365vn_D365EventSynchronous();
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
            var json = "{'BusinessUnitId':'e8ba95ba-8111-ec11-b6e5-000d3a3acbcf','CorrelationId':'3cd35fea-333e-4066-92cf-9a5cc2394518','Depth':1,'InitiatingUserId':'366e263a-d911-ec11-b6e5-000d3a3acbcf','IsExecutingOffline':false,'IsInTransaction':true,'IsOfflinePlayback':false,'IsolationMode':2,'MessageName':'Update','Mode':0,'OperationCreatedOn':'2021-09-25T11:05:02Z','OperationId':'bf0478e4-b830-49b5-9012-77713cda04d4','OrganizationId':'0ec1b318-3e75-49e6-9f6b-8ba6da999eac','OrganizationName':'unq0ec1b3183e7549e69f6b8ba6da999','OwningExtension':{'EntityReferenceId':'ef842e42-f01d-ec11-b6e6-000d3a3659e3','LogicalName':'sdkmessageprocessingstep'},'PrimaryEntityId':'97fb2164-f01d-ec11-b6e6-000d3a3659e3','PrimaryEntityName':'d365vn_d365event','RequestId':'bf0478e4-b830-49b5-9012-77713cda04d4','SecondaryEntityName':'none','SharedVariables':[{'Key':'IsAutoTransact','Value':true},{'Key':'x-ms-app-name','Value':'d365vn_D365Events'}],'UserId':'366e263a-d911-ec11-b6e5-000d3a3acbcf','InputParameters':{'Target':{'Attributes':[{'LogicalName':'d365vn_value2','Type':'int','Value':3},{'LogicalName':'d365vn_d365eventid','Type':'Guid','Value':'97fb2164-f01d-ec11-b6e6-000d3a3659e3'},{'LogicalName':'modifiedon','Type':'DateTime','Value':'2021-09-25T11:05:02Z'},{'LogicalName':'modifiedby','Type':'EntityReference','Value':'366e263a-d911-ec11-b6e5-000d3a3acbcf','EntityLogicalName':'systemuser'}],'FormattedValues':null,'KeyAttributes':[],'EntityId':'97fb2164-f01d-ec11-b6e6-000d3a3659e3','LogicalName':'d365vn_d365event','RowVersion':null},'ConcurrencyBehavior':0},'OutputParameters':null,'PostEntityImages':null,'PreEntityImages':{'PreImage':{'Attributes':[{'LogicalName':'d365vn_value1','Type':'int','Value':1},{'LogicalName':'d365vn_value2','Type':'int','Value':2},{'LogicalName':'d365vn_value3','Type':'int','Value':3}],'FormattedValues':{'d365vn_value1':'1','d365vn_value2':'2','d365vn_value3':'3'},'KeyAttributes':[],'EntityId':'97fb2164-f01d-ec11-b6e6-000d3a3659e3','LogicalName':'d365vn_d365event','RowVersion':null}}}";
            var debugContext = Debug.JsonToDebugContext(json);
            Plugin.InputParameters["Target"] = (Entity)debugContext.InputParameters["Target"];
            Plugin.PreEntityImages["PreImage"] = (Entity)debugContext.PreEntityImages["PreImage"];
            //run
            Context.ExecutePluginWith<Pred365vn_D365EventSynchronous>(Plugin);
            //result
            var entity = (Entity)Plugin.InputParameters["Target"];
            Assert.AreEqual(entity.GetAttributeValue<int?>("d365vn_valuesum").Value, 7);
        }
    }
}
