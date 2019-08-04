using System;

namespace TrackerService.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }
        public EntityNotFoundException(string message) : base(message) { }
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public EntityNotFoundException(long entityId, string entityName)
            : base($"No {entityName} entity found for Id: {entityId}")
        {

        }
    }
}
