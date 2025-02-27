using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BuildingBlocks.API.Configurations.EFCore;

/// <summary>
/// Provides extension methods for <see cref="EntityEntry"/> to assist with change tracking of owned entities.
/// </summary>
internal static class EntityExtensions
{
    /// <summary>
    /// Determines whether any owned entities associated with the given <see cref="EntityEntry"/> have been added or modified.
    /// </summary>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the entity being tracked.</param>
    /// <returns><c>true</c> if any owned entities have been added or modified; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method examines the references of the provided entity entry to identify any owned entities
    /// that are in the <see cref="EntityState.Added"/> or <see cref="EntityState.Modified"/> state.
    /// </remarks>
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry is not null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}