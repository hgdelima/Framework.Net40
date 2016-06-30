using Alcoa.Framework.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Alcoa.Framework.DataAccess.Context.Oracle
{
    public class CorporateConfigurations
    {
        public class WorkerConfiguration : EntityTypeConfiguration<Worker>
        {
            public WorkerConfiguration()
            {
                ToTable("TBG_COLABORADOR", "ALCOA");
                HasKey(co => co.Id);

                Property(co => co.Id).HasColumnName("CHAPA").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.FixedId).HasColumnName("CHAPA_FFIXO").HasColumnType("VARCHAR2");
                Property(co => co.Login).HasColumnName("LOGIN").HasColumnType("VARCHAR2");
                Property(co => co.Email).HasColumnName("CCMAIL").HasColumnType("VARCHAR2");
                Property(co => co.Status).HasColumnName("SITUACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("NOME_FUNC").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.AssociationType).HasColumnName("IDC_VINCULO").HasColumnType("VARCHAR2");
                Property(co => co.BranchLine).HasColumnName("RAMAL").HasColumnType("VARCHAR2");
                Property(co => co.Gender).HasColumnName("IDC_SEXO").HasColumnType("VARCHAR2");
                Property(co => co.Local).HasColumnName("LOCAL").HasColumnType("VARCHAR2");
                Property(co => co.PhysicalLocal).HasColumnName("LOCAL_FISICO").HasColumnType("VARCHAR2");
                Property(co => co.Domain).HasColumnName("DOMINIO_REDE").HasColumnType("VARCHAR2");
                Property(co => co.BusinessTitle).HasColumnName("BUSINESS_TITLE").HasColumnType("VARCHAR2");
                Property(co => co.ActualExchangeDate).HasColumnName("DT_ATUAL_EXCHANGE").HasColumnType("DATE");
                Property(co => co.BirthDate).HasColumnName("DT_NASCIM").HasColumnType("DATE");
                Property(co => co.WebSignature).HasColumnName("ASSINATURA").HasColumnType("VARCHAR2");
                Property(co => co.Nickname).HasColumnName("APELIDO").HasColumnType("VARCHAR2");
                Property(co => co.IdentificationDocument).HasColumnName("NUM_RG").HasColumnType("VARCHAR2");
                Property(co => co.AccountingEntityId).HasColumnName("COD_ENT").HasColumnType("VARCHAR2");
                Property(co => co.DeptId).HasColumnName("COD_DEPT").HasColumnType("VARCHAR2");
                Property(co => co.LbcId).HasColumnName("COD_LBC").HasColumnType("VARCHAR2");
                Property(co => co.FiscalId).HasColumnName("COD_FISCAL").HasColumnType("VARCHAR2");
                Property(co => co.ClassificationName).HasColumnName("ORGANIZACAO").HasColumnType("VARCHAR2");

                HasOptional(co => co.BudgetCode)
                    .WithMany()
                    .HasForeignKey(co => new { co.DeptId, co.LbcId })
                    .WillCascadeOnDelete(false);

                HasMany(co => co.Manager)
                    .WithRequired()
                    .HasForeignKey(co => co.EmployeeId)
                    .WillCascadeOnDelete(false);

                HasMany(co => co.Employees)
                    .WithRequired()
                    .HasForeignKey(co => co.ManagerId)
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.Sid);
                Ignore(co => co.LoginExpirationDate);
                Ignore(co => co.UserExtraInfo);
                Ignore(co => co.Applications);
                Ignore(co => co.Validation);
            }
        }

        public class WorkerHierarchyConfiguration : EntityTypeConfiguration<WorkerHierarchy>
        {
            public WorkerHierarchyConfiguration()
            {
                ToTable("TBG_HIERARQUIA", "ALCOA");
                HasKey(co => new { co.ManagerId, co.EmployeeId });

                Property(co => co.ManagerId).HasColumnName("CHAPA_SUPERIOR").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.EmployeeId).HasColumnName("CHAPA_SUBORDINADO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.LastUpdateDate).HasColumnName("DATA_ATUALIZACAO").HasColumnType("DATE");

                HasRequired(co => co.Manager)
                    .WithMany()
                    .HasForeignKey(co => co.ManagerId)
                    .WillCascadeOnDelete(false);

                HasRequired(co => co.Employee)
                    .WithMany()
                    .HasForeignKey(co => co.EmployeeId)
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class AreaConfiguration : EntityTypeConfiguration<Area>
        {
            public AreaConfiguration()
            {
                ToTable("TBL_DEPTOAREA", "ALCOA");
                HasKey(co => new { co.SiteId, co.AreaId });

                Property(co => co.AreaId).HasColumnName("COD_DEPTOAREA").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.SiteId).HasColumnName("COD_LOCALIDADE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("DESCR_DEPTOAREA").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.IsActive).HasColumnName("IDC_ATIVO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.SiteParentId).HasColumnName("COD_LOCALIDADE_PAI").HasColumnType("VARCHAR2");
                Property(co => co.AreaParentId).HasColumnName("COD_DEPTOAREA_PAI").HasColumnType("VARCHAR2");

                HasRequired(co => co.Site)
                    .WithMany()
                    .HasForeignKey(co => co.SiteId)
                    .WillCascadeOnDelete(false);

                HasOptional(co => co.Manager)
                    .WithMany()
                    .Map(m => m.MapKey("CHAPA_RESP"))
                    .WillCascadeOnDelete(false);

                HasMany(co => co.SubAreas)
                    .WithOptional(a => a.AreaParent)
                    .HasForeignKey(a => new { a.SiteParentId, a.AreaParentId })
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class SiteConfiguration : EntityTypeConfiguration<Site>
        {
            public SiteConfiguration()
            {
                ToTable("TBL_LOCALIDADE", "ALCOA");
                HasKey(co => co.Id);

                Property(co => co.Id).HasColumnName("COD_LOCALIDADE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("DESCR_LOCALIDADE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.IsActive).HasColumnName("IDC_ATIVO").HasColumnType("VARCHAR2").IsRequired();

                HasMany(co => co.Areas)
                    .WithRequired()
                    .HasForeignKey(si => si.SiteId)
                    .WillCascadeOnDelete(false);

                HasMany(l => l.Lbcs)
                    .WithMany(s => s.Sites)
                    .Map(m =>
                    {
                        m.MapLeftKey("COD_LOCALIDADE");
                        m.MapRightKey("COD_LBC");
                        m.ToTable("TBL_LBC_LOCALIDADE", "ALCOA");
                    });

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

                Property(co => co.LbcId).HasColumnName("COD_LBC").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.IsActive).HasColumnName("IND_ATIVO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.IsActiveGl).HasColumnName("IND_ATIVO_GL").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.CityId).HasColumnName("SER_CIDADE").IsRequired();
                Property(co => co.EnglishDescription).HasColumnName("DESCR_LBC_US").HasColumnType("VARCHAR2");
                Property(co => co.PortugueseDescription).HasColumnName("DESCR_LBC_PTB").HasColumnType("VARCHAR2");
                Property(co => co.EspanishDescription).HasColumnName("DESCR_LBC_ESA").HasColumnType("VARCHAR2");
                Property(co => co.IsHeadQuarter).HasColumnName("IDC_MATRIZ").HasColumnType("VARCHAR2");
                Property(co => co.PrefixId).HasColumnName("COD_PREFIX").HasColumnType("VARCHAR2");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class DeptConfiguration : EntityTypeConfiguration<Dept>
        {
            public DeptConfiguration()
            {
                ToTable("TBG_EBS_DEPT", "ALCOA");
                HasKey(co => co.DeptId);

                Property(co => co.DeptId).HasColumnName("COD_DEPT").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.IsActive).HasColumnName("IND_ATIVO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.IsActiveGl).HasColumnName("IND_ATIVO_GL").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.EnglishDescription).HasColumnName("DESCR_DEPT_US").HasColumnType("VARCHAR2");
                Property(co => co.PortugueseDescription).HasColumnName("DESCR_DEPT_PTB").HasColumnType("VARCHAR2");
                Property(co => co.EspanishDescription).HasColumnName("DESCR_DEPT_ESA").HasColumnType("VARCHAR2");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class BudgetCodeConfiguration : EntityTypeConfiguration<BudgetCode>
        {
            public BudgetCodeConfiguration()
            {
                ToTable("TBL_DEPTOEBS_DEPTOAREA", "ALCOA");
                HasKey(co => new { co.DeptId, co.LbcId });

                Property(co => co.SiteId).HasColumnName("COD_LOCALIDADE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.AreaId).HasColumnName("COD_DEPTOAREA").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.DeptId).HasColumnName("COD_DEPT").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.LbcId).HasColumnName("COD_LBC").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.IsActive).HasColumnName("IDC_ATIVO").HasColumnType("VARCHAR2").IsRequired();

                HasRequired(co => co.Site)
                    .WithMany()
                    .HasForeignKey(co => co.SiteId)
                    .WillCascadeOnDelete(false);

                HasRequired(co => co.Area)
                    .WithMany(b => b.BudgetCodes)
                    .HasForeignKey(k => new { k.SiteId, k.AreaId })
                    .WillCascadeOnDelete(false);

                HasRequired(co => co.Dept)
                    .WithMany()
                    .HasForeignKey(co => co.DeptId)
                    .WillCascadeOnDelete(false);

                HasRequired(co => co.Lbc)
                    .WithMany()
                    .HasForeignKey(co => co.LbcId)
                    .WillCascadeOnDelete(false);

                HasOptional(co => co.Manager)
                    .WithMany()
                    .Map(m => m.MapKey("CHAPA_RESP"))
                    .WillCascadeOnDelete(false);

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

                Property(co => co.Id).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.IsInactive).HasColumnName("IDC_INATIVO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("DESCR_APLICACAO").HasColumnType("VARCHAR2");
                Property(co => co.HomeUrl).HasColumnName("WEB_URL_HOME").HasColumnType("VARCHAR2");
                Property(co => co.Mnemonic).HasColumnName("MNEUMONICO").HasColumnType("VARCHAR2");
                Property(co => co.ToolType).HasColumnName("TP_FERRAMENTA").HasColumnType("VARCHAR2");

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

                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.Id).HasColumnName("COD_CLASSE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ProfileType).HasColumnName("TP_CLASSE").HasColumnType("VARCHAR2");
                Property(co => co.NameOrDescription).HasColumnName("DESCR_CLASSE").HasColumnType("VARCHAR2");

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

                Property(co => co.WorkerOrEmployeeId).HasColumnName("CHAPA").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ProfileId).HasColumnName("COD_CLASSE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.AdminGroup).HasColumnName("GRUPO_ADMIN").HasColumnType("VARCHAR2");
                Property(co => co.DisableNotificationDate).HasColumnName("DT_NOTIFICACAO_INATIVO").HasColumnType("DATE");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Login);
                Ignore(co => co.RequestNumber);
                Ignore(co => co.Validation);
            }
        }
    }
}