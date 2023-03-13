using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace DOANTOTNGHIEP.Models.mail
{
    public class mail
    {
        public static void guimaemailselenium(string Toemail, string chude, string noidung)
        {
            /*ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;*/
            var options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            // Chạy ngầm không pop up trình duyệt ra ngoài 
            options.AddArgument("headless");
            options.AddArgument("--window-position=-32000,-32000");
            options.AddArgument("--incognito");
            IWebDriver webDriver = new ChromeDriver(HostingEnvironment.MapPath("~/Content/chromdriver"));
            webDriver.Url = "https://mail.google.com/";

            var email = webDriver.FindElement(By.XPath("//input[@type='email']"));
            email.SendKeys("pthonlinesp");
            var nextemail = webDriver.FindElement(By.XPath("//*[@id='identifierNext']/div/button"));
            nextemail.Click();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            var pass = nextemail.FindElement(By.XPath("//*[@id='password']/div[1]/div/div[1]/input"));
            pass.SendKeys("haupham809");

            var nextpass = webDriver.FindElement(By.XPath("//*[@id='passwordNext']/div/button"));
            nextpass.Click();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            var soanthu = webDriver.FindElement(By.XPath("/html/body/div[7]/div[3]/div/div[2]/div[1]/div[1]/div[1]/div/div/div/div[1]/div/div"));
            soanthu.Click();
            var den = webDriver.FindElement(By.Name("to"));
            den.SendKeys(Toemail);
            var chudeemail = webDriver.FindElement(By.Name("subjectbox"));
            chudeemail.SendKeys(chude);
            var noidungemail = webDriver.FindElement(By.CssSelector(".Am.Al.editable.LW-avf>br"));
            noidungemail.SendKeys(noidung);
            var send = webDriver.FindElement(By.ClassName("aoO"));
            send.Click();
            int x = 1;
            while (true)
            {
                try
                {
                    if (webDriver.FindElement(By.XPath("/html/body/div[7]/div[3]/div/div[1]/div[4]/div[1]/div/div[3]/div/div/div[2]/span/span[1]")) != null)
                    {
                        if (webDriver.FindElement(By.XPath("/ html / body / div[7] / div[3] / div / div[1] / div[4] / div[1] / div / div[3] / div / div / div[2] / span / span[2] / span[1]")) == null)
                            break;
                        x++;
                    }
                }
                catch
                {
                    break;
                }

            }
            webDriver.Quit();
        }
        public static void Sendmessage(string message)
        {
            string accountSid = "ACc03883bcc5c09a070ab35d3d269c4827";
            string authToken = "7e3f8a7fdd22b7ed4a00b50934de718c";

            TwilioClient.Init(accountSid, authToken);

            var sendmessage = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber("+19785936169"),
                to: new Twilio.Types.PhoneNumber("+84338832744")
            );
        }

        public static void SendEmail(string address, string subject, string message)
        {
            /*Sendmessage(message);*/
            // guimaemailselenium(address, subject, message);
            /* string email = "pthonlinesp@gmail.com";
             string password = "haupham809";
             var loginInfo = new NetworkCredential(email, password);
             var msg = new System.Net.Mail.MailMessage();
             var smtpClient = new SmtpClient("smtp.gmail.com", 587);

             msg.From = new MailAddress(email);
             msg.To.Add(new MailAddress(address));
             msg.Subject = subject;
             msg.Body = message;
             msg.IsBodyHtml = true;

             smtpClient.EnableSsl = true;
             smtpClient.UseDefaultCredentials = false;
             smtpClient.Credentials = loginInfo;
             smtpClient.Send(msg);*/
        }
    }
}