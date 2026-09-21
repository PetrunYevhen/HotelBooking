using Bookings.Application.Contracts;
using BuildingBlock.Domain;
using Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Bookings.Application.Behaviour;

public class TransactionalBehaviour<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public TransactionalBehaviour(IUnitOfWork unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not CommandBase<TResponse>)
            return await next();

        var commandName = typeof(TRequest).Name;

        _logger.Information($"[Command] Executing {commandName}");

        var response = await next();
        
        if (response is Result result && result.IsFailure)
        {
            _logger.Warning("[Command] {Command} failed: {Error}", commandName, result.Error.Message);
            return response;
        }
        
        try
        {
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (request is ITranslatesDbConflict translator &&
                                            translator.TranslateDbConflict(ex) is { } conflictResult)
        {
            _logger.Warning("[Command] {Command} conflicted: {Error}", commandName, conflictResult.Error.Message);
            return (TResponse)(object)conflictResult;
        }

        _logger.Information("[Command] {Command} committed successfully", commandName);
        return response;
    }
}