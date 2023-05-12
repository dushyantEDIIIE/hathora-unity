// Created by dylan@hathora.dev

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hathora.Scripts.Net.Server;
using Hathora.Scripts.SdkWrapper.Editor.Auth0.Models;
using Hathora.Scripts.Utils.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Hathora.Scripts.SdkWrapper.Editor.Auth0
{
    /// <summary>
    /// 1. Get device auth code from 
    /// </summary>
    public class Auth0Login
    {
        public const int PollTimeoutMins = 5; // This should be long; the user may need to register
        private const string clientId = "tWjDhuzPmuIWrI8R9s3yV3BQVw2tW0yq";
        private const string issuerUri = "https://auth.hathora.com";
        private const string audienceUri = "https://cloud.hathora.com";

        public async Task<string> GetTokenAsync(
            NetHathoraConfig _netHathoraConfig,
            CancellationToken cancelToken)
        {
            // Share the same path as the CLI
            string refreshTokenPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".config",
                "hathora",
                "token"
            );

            if (File.Exists(refreshTokenPath))
            {
                if (!_netHathoraConfig.HathoraCoreOpts.DevAuthOpts.ForceNewToken)
                {
                    // TODO: Add a force refresh option (deletes the file)
                    Debug.Log($"A token file already present at {refreshTokenPath}. We'll use this " +
                        "token, instead. If you'd like to get a new one, please remove this file");
                
                    return File.ReadAllText(refreshTokenPath); // (!) The Async variant is bugged, freezing Unity
                }                
                
                // Delete this so we can make a new one
                File.Delete(refreshTokenPath);
            }

            Auth0DeviceResponse deviceAuthorizationResponse = await requestDeviceAuthorizationAsync();

            if (deviceAuthorizationResponse == null)
            {
                Debug.Log("Error: Failed to get device authorizatio");
                return null;
            }

            return await openBrowserAwaitAuth(
                deviceAuthorizationResponse, 
                refreshTokenPath,
                cancelToken);
        }

        private async Task<string> openBrowserAwaitAuth(
            Auth0DeviceResponse deviceAuthorizationResponse, 
            string refreshTokenPath,
            CancellationToken cancelToken)
        {

            Debug.Log("Openening browser for login; ensure you see the following code: " +
                $"'<color=yellow>{deviceAuthorizationResponse.UserCode}</color>'");

            // Open browser with the provided verification URI.
            Application.OpenURL(deviceAuthorizationResponse.VerificationUriComplete);

            string refreshToken = null;
            
            try
            {
                refreshToken = await pollForTokenAsync(
                    deviceAuthorizationResponse.DeviceCode, 
                    cancelToken);
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("[Auth0Login]*WARN @ openBrowserAwaitAuth => Cancelled (or timed out)");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Auth0Login]**ERR @ openBrowserAwaitAuth => " +
                    $"pollForTokenAsync: {e.Message}");
                return null;
            }

            if (refreshToken == null)
            {
                // Debug.LogError("[Auth0Login]**ERR @ openBrowserAwaitAuth => Failed to get refreshToken");
                return null;
            }

            await File.WriteAllTextAsync(refreshTokenPath, refreshToken);
            Debug.Log($"[Auth0Login] Successful dev auth; " +
                $"cached credentials to {refreshTokenPath}");

            return refreshToken;
        }

        /// <summary>
        /// This simply POSTs for the request token code.
        /// After this, we pass the code to a uri for the end-user to login.
        /// </summary>
        /// <returns></returns>
        private async Task<Auth0DeviceResponse> requestDeviceAuthorizationAsync()
        {
            string url = $"{issuerUri}/oauth/device/code";
            UnityWebRequest request = new(url, "POST");

            Auth0DeviceRequest requestBody = new()
            {
                ClientId = clientId,
                Scope = "openid email offline_access",
                Audience = audienceUri,
            };

            // Convert the Auth0DeviceRequest object to JSON string using Newtonsoft.Json
            string bodyJson = JsonConvert.SerializeObject(requestBody);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJson);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest().AsTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Error: {request.error}");

                return null;
            }

            // Deserialize the response using Newtonsoft.Json
            return JsonConvert.DeserializeObject<Auth0DeviceResponse>(
                request.downloadHandler.text);
        }


        private async Task<string> pollForTokenAsync(string deviceCode, CancellationToken cancelToken)
        {
            string url = $"{issuerUri}/oauth/token";
            string refreshToken = null;

            Auth0TokenRequest tokenRequest = new()
            {
                GrantType = "urn:ietf:params:oauth:grant-type:device_code",
                DeviceCode = deviceCode,
                ClientId = clientId,
            };
    
            string requestBody = JsonConvert.SerializeObject(tokenRequest);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);

            // ---------------------------------
            while (refreshToken == null)
            {
                await Task.Delay(5000, cancelToken);

                using UnityWebRequest request = new(url, "POST");
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                await request.SendWebRequest().AsTask();

                // While we wait, we should get 403 errors - expected
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Auth0TokenResponse tokenResponse = JsonConvert.DeserializeObject<Auth0TokenResponse>(
                        request.downloadHandler.text);
            
                    refreshToken = tokenResponse.RefreshToken;
                }
                else if (request.responseCode != 403 || !request.downloadHandler.text.Contains("authorization_pending"))
                {
                    // Eg: If the user clicks cancel, we timeout, or another unexpected err.
                    Debug.LogError($"[Auth0Login]**ERR @ pollForTokenAsync ({request.responseCode} " +
                        $"{request.error}): {request.downloadHandler.text}");
                    return null;
                }
            }

            return refreshToken;
        }
    }
}
        