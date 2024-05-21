using System.Globalization;

namespace SharedKernel.Results
{

    /// <summary>
    /// Represents result type
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// Standard result type for successful operation.
        /// </summary>
        Ok = 200,

        /// <summary>
        /// The operation cannot be processed due to an obvious error by the client.
        /// </summary>
        Validation = 400,

        /// <summary>
        /// The requested resource was not found
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// Indicates that the operation could not be processed because of conflict in the current state of the resource.
        /// </summary>
        Conflict = 409,

        /// <summary>
        /// The operation was refused due to lack of rights.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// The operation was refused because the user who caused the operation is not authorized.
        /// </summary>
        Unauthorized = 401
    }

    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    public class Result 
    {
        private static readonly Result _success = new() { Succeeded = true, Type = ResultType.Ok };
        protected readonly List<Error> _errors = [];
        
        protected Result() { }

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// Represents the http status code from the list defined using <see cref="ResultType"/> enum.
        /// </summary>
        public ResultType Type { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}" of <see cref="Error"/> instances 
        /// containing errors occurred during an operation />
        /// </summary>
        public IEnumerable<Error> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="Result"/> indicating successful operation.
        /// </summary>
        /// <returns>An <see cref="Result"/> indicating successful operation.</returns>
        public static Result Success => _success;

        /// <summary>
        /// Creates an <see cref="Result"/> indicating <see cref="ResultType.NotFound"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static Result FailedNotFound(params Error[] errors) =>
            Failed(ResultType.NotFound, errors);

        /// <summary>
        /// Creates an <see cref="Result"/> indicating <see cref="ResultType.Validation"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static Result FailedValidation(params Error[] errors) =>
            Failed(ResultType.Validation, errors);

        /// <summary>
        /// Creates an <see cref="Result"/> indicating <see cref="ResultType.Forbidden"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static Result FailedForbidden(params Error[] errors) =>
            Failed(ResultType.Forbidden, errors);

        /// <summary>
        /// Creates an <see cref="Result"/> indicating <see cref="ResultType.Conflict"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static Result FailedConflict(params Error[] errors) =>
            Failed(ResultType.Conflict, errors);

        /// <summary>
        /// Creates an <see cref="Result"/> indicating <see cref="ResultType.Unauthorized"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static Result FailedUnauthorized(params Error[] errors) =>
            Failed(ResultType.Unauthorized, errors);

        private static Result Failed(ResultType code, params Error[] errors)
        {
            var result = new Result { Succeeded = false, Type = code };
            if (errors is not null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="Result"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="Result"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded", otherwise it returned
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
    }

    /// <summary>
    /// Represents the result of an operation that stores the value.
    /// </summary>
    /// <typeparam name="TValue">Type of stored value</typeparam>
    public class Result<TValue> : Result
    {

        protected Result() : base() { }
        /// <summary>
        /// The stored value of <see cref="TValue"/> for a successful operation.
        /// </summary>
        /// <value>Defined and not null if operation succeeded, otherwise default</value>
        public TValue? Value { get; protected set; }


        /// <summary>
        /// Creates an <see cref="Result"/> with <typeparamref name="TValue"/> indicating succesful operation with stored <paramref name="value"/>
        /// </summary>
        /// <param name="value">Stored value of result</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="value"/> is null</exception> 
        /// <returns>An <see cref="Result"/> with <typeparamref name="TValue"/> indicating succesful operation with stored <paramref name="value"/></returns>
        public static new Result<TValue> Success(TValue value)
        {
            ArgumentNullException.ThrowIfNull(nameof(value));
            return new Result<TValue>() { Succeeded = true, Value = value, Type = ResultType.Ok };
        }

        /// <summary>
        /// Creates an <see cref="Result"/> with <typeparamref name="TValue"/> indicating <see cref="ResultType.NotFound"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> with <typeparamref name="TValue"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static new Result<TValue> FailedNotFound(params Error[] errors) =>
            Failed(ResultType.NotFound, errors);

        /// <summary>
        /// Creates an <see cref="Result"/> with <typeparamref name="TValue"/> indicating <see cref="ResultType.Validation"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> with <typeparamref name="TValue"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static new Result<TValue> FailedValidation(params Error[] errors) =>
            Failed(ResultType.Validation, errors);

        /// <summary>
        /// Creates an <see cref="Result"/> with <typeparamref name="TValue"/> indicating <see cref="ResultType.Forbidden"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> with <typeparamref name="TValue"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static new Result<TValue> FailedForbidden(params Error[] errors) =>
            Failed(ResultType.Forbidden, errors);

        /// <summary>
        /// Creates an <see cref="Result"/> with <typeparamref name="TValue"/> indicating <see cref="ResultType.Conflict"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> with <typeparamref name="TValue"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static new Result<TValue> FailedConflict(params Error[] errors) =>
            Failed(ResultType.Conflict, errors);

        /// <summary>
        /// Creates an <see cref="Result"/> with <typeparamref name="TValue"/> indicating <see cref="ResultType.Unauthorized"/> error, with list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error"/> which caused operation to fail</param>
        /// <returns>An <see cref="Result"/> with <typeparamref name="TValue"/> indicating failed operation, with list of <paramref name="errors"/> if applicable.</returns>
        public static new Result<TValue> FailedUnauthorized(params Error[] errors) =>
            Failed(ResultType.Unauthorized, errors);

        private static Result<TValue> Failed(ResultType code, params Error[] errors)
        {
            var result = new Result<TValue> { Succeeded = false, Value = default, Type = code };
            if (errors is not null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }
    }
}