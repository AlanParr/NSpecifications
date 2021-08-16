using System.Linq;

namespace NSpecifications
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TSource> Where<TSource>(
            this IQueryable<TSource> source, ISpecification<TSource> spec)
        {
            return source.Where(spec.Expression);
        }

        public static IQueryable<TSource> OrderBy<TSource>(
            this IQueryable<TSource> source, ISpecification<TSource> spec)
        {
            return source.OrderBy(spec.Expression);
        }
    }
}