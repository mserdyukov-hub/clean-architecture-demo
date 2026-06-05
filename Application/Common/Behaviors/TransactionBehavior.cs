using Application.Common.Interfaces;
using Application.Common.Messaging;
using MediatR;

namespace Application.Common.Behaviors;

public class TransactionBehavior
{
}

// public sealed class TransactionBehavior<TRequest, TResponse>(
//     IUnitOfWork unitOfWork)
//     : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : ICommand<TResponse>
// {
//     public async Task<TResponse> Handle(
//         TRequest request,
//         RequestHandlerDelegate<TResponse> next,
//         CancellationToken cancellationToken)
//     {
//         var response = await next(cancellationToken);
//
//         await unitOfWork.SaveChangesAsync(cancellationToken);
//
//         return response;
//     }
// }
