using Alcoa.Framework.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Alcoa.Framework.DataAccess.Context.Oracle
{
    public class EmailConfigurations
    {
        public class EmailGatewayConfiguration : EntityTypeConfiguration<EmailGateway>
        {
            public EmailGatewayConfiguration()
            {
                ToTable("SSM_REPOSITORIO_MAIL", "ALCOA");
                HasKey(co => new { co.CurrentCode, co.JobSequence });
                Property(co => co.CurrentCode).HasColumnName("COD_NO_CORRENTE");
                Property(co => co.JobSequence).HasColumnName("SEQ_JOB");
                Property(co => co.ApplicationCode).HasColumnName("COD_APLICACAO");
                Property(co => co.Login).HasColumnName("LOGIN");
                Property(co => co.To).HasColumnName("DESTINATARIO");
                Property(co => co.Subject).HasColumnName("TITULO");
                Property(co => co.Body).HasColumnName("CONTEUDO");
                Property(co => co.BodyComplement).HasColumnName("CONTEUDO_COMPL");
                Property(co => co.AttachmentOne).HasColumnName("ARQ_ATACHADO_1");
                Property(co => co.AttachmentTwo).HasColumnName("ARQ_ATACHADO_2");
                Property(co => co.AttachmentThree).HasColumnName("ARQ_ATACHADO_3");
                Property(co => co.IsProcessed).HasColumnName("IDC_PROC");
                Property(co => co.IsEvent).HasColumnName("IDC_EVENTO");
                Property(co => co.AttemptNumber).HasColumnName("TENTATIVA");
                Property(co => co.SentDateTime).HasColumnName("DT_HR_ENVIO");
                Property(co => co.CreationDateTime).HasColumnName("DT_HR_INCLUSAO");

                Ignore(co => co.UniqueId);
                Ignore(co => co.From);
                Ignore(co => co.FromName);
                Ignore(co => co.IsHtml);
                Ignore(co => co.SendResultMessage);
                Ignore(co => co.Attachments);
                Ignore(co => co.Validation);
            }
        }

        public class EmailGatewayLogConfiguration : EntityTypeConfiguration<EmailGatewayLog>
        {
            public EmailGatewayLogConfiguration()
            {
                ToTable("SSM_LOG", "ALCOA");
                HasKey(co => new { co.CurrentCode, co.JobSequence, co.JobType });
                Property(co => co.CurrentCode).HasColumnName("COD_NO_CORRENTE");
                Property(co => co.JobSequence).HasColumnName("SEQ_JOB");
                Property(co => co.JobType).HasColumnName("TP_JOB");
                Property(co => co.ApplicationCode).HasColumnName("COD_APLICACAO");
                Property(co => co.Login).HasColumnName("LOGIN");
                Property(co => co.MessageCode).HasColumnName("COD_MENS");
                Property(co => co.MessageDescription).HasColumnName("DESCR_MENS");
                Property(co => co.To).HasColumnName("DESTINATARIO");
                Property(co => co.Subject).HasColumnName("TITULO");
                Property(co => co.StartDateTime).HasColumnName("DT_HR_INICIAL");
                Property(co => co.CreationDateTime).HasColumnName("DT_HR_INCLUSAO");

                Ignore(co => co.UniqueId);
                Ignore(co => co.Validation);
            }
        }
    }
}