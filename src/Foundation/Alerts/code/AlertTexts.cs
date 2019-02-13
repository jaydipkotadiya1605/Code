using Sitecore.Data;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Foundation.Alerts
{
    public static class AlertTexts
    {
        public static string InvalidDataSourceTemplate(ID templateId) => string.Format(DictionaryPhraseRepository.Current.Get(Constants.InvalidDatasource, Constants.InvalidDatasourceText), templateId);
        public static string InvalidDataSourceTemplateFriendlyMessage => DictionaryPhraseRepository.Current.Get(Constants.InvalidDatasourceTemplate, Constants.InvalidDatasourceTemplateText);
        public static string InvalidDataSource => DictionaryPhraseRepository.Current.Get(Constants.InvalidDatasourceUnknownTemplate, Constants.InvalidDatasoruceUnknownTemplateText);
    }
}