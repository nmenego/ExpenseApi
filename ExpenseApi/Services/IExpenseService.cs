using System.Collections.Generic;

public interface IExpenseService
{
    ExpenseClaim parseClaim(string claim);
    void addClaim(ExpenseClaim ExpenseClaim);
    List<ExpenseClaim> listClaims();
}