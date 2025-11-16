//-----------------------------------------------------------------------
// <copyright file="PhoneNumberTestData.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.PhoneNumbers
{
    public static class PhoneNumberTestData
    {
        public static TheoryData<string> InvalidPhoneNumbers { get; } = new()
        {
            "invalid phone number",
            "111111111",
            "1234567891",
            "+3360102",
            "0102030405",
        };

        public static TheoryData<string, string> ValidPhoneNumbers { get; } = new()
        {
            { "+33111111111", "+33111111111" },
            { "+15125111111", "+15125111111" },
            { "+33767678028", "+33767678028" },
            { "+33 1 11 11 11 11", "+33111111111" },
        };
    }
}