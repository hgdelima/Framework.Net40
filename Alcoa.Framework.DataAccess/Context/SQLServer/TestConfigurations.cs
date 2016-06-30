using System.Data.Entity.ModelConfiguration;
using Alcoa.Framework.Domain.Entity;

namespace Alcoa.Framework.DataAccess.Context.SQLServer
{
    public class TestConfigurations
    {
        public class WorkerConfiguration : EntityTypeConfiguration<Worker>
        {
            public WorkerConfiguration()
            {
                ToTable("TBG_COLABORADOR", "ALCOA");
                HasKey(co => co.Id);
                Property(co => co.Id).HasColumnName("CHAPA").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("NOME_FUNC").IsRequired();
                Property(co => co.Status).HasColumnName("SITUACAO").IsRequired();
                Property(co => co.AssociationType).HasColumnName("IDC_VINCULO").IsRequired();
                Property(co => co.FixedId).HasColumnName("CHAPA_FFIXO");
                Property(co => co.Login).HasColumnName("LOGIN");
                Property(co => co.Email).HasColumnName("CCMAIL");
                Property(co => co.BranchLine).HasColumnName("RAMAL");
                Property(co => co.Gender).HasColumnName("IDC_SEXO");
                Property(co => co.Local).HasColumnName("LOCAL");
                Property(co => co.PhysicalLocal).HasColumnName("LOCAL_FISICO");
                Property(co => co.Domain).HasColumnName("DOMINIO_REDE");
                Property(co => co.BusinessTitle).HasColumnName("BUSINESS_TITLE");
                Property(co => co.ActualExchangeDate).HasColumnName("DT_ATUAL_EXCHANGE");
                Property(co => co.BirthDate).HasColumnName("DT_NASCIM");
                Property(co => co.WebSignature).HasColumnName("ASSINATURA");
                Property(co => co.Nickname).HasColumnName("APELIDO");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Sid);
                Ignore(co => co.DeptId);
                Ignore(co => co.BudgetCode);
                Ignore(co => co.FiscalId);
                Ignore(co => co.IdentificationDocument);
                Ignore(co => co.LbcId);
                Ignore(co => co.AccountingEntityId);
                Ignore(co => co.ClassificationName);
                Ignore(co => co.Applications);
                Ignore(co => co.Validation);
                Ignore(co => co.UserExtraInfo);
            }
        }
    }
}