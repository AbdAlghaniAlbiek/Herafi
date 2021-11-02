using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IdentityModel.Tokens.Jwt;
using Herafi.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Herafi.Core.Helpers;

namespace Herafi.Core.Security
{
    //public enum JwtHashAlgorithm
    //{
    //    RS256,
    //    HS384,
    //    HS512
    //}

    public static class JWTAuthorization
    {
        //public static async string GenerateToken(Claim[] claims)
        //{
        //    //RSA rsa = RSA.Create();
        //    //rsa.FromXmlString(await Common.JWT_PRIVATE_KEY());

        //    //var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
        //    //{
        //    //    CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        //    //};

        //    //var now = DateTime.Now;
        //    //var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

        //    //var jwt = new JwtSecurityToken(
        //    //    audience: _settings.Audience,
        //    //    issuer: _settings.Issuer,
        //    //    claims: new Claim[] {
        //    //        new Claim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
        //    //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    //        new Claim(nameof(claims.FirstName), claims.FirstName),
        //    //        new Claim(nameof(claims.LastName), claims.LastName),
        //    //        new Claim(nameof(claims.Email), claims.Email)
        //    //    },
        //    //    notBefore: now,
        //    //    expires: now.AddMinutes(30),
        //    //    signingCredentials: signingCredentials
        //    //);

        //    //string token = new JwtSecurityTokenHandler().WriteToken(jwt);

        //    //return new JwtResponse
        //    //{
        //    //    Token = token,
        //    //    ExpiresAt = unixTimeSeconds,
        //    //};
        //}

        public async static Task<bool> ValidateToken(string token)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            RSACryptoServiceProviderExtensions.FromXmlString(rsa, await Common.JWT_PUBLIC_KEY());

            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new RsaSecurityKey(rsa),
                CryptoProviderFactory = new CryptoProviderFactory()
                {
                    CacheSignatureProviders = false
                }
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();
                ClaimsPrincipal tokenValid = handler.ValidateToken(token, validationParameters, out var validatedSecurityToken);

                if (AESCryptography.Decrypt(tokenValid.Claims.ToList()[1].Value) == 
                    AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                {
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }





        //#region Public Methods

        ///// <summary>
        ///// Validates whether a given token is valid or not, and returns true in case the token is valid otherwise it will return false;
        ///// </summary>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //public static bool IsTokenValid(string token, string publicKey)
        //{
        //    if (string.IsNullOrEmpty(token))
        //        throw new ArgumentException("Given token is null or empty.");

        //    TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters(publicKey);

        //    JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        //    try
        //    {
        //        ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        //        var secretKeywork = tokenValid.Claims.ToList()[1].Value;

        //        if (AESCryptography.Decrypt(secretKeywork) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
        //        {
        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}


        ///// <summary>
        ///// Generates token by given model.
        ///// Validates whether the given model is valid, then gets the symmetric key.
        ///// Encrypt the token and returns it.
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns>Generated token.</returns>
        //public static string GenerateToken(Token model, string privateKey)
        //{
        //    if (model == null || model.Claims == null || model.Claims.Length == 0)
        //        throw new ArgumentException("Arguments to create token are not valid.");

        //    SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(model.Claims),
        //        Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(model.ExpireMinutes)),
        //        SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(privateKey), model.SecurityAlgorithm)
        //    };

        //    JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        //    SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        //    string token = jwtSecurityTokenHandler.WriteToken(securityToken);

        //    return token;
        //}


        ///// <summary>
        ///// Receives the claims of token by given token as string.
        ///// </summary>
        ///// <remarks>
        ///// Pay attention, one the token is FAKE the method will throw an exception.
        ///// </remarks>
        ///// <param name="token"></param>
        ///// <returns>IEnumerable of claims for the given token.</returns>
        //public static IEnumerable<Claim> GetTokenClaims(string token, string publicKey)
        //{
        //    if (string.IsNullOrEmpty(token))
        //        throw new ArgumentException("Given token is null or empty.");

        //    TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters(publicKey);

        //    JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        //    try
        //    {
        //        ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        //        return tokenValid.Claims;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //#endregion


        //#region Private Methods

        //private static SecurityKey GetSymmetricSecurityKey(string Key)
        //{
        //    byte[] symmetricKey = Convert.FromBase64String(Key);
        //    return new SymmetricSecurityKey(symmetricKey);
        //}

