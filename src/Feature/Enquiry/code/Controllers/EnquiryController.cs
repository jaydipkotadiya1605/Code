using Sitecore.Feature.Enquiry.Extensions;
using Sitecore.Feature.Enquiry.Models;
using Sitecore.Feature.Enquiry.Repositories;
using Sitecore.Mvc.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Web.Mvc;

namespace Sitecore.Feature.Enquiry.Controllers
{
    public class EnquiryController : Controller
    {
        private readonly IEnquiryRepository enquiryRepository;
        public EnquiryController(IEnquiryRepository enquiryRepository)
        {
            this.enquiryRepository = enquiryRepository;
        }

        public ActionResult EnquiryForm(EnquiryForm form)
        {
            if (this.RouteData.Values.ContainsKey(Constants.StatusForm))
            {
                var status = (Status) this.RouteData.Values[Constants.StatusForm];
                if (status == Status.Error)
                {
                    this.ModelState.AddModelError(nameof(this.EnquiryForm), this.RouteData.Values[Constants.ErrorMessage] as string);
                    this.ViewData["Status"] = Status.Error;
                }
                this.RouteData.Values.Remove(Constants.ErrorMessage);
                this.RouteData.Values.Remove(Constants.StatusForm);
            }
            return this.View(this.enquiryRepository.RenderEnquiryForm(form));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateCaptcha]
        public ActionResult Submit(EnquiryForm form)
        {
            IView pageView = PageContext.Current.PageView;
            if (this.ModelState.IsValid)
            {
                var errorMessage = this.enquiryRepository.IsValidForm(form);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    if (Constants.GeneralEnquiry.Equals(form.EnquiryTypeSelected, StringComparison.OrdinalIgnoreCase))
                    {
                        errorMessage = this.enquiryRepository.SendGeneralInquiry(form);
                    }
                    else if (Constants.LeasingEnquiry.Equals(form.EnquiryTypeSelected, StringComparison.OrdinalIgnoreCase))
                    {
                        errorMessage = this.enquiryRepository.SendLeasingInquiry(form);
                    }
                        
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        return this.RedirectToRoute(MvcSettings.SitecoreRouteName, new { status = Status.Success });
                    }
                }
                this.RenderErrorMessage(errorMessage);
            }
            else {
                this.RenderErrorMessage("Please enter required fields.");
            }
            return this.View(pageView, form);
        }

        private void RenderErrorMessage(string errorMessage) {
            if (this.RouteData.Values.ContainsKey(Constants.StatusForm))
            {
                this.RouteData.Values[Constants.StatusForm] = Status.Error;
            }
            else
            {
                this.RouteData.Values.Add(Constants.StatusForm, Status.Error);
            }

            if (this.RouteData.Values.ContainsKey(Constants.ErrorMessage))
            {
                this.RouteData.Values[Constants.ErrorMessage] = errorMessage;
            }
            else
            {
                this.RouteData.Values.Add(Constants.ErrorMessage, errorMessage);
            }
        }
    }
}