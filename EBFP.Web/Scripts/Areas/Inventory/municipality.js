var oSelectionDS = new SelectionDS();
$(document).ready(function () {
  ddlRegionValueChanged($("#municipality_Reg_Id").val(), paramProvinceID);

    toMoneyFormat($("#txtPopulationDensity")[0]);
    toMoneyFormat($("#txtLandArea")[0]);

    var count = 1;
    $.each(paramPopulationList, function (index, value) {
        toThousandSeparatorFormat($("#txtPopulation_" + count)[0]);
        count++;
    });

    count = 0;
    $.each(paramPopulationList, function (index, value) {
        toThousandSeparatorFormat($("#txtPopulationEditor_" + count)[0]);
        count++;
    });

    count = 0;
    $.each(paramPopulationList, function (index, value) {
        toThousandSeparatorFormat($("#txtIncidents_" + count)[0]);
        count++;
    });

    count = 0;
    $.each(paramFireRecordList, function (index, value) {
        toThousandSeparatorFormat($("#txtIncidents_" + count)[0]);
        toThousandSeparatorFormat($("#txtInjuries_" + count)[0]);
        toThousandSeparatorFormat($("#txtDeaths_" + count)[0]);
        toMoneyFormat($("#txtDamages_" + count)[0]);
        count++;
    });



    $("#municipality_Reg_Id").change(function() {
        var ID = $(this).val();
        LoadProvinceByRegion(ID, "");
    });

    $("#txtCSUPT").blur(function() {
        OfficersTotal();
    });

    $("#txtSSUPT").blur(function() {
        OfficersTotal();
    });

    $("#txtSUPT").blur(function() {
        OfficersTotal();
    });

    $("#txtCINSP").blur(function() {
        OfficersTotal();
    });

    $("#txtSINSP").blur(function() {
        OfficersTotal();
    });

    $("#txtINSP").blur(function() {
        OfficersTotal();
    });

             
    $("#txtSFO4").blur(function() {
        NCOsTotal();
    });

    $("#txtSFO3").blur(function() {
        NCOsTotal();
    });

    $("#txtSFO2").blur(function() {
        NCOsTotal();
    });

    $("#txtSFO1").blur(function() {
        NCOsTotal();
    });

    $("#txtFO3").blur(function() {
        NCOsTotal();
    });

    $("#txtFO2").blur(function() {
        NCOsTotal();
    });

    $("#txtFO1").blur(function() {
        NCOsTotal();
    });

    $("#txtNUP").blur(function() {
        TotalPersonnels();
    });



    //Hydrants
    $("#txtFunctional").blur(function () {
        TotalHydrants();
    });

    $("#txtNonFunctional").blur(function () {
        TotalHydrants();
    });

    $("#txtUnderRepair").blur(function () {
        TotalHydrants();
    });

    //Barangays
    $("#txtUrban").blur(function () {
        Barangays();
    });

    $("#txtRural").blur(function () {
        Barangays();
    });

    //SCBA
    $("#txtSCBASevicable").blur(function () {
      SCBATotal();
    });

    $("#txtSCBASevicableBFR").blur(function () {
      SCBATotal();
    });

    //Fire Coat
    $("#txtFCSevicable").blur(function () {
      FireCoatTotal();
    });

    $("#txtFCSevicableBFR").blur(function () {
      FireCoatTotal();
    });

    //Trouser
    $("#txtTrouserSevicable").blur(function () {
      TrouserTotal();
    });

    $("#txtTrouserSevicableBFR").blur(function () {
      TrouserTotal();
    });

    //Boots
    $("#txtBootsSevicable").blur(function () {
      BootsTotal();
    });

    $("#txtBootsSevicableBFR").blur(function () {
      BootsTotal();
    });
    //Boots
    $("#txtGlovesSevicable").blur(function () {
      GlovesTotal();
    });

    $("#txtGlovesSevicableBFR").blur(function () {
      GlovesTotal();
    });

     //Helmet
    $("#txtHelmetSevicable").blur(function () {
      HelmetTotal();
    });

    $("#txtHelmetSevicableBFR").blur(function () {
      HelmetTotal();
    });

    //FireHose 1.5
    $("#txtFireHose15Sevicable").blur(function () {
      FireHose15Total();
    });

    $("#txtFireHose15SevicableBFR").blur(function () {
      FireHose15Total();
    });

    //FireHose 2.5
    $("#txtFireHose25Sevicable").blur(function () {
      FireHose25Total();
    });

    $("#txtFireHose25SevicableBFR").blur(function () {
      FireHose25Total();
    });

    //FireNozzle 1.5
    $("#txtFireNozzle15Sevicable").blur(function () {
      FireNozzle15Total();
    });

    $("#txtFireNozzle15SevicableBFR").blur(function () {
      FireNozzle15Total();
    });

    //FireNozzle 2.5
    $("#txtFireNozzle25Sevicable").blur(function () {
      FireNozzle25Total();
    });

    $("#txtFireNozzle25SevicableBFR").blur(function () {
      FireNozzle25Total();
    });
    SCBATotal();
    FireCoatTotal();
    TrouserTotal();
    BootsTotal();
    GlovesTotal();
    HelmetTotal();
    FireHose15Total();
    FireHose25Total();
    FireNozzle15Total();
    FireNozzle25Total();
});

