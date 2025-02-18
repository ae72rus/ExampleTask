using MediatR;
using Microsoft.EntityFrameworkCore;
using Zvonarev.FinBeat.Test.Storage.Tools.Ef;

namespace Zvonarev.FinBeat.Test.Storage.UseCases.ApplyMigrations;

internal class UpdateDbCommandHandler : IRequestHandler<UpdateDbCommand>
{
    private readonly AppDbContext _appDb;
    private readonly LoggingDbContext _logDb;

    public UpdateDbCommandHandler(
            AppDbContext appDb,
            LoggingDbContext logDb
        )
    {
        _appDb = appDb;
        _logDb = logDb;
    }

    public async Task Handle(UpdateDbCommand request, CancellationToken cancellationToken)
    {
        await _appDb.Database.MigrateAsync(cancellationToken);
        await _logDb.Database.MigrateAsync(cancellationToken);
    }
}