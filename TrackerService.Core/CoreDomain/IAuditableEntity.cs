using System;

namespace TrackerService.Core.CoreDomain
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; }
        string CreatedBy { get; }
        DateTime ModifiedDate { get; }
        string ModifiedBy { get; }
    }
}