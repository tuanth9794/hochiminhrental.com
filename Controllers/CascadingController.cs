using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CoreCnice.Connect;
using CoreCnice.UtilsCs;
using CoreCnice.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Helpers;
using CoreCnice.Models;

namespace CoreCnice.Controllers
{
    [Produces("application/json")]
    [Route("api/Cascading")]
    public class CascadingController : Controller
    {
        string SessionKeyName = "CustomerSesion";
        string SessionKeyName1 = "CustomerSesion1";
        string SessionKeyName2 = "CustomerSesion2";
        string SessionKeyName3 = "CustomerSesion3";
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        clsFormat fm = new clsFormat();

        [HttpGet]
        [Route("ContactFooter")]
        public IActionResult ContactFooter(string Hoten, string sodienthoai, string diachi)
        {
            try
            {
                ESHOP_CONTACT objcart = new ESHOP_CONTACT()
                {
                    CONTACT_PUBLISHDATE = DateTime.Now,
                    CONTACT_NAME = Hoten,
                    CONTACT_PHONE = sodienthoai,
                    CONTACT_SHOWTYPE = 1,
                    CONTACT_TITLE = "Thông tin liên hệ từ footer",
                    CONTACT_ADDRESS = diachi,
                    CONTACT_EMAIL = "",
                };
                _context.Add(objcart);
                _context.SaveChangesAsync();
                return Ok("Thông tin liên hệ của bạn đã được gửi đi . Chúng tôi sẽ liên hệ tới bạn ");
            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }


        }

        [HttpGet]
        [Route("ContactFooterPR")]
        public IActionResult ContactFooterPR(string Hoten, string sodienthoai, string diachi, string namsinh, string cth, string tdh)
        {
            try
            {
                ESHOP_CONTACT objcart = new ESHOP_CONTACT()
                {
                    CONTACT_PUBLISHDATE = DateTime.Now,
                    CONTACT_NAME = Hoten,
                    CONTACT_PHONE = sodienthoai,
                    CONTACT_SHOWTYPE = 1,
                    CONTACT_TITLE = "Thông tin liên hệ từ bài viết",
                    CONTACT_ADDRESS = diachi,
                    CONTACT_EMAIL = cth,
                    CONTACT_CONTENT = namsinh,
                    CONTACT_ATT1 = tdh,
                };
                _context.Add(objcart);
                _context.SaveChangesAsync();
                return Ok("Thông tin liên hệ của bạn đã được gửi đi . Chúng tôi sẽ liên hệ tới bạn ");
            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }


        }

        [HttpGet]
        [Route("ContactDangKy1")]
        public async Task<IActionResult> ContactDangKy1(string Hoten, string sodienthoai, string email, string tieude, string noidung, string linkurl)
        {
            try
            {
                int id = 1;

                ESHOP_CONTACT objcart = new ESHOP_CONTACT()
                {
                    CONTACT_PUBLISHDATE = DateTime.Now,
                    CONTACT_NAME = Hoten,
                    CONTACT_PHONE = sodienthoai,
                    CONTACT_SHOWTYPE = 1,
                    CONTACT_TITLE = tieude,
                    CONTACT_ADDRESS = email,
                    CONTACT_EMAIL = email,
                    CONTACT_CONTENT = noidung,
                    CONTACT_ATT1 = linkurl,
                };
                _context.Add(objcart);
                await _context.SaveChangesAsync();

                var eSHOP_CONFIG = await _context.ESHOP_CONFIG.SingleOrDefaultAsync(m => m.CONFIG_ID == id);
                if (eSHOP_CONFIG == null)
                {
                    return Ok("Bạn chưa cấu hình thông tin . Vui lòng cấu hình trước khi sử dụng chức năng gửi email");
                }
                else
                {
                    string emailgui = "";
                    string emailSend = "";
                    var ListEmail = _context.ESHOP_EMAIL.SingleOrDefault(x => x.EMAIL_ID == 2);
                    if (ListEmail != null)
                    {
                        emailgui = ListEmail.EMAIL_TO;
                        emailSend = ListEmail.EMAIL_CC;
                    }

                    string Body = EmailDangKyNews(Hoten, noidung, email, tieude, sodienthoai);
                    string BodyAdmin = EmailDangKyNewsAdmin(Hoten, noidung, email, tieude, sodienthoai, linkurl);
                    SendEmailSMTPAdmin(Hoten + "gửi thông tin liên hệ tới EUREKA", eSHOP_CONFIG.CONFIG_EMAIL, eSHOP_CONFIG.CONFIG_PASSWORD, tieude, email, sodienthoai, emailgui, emailSend, BodyAdmin, true, true, eSHOP_CONFIG.CONFIG_PORT, eSHOP_CONFIG.CONFIG_SMTP);
                    SendEmailSMTP("Thông tin liên hệ từ EUREKA", eSHOP_CONFIG.CONFIG_EMAIL, eSHOP_CONFIG.CONFIG_PASSWORD, tieude, email, sodienthoai, emailgui, emailSend, Body, true, true, eSHOP_CONFIG.CONFIG_PORT, eSHOP_CONFIG.CONFIG_SMTP);
                    return Ok("Thông tin liên hệ của bạn đã được gửi đi . Chúng tôi sẽ liên hệ tới bạn ");
                }


            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }


        }

        [HttpGet]
        [Route("ContactLienHe")]
        public async Task<IActionResult> ContactLienHe(string Hoten, string sodienthoai, string email, string tieude, string noidung)
        {
            try
            {
                int id = 1;

                ESHOP_CONTACT objcart = new ESHOP_CONTACT()
                {
                    CONTACT_PUBLISHDATE = DateTime.Now,
                    CONTACT_NAME = Hoten,
                    CONTACT_PHONE = sodienthoai,
                    CONTACT_SHOWTYPE = 1,
                    CONTACT_TITLE = tieude,
                    CONTACT_ADDRESS = email,
                    CONTACT_EMAIL = email,
                    CONTACT_CONTENT = noidung,
                };
                _context.Add(objcart);
                await _context.SaveChangesAsync();

                var eSHOP_CONFIG = await _context.ESHOP_CONFIG.SingleOrDefaultAsync(m => m.CONFIG_ID == id);
                if (eSHOP_CONFIG == null)
                {
                    return Ok("Bạn chưa cấu hình thông tin . Vui lòng cấu hình trước khi sử dụng chức năng gửi email");
                }
                else
                {
                    string emailgui = "";
                    string emailSend = "";
                    var ListEmail = _context.ESHOP_EMAIL.SingleOrDefault(x => x.EMAIL_ID == 2);
                    if (ListEmail != null)
                    {
                        emailgui = ListEmail.EMAIL_TO;
                        emailSend = ListEmail.EMAIL_CC;
                    }

                    string Body = EmailDangKyNews(Hoten, noidung, email, tieude, sodienthoai);
                    string BodyAdmin = EmailDangKyNewsAdmin(Hoten, noidung, email, tieude, sodienthoai, "");
                    SendEmailSMTPAdmin(Hoten + " gửi thông tin liên hệ tới EUREKA", eSHOP_CONFIG.CONFIG_EMAIL, eSHOP_CONFIG.CONFIG_PASSWORD, tieude, email, sodienthoai, emailgui, emailSend, BodyAdmin, true, true, eSHOP_CONFIG.CONFIG_PORT, eSHOP_CONFIG.CONFIG_SMTP);
                    SendEmailSMTP("Thông tin liên hệ từ EUREKA", eSHOP_CONFIG.CONFIG_EMAIL, eSHOP_CONFIG.CONFIG_PASSWORD, tieude, email, sodienthoai, emailgui, emailSend, Body, true, true, eSHOP_CONFIG.CONFIG_PORT, eSHOP_CONFIG.CONFIG_SMTP);
                    return Ok("Thông tin liên hệ của bạn đã được gửi đi . Chúng tôi sẽ liên hệ tới bạn ");
                }

            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }


        }

