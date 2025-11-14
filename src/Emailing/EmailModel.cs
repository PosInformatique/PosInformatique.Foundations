//-----------------------------------------------------------------------
// <copyright file="EmailModel.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    /// <summary>
    /// Base class of the data model of the <see cref="EmailTemplate{TModel}"/>.
    /// </summary>
    public abstract class EmailModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailModel"/> class.
        /// </summary>
        protected EmailModel()
        {
        }
    }
}