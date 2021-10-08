using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace code.Core.Operations
{
    // An abstraction of unit-testabled application functionality. Base class for all application operations.
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

    // Auxiliary user-defined type for representing request validation results.
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