using Sitecore.Data.Fields;
using Sitecore.Data.Templates;
using Sitecore.Diagnostics;
using Sitecore.Shell;
using System;
using System.Collections.Generic;

namespace Sitecore.Foundation.HiddenFields.Pipelines
{
    public class GetFields : Shell.Applications.ContentEditor.Pipelines.GetContentEditorFields.GetFields
    {
        private IList<string> fieldsToHide;

        public virtual string HiddenFields { get; set; }

        protected virtual IList<string> FieldsToHide
        {
            get
            {
                if (this.fieldsToHide == null)
                {
                    if (string.IsNullOrWhiteSpace(this.HiddenFields))
                    {
                        this.fieldsToHide = new List<string>();
                    }
                    else
                    {
                        this.fieldsToHide = this.HiddenFields.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }

                return this.fieldsToHide;
            }
        }

        protected override bool CanShowField(Field field, TemplateField templateField)
        {
            Assert.ArgumentNotNull(field, nameof(field));
            Assert.ArgumentNotNull(templateField, nameof(templateField));

            if (!UserOptions.ContentEditor.ShowSystemFields && (this.FieldsToHide.Contains(field.ID.ToString()) || this.FieldsToHide.Contains(field.Name)))
            {
                return false;
            }

            return base.CanShowField(field, templateField);
        }
    }
}