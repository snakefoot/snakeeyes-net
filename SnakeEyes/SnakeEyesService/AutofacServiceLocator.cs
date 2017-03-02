using System;
using System.Security;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Microsoft.Practices.ServiceLocation;

namespace SnakeEyes
{
    /// <summary>
    /// Autofac implementation of the Microsoft CommonServiceLocator.
    /// </summary>
    public class AutofacServiceLocator : ServiceLocatorImplBase
    {
        /// <summary>
        /// The <see cref="Autofac.IContext"/> from which services
        /// should be located.
        /// </summary>
        private readonly IContext _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Autofac.Extras.CommonServiceLocator.AutofacServiceLocator" /> class.
        /// </summary>
        [SecuritySafeCritical]
        protected AutofacServiceLocator()
        {
            // This constructor needs to be here for SecAnnotate/CoreCLR security purposes
            // but doesn't get used in standard situations.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Autofac.Extras.CommonServiceLocator.AutofacServiceLocator" /> class.
        /// </summary>
        /// <param name="container">
        /// The <see cref="Autofac.IContext"/> from which services
        /// should be located.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="container" /> is <see langword="null" />.
        /// </exception>
        [SecuritySafeCritical]
        public AutofacServiceLocator(IContext container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
        }

        /// <summary>
        /// Resolves the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be <see langword="null" />.</param>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return key != null ? _container.Resolve(key) : _container.Resolve(serviceType);
        }

        /// <summary>
        /// Resolves all requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <returns>Sequence of service instance objects.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            IEnumerable instance = _container.Resolve(enumerableType) as IEnumerable;
            foreach (object item in instance)
                yield return item;
        }
    }

}
