namespace Raven.Abstractions.Data
{
	public static class Constants
	{
		public const string RavenAuthenticatedUser = "Raven-Authenticated-User";
		public const string LastModified = "Last-Modified";
		public const string DefaultDatabase = "<default>";
		public const string TemporaryScoreValue = "Temp-Index-Score";
		public const string DistanceFieldName = "__distance";
		public const string NullValueNotAnalyzed = "[[NULL_VALUE]]";
		public const string EmptyStringNotAnalyzed = "[[EMPTY_STRING]]";
		public const string NullValue = "NULL_VALUE";
		public const string EmptyString = "EMPTY_STRING";
		public const string DocumentIdFieldName = "__document_id";
		public const string ReduceKeyFieldName = "__reduce_key";
		public const string RavenClrType = "Raven-Clr-Type";
		public const string RavenEntityName = "Raven-Entity-Name";
		public const string RavenReadOnly = "Raven-Read-Only";
		public const string RavenDocumentDoesNotExists = "Raven-Document-Does-Not-Exists";
		public const string Metadata = "@metadata";
		public const string NotForReplication = "Raven-Not-For-Replication";
	}
}