function ddlRegionValueChanged(regionId, provinceId) {
  var province = provinceId;
  oSelectionDS.ProvinceByRegion("municipality_Province_Id", regionId, province);
}

function Barangays() {
    var total = Number($("#txtUrban").val()) + Number($("#txtRural").val());
    $("#txtBarangays").val(total);
}

function TotalHydrants() {
    var total = Number($("#txtFunctional").val()) + Number($("#txtNonFunctional").val()) + Number($("#txtUnderRepair").val());
    $("#txtHydrantTotal").val(total);
}

function UPersonnel() {
    var total = Number($("#txtTotalOfficers").val()) + Number($("#txtNCOs").val());
    $("#txtUPersonnel").val(total);
    TotalPersonnels();
}

function TotalPersonnels() {
    var total = Number($("#txtUPersonnel").val()) + Number($("#txtNUP").val());
    $("#txtTotal").val(total);
}

function OfficersTotal() {
    var total = Number($("#txtCSUPT").val()) + Number($("#txtSSUPT").val()) + 
                Number($("#txtSUPT").val()) + Number($("#txtCINSP").val()) + 
                Number($("#txtINSP").val()) + Number($("#txtSINSP").val());
    $("#txtTotalOfficers").val(total);
    UPersonnel();
}

function NCOsTotal() {
    var total = Number($("#txtSFO4").val()) + Number($("#txtSFO3").val()) + 
                Number($("#txtSFO2").val()) + Number($("#txtSFO1").val()) + 
                Number($("#txtFO3").val())+ 
                Number($("#txtFO2").val()) + Number($("#txtFO1").val());
    $("#txtNCOs").val(total);
    UPersonnel();
}

function SCBATotal() {
  var total = Number($("#txtSCBASevicable").val()) + Number($("#txtSCBASevicableBFR").val());
  $("#txtTotalSCBA").val(total);
}

function FireCoatTotal() {
  var total = Number($("#txtFCSevicable").val()) + Number($("#txtFCSevicableBFR").val());
  $("#txtTotalFireCoat").val(total);
}
function TrouserTotal() {
  var total = Number($("#txtTrouserSevicable").val()) + Number($("#txtTrouserSevicableBFR").val());
  $("#txtTotalTrouser").val(total);
}
function BootsTotal() {
  var total = Number($("#txtBootsSevicable").val()) + Number($("#txtBootsSevicableBFR").val());
  $("#txtTotalBoots").val(total);
}
function GlovesTotal() {
  var total = Number($("#txtGlovesSevicable").val()) + Number($("#txtGlovesSevicableBFR").val());
  $("#txtTotalGloves").val(total);
}
function HelmetTotal() {
  var total = Number($("#txtHelmetSevicable").val()) + Number($("#txtHelmetSevicableBFR").val());
  $("#txtTotalHelmet").val(total);
}
function FireHose15Total() {
  var total = Number($("#txtFireHose15Sevicable").val()) + Number($("#txtFireHose15SevicableBFR").val());
  $("#txtTotalFireHose15").val(total);
}
function FireHose25Total() {
  var total = Number($("#txtFireHose25Sevicable").val()) + Number($("#txtFireHose25SevicableBFR").val());
  $("#txtTotalFireHose25").val(total);
}

function FireNozzle15Total() {
  var total = Number($("#txtFireNozzle15Sevicable").val()) + Number($("#txtFireNozzle15SevicableBFR").val());
  $("#txtTotalFireNozzle15").val(total);
}
function FireNozzle25Total() {
  var total = Number($("#txtFireNozzle25Sevicable").val()) + Number($("#txtFireNozzle25SevicableBFR").val());
  $("#txtTotalFireNozzle25").val(total);
}
