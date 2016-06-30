using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Exceptions;
using Alcoa.Framework.Common.Presentation.GarService;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Alcoa.Framework.Common.Presentation.Web
{
    /// <summary>
    /// Class with GAR properties to show app infos
    /// </summary>
    public static class GarModel
    {
        private static string _appCode;

        private static string _appGarUsageId;
        public static string AppGarUsageId
        {
            get { return _appGarUsageId; }
        }

        private static GarApplication _appGar;
        public static GarApplication AppGar
        {
            get
            {
                if (_appGar == null)
                    _appGar = new GarServiceClient().GetGarApplication(_appCode);

                return _appGar;
            }
        }
        
        private static List<GarInstance> _appGarInstances;
        public static List<GarInstance> AppGarInstances
        {
            get { return (_appGarInstances ?? (_appGarInstances = AppGar.GarInstances.ToList())); }
        }

        private static List<GarUsage> _appGarUsages;
        public static List<GarUsage> AppGarUsages
        {
            get { return _appGarUsages ?? (_appGarUsages = AppGarInstances.FirstOrDefault().GarUsages.ToList()); }
        }

        static GarModel()
        {
            _appCode = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.ApplicationCode);
            _appGarUsageId = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.ApplicationGarUsageId);
        }
    }
}