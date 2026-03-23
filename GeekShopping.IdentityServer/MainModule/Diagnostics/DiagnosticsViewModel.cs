// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.IdentityModel; 
using Microsoft.AspNetCore.Authentication;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace IdentityServerHost.Quickstart.UI
{
    public class DiagnosticsViewModel
    {
        public DiagnosticsViewModel(AuthenticateResult result)
        {
            AuthenticateResult = result;

            if (result.Properties.Items.ContainsKey("client_list"))
            {
                var encoded = result.Properties.Items["client_list"];
                byte[] bytes = new byte[encoded.Length];
                Base64Url.DecodeFromUtf8(Encoding.UTF8.GetBytes(encoded), bytes, out _, out int written);
                var value = Encoding.UTF8.GetString(bytes, 0, written);
                //var bytes = Base64Url.DecodeFromUtf8(Encoding.UTF8.GetBytes(encoded), new byte[encoded.Length], out _, out _);
                //var value = Encoding.UTF8.GetString(bytes);


                Clients = JsonSerializer.Deserialize<string[]>(value);
            }
        }

        public AuthenticateResult AuthenticateResult { get; }
        public IEnumerable<string> Clients { get; } = new List<string>();
    }
}