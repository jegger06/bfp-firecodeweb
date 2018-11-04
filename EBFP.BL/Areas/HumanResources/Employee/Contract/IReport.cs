using System;
using System.Collections.Generic;

namespace EBFP.BL.HumanResources
{
    public interface IReport
    {
        List<FireCodeFeesCollectionModel> GetFireCodeFeesCollection(int month);
        List<FireCodeDepositCollectionModel> GetFireCodeDeposits(int month);
        List<FireCodeDepositCollectionModel> GetFireCodeCollections(int month);
        List<PersonnelModel> GetPersonnelPerRegion();
        List<LongevityPayModel> GetLongevityPay(DateTime startDate, DateTime endDate, int year);
        CommutationLeaveCreditsModel GetCommLeaveCredits(int retiredEmpId);
        List<AgeProfileModel> GetAgeProfile();
        List<RatioOfFemaleFireFightersModel> GetRatioOfFemaleFireFighters();
        List<RetiringPersonnelModel> GetRetiringPersonnelPerYear(string type, int? month);
        PersonnelModel GetPersonnelNumberPerRegion(int regionId);
        PersonnelModel GetPersonnelNumberPerProvince(int provinceId);
        PersonnelModel GetPersonnelNumberPerStation(int station);
        List<FireCodeDepositCollectionModel> GetFireCodeCollectionByUnit(int month, int unitId);
        List<FireCodeDepositCollectionModel> GetFireCodeDepositByUnit(int month, int unitId);
        List<MRAAFModel> GetMRAAF(int month);
    }
}