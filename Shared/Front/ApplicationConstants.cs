namespace Shared.Front
{
    public static class ApplicationConstants
    {
        public static string CouponApi { get; set; } = string.Empty;
        public static string AuthApi { get; set; } = string.Empty;

        public const string Role_User = "User";
        public const string Role_Admin = "Admin";

        //public static string Role_User { get; set; } = "User";
        //public static string Role_Admin { get; set; } = "Admin";

        public static string Local_Token { get; set; } = "JWT Token";
        public static string Current_User { get; set; } = "Current User";
    }
}
