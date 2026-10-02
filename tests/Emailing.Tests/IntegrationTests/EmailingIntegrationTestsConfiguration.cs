//-----------------------------------------------------------------------
// <copyright file="EmailingIntegrationTestsConfiguration.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using Microsoft.Extensions.Configuration;

    public class EmailingIntegrationTestsConfiguration
    {
        private readonly IConfiguration configuration;

        public EmailingIntegrationTestsConfiguration(string jsonFileName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(jsonFileName, optional: false, reloadOnChange: false);

            this.configuration = builder.Build();
        }

        public string this[string key]
        {
            get => this.configuration[key];
        }
    }
}