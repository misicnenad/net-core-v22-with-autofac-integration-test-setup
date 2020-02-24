using Autofac;

using NetCoreV22.Interfaces;
using NetCoreV22.Services;

namespace NetCoreV22
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestValidationService>().As<IRequestValidationService>();
        }
    }
}