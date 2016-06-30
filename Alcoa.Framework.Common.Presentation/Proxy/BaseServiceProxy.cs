using System.ServiceModel;

namespace Alcoa.Framework.Common.Presentation.Proxy
{
    /// <summary>
    /// Class that helps initialize WCF services channels
    /// </summary>
    public class BaseServiceProxy<Tcontract>
        where Tcontract : class
    {
        private readonly Tcontract _channel;
        private readonly string _contractName = typeof(Tcontract).Name;

        /// <summary>
        /// Instantiate and create endpoint channel (connection)
        /// </summary>
        public BaseServiceProxy()
        {
            var endPoint = ServiceModelHelper.SearchClientEndpointConfigs(_contractName);

            _channel = new ChannelFactory<Tcontract>(endPoint.Name).CreateChannel();
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseServiceProxy(string endpointName)
        {
            _channel = new ChannelFactory<Tcontract>(endpointName).CreateChannel();
        }

        /// <summary>
        /// Get the initialized endpoint channel (connection)
        /// </summary>
        public Tcontract GetChannel()
        {
            return _channel;
        }
    }
}