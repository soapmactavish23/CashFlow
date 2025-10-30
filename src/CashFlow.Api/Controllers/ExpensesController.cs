﻿using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register([FromBody] RequestRegisterExpenseJson request)
        {
            try
            {
                var useCase = new RegisterExpenseUseCase();

                var response = useCase.Execute(request);

                return Created("", response);
            } catch (ArgumentException ex)
            {
                var errorResponse = new ResponseErrorJson(ex.Message);
                return BadRequest(errorResponse);
            } catch
            {
                var errorResponse = new ResponseErrorJson("unknown Error");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

    }
}
