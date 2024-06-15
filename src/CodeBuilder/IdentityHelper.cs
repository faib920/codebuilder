// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeBuilder
{
    public static class IdentityHelper
    {
        public static async Task<CommResult<LoginData>> LoginAsync(string code, string password)
        {
            var client = new HttpClient();

            try
            {
                var response = await client.PostAsync(Consts.ApiUrl + "/login", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "code", code },
                    { "password", WebHelper.Encrypt(password) }
                }));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var context = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<CommResult<LoginData>>(context);
                    return data;
                }

                return new CommResult<LoginData> { Message = $"网络错误 {(int)response.StatusCode}-{response.ReasonPhrase}。" };
            }
            catch (Exception exp)
            {
                return new CommResult<LoginData> { Message = exp.Message };
            }
        }

        public static async Task<LoginData> ValidateAsync(string token)
        {
            var client = new HttpClient();
            var response = await client.PostAsync(Consts.ApiUrl + "/isalive", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "token", token },
            }));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var context = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<CommResult<LoginData>>(context);
                if (data.Succeed)
                {
                    return data.Data;
                }
            }

            return null;
        }

        public static HttpClient AddAccessToken(this HttpClient client)
        {
            if (!string.IsNullOrEmpty(Config.Instance.AccessToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Config.Instance.AccessToken);
            }

            return client;
        }

        public static bool CheckAuthorized(IDevHosting hosting)
        {
            if (!hosting.IsAuthorized)
            {
                hosting.ShowWarn("该功能只对注册用户开放，请点击主菜单右侧的【登录...】按钮进行登录或注册。");
                return false;
            }

            return true;
        }

        public static bool CheckAuthorized(this HttpResponseMessage response, IDevHosting hosting)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                hosting.ShowWarn("该功能只对注册用户开放，请点击主菜单右侧的【登录...】按钮进行登录或注册。");
                return false;
            }

            return true;
        }
    }

    public class LoginData
    {
        public string access_token { get; set; }

        public string name { get; set; }
    }
}
