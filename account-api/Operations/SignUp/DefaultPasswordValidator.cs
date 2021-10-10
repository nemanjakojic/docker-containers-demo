using System;
using System.Collections.Generic;
using code.Core.Operations;

namespace code.Operations.SignUp
{
    public class DefaultPasswordValidator : IPasswordValidator
    {
        public int MinimumDigitsRequired { get; set; }
        public int MinimumLowercaseLettersRequired { get; set; }
        public int MinimumNonAlphanumericsRequired { get; set; }
        public int MinimumUppercaseLettersRequired { get; set; }
        public int MinimumLength { get; set; }

        public DefaultPasswordValidator() {
            MinimumLength = 6;
        }

        public static DefaultPasswordValidator Configure(Action<DefaultPasswordValidator> config) {
            DefaultPasswordValidator validator = new DefaultPasswordValidator();
            config(validator);
            return validator;
        }

        public ValidationResult Validate(string password)
        {
            int numDigits = 0;
            int numLowercaseLetters = 0;
            int numNonAlphanumerics = 0;
            int numUppercaseLetters = 0;
            int numUnexpectedLetters = 0;

            foreach (char c in password) 
            {
                if (char.IsDigit(c)) // Requires at least one digit
                {
                    numDigits++;
                }
                else if (char.IsLower(c)) // Requires at least one lowercase letter
                {
                    numLowercaseLetters++;
                }
                else if (!char.IsLetterOrDigit(c)) // Requires at least one non-alphanumeric
                {
                    numNonAlphanumerics++;
                }
                else if (char.IsUpper(c)) // Requires at least one uppercase letter
                {
                    numUppercaseLetters++;
                }
                else // Must not contain unexpected letters
                {
                    numUnexpectedLetters++;
                }
            }

            var validationMessages = new List<string>();
            bool success = true;

            if (numDigits < MinimumDigitsRequired) 
            {
                validationMessages.Add($"Minimum digits required: { MinimumDigitsRequired }.");
                success = false;
            }
            if (numLowercaseLetters < MinimumLowercaseLettersRequired) 
            {
                validationMessages.Add($"Minimum lowercase letters required: { MinimumLowercaseLettersRequired }.");
                success = false;
            }
            if (numNonAlphanumerics < MinimumNonAlphanumericsRequired) 
            {
                validationMessages.Add($"Minimum non-alphanumeric letters required: { MinimumNonAlphanumericsRequired }.");
                success = false;
            }
            if (numUppercaseLetters < MinimumUppercaseLettersRequired) 
            {
                validationMessages.Add($"Minimum uppercase letters required: { MinimumUppercaseLettersRequired }.");
                success = false;
            }
            if (numUnexpectedLetters > 0) 
            {
                validationMessages.Add("Password contains invalid letters.");
                success = false;
            }
            if (password.Length < MinimumLength) 
            {
                validationMessages.Add($"Miminum password length required: { MinimumLength }.");
                success = false;
            }

            var validationResult = success ? ValidationResult.Success() : ValidationResult.Failure();
            if (validationMessages.Count > 0) {
                validationResult.Message = string.Join(' ', validationMessages);
            }
            return validationResult;
        }
    }
}