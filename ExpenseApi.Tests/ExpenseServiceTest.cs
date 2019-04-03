using System;
using Xunit;

namespace ExpenseApi.Tests
{
    public class ExpenseServiceTest
    {
        private readonly ExpenseService _expenseService;

        public ExpenseServiceTest()
        {
            _expenseService = new ExpenseService();
        }

        [Fact]
        public void Test_Valid_Claim()
        {
            string claim = @"
Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense><cost_centre>DEV002</cost_centre>
<total>1024.01</total><payment_method>personal card</payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd <Antoine.Lloyd@example.com>
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Thursday 27 April 2017</date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";
            ExpenseClaim expenseClaim = _expenseService.parseClaim(claim);
            Assert.NotNull(expenseClaim);
            Assert.Equal("Viaduct Steakhouse", expenseClaim.Vendor);
            Assert.Equal("development team’s project end celebration dinner", expenseClaim.Description);
            Assert.Equal("04/27/2017 00:00:00", expenseClaim.Date.ToString());
            Assert.NotNull(expenseClaim.ExpenseClaimDetail);
            Assert.Equal("DEV002", expenseClaim.ExpenseClaimDetail.CostCentre);
            Assert.Equal("personal card", expenseClaim.ExpenseClaimDetail.PaymentMethod);
            Assert.Equal(1024.01, expenseClaim.ExpenseClaimDetail.GrandTotal);
            Assert.Equal(153.6, expenseClaim.ExpenseClaimDetail.Gst);
            Assert.Equal(870.41, expenseClaim.ExpenseClaimDetail.Total);
        }

        [Fact]
        public void Test_Valid_Missing_CostCentre()
        {
            string claim = @"
Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense>
<total>1024.01</total><payment_method>personal card</payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd <Antoine.Lloyd@example.com>
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Thursday 27 April 2017</date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";
            ExpenseClaim expenseClaim = _expenseService.parseClaim(claim);
            Assert.NotNull(expenseClaim);
            Assert.Equal("Viaduct Steakhouse", expenseClaim.Vendor);
            Assert.Equal("development team’s project end celebration dinner", expenseClaim.Description);
            Assert.Equal("04/27/2017 00:00:00", expenseClaim.Date.ToString());
            Assert.NotNull(expenseClaim.ExpenseClaimDetail);
            Assert.Equal("UNKNOWN", expenseClaim.ExpenseClaimDetail.CostCentre);
            Assert.Equal("personal card", expenseClaim.ExpenseClaimDetail.PaymentMethod);
            Assert.Equal(1024.01, expenseClaim.ExpenseClaimDetail.GrandTotal);
            Assert.Equal(153.6, expenseClaim.ExpenseClaimDetail.Gst);
            Assert.Equal(870.41, expenseClaim.ExpenseClaimDetail.Total);
        }

        [Fact]
        public void Test_Invalid_Claim_Bad_Markup_1()
        {
            string claim = @"
Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense>>>>>>>>>>>>>DEV002</cost_centre>
<total>1024.01</total><payment_method>personal card</payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd <Antoine.Lloyd@example.com>
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Tuesday 27 April 2017</////date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";
            Assert.Throws<ExpenseClaimParseException>(delegate ()
                 {
                     _expenseService.parseClaim(claim);
                 });
        }

        [Fact]
        public void Test_Invalid_Claim_Bad_Markup_2()
        {
            string claim = @"
Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense><cost_centre>DEV002</cost_centre>
<total>1024.01</total><payment_method>personal card
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd <Antoine.Lloyd@example.com>
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Tuesday 27 April 2017</date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";
            Assert.Throws<ExpenseClaimParseException>(delegate ()
                 {
                     _expenseService.parseClaim(claim);
                 });
        }

        [Fact]
        public void Test_Invalid_Claim_Wrong_Date()
        {
            string claim = @"
Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense><cost_centre>DEV002</cost_centre>
<total>1024.01</total><payment_method>personal card</payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd <Antoine.Lloyd@example.com>
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Tuesday 27 April 2017</date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";
            Assert.Throws<ExpenseClaimParseException>(delegate ()
                 {
                     _expenseService.parseClaim(claim);
                 });
        }

        [Fact]
        public void Test_Invalid_Missing_Total()
        {
            string claim = @"
Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense><cost_centre>DEV002</cost_centre><payment_method>personal card</payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd <Antoine.Lloyd@example.com>
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Thursday 27 April 2017</date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";
            Assert.Throws<ExpenseClaimParseException>(delegate ()
                 {
                     _expenseService.parseClaim(claim);
                 });
        }

    }
}
