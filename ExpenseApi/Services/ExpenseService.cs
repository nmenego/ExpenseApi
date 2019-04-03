using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;

public class ExpenseService : IExpenseService
{
    public const double GST_RATE = 0.15;
    public const String DEFAULT_COST_CENTRE = "UNKNOWN";
    // dummy list to persist expenseclaims: 
    public static List<ExpenseClaim> expenseClaims = new List<ExpenseClaim>();

    public void addClaim(ExpenseClaim expenseClaim)
    {
        expenseClaims.Add(expenseClaim);
    }

    public List<ExpenseClaim> listClaims()
    {
        return expenseClaims;
    }

    public ExpenseClaim parseClaim(string claim)
    {

        ExpenseClaim expenseClaim = new ExpenseClaim();

        string input = "<root>" + claim + "</root>";
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(input);

        try
        {
            expenseClaim.Vendor = doc.DocumentNode.SelectNodes("//vendor").First().InnerHtml;
            expenseClaim.Description = doc.DocumentNode.SelectNodes("//description").First().InnerHtml;
            String dateString = doc.DocumentNode.SelectNodes("//date").First().InnerHtml;
            expenseClaim.Date = DateTime.Parse(dateString);
            expenseClaim.ExpenseClaimDetail = new ExpenseClaimDetail();
            expenseClaim.ExpenseClaimDetail.PaymentMethod = doc.DocumentNode.SelectNodes("//expense/payment_method").First().InnerHtml;
            expenseClaim.ExpenseClaimDetail.GrandTotal = double.Parse(doc.DocumentNode.SelectNodes("//expense/total").First().InnerHtml);
            expenseClaim.ExpenseClaimDetail.Gst = Math.Round(expenseClaim.ExpenseClaimDetail.GrandTotal * GST_RATE, 2, MidpointRounding.ToEven);
            expenseClaim.ExpenseClaimDetail.Total = Math.Round(expenseClaim.ExpenseClaimDetail.GrandTotal - expenseClaim.ExpenseClaimDetail.Gst, 2, MidpointRounding.ToEven);

            HtmlNodeCollection costCentre = doc.DocumentNode.SelectNodes("//expense/cost_centre");
            expenseClaim.ExpenseClaimDetail.CostCentre = costCentre == null ? DEFAULT_COST_CENTRE : costCentre.First().InnerHtml;
        }
        catch (Exception e)
        {
            throw new ExpenseClaimParseException("Incorrect format " + e.Message);
        }

        return expenseClaim;
    }


}