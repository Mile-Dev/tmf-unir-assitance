using SharedServices.Objects;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace SharedServices.ExpressionHelper
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> ConstructAndExpressionTree<T>(List<ExpressionFilter> filters)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression<T>(param, filters[0]);
            }
            else
            {
                exp = GetExpression<T>(param, filters[0]);
                for (int i = 1; i < filters.Count; i++)
                {
                    exp = Expression.And(exp, GetExpression<T>(param, filters[i]));
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }


        public static Expression GetExpression<T>(ParameterExpression param, ExpressionFilter filter)
        {
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

            MemberExpression member = Expression.Property(param, filter.PropertyName);
            ConstantExpression constant = GetValueTypeOf(filter.Value);
            ConstantExpression constantBetween = GetValueTypeOf(filter.ValueBetween);

            switch (filter.Comparison)
            {
                case Comparison.Equal:
                    return Expression.Equal(member, constant);
                case Comparison.GreaterThan:
                    return Expression.GreaterThan(member, constant);
                case Comparison.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);
                case Comparison.LessThan:
                    return Expression.LessThan(member, constant);
                case Comparison.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);
                case Comparison.NotEqual:
                    return Expression.NotEqual(member, constant);
                case Comparison.Contains:
                    return Expression.Call(member, containsMethod, constant);
                case Comparison.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);
                case Comparison.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
                case Comparison.Between:
                    return Expression.AndAlso(
                        Expression.GreaterThanOrEqual(member, constant),
                        Expression.LessThanOrEqual(member, constantBetween));
                default:
                    return null;
            }
        }

        private static ConstantExpression GetValueTypeOf(object value)
        {
            ConstantExpression constant;

            if (value is JsonElement jsonElement)
            {
                switch (jsonElement.ValueKind)
                {
                    case JsonValueKind.String:
                        if (DateTime.TryParse(jsonElement.GetString(), out DateTime dateTimeValue))
                        {
                            constant = Expression.Constant(dateTimeValue);
                        }
                        else
                        {
                            constant = Expression.Constant(jsonElement.GetString());
                        }
                        break;
                    case JsonValueKind.Number:
                        if (jsonElement.TryGetInt32(out int intValue))
                        {
                            constant = Expression.Constant(intValue);
                        }
                        else if (jsonElement.TryGetInt64(out long longValue))
                        {
                            constant = Expression.Constant(longValue);
                        }
                        else if (jsonElement.TryGetDouble(out double doubleValue))
                        {
                            constant = Expression.Constant(doubleValue);
                        }
                        else
                        {
                            throw new NotSupportedException("Unsupported number type");
                        }
                        break;
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        constant = Expression.Constant(jsonElement.GetBoolean());
                        break;
                    case JsonValueKind.Null:
                        constant = Expression.Constant(null);
                        break;
                    default:
                        throw new NotSupportedException("Unsupported JsonValueKind");
                }
            }
            else if (value is string stringValue && DateOnly.TryParse(stringValue, out DateOnly dateTimeValue))
            {
                constant = Expression.Constant(dateTimeValue);
            }
            else
            {
                constant = Expression.Constant(value);
            }

            return constant;
        }
    }
}
