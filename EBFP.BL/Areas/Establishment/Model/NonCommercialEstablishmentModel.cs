
using System;

namespace EBFP.BL.Establishment
{
    public class NonCommercialEstablishmentModel
    {      
        public int NCE_Id { get; set; }

        public string Ref_NCE_Id { get; set; }

        public string NCE_OwnerName { get; set; }

        public string NCE_ConstructionAddress { get; set; }

        public string NCE_AuthorizedRepresentative { get; set; }

        public string NCE_ContactNumber { get; set; }

        public int NCE_Unit_Id { get; set; }

        public int NCE_Created_Emp_Id { get; set; }

        public DateTime NCE_CreatedDate { get; set; }

        public int NCE_LastUpdate_Emp_Id { get; set; }

        public DateTime NCE_LastUpdateDate { get; set; }

        public bool IsSynced { get; set; }

        public string NCE_NatureOfConstruction { get; set; }


        public string Unit_StationName { get; set; }
        public long? FSEC_OPS_Number { get; set; }
        public string Unit_Address { get; set; }
        public string Province_Name { get; set; }
        public string Reg_Title { get; set; }
        public string Assessor { get; set; }
        public DateTime? FSEC_Assesed_Date { get; set; }
        public decimal? FSEC_OtherFee { get; set; }
        public decimal? FSEC_ConstructionTax { get; set; }

        //public int Auto_NCE_Id { get; set; }

        //public string NCE_Id { get; set; }

        //public string Ref_NCE_Id { get; set; }

        //public string NCE_OwnerName { get; set; }

        //public string NCE_ConstructionAddress { get; set; }

        //public string NCE_AuthorizedRepresentative { get; set; }

        //public string NCE_ContactNumber { get; set; }

        //public int NCE_Unit_Id { get; set; }

        //public int NCE_Created_Emp_Id { get; set; }

        //public DateTime NCE_CreatedDate { get; set; }

        //public int NCE_LastUpdate_Emp_Id { get; set; }

        //public DateTime NCE_LastUpdateDate { get; set; }

        //public bool IsSynced { get; set; }

        //public string NCE_NatureOfConstruction { get; set; }


        //public string Unit_StationName { get; set; }
        //public long? FSEC_OPS_Number { get; set; }
        //public string Unit_Address { get; set; }
        //public string Province_Name { get; set; }
        //public string Reg_Title { get; set; }
        //public string Assessor { get; set; }
        //public DateTime? FSEC_Assesed_Date { get; set; }
        //public decimal? FSEC_OtherFee { get; set; }
        //public decimal? FSEC_ConstructionTax { get; set; }
    }

    //
    public partial class FSECNCEReportModel
    {
        public string FSECNumber { get; set; }
        public string Region { get; set; }
        public string Province { get; set; }
        public string StationName { get; set; }
        public string PhoneNumber { get; set; }
        public string BuildingName { get; set; }
        public string OwnerName { get; set; }
        public string BusinessAddress { get; set; }
        public string FireMarshall { get; set; }
        public int ORNumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string ChiefFSES { get; set; }
        public string Unit_Address { get; set; }
        public byte[] ChiefFSESSignature { get; set; }
        public byte[] FireMarshallSignature { get; set; }
        public string NCE_NatureOfConstruction { get; set; }
        public bool? FSEC_IsManual { get; set; }
    }
    public partial class NCEOPSModel
    {
        public string NCE_OwnerName { get; set; }
        public string NCE_ConstructionAddress { get; set; }
        public string Unit_StationName { get; set; }
        public string FSEC_OPS_Number { get; set; }
        public string Unit_Address { get; set; }
        public string Province_Name { get; set; }
        public string Reg_Title { get; set; }
        public string Assessor { get; set; }
        public DateTime? FSEC_Assesed_Date { get; set; }
        public decimal? FSEC_OtherFee { get; set; }
        public decimal? FSEC_ConstructionTax { get; set; }


        public int OF_Collection_Type { get; set; }
        public DateTime? OF_Assesed_Date { get; set; }
        public decimal? OF_Fee { get; set; }
        public string OF_OPS_Number { get; set; }
    }
}
