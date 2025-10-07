namespace Evento.Application.Common.Errors;

public static class AuthErrors
{
    public static readonly Error UsernameIsEmpty =
        new("AuthErrors.UsernameIsEmpty", "Username is empty.");

    public static readonly Error EmailIsEmpty =
        new("AuthErrors.EmailIsEmpty", "Email is required.");

    public static readonly Error EmailInvalid =
        new("AuthErrors.EmailInvalid", "Email is invalid.");
    
    public static readonly Error EmailOrPasswordIncorrect =
        new("AuthErrors.EmailOrPasswordIncorrect", "Email and/or password is incorrect.");

    public static readonly Error PasswordIsEmpty =
        new("AuthErrors.PasswordIsEmpty", "Password is required.");

    public static readonly Error ConfirmPasswordIsEmpty =
        new("AuthErrors.ConfirmPasswordIsEmpty", "Confirm password is required.");

    public static readonly Error PasswordsMismatch =
        new("AuthErrors.PasswordsMismatch", "Passwords do not match.");
}