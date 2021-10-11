using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Account.Api.Core
{
    // Base class for all application operations. 
    // Implements a general request-handling flow in a Template Method fashion and handles errors. 
    // Declares abstract methods for the request flow steps that subclasses can override.
    public abstract class Operation<TRequest, TResult> : IOperation<TRequest, TResult>
        where TResult: ServiceResponse, new()
    {
        private readonly ILogger<Operation<TRequest, TResult>> _logger;

        protected Operation(ILogger<Operation<TRequest, TResult>> logger) 
        { 
            _logger = logger;
        }

        public async Task<TResult> Execute(TRequest request) {
            try 
            {
                if (request == null) 
                {
                    return new TResult { Success = false, Message = "Invalid request: null." };
                }

                if (_logger.IsEnabled(LogLevel.Debug)) {
                    _logger.LogDebug($"Processing request: { JsonConvert.SerializeObject(request) }");
                }

                var validationResult = ValidateRequest(request);

                // Deliberately skipping checking null reference - let the program blow in case of a programming error.
                // The error will be caught by the surround try-catch block and logger properly.
                if (!validationResult.Passed) 
                {
                    return new TResult { Success = false, Message = validationResult.Message };
                }

                return await ExecuteRequest(request, validationResult);
            }
            catch (Exception e) 
            {
                var errorMessage = $"Failed to execute request due to an internal server error.";
                _logger.LogError(e, errorMessage);
                return new TResult { Success = false, Message = errorMessage };
            }
        }

        // Validates input request
        protected virtual ValidationResult ValidateRequest(TRequest request) 
        {
            return new ValidationResult{ Passed = true };
        }

        // Executes validated input request
        protected abstract Task<TResult> ExecuteRequest(TRequest request, ValidationResult validationResult);
    }

    // An abstraction of request validation results.
    public class ValidationResult 
    {
        public bool Passed { get; set; }
        public string Message { get; set; }

        public static ValidationResult Success() 
        {
            return new ValidationResult { Passed = true };
        }

        public static ValidationResult Failure() 
        {
            return new ValidationResult { Passed = false };
        }

        public ValidationResult WithMessage(string message) 
        {
            Message = message;
            return this;
        }
    }
}