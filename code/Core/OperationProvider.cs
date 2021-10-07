using System;
using Microsoft.Extensions.DependencyInjection;

namespace code.Core.Operations
{
    public interface IOperationProvider {
        TOperation GetOperation<TOperation>();
    }

    public class OperationProvider : IOperationProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public OperationProvider(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public TOperation GetOperation<TOperation>()
        {
            return _serviceProvider.GetRequiredService<TOperation>();
        }
    }
}