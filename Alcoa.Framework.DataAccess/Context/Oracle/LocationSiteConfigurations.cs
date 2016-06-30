using Alcoa.Framework.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Alcoa.Framework.DataAccess.Context.Oracle
{
    public class LocationSiteConfigurations
    {
        public class WorkerConfiguration : EntityTypeConfiguration<Worker>
        {
            public WorkerConfiguration()
            {
                ToTable("TBG_COLABORADOR", "ALCOA");
                HasKey(co => co.Id);

                Property(co => co.Id).HasColumnName("CHAPA").IsRequired();
                Property(co => co.FixedId).HasColumnName("CHAPA_FFIXO");
                Property(co => co.Login).HasColumnName("LOGIN");
                Property(co => co.Email).HasColumnName("CCMAIL");
                Property(co => co.Status).HasColumnName("SITUACAO").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("NOME_FUNC").IsRequired();
                Property(co => co.AssociationType).HasColumnName("IDC_VINCULO");
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
                Property(co => co.IdentificationDocument).HasColumnName("NUM_RG");
                Property(co => co.AccountingEntityId).HasColumnName("COD_ENT");
                Property(co => co.DeptId).HasColumnName("COD_DEPT");
                Property(co => co.LbcId).HasColumnName("COD_LBC");
                Property(co => co.FiscalId).HasColumnName("COD_FISCAL");
                Property(co => co.ClassificationName).HasColumnName("ORGANIZACAO");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Sid);
                Ignore(co => co.BudgetCode);
                Ignore(co => co.Manager);
                Ignore(co => co.Employees);
                Ignore(co => co.LoginExpirationDate);
                Ignore(co => co.UserExtraInfo);
                Ignore(co => co.Applications);
                Ignore(co => co.Validation);
            }
        }

        public class BudgetCodeConfiguration : EntityTypeConfiguration<BudgetCode>
        {
            public BudgetCodeConfiguration()
            {
                HasKey(co => new { co.DeptId, co.LbcId });
                Ignore(co => co.SiteId);
                Ignore(co => co.AreaId);
                Ignore(co => co.IsActive);

                Ignore(co => co.Dept);
                Ignore(co => co.Lbc);
                Ignore(co => co.Site);
                Ignore(co => co.Area);
                Ignore(co => co.Manager);
                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class AreaConfiguration : EntityTypeConfiguration<Area>
        {
            public AreaConfiguration()
            {
                HasKey(co => new { co.SiteId, co.AreaId });
                Ignore(co => co.NameOrDescription);
                Ignore(co => co.IsActive);
                Ignore(co => co.SiteParentId);
                Ignore(co => co.AreaParentId);

                Ignore(co => co.Site);
                Ignore(co => co.Manager);
                Ignore(co => co.SubAreas);
                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class SiteConfiguration : EntityTypeConfiguration<Site>
        {
            public SiteConfiguration()
            {
                HasKey(co => co.Id);
                Ignore(co => co.NameOrDescription);
                Ignore(co => co.IsActive);

                Ignore(co => co.Areas);
                Ignore(co => co.Lbcs);
                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class LbcConfiguration : EntityTypeConfiguration<Lbc>
        {
            public LbcConfiguration()
            {
                ToTable("TBG_EBS_LBC", "ALCOA");
                HasKey(co => co.LbcId);
                Property(co => co.LbcId).HasColumnName("COD_LBC").IsRequired();
                Property(co => co.IsActive).HasColumnName("IND_ATIVO").IsRequired();
                Property(co => co.IsActiveGl).HasColumnName("IND_ATIVO_GL").IsRequired();
                Property(co => co.EnglishDescription).HasColumnName("DESCR_LBC_US");
                Property(co => co.PortugueseDescription).HasColumnName("DESCR_LBC_PTB");
                Property(co => co.EspanishDescription).HasColumnName("DESCR_LBC_ESA");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
                Ignore(co => co.CityId);
                Ignore(co => co.IsHeadQuarter);
                Ignore(co => co.PrefixId);
            }
        }

        public class DeptConfiguration : EntityTypeConfiguration<Dept>
        {
            public DeptConfiguration()
            {
                ToTable("TBG_EBS_DEPT", "ALCOA");
                HasKey(co => co.DeptId);
                Property(co => co.DeptId).HasColumnName("COD_DEPT").IsRequired();
                Property(co => co.IsActive).HasColumnName("IND_ATIVO").IsRequired();
                Property(co => co.IsActiveGl).HasColumnName("IND_ATIVO_GL").IsRequired();
                Property(co => co.EnglishDescription).HasColumnName("DESCR_DEPT_US");
                Property(co => co.PortugueseDescription).HasColumnName("DESCR_DEPT_PTB");
                Property(co => co.EspanishDescription).HasColumnName("DESCR_DEPT_ESA");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class ApplicationConfiguration : EntityTypeConfiguration<SsoApplication>
        {
            public ApplicationConfiguration()
            {
                ToTable("TBG_APLICACAO", "ALCOA");
                HasKey(co => co.Id);
                Property(co => co.Id).HasColumnName("COD_APLICACAO").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("DESCR_APLICACAO");
                Property(co => co.HomeUrl).HasColumnName("WEB_URL_HOME");
                Property(co => co.Mnemonic).HasColumnName("MNEUMONICO");
                Property(co => co.IsInactive).HasColumnName("IDC_INATIVO");
                Property(co => co.ToolType).HasColumnName("TP_FERRAMENTA");

                HasMany(co => co.Profiles)
                    .WithOptional()
                    .HasForeignKey(p => new { p.ApplicationId })
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class ProfileConfiguration : EntityTypeConfiguration<SsoProfile>
        {
            public ProfileConfiguration()
            {
                ToTable("TBG_CLASSE", "ALCOA");
                HasKey(co => new { co.ApplicationId, co.Id });
                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").IsRequired();
                Property(co => co.Id).HasColumnName("COD_CLASSE").IsRequired();
                Property(co => co.ProfileType).HasColumnName("TP_CLASSE");
                Property(co => co.NameOrDescription).HasColumnName("DESCR_CLASSE");

                HasMany(co => co.ProfilesAndWorkers)
                    .WithOptional()
                    .HasForeignKey(co => new { co.ApplicationId, co.ProfileId })
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.IsPublic);
                Ignore(co => co.Order);
                Ignore(co => co.ProfilesAndServices);
                Ignore(co => co.ProfilesAndActiveDirectories);
                Ignore(co => co.Validation);
            }
        }

        public class ProfileAndWorkerConfiguration : EntityTypeConfiguration<SsoProfileAndWorker>
        {
            public ProfileAndWorkerConfiguration()
            {
                ToTable("TBG_COLABORADOR_AUTORIZA", "ALCOA");
                HasKey(co => new { co.WorkerOrEmployeeId, co.ApplicationId, co.ProfileId });
                Property(co => co.WorkerOrEmployeeId).HasColumnName("CHAPA").IsRequired();
                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").IsRequired();
                Property(co => co.ProfileId).HasColumnName("COD_CLASSE").IsRequired();
                Property(co => co.AdminGroup).HasColumnName("GRUPO_ADMIN");
                Property(co => co.DisableNotificationDate).HasColumnName("DT_NOTIFICACAO_INATIVO");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Login);
                Ignore(co => co.RequestNumber);
                Ignore(co => co.Validation);
            }
        }
    }
}