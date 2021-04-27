using AntDesign;
using AntDesign.Pro.Layout;
using Microsoft.AspNetCore.Components;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker
{
    public class AppSettings
    {
        public static string SiteRoot { get; set; } = "/";
        public static string SiteBaseUrl { get; set; }

        public static string WebRootPath { get; set; }
        public static string ContentRootPath { get; set; }

        public static string[] SupportLanguages { get; } = new string[] { "zh-CN", "ja-JP", "en-US" };

        public static Dictionary<string, string> LanguageLabels { get; } = new Dictionary<string, string>
        {
            ["zh-CN"] = "简体中文",
            ["ja-JP"] = "日本語",
            ["en-US"] = "English",
        };

        public static IDictionary<string, string> LanguageIcons { get; set; } = new Dictionary<string, string>
        {
            ["zh-CN"] = "🇨🇳",
            ["ja-JP"] = "JP",
            ["en-US"] = "🇺🇸",
        };

        public static string Version
        {
            get
            {
                return typeof(AppSettings)
                    .GetTypeInfo()
                    .Assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;
            }
        }

        public static string OSDescription
        {
            get
            {
                return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            }
        }

        public static readonly LinkItem[] FooterLinks =
  {
        new LinkItem{ Key = "", BlankTarget = true,Href="/", Title = "Maincotech"},
        new LinkItem{ Key = "", BlankTarget = true, Href="https://github.com/maincotech/quizmaker", Title = OneOf<string, RenderFragment>.FromT1((builder) =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1,"Type","github");
            builder.CloseComponent();
        })},
        new LinkItem{ Key = "", BlankTarget = true,  Href="http://justneeds.net/",Title = "Justneeds(Partner@Japan)"},
    };
    }
}
