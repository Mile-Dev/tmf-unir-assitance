using System.ComponentModel;

namespace PhoneConsultationService.Domain.Enum
{
    public enum AssignTriage
    {
        [Description("I - Immediate (Critical)")]
        I,

        [Description("II - Very Urgent")]
        II,

        [Description("III - Urgent")]
        III,

        [Description("IV - Standard")]
        IV,

        [Description("V - Non-Urgent")]
        V
    }
}
