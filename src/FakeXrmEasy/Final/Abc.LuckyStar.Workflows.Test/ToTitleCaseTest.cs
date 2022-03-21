using Abc.LuckyStar.ProxyTypes;
using Abc.LuckyStar.Workflows.String;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace Abc.LuckyStar.Workflows.Test
{
    [TestClass]
    public class ToTitleCaseTest
    {
        public static XrmFakedContext Context { get; set; }
        public static XrmFakedWorkflowContext Workflow { get; set; }

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            Context = new XrmFakedContext();
            Context.ProxyTypesAssembly = Assembly.GetAssembly(typeof(ProxyTypesAssembly));
            Workflow = Context.GetDefaultWorkflowContext();
        }

        [TestMethod]
        public void _01_ExecuteWorkflow()
        {
            //setup
            //var json = "";
            //var debugContext = Debug.JsonToDebugContext(json);
            //Plugin.InputParameters["???"] = (???)debugContext.InputParameters["???"];
            var inputs = new Dictionary<string, object>() {
                { "Input", "hello world" }
            };
            //run
            var outputs = Context.ExecuteCodeActivity<ToTitleCase>(inputs);
            //result
            var Output = (string)outputs["Output"];
            Assert.AreEqual(Output, "Hello World", false);
            Assert.AreNotEqual(Output, "hello world", false);
        }

        [TestMethod]
        public void _02_ExecuteWorkflow()
        {
            //setup
            //var json = "";
            //var debugContext = Debug.JsonToDebugContext(json);
            //Plugin.InputParameters["???"] = (???)debugContext.InputParameters["???"];
            var inputs = new Dictionary<string, object>() {
                { "Input", null }
            };
            //run
            var outputs = Context.ExecuteCodeActivity<ToTitleCase>(inputs);
            //result
            var Output = (string)outputs["Output"];
            Assert.AreEqual(Output, null);
        }
    }
}
