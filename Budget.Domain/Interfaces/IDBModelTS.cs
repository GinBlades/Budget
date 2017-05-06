using System;

namespace Budget.Domain.Interfaces
{
    /// <summary>
    /// Contract for all entities with integer ID and timestamp
    /// </summary>
    public interface IDBModelTS
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
