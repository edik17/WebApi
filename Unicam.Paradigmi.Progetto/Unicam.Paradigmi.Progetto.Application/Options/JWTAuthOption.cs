namespace Unicam.Paradigmi.Progetto.Application.Options
{
    /// <summary>
    /// Represents the options for JWT authentication.
    /// We have the ISSUER and the KEY for the JWT token.
    /// ISSUER is the name of the server that issued the token.
    /// KEY is the secret key used to sign the token.
    /// That configuration is in the appsettings.json file.
    /// </summary>
    public class JWTAuthOption
    {
        public string Issuer { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }
}
