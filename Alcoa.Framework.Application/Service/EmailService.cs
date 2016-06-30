using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Contract.DTOs;
using Alcoa.Framework.Contract.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using System;
using System.ServiceModel;

namespace Alcoa.Framework.Application.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class EmailService : AbstractService, IEmailService
    {
        private IRepository<EmailGateway> _emailRepo;
        private IRepository<EmailGatewayLog> _emailLogRepo;

        public EmailService()
        {
            //Initialize database and repositories for gateway mail sending
            Uow = new UnitOfWork<EmailContextSsm>();
            _emailRepo = Uow.GetRepository<EmailGateway>();
            _emailLogRepo = Uow.GetRepository<EmailGatewayLog>();
        }
    }
}