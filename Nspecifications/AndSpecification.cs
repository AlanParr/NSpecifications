using System;
using System.Linq.Expressions;

namespace NSpecifications
{
    internal class AndSpecification<T> : IAndSpecification<T>
    {
        public ISpecification<T> Spec1 { get; }

        public ISpecification<T> Spec2 { get; }

        internal AndSpecification(ISpecification<T> spec1, ISpecification<T> spec2)
        {
            Spec1 = spec1 ?? throw new ArgumentNullException(nameof(spec1));
            Spec2 = spec2 ?? throw new ArgumentNullException(nameof(spec2));
        }

        public Expression<Func<T, bool>> Expression => Spec1.Expression.And(Spec2.Expression);

        public bool IsSatisfiedBy(T candidate) => Spec1.IsSatisfiedBy(candidate) && Spec2.IsSatisfiedBy(candidate);
    }
}