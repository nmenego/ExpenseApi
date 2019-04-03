using System;

public class ExpenseClaimParseException : Exception
{
    public ExpenseClaimParseException(string message) : base(message)
    {
    }
}