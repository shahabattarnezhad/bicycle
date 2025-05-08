namespace Entities.Models.Base;

public class BaseEntity<TKey> where TKey : struct
{
    public TKey Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

}
