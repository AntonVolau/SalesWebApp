using SalesUpdater.Interfaces.Core;
using System.Collections.Generic;

namespace SalesUpdater.Interfaces
{
    public interface IParser
    {
        IEnumerable<IFile> ParseFile(string filePath);

        IList<string> ParseLine(string fileName, char separator);
    }
}
