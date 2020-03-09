using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ActiveErrors
{
	[DataContract]
	public sealed class Operation<T> : Operation
	{
		public Operation(ICollection<Error> errors) : base(errors) => Result = OperationResult.Error;
		public Operation(params Error[] errors) : base(errors) { }
		public Operation(T data) : this(data, null) { }
		public Operation(T data, params Error[] errors) : base(errors) => Data = data;
		public Operation(T data, ICollection<Error> errors) : base(errors) => Data = data;
		[DataMember] public T Data { get; set; }
	}

	[DataContract]
	public class Operation
	{
		public Operation() => Result = OperationResult.Succeeded;

		public Operation(Error error)
		{
			Result = OperationResult.Error;
			Errors = new List<Error>(error == null ? Enumerable.Empty<Error>() : new[] {error});
		}

		public Operation(ICollection<Error> errors) : this()
		{
			Result = errors?.Count > 0 ? OperationResult.Error : OperationResult.Succeeded;
			Errors = new List<Error>(errors ?? Enumerable.Empty<Error>());
		}

		public static Operation CompletedWithoutErrors => new Operation();

		[DataMember] public OperationResult Result { get; set; }

		[DataMember]
		public bool Succeeded => Result == OperationResult.Succeeded || Result == OperationResult.SucceededWithErrors;

		[DataMember] public bool HasErrors => Errors?.Count > 0;

		[DataMember] public IList<Error> Errors { get; private set; }

		public static Operation<T> FromResult<T>(T data)
		{
			return new Operation<T>(data);
		}

		public static Operation<T> FromResult<T>(T data, IList<Error> errors)
		{
			return new Operation<T>(data, errors);
		}
	}

}