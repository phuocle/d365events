"use strict";
var dialog = (function () {
    "use strict";

    const EMPTY_GUID = "00000000-0000-0000-0000-000000000000";
    const position = {
        Center: 1,
        Side: 2
    }

    async function onOpen(executionContext) {
        const options = {
            position: position.Center,
            width: 530,
            height: 270
        };
        const { userName } = Xrm.Utility.getGlobalContext().userSettings;
        const params = {
            abc_para_username: userName
        };
        const result = await Xrm.Navigation.openDialog("abc_find_user", options, params)
        debugger;
    }
    async function onLoad(executionContext) {
        const formContext = executionContext.getFormContext();
        const userName = formContext.data.attributes.get("abc_para_username").getValue();
        formContext.data.attributes.get("abc_text_username").setValue(userName);
    }
    async function onFindClick(executionContext) {
        const formContext = executionContext.getFormContext();
        const userName = formContext.data.attributes.get("abc_para_username").getValue();
        const fetchData = {
            fullname: userName
        };
        let fetchXml = `
<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='systemuser'>
    <attribute name='systemuserid'/>
    <filter type='and'>
      <condition attribute='fullname' operator='eq' value='${fetchData.fullname}'/>
    </filter>
  </entity>
</fetch>
`;
        fetchXml = "?fetchXml=" + encodeURIComponent(fetchXml);
        Xrm.Utility.showProgressIndicator("Processing ...");
        const response = await Xrm.WebApi.retrieveMultipleRecords("systemuser", fetchXml);
        Xrm.Utility.closeProgressIndicator();
        let userId = EMPTY_GUID;
        if (response.entities.length === 1) {
            const entity = response.entities[0];
            userId = entity.systemuserid;
        }
        formContext.data.attributes.get("abc_text_userid").setValue(userId.toUpperCase());
    }
    async function onCloseClick(executionContext) {
        const formContext = executionContext.getFormContext();
        const userId = formContext.data.attributes.get("abc_text_userid").getValue();
        formContext.data.attributes.get("abc_para_userid").setValue(userId);
        formContext.ui.close();
    }
    return {
        OnOpen: onOpen,
        OnLoad: onLoad,
        OnFindClick: onFindClick,
        OnCloseClick: onCloseClick
    };
})();
