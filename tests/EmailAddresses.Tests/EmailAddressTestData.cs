//-----------------------------------------------------------------------
// <copyright file="EmailAddressTestData.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations
{
    public static class EmailAddressTestData
    {
        public static TheoryData<string> InvalidEmailAddresses { get; } =
        [
            "Test",
            "test1â@test.com",
            "test1@",
            "@test.com",
            "test1,()@test.com",
        ];

        public static TheoryData<string> ValidEmailAddresses { get; } =
        [
            @"""Test"" <test1@test.com>",
            "test1@test.com",
            "TEST1@TEST.COM",
        ];
    }
}
