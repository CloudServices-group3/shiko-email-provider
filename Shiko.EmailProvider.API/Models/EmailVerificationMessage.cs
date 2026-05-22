namespace Shiko.EmailProvider.API.Models;

public sealed record EmailVerificationMessage(

    string To,
    string VerificationCode

);