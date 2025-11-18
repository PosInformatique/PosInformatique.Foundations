//-----------------------------------------------------------------------
// <copyright file="EmailImportance.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    /// <summary>
    /// Importance of the <see cref="Email{TModel}"/>.
    /// </summary>
    public enum EmailImportance
    {
        /// <summary>
        /// Low importance.
        /// </summary>
        Low = 5,

        /// <summary>
        /// Normal importance.
        /// </summary>
        Normal = 3,

        /// <summary>
        /// High importance.
        /// </summary>
        High = 1,
    }
}