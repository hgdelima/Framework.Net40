using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common.Enumerator;
using System.Collections.Generic;

namespace Alcoa.Framework.Application.Service
{
    public abstract class AbstractService
    {
        protected IUnitOfWork Uow;

        protected Dictionary<CommonDatabase, string> MultipleProfiles;

        protected string LanguageCultureName;
    }
}