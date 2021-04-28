//@ts-check
"use strict";
var address = (function () {
    //@ts-check
    "use strict";
    //sync func
    function provincePreSearch(executionContext) {
        const formContext = executionContext.getFormContext();
        const provinceControl = formContext.getControl("abc_lookup_province");
        const filter = `
<filter type="and">
    <condition attribute="abc_parentlocationid" operator="null" />
</filter>`;
        provinceControl.addCustomFilter(filter, "abc_location");
    }
    function districtPreSearch(executionContext) {
        const formContext = executionContext.getFormContext();
        const districtControl = formContext.getControl("abc_lookup_district");
        const provinceAttribute = formContext.data.attributes.get("abc_lookup_province");
        const parentLocationId = provinceAttribute.getValue()[0].id;
        const filter = `
<filter type="and">
    <condition attribute="abc_parentlocationid" operator="eq" value="${parentLocationId}" />
</filter>`;
        districtControl.addCustomFilter(filter, "abc_location");
    }
    //async func
    async function onProvinceChange(executionContext) {
        const formContext = executionContext.getFormContext();
        const provinceAttribute = formContext.data.attributes.get("abc_lookup_province");
        formContext.getControl("abc_lookup_district").setDisabled(provinceAttribute.getValue() === null);
        formContext.data.attributes.get("abc_lookup_district").setValue(null);
        checkOkButton(executionContext);
    }
    async function districtOnChange(executionContext) {
        const formContext = executionContext.getFormContext();
        const districtAttribute = formContext.data.attributes.get("abc_lookup_district");
        formContext.getControl("abc_text_address").setDisabled(districtAttribute.getValue() === null);
        formContext.data.attributes.get("abc_text_address").setValue(null);
        checkOkButton(executionContext);
    }
    async function addressOnChange(executionContext) {
        checkOkButton(executionContext);
    }
    async function checkOkButton(executionContext) {
        const formContext = executionContext.getFormContext();
        const provinceAttribute = formContext.data.attributes.get("abc_lookup_province");
        const districtAttribute = formContext.data.attributes.get("abc_lookup_district");
        const addressAttribute = formContext.data.attributes.get("abc_text_address");
        if (provinceAttribute.getValue() !== null &&
            districtAttribute.getValue() !== null &&
            addressAttribute.getValue() !== null) {
            formContext.getControl("abc_button_ok").setDisabled(false);
        }
        else {
            formContext.getControl("abc_button_ok").setDisabled(true);
        }
    }
    async function onOpen(executionContext) {
        const options = {
            position: 1,
            width: 600,
            height: 310
        };
        const params = { };
        const result = await Xrm.Navigation.openDialog("abc_dialog_address", options, params)
        const fullAddress = result.parameters.abc_text_fulladdress;
        debugger;
    }
    async function onLoad(executionContext) {
        const formContext = executionContext.getFormContext();

        const provinceControl = formContext.getControl("abc_lookup_province");
        provinceControl.addPreSearch(provincePreSearch);

        const districtControl = formContext.getControl("abc_lookup_district");
        districtControl.addPreSearch(districtPreSearch);
        const districtAttribute = formContext.data.attributes.get("abc_lookup_district");
        districtAttribute.addOnChange(districtOnChange);

        const addressAttribute = formContext.data.attributes.get("abc_text_address");
        addressAttribute.addOnChange(addressOnChange);
    }
    async function onOkClick(executionContext) {
        const formContext = executionContext.getFormContext();
        const provinceAttribute = formContext.data.attributes.get("abc_lookup_province");
        const districtAttribute = formContext.data.attributes.get("abc_lookup_district");
        const addressAttribute = formContext.data.attributes.get("abc_text_address");
        const fullAddressAttribute = formContext.data.attributes.get("abc_text_fulladdress");
        fullAddressAttribute.setValue(`${addressAttribute.getValue()}, ${districtAttribute.getValue()[0].name}, ${provinceAttribute.getValue()[0].name}`);
        formContext.ui.close();
    }
    async function onCancelClick(executionContext) {
        const formContext = executionContext.getFormContext();
        formContext.ui.close();
    }
    return {
        OnOpen: onOpen,
        OnLoad: onLoad,
        OnOkClick: onOkClick,
        OnCancelClick: onCancelClick,
        OnProvinceChange: onProvinceChange
    };
})();
