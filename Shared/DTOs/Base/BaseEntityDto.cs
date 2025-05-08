namespace Shared.DTOs.Base;

public record BaseEntityDto<TKey> where TKey : struct
{
    public TKey Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
