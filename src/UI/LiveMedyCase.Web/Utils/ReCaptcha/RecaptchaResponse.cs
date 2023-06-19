﻿using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace LiveMedyCase.Web.Utils.ReCaptcha
{
    public class RecaptchaResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }
        [JsonPropertyName("challenge_ts")]
        public DateTime ChallengeTs { get; set; }
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }
        [JsonPropertyName("score")]
        public decimal Score { get; set; }
    }
}
