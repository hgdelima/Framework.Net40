using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;

namespace Alcoa.Framework.Domain.Entity
{
    public class Area : BaseDescription, IBaseDomain
    {
        public Area()
        {
            Validation = new BaseValidation();
        }

        public string AreaId { get; set; }

        public string AreaParentId { get; set; }

        public Area AreaParent { get; set; }

        public List<Area> SubAreas { get; set; }

        public string SiteId { get; set; }

        public Site Site { get; set; }

        public string SiteParentId { get; set; }

        public List<BudgetCode> BudgetCodes { get; set; }

        public Worker Manager { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(AreaId))
                Validation.Results.Add(new ValidationResult("Error: Id property can't be null"));
        }

        #endregion
    }
}