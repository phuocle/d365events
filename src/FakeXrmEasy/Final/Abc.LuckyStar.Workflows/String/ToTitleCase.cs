using Abc.LuckyStar.Shared;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace Abc.LuckyStar.Workflows.String
{
    [CrmPluginRegistration("ToTitleCase", "ToTitleCase", "", "Abc.LuckyStar.Workflows.String", IsolationModeEnum.Sandbox, PluginType = PluginType.Workflow)]
    public class ToTitleCase : CodeActivity
    {
        //https://docs.microsoft.com/en-us/dynamics365/customer-engagement/developer/workflow/add-metadata-custom-workflow-activity

        [Input("Input")]
        [RequiredArgument]
        public InArgument<string> Input { get; set; }

        [Output("Output")]
        [RequiredArgument]
        public OutArgument<string> Output { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var workflowContext = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(workflowContext.UserId);
            var tracing = executionContext.GetExtension<ITracingService>();

            //tracing.DebugContext(workflowContext);

            ExecuteWorkflow(executionContext, workflowContext, serviceFactory, service, tracing);
        }

        private void ExecuteWorkflow(CodeActivityContext executionContext, IWorkflowContext workflowContext, IOrganizationServiceFactory serviceFactory, IOrganizationService service, ITracingService tracing)
        {
            //YOUR WORKFLOW-CODE GO HERE
            var input = Input.Get<string>(executionContext);
            if (string.IsNullOrWhiteSpace(input))
            {
                Output.Set(executionContext, input);
            }
            else
            {
                var output = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input).Trim();
                Output.Set(executionContext, output);
            }
        }
    }
}
