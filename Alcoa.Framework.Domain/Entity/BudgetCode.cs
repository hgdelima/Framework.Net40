using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class BudgetCode : Base, IBaseDomain
    {
        public BudgetCode()
        {
            Validation = new BaseValidation();
        }

        public string SiteId { get; set; }

        public Site Site { get; set; }

        public string AreaId { get; set; }

        public Area Area { get; set; }

        public string DeptId { get; set; }

        public Dept Dept { get; set; }

        public string LbcId { get; set; }

        public Lbc Lbc { get; set; }

        public string IsActive { get; set; }

        public Worker Manager { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(SiteId) && 
                string.IsNullOrEmpty(AreaId) && 
                string.IsNullOrEmpty(DeptId) && 
                string.IsNullOrEmpty(LbcId))
                Validation.Results.Add(new ValidationResult("Error: Invalid BudgetCode object or was not found."));
        }

        #endregion
    }
}