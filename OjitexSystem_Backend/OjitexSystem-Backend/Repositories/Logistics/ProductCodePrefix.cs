using System.Linq.Expressions;

namespace OjitexSystem_Backend.Repositories.Logistics;

public static class ProductCodePrefix
{
    public static IQueryable<TEntity> WhereProductCodeStartsWith<TEntity>(
        this IQueryable<TEntity> query, Expression<Func<TEntity, decimal?>> selector, decimal prefix)
    {
        if (prefix < 0 || prefix > 9_999_999 || decimal.Truncate(prefix) != prefix)
            throw new ArgumentOutOfRangeException(nameof(prefix));

        // Product codes are decimal(7,0). Numeric ranges retain prefix semantics
        // without converting the indexed column to text for every row.
        var code = selector.Body;
        Expression predicate = Expression.Equal(code, NullableDecimal(prefix));
        if (prefix > 0)
        {
            for (var scale = 10m; prefix * scale <= 9_999_999; scale *= 10)
            {
                var range = Expression.AndAlso(
                    Expression.GreaterThanOrEqual(code, NullableDecimal(prefix * scale)),
                    Expression.LessThan(code, NullableDecimal((prefix + 1) * scale)));
                predicate = Expression.OrElse(predicate, range);
            }
        }

        return query.Where(Expression.Lambda<Func<TEntity, bool>>(predicate, selector.Parameters));
    }

    private static Expression NullableDecimal(decimal value) =>
        Expression.Convert(Expression.Constant(value), typeof(decimal?));
}
