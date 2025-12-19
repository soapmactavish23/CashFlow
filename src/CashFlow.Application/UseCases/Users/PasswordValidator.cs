using CashFlow.Exception;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CashFlow.Application.UseCases.Users
{
    public partial class PasswordValidator<T> : PropertyValidator<T, string>
    {
        private const string ERROR_MESSAGE_KEY = "ErrorMessage";
        public override string Name => "PasswordValidator";

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return $"{{{ERROR_MESSAGE_KEY}}}";
        }

        public override bool IsValid(ValidationContext<T> context, string password)
        {

            if(string.IsNullOrWhiteSpace(password))
            {
                return AppendArgumentPassword(context, password);
            }

            if(password.Length < 8)
            {
                return AppendArgumentPassword(context, password);
            }

            if(UpperCaseLetter().IsMatch(password) == false)
            {
                return AppendArgumentPassword(context, password);
            }

            if (LowerCaseLetter().IsMatch(password) == false)
            {
                return AppendArgumentPassword(context, password);
            }

            if (Numbers().IsMatch(password) == false)
            {
                return AppendArgumentPassword(context, password);
            }

            if (Symbols().IsMatch(password) == false)
            {
                return AppendArgumentPassword(context, password);
            }

            return true;
        }

        private bool AppendArgumentPassword(ValidationContext<T> context, string password)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessage.INVALID_PASSWORD);
            return false;
        }

        [GeneratedRegex(@"[A-Z]+")]
        private partial Regex UpperCaseLetter();
        
        [GeneratedRegex(@"[a-z]+")]
        private partial Regex LowerCaseLetter();

        [GeneratedRegex(@"[0-9]+")]
        private partial Regex Numbers();
        [GeneratedRegex(@"[\!\?\*\.]+")]
        private partial Regex Symbols();
    }
}
