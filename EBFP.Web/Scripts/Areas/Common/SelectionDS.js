function SelectionDS() {
 
  this.ProvinceByRegion = function(selectionId, regionId , selectedValue) { 
    $.getJSON("/Unit/LoadProvinceByDistrict", { regionId: regionId },
      function(data) {
          bindDropdown(selectionId, data, selectedValue);
      });
  };

  this.MunicipalityByProvince = function (selectionId, provinceId, selectedValue) {
    
    $.getJSON("/Municipality/LoadMunicipalityByProvince", { provinceId: provinceId },
        function (data) {
          bindDropdown(selectionId, data,selectedValue);
        });
  }

  this.UnitByMunicipality = function (selectionId, municipalityId, selectedValue) {
   
      $.getJSON("/FCRS/GetUnitByMunicipality", { municipalityId: municipalityId },
          function (data) {             
              bindDropdown(selectionId, data, selectedValue);
          });
  }

  this.ClearDropdown = function (selectionId) {
    var select = $("#" + selectionId);
    select.empty();
    select.append($('<option/>', {
      value: 0,
      text: " --- Please Select---"
    }));
  }

    function bindDropdown(selectionId, data, selectedValue) {
        var select = $("#" + selectionId);
        select.empty();
        select.append($("<option/>",
            {
                value: 0,
                text: " --- Please Select---"
            }));
        $.each(data,
            function(index, itemData) {
                select.append($("<option/>",
                    {
                        value: itemData.Value,
                        text: itemData.Text
                    }));
            });

        if (selectedValue != undefined) {
            select.val(selectedValue);
        }
    }
}