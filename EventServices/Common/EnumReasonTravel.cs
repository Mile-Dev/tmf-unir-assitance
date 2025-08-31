using System.ComponentModel;

namespace EventServices.Common
{
    public enum EnumReasonTravel
    {
        [Description("Tourism / Vacation")]
        TourismVacation,

        [Description("Business / Work")]
        BusinessWork,

        [Description("Studies / Academic exchange")]
        StudiesAcademicExchange,

        [Description("Visiting family or friends")]
        VisitingFamilyOrFriends,

        [Description("Medical treatment / Health")]
        MedicalTreatmentHealth,

        [Description("Official mission / Government representation")]
        OfficialMissionGovernment,

        [Description("Conferences, trade fairs or congresses")]
        ConferencesFairsCongresses,

        [Description("Religious / Pilgrimage")]
        ReligiousPilgrimage,

        [Description("Volunteering / Humanitarian missions")]
        VolunteeringHumanitarianMissions,

        [Description("Relocation / Permanent migration")]
        RelocationPermanentMigration,

        [Description("Remote work / Digital nomad")]
        RemoteWorkDigitalNomad,

        [Description("Sporting or cultural events")]
        SportsOrCulturalEvents,

        [Description("Emergency / Unplanned travel")]
        EmergencyUnplannedTravel,

        [Description("Transit / Connection to another destination")]
        TransitOtherDestination,

        [Description("Other")]
        Other
    }
}
