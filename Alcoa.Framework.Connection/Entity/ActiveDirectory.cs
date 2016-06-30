using System;

namespace Alcoa.Framework.Connection.Entity
{
    [Serializable]
    internal class ActiveDirectory
    {
        public ActiveDirectory()
        {
        }

        public string Domain { get; set; }

        public string OrganizationUnit { get; set; }

        public string RootOrganizationUnit { get; set; }

        public string ServiceUser { get; set; }

        public string ServicePassword { get; set; }

        public short? SearchPriority { get; set; }
    }
}