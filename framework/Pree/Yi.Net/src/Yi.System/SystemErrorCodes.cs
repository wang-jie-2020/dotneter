namespace Yi.System;

public class SystemErrorCodes
{
    public const string VerificationCodeTooMuch = "Account:010001";
    public const string VerificationCodeNotMatched = "Account:010002";
    public const string SignupForbidden = "Account:020001";
    
    public const string GivenNameNotExist = "Account:030001";
    public const string GivenPasswordNotMatched = "Account:030002";
    public const string UserInactive = "Account，030003";
    public const string UserNoRole = "Account:030004";
    public const string UserNoPermission = "Account:030005";
    
    public const string UserNameRepeated = "User:010001";
    public const string UserNameForbidden = "User:010002";
    public const string UserNameTooShort = "User:010003";
    public const string UserNameInvalid = "User:010004";
    public const string UserPasswordTooShort = "User:010005";
    public const string UserPhoneRepeated = "User:020001";
    public const string UserPhoneInvalid = "User:020002";
    
    public const string TenantRepeated = "Tenant:010001";
}