﻿using Dev.DevKit.Server.CustomApis.Contact;
using Dev.DevKit.Shared;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Dev.DevKit.Server.Test.CustomApis.Contact
{
    [TestClass]
    public class devkit_CustomApiEntityCollectionRequestTest
    {
        public static XrmFakedContext Context { get; set; }
        public static XrmFakedPluginExecutionContext Plugin { get; set; }
        private static string PrimaryEntityName { get; set; } = "contact";

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            Context = new XrmFakedContext();
            Context.ProxyTypesAssembly = Assembly.GetAssembly(typeof(ProxyTypes.ProxyTypesAssembly));
            Plugin = Context.GetDefaultPluginContext();
            Plugin.PrimaryEntityName = PrimaryEntityName;
        }

        [TestMethod]
        public void _01_Check_CrmPluginRegistration()
        {
            var @class = new devkit_CustomApiEntityCollectionRequest();
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
        public void _02_ExecutePlugin()
        {
            //setup
            //var json = "";
            //var debugContext = Debug.JsonToDebugContext(json);
            //Plugin.InputParameters["???"] = (???)debugContext.InputParameters["???"];
            //run
            Context.ExecutePluginWith<devkit_CustomApiEntityCollectionRequest>(Plugin);
            //result
            Assert.IsTrue(true);
        }
    }
}
