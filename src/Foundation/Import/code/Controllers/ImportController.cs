using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Foundation.Import.Configuration;
using Sitecore.Foundation.Import.Models;
using Sitecore.Foundation.Import.Pipelines.ImportItems;
using Sitecore.Pipelines;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sitecore.Foundation.Import.Controllers
{
    [ServicesController]
    public class ImportController : ServicesApiController
    {
        [HttpPost]
        public IHttpActionResult Import(ImportModel importModel)
        {
            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var languageItem = database.GetItem(importModel.Language);
            var uploadedFile = (MediaItem) database.GetItem(importModel.MediaItemId);
            if (uploadedFile == null)
            {
                return new JsonResult<ImportResultModel>(null, new JsonSerializerSettings(), Encoding.UTF8, this);
            }

            ImportResultModel result;
            try
            {
                var args = new ImportItemsArgs
                {
                    Database = database,
                    FileExtension = uploadedFile.Extension.ToLower(),
                    FileStream = uploadedFile.GetMediaStream(),
                    TargetLanguage = Globalization.Language.Parse(languageItem.Name),
                    ContentType = importModel.ContentTypeId,
                    ImportOptions = new ImportOptions
                    {
                        CsvDelimiter = new[] { importModel.CsvDelimiter },
                        MultipleValuesImportSeparator = importModel.MultipleValuesSeparator,
                        TreePathValuesImportSeparator = @"\",
                        FirstRowAsColumnNames = importModel.FirstRowAsColumnNames
                    }
                };
                args.ImportOptions.ExistingItemHandling = (ExistingItemHandling)
                    Enum.Parse(typeof(ExistingItemHandling), importModel.ExistingItemHandling);
                args.ImportOptions.InvalidLinkHandling = (InvalidLinkHandling)
                    Enum.Parse(typeof(InvalidLinkHandling), importModel.InvalidLinkHandling);

                Diagnostics.Log.Info(
                    string.Format("Sitecore.Foundation.Import: mediaItemId:{0} firstRowAsColumnNames:{1}",
                        importModel.MediaItemId, args.ImportOptions.FirstRowAsColumnNames),
                    this);
                args.Timer.Start();
                CorePipeline.Run("importItems", args);
                args.Timer.Stop();
                if (args.Aborted)
                {
                    result = new ImportResultModel
                    {
                        HasError = true,
                        Log = args.Statistics.ToString(),
                        ErrorMessage = args.Message,
                        ErrorDetail = args.ErrorDetail
                    };
                }
                else
                {
                    result = new ImportResultModel
                    {
                        Log = args.Statistics.ToString() + " Duration: " + args.Timer.Elapsed.ToString("c")
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ImportResultModel
                {
                    HasError = true,
                    ErrorMessage = ex.Message,
                    ErrorDetail = ex.ToString()
                };
            }

            return new JsonResult<ImportResultModel>(result, new JsonSerializerSettings(), Encoding.UTF8, this);
        }

        [HttpGet]
        public IHttpActionResult DefaultSettings()
        {
            var options = Factory.GetDefaultImportOptions();
            var model = new SettingsModel
            {
                CsvDelimiter = options.CsvDelimiter[0],
                ExistingItemHandling = options.ExistingItemHandling.ToString(),
                InvalidLinkHandling = options.InvalidLinkHandling.ToString(),
                MultipleValuesSeparator = options.MultipleValuesImportSeparator,
                FirstRowAsColumnNames = options.FirstRowAsColumnNames
            };
            return new JsonResult<SettingsModel>(model, new JsonSerializerSettings(), Encoding.UTF8, this);
        }
    }
}