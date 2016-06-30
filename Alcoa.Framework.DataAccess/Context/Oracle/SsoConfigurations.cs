using Alcoa.Framework.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Alcoa.Framework.DataAccess.Context.Oracle
{
    public class SsoConfigurations
    {
        public class WorkerConfiguration : EntityTypeConfiguration<Worker>
        {
            public WorkerConfiguration()
            {
                ToTable("TBG_COLABORADOR", "ALCOA");
                HasKey(co => co.Id);
                Property(co => co.Id).HasColumnName("CHAPA").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("FIRST_NAME").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.Status).HasColumnName("SITUACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.AssociationType).HasColumnName("IDC_VINCULO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.FixedId).HasColumnName("CHAPA_FFIXO").HasColumnType("VARCHAR2");
                Property(co => co.Login).HasColumnName("LOGIN").HasColumnType("VARCHAR2");
                Property(co => co.Email).HasColumnName("CCMAIL").HasColumnType("VARCHAR2");
                Property(co => co.BranchLine).HasColumnName("RAMAL").HasColumnType("VARCHAR2");
                Property(co => co.Gender).HasColumnName("IDC_SEXO").HasColumnType("VARCHAR2");
                Property(co => co.Local).HasColumnName("LOCAL").HasColumnType("VARCHAR2");
                Property(co => co.PhysicalLocal).HasColumnName("LOCAL_FISICO").HasColumnType("VARCHAR2");
                Property(co => co.Domain).HasColumnName("DOMINIO_REDE").HasColumnType("VARCHAR2");
                Property(co => co.BusinessTitle).HasColumnName("BUSINESS_TITLE").HasColumnType("VARCHAR2");
                Property(co => co.WebSignature).HasColumnName("ASSINATURA_WEB").HasColumnType("VARCHAR2");
                Property(co => co.LoginExpirationDate).HasColumnName("END_DATE").HasColumnType("DATE");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Sid);
                Ignore(co => co.DeptId);
                Ignore(co => co.BudgetCode);
                Ignore(co => co.FiscalId);
                Ignore(co => co.AccountingEntityId);
                Ignore(co => co.LbcId);
                Ignore(co => co.IdentificationDocument);
                Ignore(co => co.BirthDate);
                Ignore(co => co.Nickname);
                Ignore(co => co.ClassificationName);
                Ignore(co => co.UserExtraInfo);
                Ignore(co => co.ActualExchangeDate);
                Ignore(co => co.Manager);
                Ignore(co => co.Employees);
                Ignore(co => co.Applications);
                Ignore(co => co.Validation);
            }
        }

        public class SsoApplicationConfiguration : EntityTypeConfiguration<SsoApplication>
        {
            public SsoApplicationConfiguration()
            {
                ToTable("TBG_APLICACAO", "ALCOA");
                HasKey(co => co.Id);

                Property(co => co.Id).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.HomeUrl).HasColumnName("WEB_URL_HOME").HasColumnType("VARCHAR2");
                Property(co => co.Mnemonic).HasColumnName("MNEUMONICO").HasColumnType("VARCHAR2");
                Property(co => co.IsInactive).HasColumnName("IDC_INATIVO").HasColumnType("VARCHAR2");
                Property(co => co.ToolType).HasColumnName("TP_FERRAMENTA").HasColumnType("VARCHAR2");

                HasMany(co => co.Profiles)
                    .WithOptional()
                    .HasForeignKey(p => new { p.ApplicationId })
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.NameOrDescription);
                Ignore(co => co.Validation);
            }
        }

        public class SsoProfileConfiguration : EntityTypeConfiguration<SsoProfile>
        {
            public SsoProfileConfiguration()
            {
                ToTable("TBG_CLASSE", "ALCOA");
                HasKey(co => new { co.ApplicationId, co.Id });

                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.Id).HasColumnName("COD_CLASSE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ProfileType).HasColumnName("TP_CLASSE").HasColumnType("VARCHAR2");
                Property(co => co.IsPublic).HasColumnName("IDC_PUBLICO").HasColumnType("VARCHAR2");
                Property(co => co.Order).HasColumnName("ORDENAR").HasColumnType("VARCHAR2");

                HasMany(co => co.ProfilesAndServices)
                    .WithOptional()
                    .HasForeignKey(co => new { co.ApplicationId, co.ProfileId })
                    .WillCascadeOnDelete(false);

                HasMany(co => co.ProfilesAndActiveDirectories)
                    .WithOptional()
                    .HasForeignKey(co => new { co.ApplicationId, co.ProfileId })
                    .WillCascadeOnDelete(false);

                HasMany(co => co.ProfilesAndWorkers)
                    .WithOptional()
                    .HasForeignKey(co => new { co.ApplicationId, co.ProfileId })
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.NameOrDescription);
                Ignore(co => co.Validation);
            }
        }

        public class SsoProfileAndServiceConfiguration : EntityTypeConfiguration<SsoProfileAndService>
        {
            public SsoProfileAndServiceConfiguration()
            {
                ToTable("WEB_CLASSE_X_SERVICO", "ALCOA");
                HasKey(co => new { co.ApplicationId, co.ProfileId, co.ServiceId });

                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ProfileId).HasColumnName("COD_CLASSE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ServiceId).HasColumnName("COD_SERVICO").HasColumnType("VARCHAR2").IsRequired();

                HasRequired(co => co.Profile)
                    .WithMany()
                    .HasForeignKey(co => new { co.ApplicationId, co.ProfileId })
                    .WillCascadeOnDelete(false);

                HasRequired(co => co.Service)
                    .WithMany()
                    .HasForeignKey(co => new { co.ApplicationId, co.ServiceId })
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class SsoProfileAndActiveDirectoryConfiguration : EntityTypeConfiguration<SsoProfileAndActiveDirectory>
        {
            public SsoProfileAndActiveDirectoryConfiguration()
            {
                ToTable("WEB_GRUPO_AD_X_CLASSE", "ALCOA");
                HasKey(co => new { co.ApplicationId, co.ProfileId, co.AdGroupId });

                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ProfileId).HasColumnName("COD_CLASSE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.AdGroupId).HasColumnName("ID_GRP_AD").HasColumnType("VARCHAR2").IsRequired();

                HasRequired(co => co.Profile)
                    .WithMany()
                    .HasForeignKey(co => new { co.ApplicationId, co.ProfileId })
                    .WillCascadeOnDelete(false);

                HasRequired(co => co.AdGroup)
                    .WithMany()
                    .HasForeignKey(co => co.AdGroupId)
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class SsoProfileAndWorkerConfiguration : EntityTypeConfiguration<SsoProfileAndWorker>
        {
            public SsoProfileAndWorkerConfiguration()
            {
                ToTable("TBG_COLABORADOR_AUTORIZA", "ALCOA");
                HasKey(co => new { co.WorkerOrEmployeeId, co.ApplicationId, co.ProfileId });

                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ProfileId).HasColumnName("COD_CLASSE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.WorkerOrEmployeeId).HasColumnName("AUTORIZATION_ID").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.Login).HasColumnName("LOGIN").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.RequestNumber).HasColumnName("CHAMADO").HasColumnType("NUMBER").IsRequired();
                Property(co => co.AdminGroup).HasColumnName("GRUPO_ADMIN").HasColumnType("VARCHAR2");
                Property(co => co.DisableNotificationDate).HasColumnName("DT_NOTIFICACAO_INATIVO").HasColumnType("DATE");

                HasRequired(co => co.Profile)
                    .WithMany()
                    .HasForeignKey(co => new { co.ApplicationId, co.ProfileId })
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }

        public class SsoServiceConfiguration : EntityTypeConfiguration<SsoServices>
        {
            public SsoServiceConfiguration()
            {
                ToTable("WEB_SERVICO", "ALCOA");
                HasKey(co => new { co.ApplicationId, co.Id });

                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.Id).HasColumnName("COD_SERVICO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.GroupId).HasColumnName("COD_GRUPO").HasColumnType("VARCHAR2");
                Property(co => co.Url).HasColumnName("URL").HasColumnType("VARCHAR2");
                Property(co => co.ServiceType).HasColumnName("TP_SERVICO").HasColumnType("VARCHAR2");
                Property(co => co.IsPrincipal).HasColumnName("IDC_PRINCIPAL").HasColumnType("VARCHAR2");
                Property(co => co.IsFullScreen).HasColumnName("IDC_FULLSCREEN").HasColumnType("VARCHAR2");
                Property(co => co.IsPublic).HasColumnName("IDC_PUBLICO").HasColumnType("VARCHAR2");
                Property(co => co.IsDeepLink).HasColumnName("IDC_DEEP_LINK").HasColumnType("VARCHAR2");
                Property(co => co.IsExternalProvider).HasColumnName("IDC_EXT_PROVIDER").HasColumnType("VARCHAR2");

                HasOptional(co => co.SsoGroup)
                    .WithMany()
                    .HasForeignKey(co => new { co.ApplicationId, co.GroupId })
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.NameOrDescription);
                Ignore(co => co.Validation);
            }
        }

        public class ActiveDirectoryGroupConfiguration : EntityTypeConfiguration<ActiveDirectoryGroup>
        {
            public ActiveDirectoryGroupConfiguration()
            {
                ToTable("WEB_GRUPO_AD", "ALCOA");
                HasKey(co => co.Id);

                Property(co => co.Id).HasColumnName("ID_GRP_AD").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("NOME_GRP").HasColumnType("VARCHAR2");

                Ignore(co => co.UniqueId);
                Ignore(co => co.CreationDate);
                Ignore(co => co.Owner);
                Ignore(co => co.Path);
                Ignore(co => co.Users);
                Ignore(co => co.Validation);
            }
        }

        public class SsoGroupConfiguration : EntityTypeConfiguration<SsoGroup>
        {
            public SsoGroupConfiguration()
            {
                ToTable("WEB_GRUPO_EXIBICAO", "ALCOA");
                HasKey(co => new { co.ApplicationId, co.Id });

                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.Id).HasColumnName("COD_GRUPO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.SsoGroupParentId).HasColumnName("COD_GRUPO_PAI").HasColumnType("VARCHAR2");
                Property(co => co.Order).HasColumnName("ORDENAR");

                HasMany(co => co.SubSsoGroups)
                    .WithOptional(s => s.SsoGroupParent)
                    .HasForeignKey(a => new { a.ApplicationId, a.SsoGroupParentId })
                    .WillCascadeOnDelete(false);

                Ignore(co => co.UniqueId);
                Ignore(co => co.NameOrDescription);
                Ignore(co => co.Validation);
            }
        }

        public class ApplicationTranslationConfiguration : EntityTypeConfiguration<ApplicationTranslation>
        {
            public ApplicationTranslationConfiguration()
            {
                ToTable("WEB_TRANSLATION", "ALCOA");
                HasKey(co => new { co.TranslationId, co.ApplicationId, co.LanguageCultureName });

                Property(co => co.TranslationId).HasColumnName("TRANSLATION_ID").HasColumnType("NUMBER").IsRequired();
                Property(co => co.LanguageCultureName).HasColumnName("LANGUAGE_CODE").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.Key).HasColumnName("KEY").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.TranslationTypeId).HasColumnName("TRANSLATION_TYPE_ID");
                Property(co => co.NameOrDescription).HasColumnName("CONTENT").HasColumnType("VARCHAR2");

                Ignore(co => co.UniqueId);
                Ignore(co => co.IsActive);
                Ignore(co => co.Validation);
                Ignore(co => co.TranslationType);
            }
        }

        public class ApplicationParameterConfiguration : EntityTypeConfiguration<ApplicationParameter>
        {
            public ApplicationParameterConfiguration()
            {
                ToTable("WEB_VARIAVEL_APLICACAO", "ALCOA");
                HasKey(co => new { co.ApplicationId, co.ParameterName });

                Property(co => co.ApplicationId).HasColumnName("COD_APLICACAO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.ParameterName).HasColumnName("NOME_VARIAVEL").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.Content).HasColumnName("CONTEUDO").HasColumnType("VARCHAR2").IsRequired();
                Property(co => co.NameOrDescription).HasColumnName("DESCR_VARIAVEL").HasColumnType("VARCHAR2").IsRequired();

                Ignore(co => co.UniqueId);
                Ignore(co => co.IsActive);
                Ignore(co => co.Validation);
            }
        }
    }
}