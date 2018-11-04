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

$("#IRView").change(function() {
    readURL(this, '#RView', '#IRView');
});
$("#ILView").change(function() {
    readURL(this, '#LView', '#ILView');
});
$("#IRearView").change(function() {
    readURL(this, '#RearView', '#IRearView');
});
$("#IFView").change(function() {
    readURL(this, '#FView', '#IFView');
});
$(document).ready(function() {
    LoadProvinceDropdown();
    LoadSubStationDropdown();
    $("#municipality_Reg_Id").change(function() {
        var ID = $(this).val();
        LoadProvinceByRegion(ID, "");
    });

    $("#truck_UnitId").change(function() {
        var ID = $(this).val();
        LoadSubStation(ID, "");
    });

    $("#truck_ManufactureDate").blur(function() {
        ManuAgeTotal();
    });

    $("#truck_AcquisitionDate").blur(function () {
        AcquisitionAgeTotal();
    });

    toMoneyFormat($("#truck_AcquisitionCost")[0]);
});

function ManuAgeTotal() {
    var date = new Date();
    var year = date.getFullYear();
    var total = Number((year - 1) - $("#truck_ManufactureDate").val());
    if ($("#truck_ManufactureDate").val() == null || $("#truck_ManufactureDate").val() === "")
        $("#txtManufactureAge").val(null);
    else
        $("#txtManufactureAge").val(total);
}

function AcquisitionAgeTotal() {
    var date = new Date();
    var year = date.getFullYear();
    var total = Number((year - 1) - $("#truck_AcquisitionDate").val());
    if ($("#truck_AcquisitionDate").val() == null || $("#truck_AcquisitionDate").val() === "")
        $("#txtAcquisitionAge").val(null);
    else
        $("#txtAcquisitionAge").val(total);
}

function LoadProvinceDropdown() {
    var ID = $("#municipality_Reg_Id").val();
    LoadProvinceByRegion(ID, "FirstLoad");
}

function LoadSubStationDropdown() {
    var ID = $("#truck_UnitId").val();
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
            var select = $("#truck_SubStationId");
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
                $("#truck_SubStationId").val(subStation);
            }
        });
}