using System;
using System.Collections.Generic;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.Inventory
{
    public class PopulationBL : Repository<tblPopulations, PopulationModel>, IPopulation
    {
        public PopulationBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<PopulationModel> model, int municipalityId)
        {
            if (model == null || model.Count == 0) return;

            foreach (var item in model)
            {
                item.Population_Municipality_Id = municipalityId;
            }

            InsertBulk(model, a => a.Population_Municipality_Id == municipalityId);
        }
    }
}