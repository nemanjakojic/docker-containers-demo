using System;
using Microsoft.Extensions.DependencyInjection;

namespace Docker.Test.Core
{
    // Creates and initializes an application operation of the type TOperation.
    public interface IOperationProvider 
    {
        TOperation GetOperation<TOperation>();
    }

    // Default implementation of IOperationProvider.
    public class OperationProvider : IOperationProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public OperationProvider(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public TOperation GetOperation<TOperation>()
        {
            return _serviceProvider.GetRequiredService<TOperation>();
        }
    }
}