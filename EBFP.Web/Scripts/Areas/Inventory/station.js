
$(document).ready(function() {
 LoadProvinceDropdown();
 LoadMarshallDropdown();
 toMoneyFormat($("#txtFloorArea")[0]);
 toMoneyFormat($("#txtLotArea")[0]);

    $("#ddlMarshallName").change(function () {
        var ID = $(this).val();
        LoadFireMarshallDetails(ID);
    });
    $("#ddlRegion").change(function () {
        var ID = $(this).val();
        LoadProvinceByRegion(ID, "");
    });
});

function LoadProvinceDropdown() {
    var ID = $("#ddlRegion").val();
    LoadProvinceByRegion(ID, "FirstLoad");
}

function LoadMarshallDropdown() {
    var ID = paramMarshallID;
    LoadFireMarshallDetails(ID);
}

function LoadProvinceByRegion(ID, type) {
    $.getJSON("/Inventory/LoadProvinceByDistrict",
        { regionId: ID },
        function (data) {
            var select = $("#ddlProvince");
            select.empty();
            select.append($('<option/>',
            {
                value: 0,
                text: " --- Please Select---"
            }));
            $.each(data,
                function (index, itemData) {
                    select.append($('<option/>',
                    {
                        value: itemData.Value,
                        text: itemData.Text
                    }));
                });

            if (type === "FirstLoad") {
                var province = paramProvinceID;
                $("#ddlProvince").val(province);
            }
        });
}

function LoadFireMarshallDetails(ID) {
    $.getJSON("/Inventory/LoadFireMarshallDetails",
        { empId: ID },
        function (data) {
            if (data.FireMarshall_Position) {
                $("#JobFunctions").val(Number(data.FireMarshall_Position));
            }
            $("#txtMarshallCellNumber").val(data.FireMarshall_ContactNumber);
        });

}
