using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrPay.Shared.Interfaces;
using QrPay.Shared.Models;
using System.Net.Mime;

namespace QrPay.API.Controllers.Shared
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ControllerBase<TController>(IMediator mediator) : ControllerBase
    {
        protected readonly IMediator _mediator = mediator;

        protected virtual async Task<IActionResult> QueryAsync<TResponse>(IRequest<IResponseResult<TResponse>> request)
        {
            var response = await _mediator.Send(request);
            return ResponseResult(response);
        }

        protected virtual async Task<IActionResult> CommandAsync(IRequest<IResponseResult> command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        protected virtual async Task<IActionResult> CommandAsync<TResponse>(IRequest<IResponseResult<TResponse>> command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        protected virtual async Task<IActionResult> CommandAsync<TResponse>(IRequest<IResponseResult<TResponse>> command, IRequest<IResponseResult<TResponse>> getById)
            where TResponse : class
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return ResponseResult(await _mediator.Send(getById));

            return BadRequest(response);
        }

        protected virtual async Task<IActionResult> CommandAsync<TResponse>(IRequest<IResponseResult<TResponse>> command, Func<IResponseResult<TResponse>, IRequest<IResponseResult<TResponse>>> funcGetById)
            where TResponse : class
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                var getById = funcGetById(response);

                return ResponseResult(await _mediator.Send(getById));
            }

            return BadRequest(response);
        }

        protected virtual async Task<IActionResult> CommandAsync(IRequest<ResponseFileResult> command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return File(response.File, response.ContentType ?? "application/octet-stream", fileDownloadName: response.FileName);

            return BadRequest(response);
        }

        private IActionResult ResponseResult<T>(IResponseResult<T> responseResult)
        {
            if (responseResult.IsSuccess)
            {
                return Ok(responseResult);
            }

            return BadRequest(responseResult);
        }
    }
}

