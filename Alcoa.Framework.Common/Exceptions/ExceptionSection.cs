using Alcoa.Framework.Common.Enumerator;
using System;
using System.Configuration;

namespace Alcoa.Framework.Common.Exceptions
{
    internal class ExceptionSection : ConfigurationSection
    {
        [ConfigurationProperty("loggingExceptionPolicy")]
        public ExceptionPolicyElementCollection Policies
        {
            get { return ((ExceptionPolicyElementCollection)(base["loggingExceptionPolicy"])); }
            set { base["loggingExceptionPolicy"] = value; }
        }
    }

    [ConfigurationCollection(typeof(ExceptionPolicyItem))]
    internal class ExceptionPolicyElementCollection : ConfigurationElementCollection
    {
        private const string PropertyName = "exceptionPolicy";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        protected override string ElementName
        {
            get { return PropertyName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExceptionPolicyItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExceptionPolicyItem)(element)).Name;
        }

        public ExceptionPolicyItem this[int index]
        {
            get { return (ExceptionPolicyItem)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        new public ExceptionPolicyItem this[string policyName]
        {
            get { return (ExceptionPolicyItem)BaseGet(policyName); }
        }
    }

    internal class ExceptionPolicyItem : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (String)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (String)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("levels", IsRequired = true)]
        public LogLevelElementCollection Levels
        {
            get { return (LogLevelElementCollection)this["levels"]; }
        }

        [ConfigurationProperty("layerSource", IsRequired = false)]
        public string LayerSource
        {
            get { return (string)this["layerSource"]; }
        }

        public bool IsLayerSourceDefined
        {
            get { return !string.IsNullOrEmpty(LayerSource); }
        }

        [ConfigurationProperty("layer", IsRequired = false)]
        public string Layer
        {
            get { return (string)this["layer"]; }
        }
    }

    [ConfigurationCollection(typeof(LogLevelElement))]
    internal class LogLevelElementCollection : ConfigurationElementCollection
    {
        private const string PropertyName = "level";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }
        protected override string ElementName
        {
            get { return PropertyName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new LogLevelElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LogLevelElement)(element)).Layer;
        }

        public LogLevelElement this[int index]
        {
            get { return (LogLevelElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        new public LogLevelElement this[string policyName]
        {
            get { return (LogLevelElement)BaseGet(policyName); }
        }
    }

    internal class LogLevelElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
        }

        [ConfigurationProperty("layer", IsRequired = false)]
        public string Layer
        {
            get { return (string)this["layer"]; }
        }

        [ConfigurationProperty("includeStackTrace", IsRequired = false, DefaultValue = false)]
        public bool IncludeStackTrace
        {
            get { return Convert.ToBoolean(this["includeStackTrace"]); }
        }

        [ConfigurationProperty("defaultExceptionMessage", IsRequired = false)]
        public string DefaultExceptionMessage
        {
            get { return (string)this["defaultExceptionMessage"]; }
        }

        public bool IsDefaultExceptiontMessageDefined
        {
            get { return !string.IsNullOrEmpty(DefaultExceptionMessage); }
        }
    }

    internal class LogLevelExceptionMappingItem
    {
        public string ExceptionFulTypeName { get; set; }
        public Layer Layer { get; set; }
        public LogLevel LogLevel { get; set; }
        public bool IncludeStackTrace { get; set; }
        public bool Handled { get; set; }
        public string DefaultFriendlyExceptionMessage { get; set; }
    }
}