using System.Runtime.Serialization;

namespace ActiveErrors
{
	[DataContract]
	public enum OperationResult : byte
	{
		[EnumMember] Refused,
		[EnumMember] Error,
		[EnumMember] Succeeded,
		[EnumMember] SucceededWithErrors
	}
}