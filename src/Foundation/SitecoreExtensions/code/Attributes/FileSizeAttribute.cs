using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.SitecoreExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _fileSize;

        public FileSizeAttribute(int fileSize)
        {
            this._fileSize = fileSize;
        }
        public override bool IsValid(object value)
        {
            var fileUpload = (HttpPostedFileBase)value;
            if (fileUpload != null && fileUpload.ContentLength > _fileSize)
            {
                return false;
            }
            return true;
        }
        public override string FormatErrorMessage(string name)
            => String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
    }
}