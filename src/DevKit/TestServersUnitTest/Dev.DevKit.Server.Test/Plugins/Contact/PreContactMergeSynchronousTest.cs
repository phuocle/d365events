﻿using Dev.DevKit.ProxyTypes;
using Dev.DevKit.Server.Plugins.Contact;
using Dev.DevKit.Shared;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Reflection;

namespace Dev.DevKit.Server.Test.Plugins.Contact
{
    [TestClass]
    public class PreContactMergeSynchronousTest
    {
        public static XrmFakedContext Context { get; set; }
        public static XrmFakedPluginExecutionContext Plugin { get; set; }
        private static string PrimaryEntityName { get; set; } = "contact";
        private static string MessageName { get; set; } = "Merge";

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
            Context.ExecutePluginWithConfigurations<PreContactMergeSynchronous>(Plugin, unsecureString, secureString);
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
                context.ExecutePluginWith<PreContactMergeSynchronous>(plugin);
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
                context.ExecutePluginWith<PreContactMergeSynchronous>(plugin);
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
                context.ExecutePluginWith<PreContactMergeSynchronous>(plugin);
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
                context.ExecutePluginWith<PreContactMergeSynchronous>(plugin);
            }, "Execution does not equals Synchronous");
        }

        [TestMethod]
        public void _05_Check_CrmPluginRegistration()
        {
            var @class = new PreContactMergeSynchronous();
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
            //var json = "";
            //var debugContext = Debug.JsonToDebugContext(json);
            //Plugin.InputParameters["???"] = (???)debugContext.InputParameters["???"];
            //run
            Context.ExecutePluginWith<PreContactMergeSynchronous>(Plugin);
            //result
            Assert.IsTrue(true);
        }
    }
}
