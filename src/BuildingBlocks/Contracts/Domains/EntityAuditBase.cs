
using Contracts.Domains.Interfaces;

namespace Contracts.Domains;

public abstract class EntityAuditBase<T> : EntityBase<T>, IAuditable
{
    public DateTimeOffset CreateDate { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}