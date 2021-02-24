using CsvHelper;
using SalesUpdater.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using static System.String;

namespace SalesUpdater.Core
{
    public class Parser : Interfaces.IParser
    {
        public IEnumerable<IFile> ParseFile(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

                foreach (var record in csvReader.GetRecords<File>())
                {
                    yield return record.Product != Empty &&
                                 record.Client != Empty &&
                                 record.Date != Empty &&
                                 record.Price != Empty
                        ? record
                        : throw new FormatException();
                }
            }
        }

        public IList<string> ParseLine(string line, char separator)
        {
            return line.Split(separator);
        }
    }
}
