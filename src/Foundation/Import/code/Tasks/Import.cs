using System.Web.Hosting;
using Sitecore.Foundation.Import.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Tasks;
using System;
using System.IO;

namespace Sitecore.Foundation.Import.Tasks
{
    public class Import
    {
        public void Run(Item[] items, CommandItem command, ScheduleItem schedule)
        {
            var importCommand = new ImportCommandItem(command.InnerItem);
            Log.Info(
                "Sitecore.Foundation.Import.Tasks.Import.Run() Starting Import: "
                + " fileName=" + importCommand.FileName
                + " lang=" + importCommand.TargetLanguage
                + " contentType=" + importCommand.ContentTypeId
                + " importMap=" + importCommand.ImportMapId
                + " database=" + importCommand.Database.Name
                + " csvDelimiter=" + importCommand.CsvDelimiter
                + " ExistingItemHandling=" + importCommand.ExistingItemHandling
                + " InvalidLinkHandling=" + importCommand.InvalidLinkHandling
                + " MultipleValuesImportSeparator=" + importCommand.MultipleValuesImportSeparator
                + " TreePathValuesImportSeparator=" + importCommand.TreePathValuesImportSeparator, this);

            var options = Factory.GetDefaultImportOptions();
            if (importCommand.CsvDelimiter != null)
            {
                options.CsvDelimiter = new[] {importCommand.CsvDelimiter};
            }
            if (importCommand.ExistingItemHandling != null)
            {
                options.ExistingItemHandling = (ExistingItemHandling)
                    Enum.Parse(typeof (ExistingItemHandling), importCommand.ExistingItemHandling);
            }
            if (importCommand.InvalidLinkHandling != null)
            {
                options.InvalidLinkHandling = (InvalidLinkHandling)
                    Enum.Parse(typeof (InvalidLinkHandling), importCommand.InvalidLinkHandling);
            }
            if (importCommand.MultipleValuesImportSeparator != null)
            {
                options.MultipleValuesImportSeparator = importCommand.MultipleValuesImportSeparator;
            }
            if (importCommand.TreePathValuesImportSeparator != null)
            {
                options.TreePathValuesImportSeparator = importCommand.TreePathValuesImportSeparator;
            }
            options.FirstRowAsColumnNames = importCommand.FirstRowAsColumnNames;
            if (string.IsNullOrWhiteSpace(importCommand.FileName))
            {
                Log.Error(
                    "Sitecore.Foundation.Import.Tasks.Import.Run() - Import Error: File not specified",
                    this);
                return;
            }
            string fileName;
            if (File.Exists(importCommand.FileName))
            {
                fileName = importCommand.FileName;
            }
            else
            {
                fileName = HostingEnvironment.MapPath(importCommand.FileName);
                if (!File.Exists(fileName))
                {
                    Log.Error(
                        "Sitecore.Foundation.Import.Tasks.Import.Run() - Import Error: File not found (" + importCommand.FileName + ")",
                        this);
                    return;
                }
            }
            var extension = GetFileExtension(fileName);
            if (extension == null)
            {
                Log.Error(
                    "Sitecore.Foundation.Import.Tasks.Import.Run() - Import Error: Unknown file extension (" + importCommand.FileName +
                    ")", this);
            }
        }

        private string GetFileExtension(string fileName)
        {
            var index = fileName.LastIndexOf(".");
            if (index > -1 && index < (fileName.Length - 1))
            {
                return fileName.Substring(index + 1);
            }
            return "";
        }

    }
}