namespace EventServices.Domain.Dto.Query;

    public record DocumentGetDto
    (
        int Id,    
        int EventId,
        string Name,
        string Description,
        string Path,
        string Table,
        DateTime CreatedAt
    );

