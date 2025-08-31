namespace SharedServices.Objects
{
    public class Filters
    {
        public List<ExpressionFilter> Filter { get; set; } = [];

        public ParameterGetList ParameterGetList { get; set; } = new ParameterGetList();

    }
}
