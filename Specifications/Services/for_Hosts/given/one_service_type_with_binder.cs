/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.DependencyInversion;
using Dolittle.Logging;
using Dolittle.Types;
using Dolittle.Types.Testing;
using Machine.Specifications;
using Moq;

namespace Dolittle.Services.given
{
    public class one_service_type_with_binder
    {
        protected const string service_type_identifier = "My Service Type";

        protected static Mock<IContainer> container;
        protected static Mock<ITypeFinder> type_finder;
        protected static Mock<IBoundServices> bound_services;
        protected static ILogger logger;
        

        protected static Mock<IRepresentServiceType> service_type;

        protected static StaticInstancesOf<IRepresentServiceType> service_types;

        protected static Mock<ICanBindMyServiceType> binder;
        protected static Mock<IHost> host;

        protected static ServiceType identifier;
        protected static Type binding_interface;
        protected static HostConfiguration configuration;

        Establish context = () =>
        {
            container = new Mock<IContainer>();
            type_finder = new Mock<ITypeFinder>();
            bound_services = new Mock<IBoundServices>();
            logger = Mock.Of<ILogger>();

            host = new Mock<IHost>();
            container.Setup(_ => _.Get<IHost>()).Returns(host.Object);

            identifier = service_type_identifier;
            binding_interface = typeof(ICanBindMyServiceType);
            configuration = new HostConfiguration();

            binder = new Mock<ICanBindMyServiceType>();
            var binderType = binder.Object.GetType();
            type_finder.Setup(_ => _.FindMultiple(typeof(ICanBindMyServiceType))).Returns(new [] { binderType });
            container.Setup(_ => _.Get(binderType)).Returns(binder.Object);

            service_type = new Mock<IRepresentServiceType>();
            service_type.SetupGet(_ => _.Identifier).Returns(identifier);
            service_type.SetupGet(_ => _.BindingInterface).Returns(binding_interface);
            service_type.SetupGet(_ => _.Configuration).Returns(configuration);
            service_types = new StaticInstancesOf<IRepresentServiceType>(service_type.Object);
        };
    }
}