        //private static TokenValidationParameters GetTokenValidationParameters(string Key)
        //{
        //    return new TokenValidationParameters()
        //    {
        //        ValidateIssuer = false,
        //        ValidateAudience = false,
        //        IssuerSigningKey = GetSymmetricSecurityKey(Key)
        //    };
        //}

        //#endregion




        //private static Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms =
        //    new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
        //    {
        //        { JwtHashAlgorithm.RS256, (key, value) => { using (var sha = new HMACSHA256(key)) { return sha.ComputeHash(value); } } },
        //        { JwtHashAlgorithm.HS384, (key, value) => { using (var sha = new HMACSHA384(key)) { return sha.ComputeHash(value); } } },
        //        { JwtHashAlgorithm.HS512, (key, value) => { using (var sha = new HMACSHA512(key)) { return sha.ComputeHash(value); } } }
        //    };

        //public static string Encode(object payload, string key, JwtHashAlgorithm algorithm)
        //{
        //    return Encode(payload, Encoding.UTF8.GetBytes(key), algorithm);
        //}

        //public static string Encode(object payload, byte[] keyBytes, JwtHashAlgorithm algorithm)
        //{
        //    var segments = new List<string>();
        //    var header = new { alg = algorithm.ToString(), typ = "JWT" };

        //    byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Newtonsoft.Json.Formatting.None));
        //    byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Newtonsoft.Json.Formatting.None));
        //    //byte[] payloadBytes = Encoding.UTF8.GetBytes(@"{"iss":"761326798069-r5mljlln1rd4lrbhg75efgigp36m78j5@developer.gserviceaccount.com","scope":"https://www.googleapis.com/auth/prediction","aud":"https://accounts.google.com/o/oauth2/token","exp":1328554385,"iat":1328550785}");

        //    segments.Add(Base64UrlEncode(headerBytes));
        //    segments.Add(Base64UrlEncode(payloadBytes));

        //    var stringToSign = string.Join(".", segments.ToArray());

        //    var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

        //    byte[] signature = HashAlgorithms[algorithm](keyBytes, bytesToSign);
        //    segments.Add(Base64UrlEncode(signature));

        //    return string.Join(".", segments.ToArray());
        //}

        //public static string Decode(string token, string key)
        //{
        //    return Decode(token, key, true);
        //}

        //public static string Decode(string token, string key, bool verify)
        //{
        //    var parts = token.Split('.');
        //    var header = parts[0];
        //    var payload = parts[1];
        //    byte[] crypto = Base64UrlDecode(parts[2]);

        //    var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
        //    var headerData = JObject.Parse(headerJson);
        //    var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
        //    var payloadData = JObject.Parse(payloadJson);

        //    if (verify)
        //    {
        //        var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
        //        var keyBytes = Encoding.UTF8.GetBytes(key);
        //        var algorithm = (string)headerData["alg"];

        //        var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
        //        var decodedCrypto = Convert.ToBase64String(crypto);
        //        var decodedSignature = Convert.ToBase64String(signature);

        //        if (decodedCrypto != decodedSignature)
        //        {
        //            throw new ApplicationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
        //        }
        //    }

        //    return payloadData.ToString();
        //}

        //private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        //{
        //    switch (algorithm)
        //    {
        //        case "RS256": return JwtHashAlgorithm.RS256;
        //        case "HS384": return JwtHashAlgorithm.HS384;
        //        case "HS512": return JwtHashAlgorithm.HS512;
        //        default: throw new InvalidOperationException("Algorithm not supported.");
        //    }
        //}

        //// from JWT spec
        //private static string Base64UrlEncode(byte[] input)
        //{
        //    var output = Convert.ToBase64String(input);
        //    output = output.Split('=')[0]; // Remove any trailing '='s
        //    output = output.Replace('+', '-'); // 62nd char of encoding
        //    output = output.Replace('/', '_'); // 63rd char of encoding
        //    return output;
        //}

        //// from JWT spec
        //private static byte[] Base64UrlDecode(string input)
        //{
        //    var output = input;
        //    output = output.Replace('-', '+'); // 62nd char of encoding
        //    output = output.Replace('_', '/'); // 63rd char of encoding
        //    switch (output.Length % 4) // Pad with trailing '='s
        //    {
        //        case 0: break; // No pad chars in this case
        //        case 2: output += "=="; break; // Two pad chars
        //        case 3: output += "="; break; // One pad char
        //        default: throw new System.Exception("Illegal base64url string!");
        //    }
        //    var converted = Convert.FromBase64String(output); // Standard base64 decoder
        //    return converted;
        //}
    }
}

