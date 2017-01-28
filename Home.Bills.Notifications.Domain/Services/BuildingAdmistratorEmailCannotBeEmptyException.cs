using System;
using System.Runtime.Serialization;

namespace Home.Bills.Notifications.Domain.Services
{
    public class BuildingAdmistratorEmailCannotBeEmptyException : Exception
    {
        public BuildingAdmistratorEmailCannotBeEmptyException()
        {
        }

        public BuildingAdmistratorEmailCannotBeEmptyException(string message) : base(message)
        {
        }

        public BuildingAdmistratorEmailCannotBeEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BuildingAdmistratorEmailCannotBeEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}