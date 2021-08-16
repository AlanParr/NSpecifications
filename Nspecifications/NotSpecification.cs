using System;
using System.Linq.Expressions;

namespace NSpecifications
{
    internal class NotSpecification<T> : INotSpecification<T>
    {
        public ISpecification<T> Inner { get; private set; }

        internal NotSpecification(ISpecification<T> inner)
        {
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public Expression<Func<T, bool>> Expression => Inner.Expression.Not();

        public bool IsSatisfiedBy(T candidate) => !Inner.IsSatisfiedBy(candidate);
    }

}