namespace EventServices.Domain.Dto.Query;

public record ViewGuaranteesPaymentEventProviderGetDto
(
     int Id,
     int EventId,
     int EventProviderId,
     int StatusEventProviderId,
     int GuaranteePaymentStatusId,
     string? NameStatusEventProvider,
     string? NameStatusGuaranteePayment,
     string? Country,
     string? City,
     string? Address,
     string? NameProvider,
     string? TypeProvider,
     string? TypeMoney,
     decimal? AmountLocal,
     decimal? AmountUsd,
     decimal? ExchangeRate,
     decimal? DeductibleAmountLocal,
     decimal? DeductibleAmountUsd,
     string? Description,
     DateTime? ScheduledAppointment,
     DateTime? EndDate,
     DateTime CreatedAt
);