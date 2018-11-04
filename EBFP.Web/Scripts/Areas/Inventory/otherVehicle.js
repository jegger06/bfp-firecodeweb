
function readURL(input, id, fileId) {
    if (input.files && input.files[0]) {

        if (input.files[0].size > 1000000) {
            alert("Image size must not be greater than 1MB!");
            $(fileId).val(null);
        } else {
            var reader = new FileReader();
            reader.onload = function(e) {
                $(id).attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }

    }
}

$("#IOVRView").change(function() {
    readURL(this, '#OVRView', '#IOVRView');
});
$("#IOVLView").change(function() {
    readURL(this, '#OVLView', '#IOVLView');
});
$("#IOVRearView").change(function() {
    readURL(this, '#OVRearView', '#IOVRearView');
});
$("#IOVFView").change(function() {
    readURL(this, '#OVFView', '#IOVFView');
});

$(document).ready(function () {
    loadUnit();
    LoadProvinceDropdown();
    LoadSubStationDropdown();
    $("#municipality_Reg_Id").change(function() {
        var ID = $(this).val();
        LoadProvinceByRegion(ID, "");
    });

    $("#vehicle_UnitId").change(function() {
        var ID = $(this).val();
        LoadSubStation(ID, "");
    });

    $("#vehicle_AcquisitionDate").blur(function() {
        AcquisitionAgeTotal();
    });

    toMoneyFormat($("#vehicle_AcquisitionCost")[0]);

    setTimeout(function () { LoadSubStation(unitId, "FirstLoad"); }, 1000);
});

function loadUnit() {
    
    oSelectionDS.UnitByMunicipality("vehicle_UnitId", paramMunicipalityId, unitId);
}

function AcquisitionAgeTotal() {
    var date = new Date();
    var year = date.getFullYear();
    var total = Number((year - 1) - $("#vehicle_AcquisitionDate").val());
    if ($("#vehicle_AcquisitionDate").val() == null || $("#vehicle_AcquisitionDate").val() === "")
        $("#txtvehicle_AcquisitionAge").val(null);
    else
        $("#txtvehicle_AcquisitionAge").val(total);
}

function LoadProvinceDropdown() {
    var ID = $("#municipality_Reg_Id").val();
    LoadProvinceByRegion(ID, "FirstLoad");
}

function LoadSubStationDropdown() {
    var ID = $("#vehicle_UnitId").val();
    LoadSubStation(ID, "FirstLoad");
}

function LoadProvinceByRegion(ID, type) {
    $.getJSON("/Inventory/LoadProvinceByDistrict",
        { regionId: ID },
        function(data) {
            var select = $("#municipality_Province_Id");
            select.empty();
            select.append($('<option/>',
            {
                value: 0,
                text: " --- Please Select---"
            }));
            $.each(data,
                function(index, itemData) {
                    select.append($('<option/>',
                    {
                        value: itemData.Value,
                        text: itemData.Text
                    }));
                });

            if (type === "FirstLoad") {
                var province = paramProvinceID;
                $("#municipality_Province_Id").val(province);
            }
        });
}

function LoadSubStation(ID, type) {
    
    $.getJSON("/Inventory/LoadSubStationByStation",
        { stationId: ID },
        function(data) {
            var select = $("#vehicle_SubStationId");
            select.empty();
            select.append($('<option/>',
            {
                value: 0,
                text: " --- Please Select---"
            }));
            $.each(data,
                function(index, itemData) {
                    select.append($('<option/>',
                    {
                        value: itemData.Value,
                        text: itemData.Text
                    }));
                });

            if (type === "FirstLoad") {

                var subStation = paramSubStationID;
                if (subStation == null)
                    subStation = 0;
                $("#vehicle_SubStationId").val(subStation);
            }
        });
}