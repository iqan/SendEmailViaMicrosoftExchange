using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Microsoft.Exchange.WebServices.Data;

namespace SendEmail.Models
{
    public static class Service
    {
        public static ExchangeService GetService(UserData userData, Uri uri)
        {
            ExchangeService service = new ExchangeService();

            try
            {
                service.Credentials = new NetworkCredential(userData.UserName, userData.UserPassword, userData.DomainName);

                if (uri == null)
                {
                    service.AutodiscoverUrl(userData.UserEmail, RedirectionUrlValidationCallback);
                    uri = service.Url;
                }
                else
                {
                    service.Url = uri;
                }
                return service;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }

            return result;
        }
        public static string SendBatchEmails(ExchangeService service, UserData userData)
        {
            string responseText = string.Empty;
            // Create three separate email messages.
            EmailMessage message1 = new EmailMessage(service);
            message1.ToRecipients.Add(userData.ToName);
            message1.Subject = userData.EmailSubject;
            message1.Body = userData.EmailBody + "\r\n\r\n";

            Collection<EmailMessage> msgs = new Collection<EmailMessage>() { message1 };

            try
            {
                // Send the batch of email messages. This results in a call to EWS. The response contains the results of the batched request to send email messages.
                ServiceResponseCollection<ServiceResponse> response = service.CreateItems(msgs, WellKnownFolderName.Drafts, MessageDisposition.SendOnly, null);

                // Check the response to determine whether the email messages were successfully submitted.
                if (response.OverallResult == ServiceResult.Success)
                {
                    return "Email was successfully sent.";
                }

                int counter = 1;

                /* If the response was not an overall success, access the errors.
                 * Results are returned in the order that the action was submitted. For example, the attempt for message1
                 * will be represented by the first result since it was the first one added to the collection.
                 * Errors are not returned if an NDR is returned.
                 */
                
                foreach (ServiceResponse resp in response)
                {
                    responseText = string.Format("Result (message {0}): {1}\r\nError Code: {2}\r\nError Message: {3}", 
                        counter, resp.Result, resp.ErrorCode, resp.ErrorMessage);
                    counter++;
                }
            }
            catch (Exception e)
            {
                responseText = e.Message;
            }
            return responseText;
        }
    }
}