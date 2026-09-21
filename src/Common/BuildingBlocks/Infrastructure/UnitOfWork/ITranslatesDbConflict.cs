using BuildingBlock.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork;

public interface ITranslatesDbConflict
{
    Result? TranslateDbConflict(DbUpdateException exception);
}