        public void SendEmailSMTP(string EmailDisplay, string EmailSend, string emailPass, string strSubject, string toAddress, string sodt, string ccAddress, string bccAddress, string body, bool isHtml, bool isSSL, string port, string sv)
        {
            try
            {
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress(Utils.CStrDef(EmailSend), Utils.CStrDef(EmailDisplay));
                //    //mail.To.Add(toAddress);
                //    if (ccAddress != "")
                //    {
                //        mail.CC.Add(ccAddress);
                //    }

                //    mail.Subject = strSubject;

                //    string str = "<html>" + body + "</html>";
                //    mail.Body = str;
                //    mail.IsBodyHtml = isHtml;
                //    SmtpClient client = new SmtpClient();

                //    client.EnableSsl = isSSL;
                //    client.Host = Utils.CStrDef("smtp.gmail.com");
                //    client.Port = Utils.CIntDef("587");

                //    //client.Credentials = new System.Net.NetworkCredential(Utils.CStrDef(EmailSend), Utils.CStrDef(emailPass));
                //    client.Credentials = new System.Net.NetworkCredential(EmailSend, emailPass);
                //    client.Send(mail);
                //}
                MailMessage mail = new MailMessage();
                mail.To.Add(toAddress);
                //if (ccAddress != "")
                //{
                //    mail.CC.Add(ccAddress);
                //}
                //if (bccAddress != "")
                //{
                //    mail.Bcc.Add(bccAddress);
                //}

                mail.From = new MailAddress(EmailSend);
                mail.Subject = EmailDisplay;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EmailSend, emailPass);
                smtp.Send(mail);
            }
            catch (SmtpException ex)
            {

            }
        }

