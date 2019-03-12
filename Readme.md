# Azure AD B2C: Integrate REST API claims exchanges and input validation

With the Identity Experience Framework, which underlies Azure Active Directory B2C (Azure AD B2C), you can integrate with a RESTful API in a user journey. This sample .Net core web API, demonstrate the use of [Restful technical profile](https://docs.microsoft.com/en-us/azure/active-directory-b2c/restful-technical-profile) in user journey's orchestration step and as a [validation technical profile](https://docs.microsoft.com/en-us/azure/active-directory-b2c/validation-technical-profile)

## Integrate REST API claims exchanges in your Azure AD B2C user journey as an orchestration step
In the **SignUpOrSignIn** user journey step number 7, Azure AD B2C makes a call to the **REST-GetLoyaltyNumber** technical profile. This  technical profile returns a random loyalty number. The  **SignUpOrSignIn** relying party policy returns **loyaltyNumber** claim to the relying party application.

**REST-GetLoyaltyNumber** technical profile, sends the user UI language. The loyalty number includes the language Id and a random value, for example:

```JSON
{
    "loyaltyNumber": "1033-1282",
    "email": "someone@contoso.com"
}
``` 

## Validate user email during sign-up 
The **LocalAccountSignUpWithLogonEmail** is configured to add the **REST-ValidateEmail** validation technical profile (before the AAD-UserWriteUsingLogonEmail). Make sure to keep this order, because you want to: first validate the user input, then create the account in the directory.

The validation technical profile, simply checks if the email address provided by the user, starts with 'test'. If yes, the REST API returns the error, preventing the user from creating the account. Otherwise the REST API return the email in lower case and the loyalty number. 

## Test the policy by using Run Now
1. From Azure Portal select **Azure AD B2C Settings**, and then select **Identity Experience Framework**.
1. Open **B2C_1A_REST_signup_signin**, the relying party (RP) custom policy that you uploaded, and then select **Run now**.
1. Sign-in with any account
1. Check the return JTW token contains the **loyaltyNumber** claim
1. Run the policy again, this time click on *Don't have an account?Sign up now* link
1. In the email address, type any email that starts with 'test'. Note: the email verification is disabled, so you can type any email address. Azure AD B2C will not validate the email address, unless you remove the **EnforceEmailVerification** metadata from the **LocalAccountSignUpWithLogonEmail** technical profile.

## Source code
Links to custom REST API source code for following platform: 

<table border="0" style="border:none;">
    <tr style="border: none;">
        <td style="padding-left:0; border: none;"><a href="https://github.com/azure-ad-b2c/rest-api/tree/master/source-code/DotNet-Core" ><img src="https://docs.microsoft.com/azure/app-service/media/index/logo_net.svg" height="48px" width="48px" alt=".Net Core" ><br /><span>.Net Core</span></a></div></td>
        <td style="padding-left:20px; border: none;"><a href="https://github.com/azure-ad-b2c/rest-api/tree/master/source-code/NodeJs-Express" ><img src="https://docs.microsoft.com/azure/app-service/media/index/logo_nodejs.svg" height="48px" width="48px" alt="Node.js" ><br /?<span>Node.js</span></a></div></td>
        <td style="padding-left:20px; border: none;"><a href="https://github.com/azure-ad-b2c/rest-api/tree/master/source-code/Java-Spring" ><img src="https://docs.microsoft.com/azure/app-service/media/index/logo_java.svg" height="48px" width="48px" alt="Java" ><br /><span>Java</span></a></div></td>
        <td style="padding-left:20px; border: none;"><a href="https://github.com/azure-ad-b2c/rest-api/tree/master/source-code/Python-Flask" ><img src="https://docs.microsoft.com/azure/app-service/media/index/logo_python.svg" height="48px" width="48px" alt="Python (on Linux)" ><br /><span>Python (on Linux)</span></a></div></td>
    </tr>
</table>


## Disclaimer
The sample is developed and managed by the open-source community in GitHub. The application is not part of Azure AD B2C product and it's not supported under any Microsoft standard support program or service. The sample (Azure AD B2C policy and any companion code) is provided AS IS without warranty of any kind.

> Note:  This sample policy is based on [SocialAndLocalAccounts starter pack](https://github.com/Azure-Samples/active-directory-b2c-custom-policy-starterpack/tree/master/SocialAndLocalAccounts). All changes are marked with **Demo:** comment inside the policy XML files. Make the nessacery changes in the **Demo action required** sections.
