﻿using System;

namespace TrackerService.Core.CoreDomain.Tasks
{
    public interface ITaskAssignment
    {
        AssignmentEntityType Type { get; }
        string EntityId { get; }
        DateTime CreatedDate { get; }
        string CreatedBy { get; }
    }
}