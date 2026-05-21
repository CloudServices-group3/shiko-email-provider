namespace Shiko.EmailProvider.API.Models;

public record EmailRequest(
    
    string ToEmail,
    string VerificationCode
    
    );