# CallAutomation.AspNetCore.Authentication.JwtBearer

This project contains several extension methods used with an ASP.NET web application to protect and validate public web endpoints.

## Protecting Webhook Endpoints

The Azure Communication Services Call Automation platform uses HTTPS webhook callbacks to send events for call setup and mid-call action responses. The webhook endpoint must be publicly accessible by the Call Automation servers which leaves some people to be concerned about protecting these endpoints from unauthorized access.

A recent addition to the Call Automation platform allows you to use Json Web Token (JWT) bearer authentication and OAuth2's Open ID Connect extensions to verify the inbound communications to your web server.

The JWT bearer token is present on all callbacks and is in the `Authorization` header of the inbound HTTP request.

## Application Configuration

You can use the built-in constants from the `AcsOpenIdDefaults` class to retrieve configuration information.

As an example, you can configure your `secrets.json` file for local development using the `AcsJwtBearerOptions` section name which is defined in the `AcsJwtBearerOptions` class as a static string value. The `ValidAudience` property refers to your Azure Communication Services immutable resource ID which can be found in your ACS resource. The `aud` claim of the JWT bearer token will need to match this resource ID value.

```json
{
  "AcsJwtBearerOptions": {
    "ValidAudience": "abc8b7b5-6666-4e99-a66f-r90c600e6cb9"
  }
}
```

## Program.cs configuration

```csharp

// use the extension method to add the authentication scheme and policy and bind to the configuration section name automatically.
builder.Services.AddAcsWebHookAuthentication(x => builder.Configuration.Bind(AcsOpenIdDefaults.SectionName, x));


// protect an HTTP endpoint by adding the extension method as follows
app.MapGet("/{name}", (string name) => $"Hello {name}!")
    .RequireAcsWebHookAuthorization();

```

You have the option of using your own policy name on both the `AddAcsWebHookAuthentication` and `RequireAcsWebHookAuthorization` methods.

## Validation behavior

This library will perform JWT bearer token validation on endpoints matching the policy you specify or using the default policy in this library. On protected endpoints the authentication middleware in ASP.NET will trigger the retrieval of the JWKS signing keys and issuer value from the `OpenIdConfigurationUrl` which has already been set to the correct default value for Call Automation.

The token validation parameters will validate the following:

- The `iss` claim must match the issuer from the JWKS URL
- The `aud` claim must match the `ValidAudience` property of the `AcsJwtBearerOptions` class which was bound during startup.
- The token's lifetime is validated (i.e. no more than 5 minutes past the expiration time)
- All other default values from the ASP.NET `TokenValidationParameters` class also apply.

## More information

Azure Communication Services Call Automation documentation: https://review.learn.microsoft.com/en-us/azure/communication-services/how-tos/call-automation/secure-webhook-endpoint?tabs=csharp