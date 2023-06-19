using System.Text.Json;
using LiveMedyCase.Web.Configs;
using Microsoft.Extensions.Options;

namespace LiveMedyCase.Web.Utils.ReCaptcha
{
    public interface IRecaptchaValidator
    {
        bool IsRecaptchaValid(string token);
    }

    public class RecaptchaValidator : IRecaptchaValidator
    {
        public readonly ReCaptchaConfig _config;

        public RecaptchaValidator(IOptions<ReCaptchaConfig> config)
        {
            _config = config.Value;
        }

        public bool IsRecaptchaValid(string token)
        {
            using var client = new HttpClient();
            var response = client.GetStringAsync($@"{_config.GoogleRecaptchaAddress}?secret={_config.RecaptchaV3SecretKey}&response={token}").Result;
            var recaptchaResponse = JsonSerializer.Deserialize<RecaptchaResponse>(response);

            if (recaptchaResponse == null
                || !recaptchaResponse.Success
                || recaptchaResponse.Score > Convert.ToDecimal(_config.RecaptchaMinScore))
                return false;

            return true;
        }

    }
}
