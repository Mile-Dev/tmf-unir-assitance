namespace TrackingMokServices.Domain.Entities
{
    public class EventMok
    {
        public string EventoId { get; set; } = string.Empty;
       
        public string EventoIdMOk { get; set; } = string.Empty;

        public string OTMOk { get; set; } = string.Empty;

        public DateOnly FechaRecepcionDeSolicitud { get; set; }
      
        public TimeOnly HoraRecepcionDeSolicitud { get; set; }

        public string TipoDeCliente { get; set; } = string.Empty;

        public string NombresApellidos { get; set; } = string.Empty;

        public string Voucher { get; set; } = string.Empty;

        public string ProductoOPlan { get; set; } = string.Empty;

        public DateTime FechaInicioDeVigencia { get; set; }

        public DateTime FechaFinDeVigencia { get; set; }

        public string TipoDeDocumento { get; set; } = string.Empty;

        public string NumeroDeIdentificacion { get; set; } = string.Empty;

        public DateOnly FechaDeNacimiento { get; set; }

        public int Edad { get; set; } 

        public string Genero { get; set; } = string.Empty;

        public string TelefonoVoz { get; set; } = string.Empty;

        public string TelefonoMsnWpp { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public string Pais { get; set; } = string.Empty;

        public string Ciudad { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public string AntecedentesPersonales { get; set; } = string.Empty;

        public string SintomasOMotivoDeConsulta { get; set; } = string.Empty;

        public string SolicitudRecibidaPor { get; set; } = string.Empty;

        public string MotivoDelViaje { get; set; } = string.Empty;

        public string Parentesco { get; set; } = string.Empty;

        public string EmpresaEnDondeLabora { get; set; } = string.Empty;
       
        public string CargoQueDesempena { get; set; } = string.Empty;
        
        public string EnMisionLaboral { get; set; } = string.Empty;
        
        public string DiagnosticoCie10Inicial { get; set; } = string.Empty;

        public string CodigoPirEquipaje { get; set; } = string.Empty;

        public string RutaAerea { get; set; } = string.Empty;

        public string HorarioDelVuelo { get; set; } = string.Empty;

        public string Aerolinea { get; set; } = string.Empty;

        public string DescripcionOMotivoDeSolicitudDeAsistencia { get; set; } = string.Empty;

        public string Preexistencia { get; set; } = string.Empty;

        public string Autoasistencia { get; set; } = string.Empty;

        public string EmergenciaOUrgencia { get; set; } = string.Empty;

        public string TipoDeAutoasistencia { get; set; } = string.Empty;

        public DateOnly FechaDeRegreso { get; set; } 
    }
}
