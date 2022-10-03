using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NSpecifications
{
    /// <summary>
    /// Abstract Specification defined by an Expressions that can be used on IQueryables.
    /// </summary>
    /// <typeparam name="T">The type of the candidate.</typeparam>
    public abstract class ASpec<T> : ISpecification<T>
    {
        /// <summary>
        /// Holds the compiled expression so that it doesn't need to compile it everytime.
        /// </summary>
        Func<T, bool> _compiledFunc;

        /// <summary>
        /// Checks if a certain candidate meets a given specification.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns>New specification</returns>
        public virtual bool IsSatisfiedBy(T candidate)
        {
            _compiledFunc = _compiledFunc ?? this.Expression.Compile();
            return _compiledFunc(candidate);
        }

        /// <summary>
        /// Expression that defines the specification.
        /// </summary>
        public abstract Expression<Func<T, bool>> Expression { get; }

        /// <summary>
        /// Composes two specifications with an And operator.
        /// </summary>
        /// <param name="spec1">Specification</param>
        /// <param name="spec2">Specification</param>
        /// <returns>New specification</returns>
        public static And operator &(ASpec<T> spec1, ASpec<T> spec2)
        {
            return new And(spec1, spec2);
        }

        /// <summary>
        /// Composes two specifications with an Or operator.
        /// </summary>
        /// <param name="spec1">Specification</param>
        /// <param name="spec2">Specification</param>
        /// <returns>New specification</returns>
        public static Or operator |(ASpec<T> spec1, ASpec<T> spec2)
        {
            return new Or(spec1, spec2);
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is true.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static ASpec<T> operator ==(bool value, ASpec<T> spec)
        {
            return value ? spec : !spec;
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is true.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static ASpec<T> operator ==(ASpec<T> spec, bool value)
        {
            return value ? spec : !spec;
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is false.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static ASpec<T> operator !=(bool value, ASpec<T> spec)
        {
            return value ? !spec : spec;
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is false.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static ASpec<T> operator !=(ASpec<T> spec, bool value)
        {
            return value ? !spec : spec;
        }

        /// <summary>
        /// Creates a new specification that negates a given specification.
        /// </summary>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static Not operator !(ASpec<T> spec)
        {
            return new Not(spec);
        }

        /// <summary>
        /// Allows using ASpec[T] in place of a lambda expression.
        /// </summary>
        /// <param name="spec"></param>
        public static implicit operator Expression<Func<T, bool>>(ASpec<T> spec)
        {
            return spec.Expression;
        }

        /// <summary>
        /// Allows using ASpec[T] in place of Func[T, bool].
        /// </summary>
        /// <param name="spec"></param>
        public static implicit operator Func<T, bool>(ASpec<T> spec)
        {
            return spec.IsSatisfiedBy;
        }

        /// <summary>
        /// Converts the expression into a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Expression.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is ASpec<T> spec &&
                   EqualityComparer<Expression<Func<T, bool>>>.Default.Equals(Expression, spec.Expression);
        }

        public override int GetHashCode()
        {
            return -1489834557 + EqualityComparer<Expression<Func<T, bool>>>.Default.GetHashCode(Expression);
        }

        public sealed class And : ASpec<T>, IOrSpecification<T>
        {
            public ASpec<T> Spec1 { get; }

            public ASpec<T> Spec2 { get; }

            ISpecification<T> IOrSpecification<T>.Spec1 => Spec1;

            ISpecification<T> IOrSpecification<T>.Spec2 => Spec1;

            internal And(ASpec<T> spec1, ASpec<T> spec2)
            {
                Spec1 = spec1 ?? throw new ArgumentNullException(nameof(spec1));
                Spec2 = spec2 ?? throw new ArgumentNullException(nameof(spec2));
            }

            public override Expression<Func<T, bool>> Expression => Spec1.Expression.And(Spec2.Expression);

            public new bool IsSatisfiedBy(T candidate) =>
                Spec1.IsSatisfiedBy(candidate) && Spec2.IsSatisfiedBy(candidate);
        }

        public sealed class Or : ASpec<T>, IOrSpecification<T>
        {
            public ASpec<T> Spec1 { get; }

            public ASpec<T> Spec2 { get; }

            ISpecification<T> IOrSpecification<T>.Spec1 => Spec1;

            ISpecification<T> IOrSpecification<T>.Spec2 => Spec2;

            internal Or(ASpec<T> spec1, ASpec<T> spec2)
            {
                Spec1 = spec1 ?? throw new ArgumentNullException(nameof(spec1));
                Spec2 = spec2 ?? throw new ArgumentNullException(nameof(spec2));
            }

            public override Expression<Func<T, bool>> Expression => Spec1.Expression.Or(Spec2.Expression);

            public bool Is(T candidate) => Spec1.IsSatisfiedBy(candidate) || Spec2.IsSatisfiedBy(candidate);
        }

        public sealed class Not : ASpec<T>, INotSpecification<T>
        {
            public ASpec<T> Inner { get; }

            ISpecification<T> INotSpecification<T>.Inner => Inner;

            internal Not(ASpec<T> spec)
            {
                Inner = spec ?? throw new ArgumentNullException(nameof(spec));
            }

            public override Expression<Func<T, bool>> Expression => Inner.Expression.Not();

            public bool Is(T candidate) => !Inner.IsSatisfiedBy(candidate);
        }
    }
}
