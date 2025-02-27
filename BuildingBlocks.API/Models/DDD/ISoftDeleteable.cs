namespace BuildingBlocks.API.Models.DDD;

public interface ISoftDeletable
{
    DateTime? DeletedOn { get; set; }
}