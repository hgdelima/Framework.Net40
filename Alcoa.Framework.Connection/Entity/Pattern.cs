using System;
using System.Collections.Generic;

namespace Alcoa.Framework.Connection.Entity
{
    [Serializable]
    internal class Pattern
    {
        public Pattern()
        {
            //Initialize pattern with pre-defined pattern options
            PatternOptions = new Dictionary<string, string>
            {
                {"[APP]", string.Empty},
                {"[PROFILE]", string.Empty},
                {"[YYYY]", DateTime.Today.Year.ToString()},
                {"[COMPANYNAME]", "Alcoa"},
                {"[NUMBER]", "7878"},
                {"[SPECIALCHAR]", "!#"}
            };
        }

        public string PatternValue { get; set; }

        public Dictionary<string, string> PatternOptions { get; set; }
    }
}