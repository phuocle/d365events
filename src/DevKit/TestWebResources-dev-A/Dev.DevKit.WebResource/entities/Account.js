//@ts-check
///<reference path="Account.d.ts" />
"use strict";
var formAccount = (function () {
	"use strict";
	/** @type DevKit.FormAccount */
	var form = null;
	async function onLoad(executionContext) {
		form = new DevKit.FormAccount(executionContext);
		if (form.FormType !== OptionSet.FormType.Create) {
			form.Body.Name.Disabled = true;
		}
	}
	async function onSave(executionContext) {
	}
	async function createEmail(executionContext) {
		form = new DevKit.FormAccount(executionContext, "d365vn_/resources/Resources");

		var from = new DevKit.ActivityPartyApi();
		from.partyid_systemuser.Value = form.Utility.UserSettings.UserId;
		from.ParticipationTypeMask.Value = OptionSet.ActivityParty.ParticipationTypeMask.Sender;

		var to = new DevKit.ActivityPartyApi();
		to.partyid_account.Value = form.EntityId;
		to.ParticipationTypeMask.Value = OptionSet.ActivityParty.ParticipationTypeMask.To_Recipient;

		var email = new DevKit.EmailApi();
		email.Subject.Value = "EMAIL SUBJECT";
		email.Description.Value = "EMAIL BODY"
		email.DirectionCode.Value = true;
		email.regardingobjectid_account_email.Value = form.EntityId;
		email.PriorityCode.Value = OptionSet.Email.PriorityCode.High;
		email.ActivityParties = [from.Entity, to.Entity];

		//var createEmail = await Xrm.WebApi.createRecord(email.EntityName, email.Entity);

		form.Utility.OpenAlertDialog({ text: form.Utility.Resource("LABEL") });
	}
	async function getAccount(accountId) {
		var fetchData = {
			accountid: accountId
		};
		var fetchXml = `
<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='account'>
    <attribute name='name'/>
    <attribute name='primarycontactid'/>
    <attribute name='telephone1'/>
    <attribute name='accountid'/>
    <order attribute='name' descending='false'/>
    <filter type='and'>
      <condition attribute='accountid' operator='eq' value='${fetchData.accountid}'/>
    </filter>
  </entity>
</fetch>
`;
	/*
		{
			'@odata.etag': string
			'accountid': guid
			'name': string
			'_primarycontactid_value': guid
			'_primarycontactid_value@Microsoft.Dynamics.CRM.associatednavigationproperty': string
			'_primarycontactid_value@Microsoft.Dynamics.CRM.lookuplogicalname': string
			'_primarycontactid_value@OData.Community.Display.V1.FormattedValue': string
			'telephone1': string
		}
		*/
		fetchXml = "?fetchXml=" + encodeURIComponent(fetchXml);
		var rows = await Xrm.WebApi.retrieveMultipleRecords("account", fetchXml);
		if (rows.entities.length !== 1) throw new Error(`retrieveMultipleRecords failed with fetchXml = ${fetchXml}`);
		var entity = rows.entities[0];

		var accountId = entity.accountid;
		var name = entity.name;
		var _primarycontactid_value = entity._primarycontactid_value;
		var _primarycontactid_value_name = entity["_primarycontactid_value@OData.Community.Display.V1.FormattedValue"];
		var telephone1 = entity.telephone1;

		var account = new DevKit.AccountApi(entity);
		var _accountId = account.AccountId.Value;
		var _name = account.Name.Value;
		var _primaryContactId = account.PrimaryContactId.Value;
		var _primaryContactName = account.PrimaryContactId.FormattedValue;
		var _telephone1 = account.Telephone1.Value;
    }
	return {
		OnLoad: onLoad,
		OnSave: onSave,
		CreateEmail: createEmail
	};
})();