using Autofac;
using Autofac.Integration.Web;
using Enexure.MicroBus;
using Enexure.MicroBus.Autofac;
using ForMicroBus.DemoMicroBus;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace ForMicroBus
{
    public class Global : HttpApplication, IContainerProviderAccessor
    {
        #region DI with Autofac and CQRS with MicroBus
        // Provider that holds the application container.
        static IContainerProvider _containerProvider;

        // Instance property that will be used by Autofac HttpModules
        // to resolve and inject dependencies.
        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }

        void RegisterContainerProvider()
        {
            // Build up your application container and register your dependencies.
            var builder = new ContainerBuilder();
            var busBuilder = new BusBuilder()
                .RegisterCancelableCommandHandler<InterruptableSlowWorkingCommand, DemoCommandHandler>() // Footnote #2
                .RegisterCommandHandler<SlowWorkingCommand, DemoCommandHandler>();
            builder.RegisterMicroBus(busBuilder); // Footnote #3

            // Once you're done registering things, set the container
            // provider up with your registrations.
            _containerProvider = new ContainerProvider(builder.Build());
        }
        #endregion

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterContainerProvider(); // Footnote #1
        }
    }
    /* ***********
     * Footnotes:
     *  1 - For notes on using Autofac with Web Forms, see
     *      http://autofaccn.readthedocs.io/en/latest/integration/webforms.html
     *  
     *  2 - See https://github.com/Lavinski/Enexure.MicroBus/wiki/Cancellable-Handlers

     *  3 - For notes on using MicroBus with Autofac, see
     *      https://github.com/Lavinski/Enexure.MicroBus/blob/master/Readme.md
     */
}