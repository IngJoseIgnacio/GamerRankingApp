﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GamerRankingApp.Startup))]
namespace GamerRankingApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
