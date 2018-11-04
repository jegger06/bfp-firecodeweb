using System;
using System.ComponentModel.DataAnnotations;
using EBFP.BL.Helper;

namespace EBFP.BL.Inventory
{
    public class PopulationModel
    {
        public int Population_Id { get; set; }
        public int Population_Municipality_Id { get; set; }
        public int count { get; set; }
        [Required]
        public string Population_Year { get; set; }

        public Nullable<int> Population_Count { get; set; }
        private string _sPopulation_Count = "";
        [Required]
        public string sPopulation_Count
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sPopulation_Count) ?
                    Population_Count.ToString() :
                    _sPopulation_Count;
            }
            set
            {
                this.Population_Count = Functions.ConvertToSafeInteger(value);
            }
        }
    
        public bool CreatedFromAjax { get; set; }
    }
}
