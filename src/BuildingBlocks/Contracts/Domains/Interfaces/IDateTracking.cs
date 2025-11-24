namespace Contracts.Domains.Interfaces;

public interface IDateTracking
{
    DateTimeOffset CreateDate{get; set; }
    DateTimeOffset? LastModifiedDate{get; set; }
}