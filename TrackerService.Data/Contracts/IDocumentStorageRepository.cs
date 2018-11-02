using System.IO;
using System.Threading.Tasks;

namespace TrackerService.Data.Contracts
{
    public interface IDocumentStorageRepository
    {
        Task SaveDocument(string fileName, Stream stream);
    }
}