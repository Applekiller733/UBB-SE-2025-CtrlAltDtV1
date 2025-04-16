﻿// <copyright file="ImgurImageUploader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Storage;

public class ImgurImageUploader
{
    private const string ClientId = "ecde1e79945f70c";

    public static async Task<string?> UploadImageAndGetUrl(StorageFile file)
    {
        if (file == null)
        {
            return null;
        }

        using (var httpClient = new HttpClient())
        {
            byte[] imageBytes;
            using (var stream = await file.OpenStreamForReadAsync())
            {
                imageBytes = new byte[stream.Length];
                await stream.ReadAsync(imageBytes, 0, imageBytes.Length);
            }

            string base64Image = Convert.ToBase64String(imageBytes);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.imgur.com/3/image")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("image", base64Image),
                }),
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Client-ID", ClientId);

            var response = await httpClient.SendAsync(request);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            return result?.data?.link;
        }
    }
}