        public void SendEmailSMTPAdmin(string EmailDisplay, string EmailSend, string emailPass, string strSubject, string toAddress, string sodt, string ccAddress, string bccAddress, string body, bool isHtml, bool isSSL, string port, string sv)
        {
            try
            {
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress(Utils.CStrDef(EmailSend), Utils.CStrDef(EmailDisplay));
                //    //mail.To.Add(toAddress);
                //    if (ccAddress != "")
                //    {
                //        mail.CC.Add(ccAddress);
                //    }
                //    if (bccAddress != "")
                //    {
                //        mail.Bcc.Add(bccAddress);
                //    }

                //    mail.Subject = strSubject;

                //    string str = "<html>" + body + "</html>";
                //    mail.Body = str;
                //    mail.IsBodyHtml = isHtml;
                //    SmtpClient client = new SmtpClient();

                //    client.EnableSsl = isSSL;
                //    client.Host = Utils.CStrDef("smtp.gmail.com");
                //    client.Port = Utils.CIntDef("587");

                //    //client.Credentials = new System.Net.NetworkCredential(Utils.CStrDef(EmailSend), Utils.CStrDef(emailPass));
                //    client.Credentials = new System.Net.NetworkCredential(EmailSend, emailPass);
                //    client.Send(mail);
                //}
                MailMessage mail = new MailMessage();
                //mail.To.Add(toAddress);
                if (ccAddress != "")
                {
                    mail.To.Add(ccAddress);
                }
                if (bccAddress != "")
                {
                    mail.CC.Add(bccAddress);
                }

                mail.From = new MailAddress(EmailSend);
                mail.Subject = EmailDisplay;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EmailSend, emailPass);
                smtp.Send(mail);
            }
            catch (SmtpException ex)
            {

            }
        }

        public string EmailDangKy(string hoten, string noidung, string email, string tieude, string sodienthoai)
        {
            string src = "";
            src += "<table border = '0' cellpadding = '0' cellspacing = '0' height = '100%' width = '100%'><tbody><tr><td align='center' valign='top'>";
            src += " <table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_container' style='background-color:#ffffff;border:1px solid #dedede;border-radius:3px!important'>";
            src += "<tbody><tr>";
            src += "<td align='center' valign='top'>	";
            src += "<table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_header' style='background-color:#96588a;border-radius:3px 3px 0 0!important;color:#ffffff;border-bottom:0;font-weight:bold;line-height:100%;vertical-align:middle;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif'>";
            src += "<tbody><tr>";
            src += "<td id='m_1851103133717073586header_wrapper' style='padding:36px 48px;display:block'>";
            src += "<h1 style='color:#ffffff;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:30px;font-weight:300;line-height:150%;margin:0;text-align:left'>THÔNG TIN LIÊN HỆ</h1>";
            src += "</td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_body'>";
            src += "<tbody><tr><td valign='top' id='m_1851103133717073586body_content' style='background-color:#ffffff'>";
            src += "<table border='0' cellpadding='20' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td valign='top' style='padding:48px 48px 0'>";
            src += "<div id='m_1851103133717073586body_content_inner' style='color:#636363;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:14px;line-height:150%;text-align:left'>";
            src += "<p style='margin:0 0 16px'>Cảm ơn bạn đã đăng ký nhận thông tin dự án từ website chúng tôi thông tin đăng ký:</p>";
            //src += "<p style='margin:0 0 16px'>Họ và tên :" + hoten + " </p>";
            //src += "<p style='margin:0 0 16px'>Địa chỉ :" + email + " </p>";
            src += "<p style='margin:0 0 16px'>Số điện thoại  :" + sodienthoai + " </p>";
            src += "<p style='margin:0 0 16px'>Đăng ký về :" + tieude + " </p>";
            //src += "<p style='margin:0 0 16px'>Nội dung :" + noidung + " </p>";
            src += "<p style='margin:0 0 16px'>Bạn có thể truy cập trang chủ của dự án để xem lại thông tin chi tiết của dự án tại đây: <a href='https://savihouse.com/' rel='nofollow' style='color:#96588a;font-weight:normal;text-decoration:underline' target='_blank'>SAVIHOUSE</a>.</p>";
            src += "</div></td></tr></tbody></table></td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'>";
            src += "<table border='0' cellpadding='10' cellspacing='0' width='600' id='m_1851103133717073586template_footer'><tbody><tr>";
            src += "<td valign='top' style='padding:0'><table border='0' cellpadding='10' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td colspan='2' valign='middle' id='m_1851103133717073586credit' style='padding:0 48px 48px 48px;border:0;color:#c09bb9;font-family:Arial;font-size:12px;line-height:125%;text-align:center'>";
            src += "<p>© 2020 SAVIHOUSE<br>";
            src += "Hotline: 0901 866 599<br>";
            src += "Địa chỉ: 173A Nguyễn Văn Trỗi, P.11, Q. Phú Nhuận, TP. Hồ Chí Minh, VN</p>";
            src += "</td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            src += "</td></tr></tbody></table>";
            return src;
        }


        public string EmailDangKyLienHe(string hoten, string noidung, string email, string tieude, string sodienthoai)
        {
            string src = "";
            src += "<table border = '0' cellpadding = '0' cellspacing = '0' height = '100%' width = '100%'><tbody><tr><td align='center' valign='top'>";
            src += " <table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_container' style='background-color:#ffffff;border:1px solid #dedede;border-radius:3px!important'>";
            src += "<tbody><tr>";
            src += "<td align='center' valign='top'>	";
            src += "<table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_header' style='background-color:#96588a;border-radius:3px 3px 0 0!important;color:#ffffff;border-bottom:0;font-weight:bold;line-height:100%;vertical-align:middle;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif'>";
            src += "<tbody><tr>";
            src += "<td id='m_1851103133717073586header_wrapper' style='padding:36px 48px;display:block'>";
            src += "<h1 style='color:#ffffff;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:30px;font-weight:300;line-height:150%;margin:0;text-align:left'>THÔNG TIN LIÊN HỆ</h1>";
            src += "</td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_body'>";
            src += "<tbody><tr><td valign='top' id='m_1851103133717073586body_content' style='background-color:#ffffff'>";
            src += "<table border='0' cellpadding='20' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td valign='top' style='padding:48px 48px 0'>";
            src += "<div id='m_1851103133717073586body_content_inner' style='color:#636363;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:14px;line-height:150%;text-align:left'>";
            src += "<p style='margin:0 0 16px'>Cảm ơn bạn đã đăng ký nhận thông tin dự án từ website chúng tôi thông tin đăng ký:</p>";
            src += "<p style='margin:0 0 16px'>Họ và tên :" + hoten + " </p>";
            src += "<p style='margin:0 0 16px'>Địa chỉ :" + email + " </p>";
            //src += "<p style='margin:0 0 16px'>Số điện thoại  :" + sodienthoai + " </p>";
            src += "<p style='margin:0 0 16px'>Đăng ký về :" + tieude + " </p>";
            src += "<p style='margin:0 0 16px'>Nội dung :" + noidung + " </p>";
            src += "<p style='margin:0 0 16px'>Bạn có thể truy cập trang chủ của dự án để xem lại thông tin chi tiết của dự án tại đây: <a href='https://savihouse.com/' rel='nofollow' style='color:#96588a;font-weight:normal;text-decoration:underline' target='_blank'>SAVIHOUSE</a>.</p>";
            src += "</div></td></tr></tbody></table></td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'>";
            src += "<table border='0' cellpadding='10' cellspacing='0' width='600' id='m_1851103133717073586template_footer'><tbody><tr>";
            src += "<td valign='top' style='padding:0'><table border='0' cellpadding='10' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td colspan='2' valign='middle' id='m_1851103133717073586credit' style='padding:0 48px 48px 48px;border:0;color:#c09bb9;font-family:Arial;font-size:12px;line-height:125%;text-align:center'>";
            src += "<p>© 2020 SAVIHOUSE<br>";
            src += "Hotline: 0901 866 599<br>";
            src += "Địa chỉ: 173A Nguyễn Văn Trỗi, P.11, Q. Phú Nhuận, TP. Hồ Chí Minh, VN</p>";
            src += "</td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            src += "</td></tr></tbody></table>";
            return src;
        }

        public string EmailDangKyNews(string hoten, string noidung, string email, string tieude, string sodienthoai)
        {
            string src = "";
            src += "<table border = '0' cellpadding = '0' cellspacing = '0' height = '100%' width = '100%'><tbody><tr><td align='center' valign='top'>";
            src += " <table border='0' cellpadding='0' cellspacing='0' width='780' id='m_1851103133717073586template_container' style='border:1px solid #dedede;border-radius:3px!important'>";
            src += "<tbody><tr>";
            src += "<td align='center' valign='middle'>";
            src += "<table border='0' cellpadding='0' cellspacing='0' width='780' id='m_1851103133717073586template_header' style='border-radius:3px 3px 0 0!important;color:#ffffff;border-bottom:0;font-weight:bold;line-height:100%;vertical-align:middle;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif'>";
            src += "<tbody><tr>";
            src += "<td id='m_1851103133717073586header_wrapper' style='padding:35px 0px 5px 0px;display:block' align='center' valign='middle'>";
            src += "<a href='https://hochiminhrental.com'><img src='https://hochiminhrental.com/UploadImages/Data/Banner/logo_erc.png' alt='Trang chủ - EUREKA' style='width: 230px;'></a>";
            src += "</td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='780' id='m_1851103133717073586template_body'>";
            src += "<tbody><tr><td valign='top' id='m_1851103133717073586body_content' style='background-color:#ffffff'>";
            src += "<table border='0' cellpadding='20' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td valign='top'>";
            src += "<div id='m_1851103133717073586body_content_inner' style='color:#636363;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:14px;line-height:150%;text-align:left'>";
            src += "<h1 style='margin:0 0 16px;color:#00a29a;text-align:center;font-size: 17px;font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;margin-bottom: 10px;'>Cảm ơn bạn đã liên hệ EUREKA! </h1>";
            src += "<div style='margin:0 0 16px;text-align: center;font-size: 13px;font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;'>Scroll down for English version</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>Xin chào " + hoten + " ;</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>EUREKA đã nhận được thông tin tư vấn của bạn. Chúng tôi sẽ liên lạc với bạn ngay. Trong thời gian chờ đợi bạn có thể tìm thêm các ý tưởng vể nơi ở của bạn tại website của chúng tôi nhé! </div>";
            src += "<div style='margin:0 0 16px;text-align: left;'> Hoặc để có thể trao đổi ngay với EUREKA bạn có thể liên hệ <span style='color: hsl(177deg 100% 32%);font-weight: bold;'> Hotline +84 898 303 929 </span></div>";
            src += "<a href='https://hochiminhrental.com/can-ho-dich-vu' class='btn_three' style='text-align:center;text-decoration: none; font-weight: bold;'><div style='background:linear-gradient(45deg,#00a29a 10%,#00a29a 100%);color:#fff;border-radius:10px;padding:12px 25px!important;display:block!important;text-align:center;text-transform:uppercase;min-width:140px;border-bottom:1px solid #8f8f8f;border-right:1px solid #8f8f8f;width:220px;margin: 30px auto;'>Tìm hiểu thêm căn hộ</div></a>";

            src += "<h1 style='margin:0 0 16px;color:#00a29a;text-align:center;font-size: 17px;font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;margin-bottom: 10px;'>Thank you for contacting ! </h1>";
            src += "<div style='margin:0 0 16px;text-align: left;'>Hi " + hoten + " ;</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>EUREKA has received your advisory information. We will contact you shortly. In the mean time you can find more ideas about your accommodation on our website!</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>In order to talk to EUREKA immediately, you can contact us via <span style='color: hsl(177deg 100% 32%);font-weight: bold;'> Hotline +84 898 303 929 </span></div>";
            src += "<a href='https://hochiminhrental.com/can-ho-dich-vu' class='btn_three' style='text-align:center;text-decoration: none; font-weight: bold;'><div style='background:linear-gradient(45deg,#00a29a 10%,#00a29a 100%);color:#fff;border-radius:10px;padding:12px 25px!important;display:block!important;text-align:center;text-transform:uppercase;min-width:140px;border-bottom:1px solid #8f8f8f;border-right:1px solid #8f8f8f;width:220px;margin: 30px auto;'>Find apartments for rent</div></a>";

            src += "</div></td></tr></tbody></table></td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'>";
            src += "<table border='0' cellpadding='10' cellspacing='0' width='600' id='m_1851103133717073586template_footer'><tbody><tr>";
            src += "<td valign='top' style='padding:0'><table border='0' cellpadding='10' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td colspan='2' valign='middle' id='m_1851103133717073586credit' style='padding:0 48px 48px 48px;border:0;color:#c09bb9;font-family:Arial;font-size:12px;line-height:125%;text-align:center'>";

            src += "</td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            src += "</td></tr></tbody></table>";
            return src;
        }


        public string EmailKhachHangThue(string hoten, string noidung, string email, string tieude, string sodienthoai)
        {
            string src = "";
            src += "<table border = '0' cellpadding = '0' cellspacing = '0' height = '100%' width = '100%'><tbody><tr><td align='center' valign='top'>";
            src += " <table border='0' cellpadding='0' cellspacing='0' width='780' id='m_1851103133717073586template_container' style='border:1px solid #dedede;border-radius:3px!important'>";
            src += "<tbody><tr>";
            src += "<td align='center' valign='middle'>";
            src += "<table border='0' cellpadding='0' cellspacing='0' width='780' id='m_1851103133717073586template_header' style='border-radius:3px 3px 0 0!important;color:#ffffff;border-bottom:0;font-weight:bold;line-height:100%;vertical-align:middle;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif'>";
            src += "<tbody><tr>";
            src += "<td id='m_1851103133717073586header_wrapper' style='padding:35px 0px 5px 0px;display:block' align='center' valign='middle'>";
            src += "<a href='https://hochiminhrental.com'><img src='https://hochiminhrental.com/UploadImages/Data/Banner/logo_erc.png' alt='Trang chủ - EUREKA' style='width: 230px;'></a>";
            src += "</td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='780' id='m_1851103133717073586template_body'>";
            src += "<tbody><tr><td valign='top' id='m_1851103133717073586body_content' style='background-color:#ffffff'>";
            src += "<table border='0' cellpadding='20' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td valign='top'>";
            src += "<div id='m_1851103133717073586body_content_inner' style='color:#636363;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:14px;line-height:150%;text-align:left'>";
            src += "<h1 style='margin:0 0 16px;color:#00a29a;text-align:center;font-size: 17px;font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;margin-bottom: 10px;'>Cảm ơn bạn đã liên hệ EUREKA! </h1>";
            src += "<div style='margin:0 0 16px;text-align: center;font-size: 13px;font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;'>Scroll down for English version</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>Xin chào " + hoten + " ;</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>EUREKA đã nhận được thông tin tư vấn của bạn. Chúng tôi sẽ liên lạc với bạn ngay. Trong thời gian chờ đợi bạn có thể tìm thêm các ý tưởng vể nơi ở của bạn tại website của chúng tôi nhé! </div>";
            src += "<div style='margin:0 0 16px;text-align: left;'> Hoặc để có thể trao đổi ngay với EUREKA bạn có thể liên hệ <span style='color: hsl(177deg 100% 32%);font-weight: bold;'> Hotline +84 898 303 929 </span></div>";
            src += "<a href='https://hochiminhrental.com/can-ho-dich-vu' class='btn_three' style='text-align:center;text-decoration: none; font-weight: bold;'><div style='background:linear-gradient(45deg,#00a29a 10%,#00a29a 100%);color:#fff;border-radius:10px;padding:12px 25px!important;display:block!important;text-align:center;text-transform:uppercase;min-width:140px;border-bottom:1px solid #8f8f8f;border-right:1px solid #8f8f8f;width:220px;margin: 30px auto;'>Tìm hiểu thêm căn hộ</div></a>";

            src += "<h1 style='margin:0 0 16px;color:#00a29a;text-align:center;font-size: 17px;font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;margin-bottom: 10px;'>Thank you for contacting ! </h1>";
            src += "<div style='margin:0 0 16px;text-align: left;'>Hi " + hoten + " ;</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>EUREKA has received your advisory information. We will contact you shortly. In the mean time you can find more ideas about your accommodation on our website!</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>In order to talk to EUREKA immediately, you can contact us via <span style='color: hsl(177deg 100% 32%);font-weight: bold;'> Hotline +84 898 303 929 </span></div>";
            src += "<a href='https://hochiminhrental.com/can-ho-dich-vu' class='btn_three' style='text-align:center;text-decoration: none; font-weight: bold;'><div style='background:linear-gradient(45deg,#00a29a 10%,#00a29a 100%);color:#fff;border-radius:10px;padding:12px 25px!important;display:block!important;text-align:center;text-transform:uppercase;min-width:140px;border-bottom:1px solid #8f8f8f;border-right:1px solid #8f8f8f;width:220px;margin: 30px auto;'>Find apartments for rent</div></a>";

            src += "</div></td></tr></tbody></table></td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'>";
            src += "<table border='0' cellpadding='10' cellspacing='0' width='600' id='m_1851103133717073586template_footer'><tbody><tr>";
            src += "<td valign='top' style='padding:0'><table border='0' cellpadding='10' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td colspan='2' valign='middle' id='m_1851103133717073586credit' style='padding:0 48px 48px 48px;border:0;color:#c09bb9;font-family:Arial;font-size:12px;line-height:125%;text-align:center'>";

            src += "</td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            src += "</td></tr></tbody></table>";
            return src;
        }

        public string EmailDkTaiKhoan(string taikhoan, string mk, string hoten, string email, string sodienthoai)
        {
            string src = "";
            src += "<table border = '0' cellpadding = '0' cellspacing = '0' height = '100%' width = '100%'><tbody><tr><td align='center' valign='top'>";
            src += " <table border='0' cellpadding='0' cellspacing='0' width='780' id='m_1851103133717073586template_container' style='border:1px solid #dedede;border-radius:3px!important'>";
            src += "<tbody><tr>";
            src += "<td align='center' valign='middle'>";
            src += "<table border='0' cellpadding='0' cellspacing='0' width='780' id='m_1851103133717073586template_header' style='border-radius:3px 3px 0 0!important;color:#ffffff;border-bottom:0;font-weight:bold;line-height:100%;vertical-align:middle;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif'>";
            src += "<tbody><tr>";
            src += "<td id='m_1851103133717073586header_wrapper' style='padding:35px 0px 5px 0px;display:block' align='center' valign='middle'>";
            src += "<a href='https://hochiminhrental.com'><img src='https://hochiminhrental.com/UploadImages/Data/Banner/logo_erc.png' alt='Trang chủ - EUREKA' style='width: 230px;'></a>";
            src += "</td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='780' id='m_1851103133717073586template_body'>";
            src += "<tbody><tr><td valign='top' id='m_1851103133717073586body_content' style='background-color:#ffffff'>";
            src += "<table border='0' cellpadding='20' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td valign='top'>";
            src += "<div id='m_1851103133717073586body_content_inner' style='color:#636363;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:14px;line-height:150%;text-align:left'>";
            src += "<h1 style='margin:0 0 16px;color:#00a29a;text-align:center;font-size: 17px;font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;margin-bottom: 10px;'>Cảm ơn bạn đã liên hệ EUREKA! </h1>";
            src += "<div style='margin:0 0 16px;text-align: center;font-size: 13px;font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;'>Scroll down for English version</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>Xin chào " + hoten + " ;</div>";
            src += "<p style='margin:0 0 16px'>Tài khoản đăng nhập  :" + taikhoan + " </p>";
            src += "<p style='margin:0 0 16px'>Mật khẩu :" + mk + " </p>";
            src += "<p style='margin:0 0 16px'>Họ và tên  :" + hoten + " </p>";
            src += "<p style='margin:0 0 16px'>Email :" + email + " </p>";
            src += "<div style='margin:0 0 16px;text-align: left;'>Cảm ơn bạn đã đăng ký tài khoản tại EUREKA. Chúng tôi sẽ liên lạc với bạn ngay. Trong thời gian chờ đợi bạn có thể tìm thêm các ý tưởng vể nơi ở của bạn tại website của chúng tôi nhé! </div>";
            src += "<div style='margin:0 0 16px;text-align: left;'> Hoặc để có thể trao đổi ngay với EUREKA bạn có thể liên hệ <span style='color: hsl(177deg 100% 32%);font-weight: bold;'> Hotline +84 898 303 929 </span></div>";
            src += "<a href='https://hochiminhrental.com/can-ho-dich-vu' class='btn_three' style='text-align:center;text-decoration: none; font-weight: bold;'><div style='background:linear-gradient(45deg,#00a29a 10%,#00a29a 100%);color:#fff;border-radius:10px;padding:12px 25px!important;display:block!important;text-align:center;text-transform:uppercase;min-width:140px;border-bottom:1px solid #8f8f8f;border-right:1px solid #8f8f8f;width:220px;margin: 30px auto;'>Tìm hiểu thêm căn hộ</div></a>";

            src += "<h1 style='margin:0 0 16px;color:#00a29a;text-align:center;font-size: 17px;font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;margin-bottom: 10px;'>Thank you for contacting ! </h1>";
            src += "<div style='margin:0 0 16px;text-align: left;'>Hi " + hoten + " ;</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>EUREKA has received your advisory information. We will contact you shortly. In the mean time you can find more ideas about your accommodation on our website!</div>";
            src += "<div style='margin:0 0 16px;text-align: left;'>In order to talk to EUREKA immediately, you can contact us via <span style='color: hsl(177deg 100% 32%);font-weight: bold;'> Hotline +84 898 303 929 </span></div>";
            src += "<a href='https://hochiminhrental.com/can-ho-dich-vu' class='btn_three' style='text-align:center;text-decoration: none; font-weight: bold;'><div style='background:linear-gradient(45deg,#00a29a 10%,#00a29a 100%);color:#fff;border-radius:10px;padding:12px 25px!important;display:block!important;text-align:center;text-transform:uppercase;min-width:140px;border-bottom:1px solid #8f8f8f;border-right:1px solid #8f8f8f;width:220px;margin: 30px auto;'>Find apartments for rent</div></a>";

            src += "</div></td></tr></tbody></table></td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'>";
            src += "<table border='0' cellpadding='10' cellspacing='0' width='600' id='m_1851103133717073586template_footer'><tbody><tr>";
            src += "<td valign='top' style='padding:0'><table border='0' cellpadding='10' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td colspan='2' valign='middle' id='m_1851103133717073586credit' style='padding:0 48px 48px 48px;border:0;color:#c09bb9;font-family:Arial;font-size:12px;line-height:125%;text-align:center'>";

            src += "</td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            src += "</td></tr></tbody></table>";
            return src;
        }

        public string EmailDangKyNewsAdmin(string hoten, string noidung, string email, string tieude, string sodienthoai, string linkurl)
        {
            string src = "";
            src += "<p>Thông tin đăng ký của khách hàng</p>";
            src += "<p>Họ và tên : " + hoten + "</p>";
            src += "<p>Số điện thoại : " + sodienthoai + "</p>";
            src += "<p>Email : " + email + "</p>";
            src += "<p>Thông tin quan tâm : " + linkurl + "</p>";
            src += "<p>Nội dung liên hệ : " + noidung + "</p>";
            return src;
        }


        public string EmailDangKyThueAdmin(string Hoten, string sodienthoai, string email, string quoctich, string duan, string phongngu, string phongbep, string dientich, string ngansach, string thoigiandonvao, string thoihanthu, string mota, string check, string img)
        {
            string src = "";
            src += "<p>Thông tin gửi yêu cầu thuê của khách hàng</p>";
            src += "<p>Họ và tên : " + Hoten + "</p>";
            src += "<p>Số điện thoại : " + sodienthoai + "</p>";
            src += "<p>Email : " + email + "</p>";
            src += "<p>Quốc tịch : " + quoctich + "</p>";
            src += "<p>Dự án : " + duan + "</p>";
            src += "<p>Phòng ngủ : " + phongngu + "</p>";
            src += "<p>Phòng bếp : " + phongbep + "</p>";
            src += "<p>Diện tích : " + dientich + "</p>";
            src += "<p>Ngân sách : " + ngansach + "</p>";
            src += "<p>Thời gian dọn vào : " + thoigiandonvao + "</p>";
            src += "<p>Thời gian thuê : " + thoihanthu + "</p>";
            src += "<p>Nội dung liên hệ : " + mota + "</p>";
            return src;
        }

        [HttpGet]
        [Route("ChangeLangue")]
        public IActionResult ChangeLangue(string IdName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
                {
                    HttpContext.Session.SetString(SessionKeyName1, IdName);
                }
                else
                {
                    if (Utils.CIntDef(IdName) != Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1)))
                    {
                        HttpContext.Session.SetString(SessionKeyName1, IdName);
                    }
                    else
                    {

                    }
                }

                //Set("langue", IdName, 100);
                //CookieOptions option = new CookieOptions();
                //option.Expires = DateTime.Now.AddDays(1);
                //option.HttpOnly = false;
                //option.Domain = Request.Host.ToUriComponent();
                //option.Path = "/";
                //HttpContext.Response.Cookies.Append("langue", IdName, option);
                return Ok("OKENGONNGU");
            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }
        }

        [HttpGet]
        [Route("ChangeMoney")]
        public IActionResult ChangeMoney(string IdName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
                {
                    HttpContext.Session.SetString(SessionKeyName, IdName);
                }
                else
                {
                    if (Utils.CIntDef(IdName) != Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName)))
                    {
                        HttpContext.Session.SetString(SessionKeyName, IdName);
                    }
                    else
                    {

                    }
                }

                //Set("langue", IdName, 100);
                //CookieOptions option = new CookieOptions();
                //option.Expires = DateTime.Now.AddDays(1);
                //option.HttpOnly = false;
                //option.Domain = Request.Host.ToUriComponent();
                //option.Path = "/";
                //HttpContext.Response.Cookies.Append("langue", IdName, option);
                return Ok("OKENGONNGU");
            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetSapXep")]
        public IActionResult GetSapXep(string IdName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName2)))
                {
                    HttpContext.Session.SetString(SessionKeyName2, IdName);
                }
                else
                {
                    if (Utils.CIntDef(IdName) != Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName2)))
                    {
                        HttpContext.Session.SetString(SessionKeyName2, IdName);
                    }
                    else
                    {

                    }
                }

                //Set("langue", IdName, 100);
                //CookieOptions option = new CookieOptions();
                //option.Expires = DateTime.Now.AddDays(1);
                //option.HttpOnly = false;
                //option.Domain = Request.Host.ToUriComponent();
                //option.Path = "/";
                //HttpContext.Response.Cookies.Append("langue", IdName, option);
                return Ok("OKESAPXEP");
            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }
        }

        [HttpGet]
        [Route("ChangTypePrice")]
        public IActionResult ChangTypePrice(string IdName)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName3)))
                {
                    HttpContext.Session.SetString(SessionKeyName3, IdName);
                }
                else
                {
                    if (Utils.CIntDef(IdName) != Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName3)))
                    {
                        HttpContext.Session.SetString(SessionKeyName3, IdName);
                    }
                    else
                    {

                    }
                }

                //Set("langue", IdName, 100);
                //CookieOptions option = new CookieOptions();
                //option.Expires = DateTime.Now.AddDays(1);
                //option.HttpOnly = false;
                //option.Domain = Request.Host.ToUriComponent();
                //option.Path = "/";
                //HttpContext.Response.Cookies.Append("langue", IdName, option);
                return Ok("OKETYPE");
            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }
        }

        [HttpGet]
        [Route("ContactEmailKM")]
        public async Task<IActionResult> ContactEmailKM(string Hoten, string email, string tieude, string noidung)
        {
            try
            {
                int id = 1;

                ESHOP_CONTACT objcart = new ESHOP_CONTACT()
                {
                    CONTACT_PUBLISHDATE = DateTime.Now,
                    CONTACT_NAME = Hoten,
                    CONTACT_PHONE = "",
                    CONTACT_SHOWTYPE = 1,
                    CONTACT_TITLE = tieude,
                    CONTACT_ADDRESS = email,
                    CONTACT_EMAIL = email,
                    CONTACT_CONTENT = noidung,
                };
                _context.Add(objcart);
                await _context.SaveChangesAsync();

                return Ok("Thông tin liên hệ của bạn đã được gửi đi . Chúng tôi sẽ liên hệ tới bạn ");

            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }


        }


        [HttpGet]
        [Route("SaveLike")]
        public IActionResult SaveLike(int IdNews)
        {
            try
            {
                var eSHOP_CATEGORIES = (from c in _context.ESHOP_CATEGORIES
                                        join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                                        join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                                        where n.NEWS_ID == IdNews
                                        select new
                                        {
                                            n.NEWS_ID,
                                            n.NEWS_CODE,
                                            n.NEWS_TITLE,
                                            n.NEWS_DESC,
                                            n.NEWS_TARGET,
                                            n.NEWS_SEO_KEYWORD,
                                            n.NEWS_SEO_DESC,
                                            n.NEWS_SEO_TITLE,
                                            n.NEWS_SEO_URL,
                                            n.NEWS_FILEHTML,
                                            n.NEWS_PUBLISHDATE,
                                            n.NEWS_UPDATE,
                                            n.NEWS_SHOWTYPE,
                                            n.NEWS_SHOWINDETAIL,
                                            n.NEWS_FEEDBACKTYPE,
                                            n.NEWS_TYPE,
                                            n.NEWS_PERIOD,
                                            n.NEWS_ORDER_PERIOD,
                                            n.NEWS_ORDER,
                                            n.NEWS_PRICE1,
                                            n.NEWS_PRICE2,
                                            n.NEWS_IMAGE1,
                                            n.NEWS_IMAGE2,
                                            n.NEWS_TITLE_EN,
                                            n.NEWS_DESC_EN,
                                            n.NEWS_URL,
                                            c.CAT_SEO_URL,
                                            n.NEWS_FILEHTML_EN,
                                            n.NEWS_HTML_EN1,
                                            n.NEWS_HTML_EN2,
                                            n.NEWS_HTML_EN3,
                                            n.NEWS_FIELD4,
                                            n.NEWS_TITLE_JS,
                                            n.NEWS_SEO_URL_EN,
                                            c.CAT_SEO_URL_EN
                                        }
                                                 );
                if (SessionHelper.GetObjectFromJson<List<NewsModelCat>>(HttpContext.Session, "cart") == null)
                {
                    List<NewsModelCat> NewsCat = new List<NewsModelCat>();
                    NewsCat.Add(new NewsModelCat
                    {

                        NEWS_ID = eSHOP_CATEGORIES.ToList()[0].NEWS_ID,
                        NEWS_CODE = eSHOP_CATEGORIES.ToList()[0].NEWS_CODE,
                        NEWS_TITLE = eSHOP_CATEGORIES.ToList()[0].NEWS_TITLE,
                        NEWS_TITLE_EN = eSHOP_CATEGORIES.ToList()[0].NEWS_TITLE_EN,
                        NEWS_DESC_EN = eSHOP_CATEGORIES.ToList()[0].NEWS_DESC_EN,
                        NEWS_DESC = eSHOP_CATEGORIES.ToList()[0].NEWS_DESC,
                        NEWS_URL = eSHOP_CATEGORIES.ToList()[0].NEWS_URL,
                        NEWS_TARGET = eSHOP_CATEGORIES.ToList()[0].NEWS_TARGET,
                        NEWS_SEO_KEYWORD = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_KEYWORD,
                        NEWS_SEO_DESC = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_DESC,
                        NEWS_SEO_TITLE = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_TITLE,
                        NEWS_SEO_URL = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_URL,
                        NEWS_FILEHTML = eSHOP_CATEGORIES.ToList()[0].NEWS_FILEHTML,
                        NEWS_PUBLISHDATE = eSHOP_CATEGORIES.ToList()[0].NEWS_PUBLISHDATE,
                        NEWS_UPDATE = eSHOP_CATEGORIES.ToList()[0].NEWS_UPDATE,
                        NEWS_SHOWTYPE = eSHOP_CATEGORIES.ToList()[0].NEWS_SHOWTYPE,
                        NEWS_SHOWINDETAIL = eSHOP_CATEGORIES.ToList()[0].NEWS_SHOWINDETAIL,
                        NEWS_FEEDBACKTYPE = eSHOP_CATEGORIES.ToList()[0].NEWS_FEEDBACKTYPE,
                        NEWS_TYPE = eSHOP_CATEGORIES.ToList()[0].NEWS_TYPE,
                        NEWS_PERIOD = eSHOP_CATEGORIES.ToList()[0].NEWS_PERIOD,
                        NEWS_ORDER_PERIOD = eSHOP_CATEGORIES.ToList()[0].NEWS_ORDER_PERIOD,
                        NEWS_ORDER = eSHOP_CATEGORIES.ToList()[0].NEWS_ORDER,
                        NEWS_PRICE1 = eSHOP_CATEGORIES.ToList()[0].NEWS_PRICE1,
                        NEWS_PRICE2 = eSHOP_CATEGORIES.ToList()[0].NEWS_PRICE2,
                        NEWS_IMAGE1 = eSHOP_CATEGORIES.ToList()[0].NEWS_IMAGE1,
                        NEWS_IMAGE2 = eSHOP_CATEGORIES.ToList()[0].NEWS_IMAGE2,
                        CAT_SEO_URL = eSHOP_CATEGORIES.ToList()[0].CAT_SEO_URL,
                        NEWS_FILEHTML_EN = eSHOP_CATEGORIES.ToList()[0].NEWS_FILEHTML_EN,
                        NEWS_HTML_EN1 = eSHOP_CATEGORIES.ToList()[0].NEWS_HTML_EN1,
                        NEWS_HTML_EN2 = eSHOP_CATEGORIES.ToList()[0].NEWS_HTML_EN2,
                        NEWS_HTML_EN3 = eSHOP_CATEGORIES.ToList()[0].NEWS_HTML_EN3,
                        NEWS_FIELD4 = eSHOP_CATEGORIES.ToList()[0].NEWS_FIELD4,
                        NEWS_TITLE_JS = eSHOP_CATEGORIES.ToList()[0].NEWS_TITLE_JS,
                        NEWS_SEO_URL_EN = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_URL_EN,
                        CAT_SEO_URL_EN = eSHOP_CATEGORIES.ToList()[0].CAT_SEO_URL_EN
                        //CAT_SEO_URL = ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + eSHOP_CATEGORIES.NEWS_SEO_URL,
                    });
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", NewsCat);
                }
                else
                {
                    List<NewsModelCat> NewsCat = SessionHelper.GetObjectFromJson<List<NewsModelCat>>(HttpContext.Session, "cart");
                    int index = isExist(Utils.CIntDef(eSHOP_CATEGORIES.ToList()[0].NEWS_ID));
                    if (index != -1)
                    {

                    }
                    else
                    {
                        NewsCat.Add(new NewsModelCat
                        {

                            NEWS_ID = eSHOP_CATEGORIES.ToList()[0].NEWS_ID,
                            NEWS_CODE = eSHOP_CATEGORIES.ToList()[0].NEWS_CODE,
                            NEWS_TITLE = eSHOP_CATEGORIES.ToList()[0].NEWS_TITLE,
                            NEWS_TITLE_EN = eSHOP_CATEGORIES.ToList()[0].NEWS_TITLE_EN,
                            NEWS_DESC_EN = eSHOP_CATEGORIES.ToList()[0].NEWS_DESC_EN,
                            NEWS_DESC = eSHOP_CATEGORIES.ToList()[0].NEWS_DESC,
                            NEWS_URL = eSHOP_CATEGORIES.ToList()[0].NEWS_URL,
                            NEWS_TARGET = eSHOP_CATEGORIES.ToList()[0].NEWS_TARGET,
                            NEWS_SEO_KEYWORD = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_KEYWORD,
                            NEWS_SEO_DESC = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_DESC,
                            NEWS_SEO_TITLE = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_TITLE,
                            NEWS_SEO_URL = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_URL,
                            NEWS_FILEHTML = eSHOP_CATEGORIES.ToList()[0].NEWS_FILEHTML,
                            NEWS_PUBLISHDATE = eSHOP_CATEGORIES.ToList()[0].NEWS_PUBLISHDATE,
                            NEWS_UPDATE = eSHOP_CATEGORIES.ToList()[0].NEWS_UPDATE,
                            NEWS_SHOWTYPE = eSHOP_CATEGORIES.ToList()[0].NEWS_SHOWTYPE,
                            NEWS_SHOWINDETAIL = eSHOP_CATEGORIES.ToList()[0].NEWS_SHOWINDETAIL,
                            NEWS_FEEDBACKTYPE = eSHOP_CATEGORIES.ToList()[0].NEWS_FEEDBACKTYPE,
                            NEWS_TYPE = eSHOP_CATEGORIES.ToList()[0].NEWS_TYPE,
                            NEWS_PERIOD = eSHOP_CATEGORIES.ToList()[0].NEWS_PERIOD,
                            NEWS_ORDER_PERIOD = eSHOP_CATEGORIES.ToList()[0].NEWS_ORDER_PERIOD,
                            NEWS_ORDER = eSHOP_CATEGORIES.ToList()[0].NEWS_ORDER,
                            NEWS_PRICE1 = eSHOP_CATEGORIES.ToList()[0].NEWS_PRICE1,
                            NEWS_PRICE2 = eSHOP_CATEGORIES.ToList()[0].NEWS_PRICE2,
                            NEWS_IMAGE1 = eSHOP_CATEGORIES.ToList()[0].NEWS_IMAGE1,
                            NEWS_IMAGE2 = eSHOP_CATEGORIES.ToList()[0].NEWS_IMAGE2,
                            CAT_SEO_URL = eSHOP_CATEGORIES.ToList()[0].CAT_SEO_URL,
                            NEWS_FILEHTML_EN = eSHOP_CATEGORIES.ToList()[0].NEWS_FILEHTML_EN,
                            NEWS_HTML_EN1 = eSHOP_CATEGORIES.ToList()[0].NEWS_HTML_EN1,
                            NEWS_HTML_EN2 = eSHOP_CATEGORIES.ToList()[0].NEWS_HTML_EN2,
                            NEWS_HTML_EN3 = eSHOP_CATEGORIES.ToList()[0].NEWS_HTML_EN3,
                            NEWS_FIELD4 = eSHOP_CATEGORIES.ToList()[0].NEWS_FIELD4,
                            NEWS_TITLE_JS = eSHOP_CATEGORIES.ToList()[0].NEWS_TITLE_JS,
                            NEWS_SEO_URL_EN = eSHOP_CATEGORIES.ToList()[0].NEWS_SEO_URL_EN,
                            CAT_SEO_URL_EN = eSHOP_CATEGORIES.ToList()[0].CAT_SEO_URL_EN
                            //CAT_SEO_URL = ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + eSHOP_CATEGORIES.NEWS_SEO_URL,
                        });
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", NewsCat);
                }
                return Ok("OKE");
            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = ex.Message });
            }
        }


        private int isExist(int id)
        {
            List<NewsCatModel> cart = SessionHelper.GetObjectFromJson<List<NewsCatModel>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (Utils.CIntDef(cart[i].NEWS_ID) == id)
                {
                    return i;
                }
            }
            return -1;
        }


        //Đăng ký 

        [HttpGet]
        [Route("PostNewsCus")]
        public async Task<IActionResult> PostNewsCus(string Hoten, string sodienthoai, string diachiemail, string quoctich, string duan, string phongngu, string phongbep, string dientich, string ngansach, string thoigiandonvao, string thoihanthu, string mota, string check, string img)
        {
            try
            {
                int id = 1;
                string mk = "";
                string bodydk = "";
                int cookieValueId = Utils.CIntDef(Request.Cookies["CustomerID"]);
                Customer_Request objcart = new Customer_Request()
                {
                    Customer_PublishDate = DateTime.Now,
                    Customer_Name = Hoten,
                    Customer_Phone = sodienthoai,
                    Customer_Active = 0,
                    Customer_Desc = mota,
                    Customer_Email = diachiemail,
                    Customer_DuAn = duan,
                    Customer_Check = Utils.CIntDef(check),
                    Customer_Image = img,
                    Customer_Quoctich = quoctich,
                    Customer_PN = Utils.CIntDef(phongngu),
                    Customer_PB = Utils.CIntDef(phongbep),
                    Customer_Dt = dientich,
                    Customer_Ns = ngansach,
                    Customer_TimeAv = Utils.CDateDef(thoigiandonvao, DateTime.Now),
                    Customer_Request_Order = 1,
                    Customer_TimeThue = Utils.CIntDef(thoihanthu),
                    CUSTOMER_ID = cookieValueId
                };

                _context.Add(objcart);
                await _context.SaveChangesAsync();

                if (Utils.CIntDef(check) == 1)
                {
                    var LogResult = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_UN == diachiemail);
                    if (LogResult != null)
                    {
                        ViewBag.Error = "Tài khoản Email đã được đăng ký";
                        ViewBag.Display = "display:block";
                        return Json(new { Result = false, Message = "Tài khoản Email đã được đăng ký" });
                    }
                    else
                    {
                        mk = fm.TaoChuoiTuDong(10);

                        ESHOP_CUSTOMER cs = new ESHOP_CUSTOMER();
                        cs.CUSTOMER_PW = fm.Encrypt(mk);
                        cs.CUSTOMER_UN = diachiemail;
                        cs.CUSTOMER_FULLNAME = Hoten.Trim();
                        cs.CUSTOMER_FIELD1 = diachiemail.Trim();
                        cs.CUSTOMER_PHONE1 = sodienthoai.Trim();
                        cs.CUSTOMER_PUBLISHDATE = DateTime.Now;
                        _context.Add(cs);
                        await _context.SaveChangesAsync();

                        bodydk = EmailDkTaiKhoan(diachiemail, mk, Hoten, diachiemail, sodienthoai);

                    }
                }

                var eSHOP_CONFIG = await _context.ESHOP_CONFIG.SingleOrDefaultAsync(m => m.CONFIG_ID == id);
                if (eSHOP_CONFIG == null)
                {
                    return Ok(new { status = false, response = "Bạn chưa cấu hình thông tin . Vui lòng cấu hình trước khi sử dụng chức năng gửi email" });
                }
                else
                {
                    string emailgui = "";
                    string emailSend = "";
                    var ListEmail = _context.ESHOP_EMAIL.SingleOrDefault(x => x.EMAIL_ID == 2);
                    if (ListEmail != null)
                    {
                        emailgui = ListEmail.EMAIL_TO;
                        emailSend = ListEmail.EMAIL_CC;
                    }

                    string Body = EmailKhachHangThue(Hoten, mota, diachiemail, "Gửi yêu cầu thuê nhà", sodienthoai);
                    string BodyAdmin = EmailDangKyThueAdmin(Hoten, sodienthoai, diachiemail, quoctich, duan, phongngu, phongbep, dientich, ngansach, thoigiandonvao, thoihanthu, mota, check, img);

                    SendEmailSMTPAdmin(Hoten + "gửi thông tin yêu cầu thuê tới EUREKA", eSHOP_CONFIG.CONFIG_EMAIL, eSHOP_CONFIG.CONFIG_PASSWORD, "Gửi yêu cầu thuê nhà", diachiemail, sodienthoai, emailgui, emailSend, BodyAdmin, true, true, eSHOP_CONFIG.CONFIG_PORT, eSHOP_CONFIG.CONFIG_SMTP);

                    SendEmailSMTP("Thông tin gửi yêu cầu từ EUREKA", eSHOP_CONFIG.CONFIG_EMAIL, eSHOP_CONFIG.CONFIG_PASSWORD, "Gửi yêu cầu thuê nhà", diachiemail, sodienthoai, emailgui, emailSend, Body, true, true, eSHOP_CONFIG.CONFIG_PORT, eSHOP_CONFIG.CONFIG_SMTP);

                    if (Utils.CIntDef(check) == 1)
                    {
                        SendEmailSMTP("Thông tin đăng ký tài khoản tại EUREKA", eSHOP_CONFIG.CONFIG_EMAIL, eSHOP_CONFIG.CONFIG_PASSWORD, "Gửi yêu cầu thuê nhà", diachiemail, sodienthoai, emailgui, emailSend, bodydk, true, true, eSHOP_CONFIG.CONFIG_PORT, eSHOP_CONFIG.CONFIG_SMTP);
                    }
                    return Ok(new { status = true, response = "Thông tin liên hệ của bạn đã được gửi đi.Chúng tôi sẽ liên hệ tới bạn " });
                }
                //return Ok(new { status = true, response = "Thông tin liên hệ của bạn đã được gửi đi.Chúng tôi sẽ liên hệ tới bạn " });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, response = ex.Message });
            }
        }

        [HttpGet]
        [Route("CustomerSugget")]
        public async Task<IActionResult> CustomerSugget(string IdRequest, string IdNew, string CustomerId)
        {
            try
            {
                CustomerConfirm objcart = new CustomerConfirm()
                {
                    CUSTOMER_ID = Utils.CIntDef(CustomerId),
                    CustomerConfirm_Active = 0,
                    CustomerConfirm_Comment = "",
                    CustomerConfirm_PublishDate = DateTime.Now,
                    CustomerConfirm_Type = 1, //Gợi ý cho khách hàng chưa xác nhận
                    NEWS_ID = Utils.CIntDef(IdNew),
                    Customer_request_id = Utils.CIntDef(IdRequest)
                };

                _context.Add(objcart);
                await _context.SaveChangesAsync();
                return Ok(new { status = true, response = "Gợi ý khách hàng thành công" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, response = ex.Message });
            }
        }


        [HttpGet]
        [Route("CustomerCform")]
        public async Task<IActionResult> CustomerCform(string IdRequest, string IdNew, string CustomerId,string Status)
        {
            try
            {
                int idn = Utils.CIntDef(IdNew);
                int idc = Utils.CIntDef(CustomerId);

                var CustomerCf = _context.CustomerConfirm.SingleOrDefault(x => x.NEWS_ID == idn && x.CUSTOMER_ID == idc);

                if (CustomerCf != null)
                {
                    CustomerCf.CustomerConfirm_Active = Utils.CIntDef(Status);
                    CustomerCf.CustomerConfirm_Date = DateTime.Now;
                    _context.Update(CustomerCf);
                    await _context.SaveChangesAsync();
                }
                return Ok(new { status = true, response = "Gợi ý khách hàng thành công" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, response = ex.Message });
            }
        }


        [HttpGet]
        [Route("CounterNotification")]
        public async Task<IActionResult> CounterNotification()
        {
            int fl = 0;
            int cookieValueId = Utils.CIntDef(Request.Cookies["CustomerID"]);
            var check = _context.CustomerConfirm.Where(x => x.CustomerConfirm_Active == 0 && x.CUSTOMER_ID == cookieValueId);
            if (check != null)
            {
                fl = check.ToList().Count();
            }

            return Ok(new { status = true, response = fl });
        }

        [HttpGet]
        [Route("CheckLogin")]
        public async Task<IActionResult> CheckLogin()
        {
            string fl = "";
            int? type = 0;
            int cookieValueId = Utils.CIntDef(Request.Cookies["CustomerID"]);
            var check =await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(x => x.CUSTOMER_ID == cookieValueId);
            if (check != null)
            {
                fl = check.CUSTOMER_FULLNAME;
                type = check.CUSTOMER_SHOWTYPE;
                return Ok(new { status = true, response = fl, typecheck = type });
            }
            else
            {
                return Ok(new { status = false, response = fl, typecheck = type });
            }    

            
        }
    }
}