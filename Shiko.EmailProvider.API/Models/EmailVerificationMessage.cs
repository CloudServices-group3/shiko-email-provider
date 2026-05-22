namespace Shiko.EmailProvider.API.Models;

public sealed record EmailVerificationMessage(

    string To,
    string Subject,
    string Body,
    string VerificationCode,
    DateTimeOffset IssuedAtUtc,
    DateTimeOffset ExpiresAtUtc

);