using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps get WCF services configurations
    /// </summary>
    public static class ServiceModelHelper
    {
        private static ClientSection _clientEndpointSection = ConfigHelper.GetSection<ClientSection>(@"system.serviceModel/client");
        private static BindingsSection _bindingsSection = ConfigHelper.GetSection<BindingsSection>(@"system.serviceModel/bindings");

        /// <summary>
        /// Search for a endpoint with a given name or with a contract name
        /// </summary>
        public static ChannelEndpointElement SearchClientEndpointConfigs(string name)
        {
            var endpoint = default(ChannelEndpointElement);

            if (_clientEndpointSection != null &&
                _clientEndpointSection.Endpoints != null)
            {
                endpoint = _clientEndpointSection.Endpoints
                     .Cast<ChannelEndpointElement>()
                     .FirstOrDefault(cee => cee.Name.EndsWith(name) || cee.Contract.EndsWith(name));
            }

            return endpoint;
        }

        /// <summary>
        /// Creates a service binding object using .Config data
        /// </summary>
        public static Binding CreateBinding<T>()
            where T : Binding, new()
        {
            var binding = default(Binding);

            if (_bindingsSection != null)
            {
                var bcConfig = _bindingsSection.BindingCollections
                    .FirstOrDefault(bc => bc.BindingType == typeof(T) && bc.ConfiguredBindings.Count > 0);

                if (bcConfig == null)
                    throw new Exception("Error");

                var config = bcConfig.ConfiguredBindings.FirstOrDefault();

                binding = new T();
                binding.CloseTimeout = config.CloseTimeout;
                binding.OpenTimeout = config.OpenTimeout;
                binding.ReceiveTimeout = config.ReceiveTimeout;
                binding.SendTimeout = config.SendTimeout;
            }

            return binding;
        }

        /// <summary>
        /// Creates a new service channel using .Config data for a specific contract
        /// </summary>
        public static TContract CreateChannel<TContract>()
        {
            var endpointConfig = ServiceModelHelper.SearchClientEndpointConfigs(typeof(TContract).Name);
            var endpointAddress = new EndpointAddress(endpointConfig.Address);
            var binding = default(Binding);

            switch (endpointConfig.Binding.ToUpper())
            {
                case "BASICHTTPBINDING":
                    binding = CreateBinding<BasicHttpBinding>();
                    break;
                case "WSHTTPBINDING":
                    binding = CreateBinding<WSHttpBinding>();
                    break;
                case "NETTCPBINDING":
                    binding = CreateBinding<NetTcpBinding>();
                    break;
                case "NETNAMEDPIPEBINDING":
                    binding = CreateBinding<NetNamedPipeBinding>();
                    break;
                default:
                    binding = CreateBinding<BasicHttpBinding>();
                    break;
            }

            return new ChannelFactory<TContract>(binding, endpointAddress).CreateChannel();
        }
    }
}