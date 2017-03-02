using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace SnakeEyes
{
    public class WCFErrorHandler : IErrorHandler
    {
        TraceSource _traceSource;

        public WCFErrorHandler(TraceSource traceSource)
        {
            _traceSource = traceSource;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {

        }

        public bool HandleError(Exception error)
        {
            if (_traceSource.Listeners.Count == 0)
                Trace.WriteLine("exception");
            else
                _traceSource.TraceEvent(TraceEventType.Critical, 0, error.Message);
            return false;
        }
    }

    public class WCFErrorServiceBehavior : IServiceBehavior
    {
        TraceSource _traceSource;

        public WCFErrorServiceBehavior(TraceSource traceSource)
        {
            _traceSource = traceSource;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            WCFErrorHandler handler = new WCFErrorHandler(_traceSource);
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(handler);
            }
        }
    }
}
