namespace Shared.Models.Interfaces;

public interface ITimeAuditable
{
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}