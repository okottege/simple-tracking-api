using System.Collections.Generic;

namespace TrackerService.Core.CoreDomain.Tasks.Definitions
{
    public interface IDocumentSignatureTask : IApprovalTask
    {
        List<string> DocumentIdList { get; }
    }
}