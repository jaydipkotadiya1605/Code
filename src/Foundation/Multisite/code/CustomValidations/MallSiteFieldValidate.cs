namespace Sitecore.Foundation.Multisite.CustomValidations
{
    using Sitecore.Data;
    using Sitecore.Data.Validators;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class MallSiteFieldValidate : StandardValidator
    {

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The validator name.</value>
        public override string Name => "Display on malls";

        public MallSiteFieldValidate() : base()
        {
        }

        private MallSiteFieldValidate(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override ValidatorResult Evaluate()
        {
            var currentItem = this.GetItem();
            var storedItem = Database.GetItem(this.ItemUri);
            var oldValueMalls = storedItem.Fields[Templates.MallSite.Fields.SiteDisplaySettings].Value;

            if (!this.ControlValidationValue.Equals(oldValueMalls)) // MallSite Value changed
            {
                var sourceId = currentItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId];
                if (sourceId.HasValue)
                {
                    this.Text = this.GetText("Item is came from mall site. Can't be send to another sites.");
                    Sitecore.Context.ClientPage.ClientResponse.Alert("Item is came from mall site. Can't be send to another sites.");
                    return base.GetFailedResult(ValidatorResult.FatalError);
                }
            }
            return ValidatorResult.Valid;
        }

        protected override ValidatorResult GetMaxValidatorResult()
        {
            return base.GetFailedResult(ValidatorResult.FatalError);
        }
    }
}