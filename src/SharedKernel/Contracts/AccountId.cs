using BuildingBlock.Domain;

namespace SharedKernel.Contracts;

public sealed class AccountId(Guid value) : TypedIdValueBase(value);
