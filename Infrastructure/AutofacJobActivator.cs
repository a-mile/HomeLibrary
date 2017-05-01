using System;
using Autofac;
using Hangfire;

namespace HomeLibrary.Infrastructure
{
    public class AutofacJobActivator : JobActivator
    {
        private IContainer _container;

        public AutofacJobActivator(IContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type type)
        {
            return _container.Resolve(type);
        }
    }
}