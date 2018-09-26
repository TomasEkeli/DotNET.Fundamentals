using System;
using Dolittle.Concepts;

namespace Dolittle.Execution
{
    /// <summary>
    /// A unique identifier to allow us to trace actions and their consequencies throughout the system
    /// </summary>
    public class CorrelationId : ConceptAs<Guid>
    {
        /// <summary>
        /// Represents an Empty <see cref="CorrelationId" />
        /// </summary>
        public static readonly CorrelationId Empty = Guid.Empty;

        /// <summary>
        /// Represents the correlation used by the system
        /// </summary>
        public static readonly CorrelationId System = Guid.Parse("868ff40f-a133-4d0f-bfdd-18d726181e01");

        /// <summary>
        /// Creates an empty <see cref="CorrelationId" />
        /// </summary>
        public CorrelationId() => new CorrelationId(Guid.Empty);

        /// <summary>
        /// Instantiates a <see cref="CorrelationId" /> with the specified value
        /// </summary>
        /// <param name="guid">The value to initialize the <see cref="CorrelationId" /> with</param>
        public CorrelationId(Guid guid) => Value = guid;
    
        /// <summary>
        /// Creates a new <see cref="CorrelationId" /> with a generated Guid value.
        /// </summary>
        /// <returns>A <see cref="CorrelationId" /> initialised with a random Guid value</returns>
        public static CorrelationId New() => new CorrelationId(Guid.NewGuid());
    
        /// <summary>
        /// Implicitly converts a <see cref="Guid" /> to an instance of <see cref="CorrelationId" />
        /// </summary>
        /// <param name="value">The value to initialize the <see cref="CorrelationId" /> with</param>
        public static implicit operator CorrelationId(Guid value) => new CorrelationId(value);
    }
}