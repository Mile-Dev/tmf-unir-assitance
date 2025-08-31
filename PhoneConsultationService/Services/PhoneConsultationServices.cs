using AutoMapper;
using PhoneConsultationService.Common.Constans;
using PhoneConsultationService.Domain.Dto;
using PhoneConsultationService.Domain.Entities;
using PhoneConsultationService.Domain.Interfaces;

namespace PhoneConsultationService.Services;

/// <summary>
/// Servicio para gestionar consultas telef�nicas y sus archivos adjuntos.
/// Proporciona funcionalidad para crear consultas telef�nicas, obtener informaci�n por evento
/// y gestionar archivos adjuntos asociados a las consultas.
/// </summary>
public class PhoneConsultationServices(IPhoneConsultationRepository repository, IMapper mapper, ILogger<PhoneConsultationServices> logger)
{
    private readonly IPhoneConsultationRepository _repository = repository;
    private readonly ILogger<PhoneConsultationServices> _logger = logger;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Crea una nueva consulta telef�nica con sus archivos adjuntos asociados.
    /// Establece la fecha de creaci�n y procesa los archivos adjuntos configurando
    /// sus claves de partici�n y clasificaci�n correspondientes.
    /// </summary>
    /// <param name="phoneConsultationDto">DTO con la informaci�n de la consulta telef�nica a crear.</param>
    /// <returns>Una tarea que representa la operaci�n as�ncrona.</returns>

    public async Task CreateAsync(PhoneConsultationDto phoneConsultationDto)
    {
        _logger.LogInformation("Init CreateAsync items from PhoneConsultation");
        var phoneConsultation = _mapper.Map<PhoneConsultation>(phoneConsultationDto);
        phoneConsultation.CreatedAt = DateTime.Now;
        var recordsPhoneConsultation = await _repository.CreateAsync(phoneConsultation);

        if (phoneConsultationDto.Attachments.Count > 0)
        {
            var recordsAttachmentPhoneConsultations = new List<Attachment>();

            foreach (var attachment in phoneConsultationDto.Attachments)
            {
                var attachmentPhoneConsultation = _mapper.Map<Attachment>(attachment);
                attachmentPhoneConsultation.PartitionKey = recordsPhoneConsultation.PartitionKey;
                attachmentPhoneConsultation.ClasificationKey = $"{Constans.AttachmentStartWith}{attachmentPhoneConsultation.Id}{recordsPhoneConsultation.ClasificationKey}";
                attachmentPhoneConsultation.PhoneRecordId = recordsPhoneConsultation.PhoneRecordId;

                recordsAttachmentPhoneConsultations.Add(attachmentPhoneConsultation);
            }
            await _repository.BatchWriteUpdateAsync(recordsAttachmentPhoneConsultations);
        }
    }

    /// <summary>
    /// Obtiene una consulta telef�nica espec�fica por identificador de evento y registro telef�nico.
    /// Incluye el mapeo de la informaci�n de triaje con su descripci�n correspondiente.
    /// </summary>
    /// <param name="idEvent">Identificador del evento asociado a la consulta.</param>
    /// <param name="idPhoneRecord">Identificador del registro telef�nico.</param>
    /// <returns>DTO con la informaci�n de la consulta telef�nica encontrada, o null si no existe.</returns>

    public async Task<PhoneConsultationDto> GetIdPhoneRecordByIdEventAsync(string idEvent, string idPhoneRecord)
    {
        var fieldSk = $"{Constans.PhoneConsultationStartWith}{idPhoneRecord}";
        var phoneConsultation = await _repository.GetIdPhoneRecordByIdEventAsync(idEvent, fieldSk);
        if (phoneConsultation == null) return null;

        var phoneConsultationDto = _mapper.Map<PhoneConsultationDto>(phoneConsultation);
        phoneConsultationDto.AssignTriage = phoneConsultation.AssignTriage.GetDescription();

        _logger.LogInformation("GetIdPhoneRecordByIdEventAsync items from PhoneConsultation");
        return phoneConsultationDto;
    }

    /// <summary>
    /// Obtiene la lista de archivos adjuntos asociados a una consulta telef�nica
    /// filtrados por identificador de evento.
    /// </summary>
    /// <param name="idEvent">Identificador del evento para filtrar los archivos adjuntos.</param>
    /// <returns>Lista de DTOs con la informaci�n de los archivos adjuntos encontrados.</returns>

    public async Task<List<AttachmentDto>> GetAttachmentPhoneConsultationByIdEventAsync(string idEvent)
    {
        var AttachmentGet = await _repository.GetListAttachmentByIdAsync(idEvent, Constans.FieldClasificationKey, Constans.AttachmentStartWith);
        var AttachmentDto = _mapper.Map< List<AttachmentDto>>(AttachmentGet);

        _logger.LogInformation("GetIdPhoneRecordByIdEventAsync items from PhoneConsultation");
        return AttachmentDto;
    } 

}