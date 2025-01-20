﻿using System.ComponentModel;
using Exiled.API.Interfaces;

namespace IPHider
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        // ReSharper disable once UseCollectionExpression
        [Description("The keywords that will trigger the IP to be hidden. Modify these incase you have plugins that output IPs in a different way.")]
        public IEnumerable<string> Keywords { get; set; } = new[]
        {
            "player",
            "incoming connection",
            "with the ip",
            "disconnected"
        };
    }
}