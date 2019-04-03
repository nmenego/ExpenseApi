using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {

        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // GET api/expense
        [HttpGet]
        public ActionResult Get()
        {
            return new JsonResult(_expenseService.listClaims());
        }

        // POST api/expense
        [HttpPost]
        public ActionResult Post(ExpenseClaim expenseClaim)
        {
            try
            {
                _expenseService.addClaim(expenseClaim);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/expense
        [HttpPut]
        public ActionResult Put(ExpenseClaimDTO expenseClaimDto)
        {
            try
            {
                ExpenseClaim expenseClaim = _expenseService.parseClaim(expenseClaimDto.Request);
                return new JsonResult(expenseClaim);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
