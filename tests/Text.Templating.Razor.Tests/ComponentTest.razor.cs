//-----------------------------------------------------------------------
// <copyright file="ComponentTest.razor.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Razor.Tests
{
    using Microsoft.AspNetCore.Components;

    public partial class ComponentTest : ComponentBase
    {
        [Inject]
        public RazorTextTemplateRendererTest.IService Service { get; set; }

        [Parameter]
        public RazorTextTemplateRendererTest.ModelTest Model { get; set; }
    }
}