namespace LiveMedyCase.Core.Constants
{
    public static class ServiceMessages
    {
        public static string CouldNotCreated(string entityName) => $"{entityName} entity could not be created.";
        public static string CouldNotUpdated(string entityName) => $"{entityName} entity could not be updated.";
        public static string CouldNotRemoved(string entityName) => $"{entityName} entity could not be removed.";

        public const string UserNotFound = "User not found!";
        public const string UserRegistrationSucceeded = "User Registration Succeeded";
        public const string UserReferrerInvalid = "Referrer code is not valid!";
		public const string UserRegistrationError = "User Registration Error";
        public const string UsernameCheckError = "Username is not valid!";
        public const string PasswordOrUsernameError = "Password / Username error!";
    }
}
