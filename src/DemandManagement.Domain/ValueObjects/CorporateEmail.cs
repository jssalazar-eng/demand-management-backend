using System;
using System.Text.RegularExpressions;

namespace DemandManagement.Domain.ValueObjects;

public sealed record CorporateEmail
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; init; }

    private CorporateEmail()
    {
    }

    private CorporateEmail(string email) => Value = email;

    public static CorporateEmail From(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        email = email.Trim();
        if (!EmailRegex.IsMatch(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        return new CorporateEmail(email);
    }

    public override string ToString() => Value;
}