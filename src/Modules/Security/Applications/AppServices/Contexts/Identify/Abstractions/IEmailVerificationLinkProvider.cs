public interface IEmailVerificationLinkProvider
{
    Task<string> GenerateLinkAsync();
}