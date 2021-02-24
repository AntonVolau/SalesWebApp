using CsvHelper;
using SalesUpdater.Interfaces;
using SalesUpdater.Interfaces.Core;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SalesUpdater.Core
{
    public class FileHandler : IFileHandler
    {
        private const string dateFormatKey = "dateFormat";
        private const string clientNameSplitterKey = "clientNameSplitter";
        private const string processedFileLocation = "newFileLocation";
        private const string failedFileLocation = "newFailedFileLocation";
        private ReaderWriterLockSlim Locker { get; }

        private IUnit Unit { get; }
        private Interfaces.IParser Parser { get; }
        public FileHandler(IUnit singleUnit, Interfaces.IParser parser, ReaderWriterLockSlim locker)
        {
            Unit = singleUnit;
            Parser = parser;
            Locker = locker;
        }

        public void ProcessFile(object source, FileSystemEventArgs e)
        {
            Task.Run(() => Run(e));
        }

        private void Run(FileSystemEventArgs e)
        {
            string newFileLocation = ConfigurationManager.AppSettings[processedFileLocation] + e.Name;
            string failedFilesLocation = ConfigurationManager.AppSettings[failedFileLocation] + e.Name;
            var fileNameSplitter = char.Parse(ConfigurationManager.AppSettings["fileNameSplitter"]);
            bool added = false;
            try
            {
                WriteToDatabase(
                    Parser.ParseFile(e.FullPath),
                    Parser.ParseLine(e.Name, fileNameSplitter).First());
                Console.WriteLine($"{e.Name} was added to database");
                Logger.Log($"{e.Name} was processed and added to database");
                added = true;
            }
            catch (HeaderValidationException)
            {
                Logger.Log($"Error occured when trying to process {e.Name}. First line must match  Date, Client, Product, Price");
            }
            catch (FormatException)
            {
                Logger.Log($"{e.Name} have incorrect format");
            }
            catch (ArgumentNullException)
            {
                Logger.Log($"AppConfig doesn't contain date entry or separator for client Name and Surname");
            }
            catch (Exception m)
            {
                Logger.Log($"An error occured when trying to add {e.Name} to database: " + m.Message);
            }
            finally
            {
                if (added)
                {
                    try
                    {
                        System.IO.File.Move(e.FullPath, newFileLocation);
                    }
                    catch (Exception x)
                    {
                        Logger.Log($"Error occured when trying to replace file {e.Name}: " + x.Message);
                    }
                }
                else
                {
                    try
                    {
                        System.IO.File.Move(e.FullPath, failedFilesLocation);

                    }
                    catch (Exception x)
                    {
                        Console.WriteLine($"Error occured when trying to replace file {e.Name}: " + x.Message);
                        Logger.Log($"Error occured when trying to replace file {e.Name}: " + x.Message);
                    }
                }
            }
        }

        private void WriteToDatabase(IEnumerable<IFile> sales, string managerSurname)
        {
            Unit.Add(CreateDataObjects(sales, managerSurname).ToArray());
        }

        private IEnumerable<SaleDTO> CreateDataObjects(IEnumerable<IFile> fileContents,
        string managerLastName)
        {
            try
            {
                var dateFormat = ConfigurationManager.AppSettings[dateFormatKey];
                var clientNameSplitter = char.Parse(ConfigurationManager.AppSettings[clientNameSplitterKey]);

                return (from fileContent in fileContents
                        let date = DateTime.ParseExact(fileContent.Date, dateFormat, null)
                        let ClientName = Parser.ParseLine(fileContent.Client, clientNameSplitter)
                        let ClientDto = new ClientDTO(ClientName[0], ClientName[1])
                        let productDto = new ProductDTO(fileContent.Product)
                        let sum = decimal.Parse(fileContent.Price)
                        let managerDto = new ManagerDTO(managerLastName)
                        select new SaleDTO(date, ClientDto, productDto, sum, managerDto))
                    .ToList();
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FormatException("Client field must contain Name and Surname");
            }
        }
    }
}
