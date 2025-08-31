namespace SharedServices.Objects
{
    public class ExpressionFilter
    {
        public string? PropertyName { get; set; }
        
        public object? Value { get; set; }
     
        public object? ValueBetween { get; set; } // Segundo valor para Between

        public Comparison Comparison { get; set; }
    }
}
