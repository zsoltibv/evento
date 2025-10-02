namespace Evento.Application.Common.Errors;

public static class AuthErrors
{
    public static readonly ErrorResponse UsernameIsEmpty =
        new("AuthErrors.UsernameIsEmpty", "Username is empty.");

    public static readonly ErrorResponse EmailIsEmpty =
        new("AuthErrors.EmailIsEmpty", "Email is required.");

    public static readonly ErrorResponse EmailInvalid =
        new("AuthErrors.EmailInvalid", "Email is invalid.");
    
    public static readonly ErrorResponse EmailOrPasswordIncorrect =
        new("AuthErrors.EmailOrPasswordIncorrect", "Email and/or password is incorrect.");

    public static readonly ErrorResponse PasswordIsEmpty =
        new("AuthErrors.PasswordIsEmpty", "Password is required.");

    public static readonly ErrorResponse ConfirmPasswordIsEmpty =
        new("AuthErrors.ConfirmPasswordIsEmpty", "Confirm password is required.");

    public static readonly ErrorResponse PasswordsMismatch =
        new("AuthErrors.PasswordsMismatch", "Passwords do not match.");
}