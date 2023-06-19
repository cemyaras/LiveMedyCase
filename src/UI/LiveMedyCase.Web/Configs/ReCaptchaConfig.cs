namespace LiveMedyCase.Web.Configs
{
    public record ReCaptchaConfig
    {
		public string GoogleRecaptchaAddress { get; init; }
		public string RecaptchaV3SecretKey { get; init; }
		public string RecaptchaV3SiteKey { get; init; }
        public string RecaptchaMinScore { get; init; }
    }
}
