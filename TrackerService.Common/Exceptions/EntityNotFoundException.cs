using System;

namespace TrackerService.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public EntityNotFoundException(string entityName, string id) : this($"{entityName} not found for id: {id}")
        {

        }
    }
}
