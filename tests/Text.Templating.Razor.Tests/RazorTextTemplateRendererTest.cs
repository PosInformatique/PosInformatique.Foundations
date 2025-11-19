//-----------------------------------------------------------------------
// <copyright file="RazorTextTemplateRendererTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Razor.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class RazorTextTemplateRendererTest
    {
        public interface IService
        {
            string GetData();
        }

        [Fact]
        public async Task RenderAsync()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<IService, Service>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var renderer = new RazorTextTemplateRenderer(
                serviceProvider,
                serviceProvider.GetRequiredService<ILoggerFactory>());

            var model = new ModelTest()
            {
                Name = "The name",
            };

            var output = new StringWriter();

            await renderer.RenderAsync(typeof(ComponentTest), model, output, cancellationToken);

            output.ToString().Should().Be(@"
The model name : The name
The service data : The data !");
        }

        public class ModelTest
        {
            public string Name { get; set; }
        }

        public class Service : IService
        {
            public string GetData() => "The data !";
        }
    }
}