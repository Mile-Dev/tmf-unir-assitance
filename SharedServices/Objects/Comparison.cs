using System.ComponentModel.DataAnnotations;

namespace SharedServices.Objects
{
    public enum Comparison
    {
        [Display(Name = "==")]
        Equal,

        [Display(Name = "<")]
        LessThan,

        [Display(Name = "<=")]
        LessThanOrEqual,

        [Display(Name = ">")]
        GreaterThan,

        [Display(Name = ">=")]
        GreaterThanOrEqual,

        [Display(Name = "!=")]
        NotEqual,

        [Display(Name = "Contains")]
        Contains, //for strings  

        [Display(Name = "StartsWith")]
        StartsWith, //for strings  

        [Display(Name = "EndsWith")]
        EndsWith, //for strings  

        [Display(Name = "Between")]
        Between,
    }
}
