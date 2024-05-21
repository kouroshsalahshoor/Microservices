namespace Shared.Front
{
    public static class ApplicationConstants
    {
        public static string ProductApi { get; set; } = string.Empty;
        public static string CouponApi { get; set; } = string.Empty;
        public static string AuthApi { get; set; } = string.Empty;
        public static string CarthApi { get; set; } = string.Empty;

        public const string Role_User = "User";
        public const string Role_Admin = "Admin";

        public const string Local_Token = "JWT Token";
        public const string Current_User = "Current User";
    }
}
