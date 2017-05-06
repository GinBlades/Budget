using System;

namespace Budget.Domain.Interfaces
{
    public interface IDBModelTS
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
