﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Dalamud.Plugin.Internal.Types;
using Dalamud.Utility;
using Newtonsoft.Json;

namespace Dalamud.Support
{
    /// <summary>
    /// Class responsible for sending feedback.
    /// </summary>
    internal static class BugBait
    {
        private const string BugBaitUrl = "https://dalamud-bugbait.goatsoft.workers.dev/feedback";

        /// <summary>
        /// Send feedback to Discord.
        /// </summary>
        /// <param name="plugin">The plugin to send feedback about.</param>
        /// <param name="content">The content of the feedback.</param>
        /// <param name="reporter">The reporter name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task SendFeedback(PluginManifest plugin, string content, string reporter)
        {
            if (content.IsNullOrWhitespace())
                return;

            using var client = new HttpClient();

            var model = new FeedbackModel
            {
                Content = content,
                Reporter = reporter,
                Name = plugin.InternalName,
                Version = plugin.AssemblyVersion.ToString(),
            };

            var postContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(BugBaitUrl, postContent);

            response.EnsureSuccessStatusCode();
        }

        private class FeedbackModel
        {
            [JsonProperty("content")]
            public string? Content { get; set; }

            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("version")]
            public string? Version { get; set; }

            [JsonProperty("reporter")]
            public string? Reporter { get; set; }
        }
    }
}
