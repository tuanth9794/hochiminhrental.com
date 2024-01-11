using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using CoreCnice.Connect;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CoreCnice.UtilsCs
{
    public class clsFormat
    {
        #region Declare

        private readonly BulSoftCmsConnectContext db = new BulSoftCmsConnectContext();
        string SessionKeyName = "CustomerSesion1";

        #endregion

        public string FormatMoney(object Expression)
        {
            try
            {
                string Money = String.Format("{0:0,0}", Expression);
                return Money.Replace(",", ".");
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        // Chuyển định dạng chuỗi ngày thành định dạng ngày
        public DateTime StrDateToDate(string dateInput, string formatInput)
        {
            var formatInfo = new DateTimeFormatInfo { ShortDatePattern = formatInput };
            return DateTime.Parse(dateInput, formatInfo);
        }

        // Chuyển định dạng ngày thành định dạng chuỗi ngày
        public string DateToStrDate(object dateInput, string formatOutput)
        {
            //return dateInput.ToString(formatOutput);
            return string.Format(formatOutput, dateInput);
        }

        // Chuyển định dạng chuỗi ngày thành định dạng chuỗi ngày
        public string StrDateToStrDate(string dateInput, string formatInput, string formatOutput)
        {
            var date = StrDateToDate(dateInput, formatInput);
            return DateToStrDate(date, formatOutput);
        }


        /// <summary>
        /// Cắt chuỗi url chuyển về Cat_seo_url
        /// </summary>
        /// <param name="s">đường dẫn url</param>
        /// <returns>Cat_seo_url</returns>
        public string CatChuoiURL(string s)
        {
            string[] sep = { "/" };
            string[] sep1 = { " " };
            string[] t1 = s.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            string res = "";
            for (int i = 1; i < t1.Length; i++)
            {
                string[] t2 = t1[i].Split(sep1, StringSplitOptions.RemoveEmptyEntries);
                if (t2.Length > 0)
                {
                    if (res.Length > 0)
                    {
                        res += "//";
                    }
                    res += t2[0];
                }
            }
            return res.Substring(0, res.Length - 5);
        }


        /// <summary>
        /// Chuyển chuỗi kiểu số thành chuỗi kiểu chữ
        /// </summary>
        /// <param name="str">mảng chứa đường dẫn kiểu số</param>
        /// <returns>đường dẫn kiểu chữ</returns>
        public string Convert_Name(string[] str)
        {
            string s = "";

            try
            {
                int _value = 0;

                for (int i = 1; i < str.Count(); i++)
                {
                    _value = Utils.CIntDef(str[i]);

                    var rausach = from r in db.ESHOP_CATEGORIES
                                  where r.CAT_ID == _value
                                  select r.CAT_NAME;
                    s += rausach.ToList()[0] + " > ";
                }
                return s;
            }
            catch (Exception ex)
            {

                return "";
            }
        }

        /// <summary>
        /// Mã hóa mật khẩu
        /// </summary>
        /// <param name="sPassword">Mật khẩu</param>
        /// <returns>Mật khẩu đã mã hóa</returns>
        public string MaHoaMatKhau(string Password)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedDataBytes = md5Hasher.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(Password));
            string EncryptPass = Convert.ToBase64String(hashedDataBytes);
            return EncryptPass;
        }

        public string TaoChuoiTuDong(int dodai)
        {
            string _allowedChars = "abcdefghijk0123456789mnopqrstuvwxyz";
            Random randNum = new Random();
            char[] chars = new char[dodai];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < dodai; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        public static string ClearUnicode(string SourceString)
        {

            SourceString = Regex.Replace(SourceString, "[ÂĂÀÁẠẢÃÂẦẤẬẨẪẰẮẶẲẴàáạảãâầấậẩẫăằắặẳẵ]", "a");
            SourceString = Regex.Replace(SourceString, "[ÈÉẸẺẼÊỀẾỆỂỄèéẹẻẽêềếệểễ]", "e");
            SourceString = Regex.Replace(SourceString, "[IÌÍỈĨỊìíịỉĩ]", "i");
            SourceString = Regex.Replace(SourceString, "[ÒÓỌỎÕÔỒỐỔỖỘƠỜỚỞỠỢòóọỏõôồốộổỗơờớợởỡ]", "o");
            SourceString = Regex.Replace(SourceString, "[ÙÚỦŨỤƯỪỨỬỮỰùúụủũưừứựửữ]", "u");
            SourceString = Regex.Replace(SourceString, "[ỲÝỶỸỴỳýỵỷỹ]", "y");
            SourceString = Regex.Replace(SourceString, "[đĐ]", "d");

            return SourceString;
        }

        private static string GetFirstCharacter(string s)
        {
            string[] chuoi_cat = s.Split(' ');
            string _sResult = "";
            for (int i = 0; i <= chuoi_cat.Length - 1; i++)
            {
                _sResult += chuoi_cat[i].Substring(0, 1).ToLower();
            }
            return _sResult;
        }

        public string GetProductName(object Title, int intLength)
        {
            if (Utils.CStrDef(Title).Length > intLength)
                return Utils.CStrDef(Title).Substring(0, intLength) + "...";
            return Utils.CStrDef(Title);
        }

        public string ReadFile(string FileName)
        {
            try
            {
                bool exists = System.IO.File.Exists(FileName);
                if (exists == true)
                {
                    using (StreamReader reader = File.OpenText(FileName))
                    {
                        string fileContent = reader.ReadToEnd();
                        if (fileContent != null && fileContent != "")
                        {
                            return fileContent;
                        }
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                //Log
                throw ex;
            }
            return null;
        }

        public string DocTien(decimal NumCurrency)
        {           
            string SoRaChu = "";
            NumCurrency = Math.Abs(NumCurrency);
            if (NumCurrency == 0)
            {
                SoRaChu = "Liên hệ";
                return SoRaChu;
            }
            string[] CharVND = new string[10] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string BangChu;
            int I;
            //As String, BangChu As String, I As Integer
            int SoLe, SoDoi;
            string PhanChan, Ten;
            string DonViTien, DonViLe;
            int NganTy, Ty, Trieu, Ngan;
            int Dong, Tram, Muoi, DonVi;
            SoDoi = 0;
            Muoi = 0;
            Tram = 0;
            DonVi = 0;
            Ten = "";
            DonViTien = "đồng";
            DonViLe = "xu";

            SoLe = (int)((NumCurrency - (Int64)NumCurrency) * 100); //'2 kí so^' le?
            PhanChan = ((Int64)NumCurrency).ToString().Trim();
            int khoangtrang = 15 - PhanChan.Length;
            for (int i = 0; i < khoangtrang; i++)
                PhanChan = "0" + PhanChan;
            NganTy = int.Parse(PhanChan.Substring(0, 3));
            Ty = int.Parse(PhanChan.Substring(3, 3));
            Trieu = int.Parse(PhanChan.Substring(6, 3));
            Ngan = int.Parse(PhanChan.Substring(9, 3).Replace("00", " "));
            Dong = int.Parse(PhanChan.Substring(12, 3));

            if (NganTy == 0 & Ty == 0 & Trieu == 0 & Ngan == 0 & Dong == 0)
            {
                BangChu = String.Format("không {0} ", DonViTien);
                I = 5;
            }
            else if (NganTy == 0 & Ty == 0 & Trieu == 0 & Ngan == 0 & Dong != 0)
            {
                BangChu = String.Format(Dong + "{0} ", DonViTien);
                I = 0;
            }
            else if (NganTy == 0 & Ty == 0 & Trieu == 0 & Ngan != 0)
            {
                BangChu = String.Format(Ngan + "{0} ", " ngàn");
                I = 0;
            }
            else if (NganTy == 0 & Ty == 0 & Trieu != 0)
            {
                if (Ngan != 0)
                {
                    BangChu = String.Format(Trieu + "," + Ngan + "{0}", " triệu");
                    I = 0;
                }
                else
                {
                    BangChu = String.Format(Trieu + "{0} ", " triệu");
                    I = 0;
                }

            }
            else if (NganTy == 0 & Ty != 0)
            {
                BangChu = String.Format(Ty + "{0} ", DonViTien);
                I = 0;
            }
            else if (NganTy != 0)
            {
                BangChu = String.Format(NganTy + "{0} ", DonViTien);
                I = 0;
            }
            else
            {
                BangChu = "";
                I = 0;
            }

            //while (I <= 5)
            //{
            //    switch (I)
            //    {
            //        case 0:
            //            SoDoi = NganTy;
            //            Ten = "ngàn tỷ";
            //            break;
            //        case 1:
            //            SoDoi = Ty;
            //            Ten = "tỷ";
            //            break;
            //        case 2:
            //            SoDoi = Trieu;
            //            Ten = "triệu";
            //            break;
            //        case 3:
            //            SoDoi = Ngan;
            //            Ten = "ngàn";
            //            break;
            //        case 4:
            //            SoDoi = Dong;
            //            Ten = DonViTien;
            //            break;
            //        case 5:
            //            SoDoi = SoLe;
            //            Ten = DonViLe;
            //            break;
            //    }
            //    if (SoDoi != 0)
            //    {
            //        Tram = (int)(SoDoi / 100);
            //        Muoi = (int)((SoDoi - Tram * 100) / 10);
            //        DonVi = (SoDoi - Tram * 100) - Muoi * 10;
            //        BangChu = BangChu.Trim() + (BangChu.Length == 0 ? "" : " ") + (Tram != 0 ? CharVND[Tram].Trim() + " trăm " : "");
            //        if (Muoi == 0 & Tram == 0 & DonVi != 0 & BangChu != "")
            //            BangChu = BangChu + "không trăm lẻ ";
            //        else if (Muoi != 0 & Tram == 0 & (DonVi == 0 || DonVi != 0) & BangChu != "")
            //            BangChu = BangChu + "không trăm ";
            //        else if (Muoi == 0 & Tram != 0 & DonVi != 0 & BangChu != "")
            //            BangChu = BangChu + "lẻ ";
            //        if (Muoi != 0 & Muoi > 0)
            //            BangChu = BangChu + ((Muoi != 0 & Muoi != 1) ? CharVND[Muoi].Trim() + " mươi " : "mười ");
            //        if (Muoi != 0 & DonVi == 5)
            //            BangChu = String.Format("{0}lăm {1} ", BangChu, Ten);
            //        else if (Muoi > 1 & DonVi == 1)
            //            BangChu = String.Format("{0}mốt {1} ", BangChu, Ten);
            //        else
            //            BangChu = BangChu + ((DonVi != 0) ? String.Format("{0} {1} ", CharVND[DonVi].Trim(), Ten) : Ten + " ");
            //    }
            //    else
            //        BangChu = BangChu + ((I == 4) ? DonViTien + " " : "");
            //    I = I + 1;
            //}
            //BangChu = BangChu[0].ToString().ToUpper() + BangChu.Substring(1);
            //SoRaChu = BangChu;
            return BangChu;
        }

        public string DocTienEn(decimal NumCurrency)
        {
            string SoRaChu = "";
            NumCurrency = Math.Abs(NumCurrency);
            if (NumCurrency == 0)
            {
                SoRaChu = "Liên hệ";
                return SoRaChu;
            }
            string[] CharVND = new string[10] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string BangChu;
            int I;
            //As String, BangChu As String, I As Integer
            int SoLe, SoDoi;
            string PhanChan, Ten;
            string DonViTien, DonViLe;
            int NganTy, Ty, Trieu, Ngan;
            int Dong, Tram, Muoi, DonVi;
            SoDoi = 0;
            Muoi = 0;
            Tram = 0;
            DonVi = 0;
            Ten = "";
            DonViTien = "đồng";
            DonViLe = "xu";

            SoLe = (int)((NumCurrency - (Int64)NumCurrency) * 100); //'2 kí so^' le?
            PhanChan = ((Int64)NumCurrency).ToString().Trim();
            int khoangtrang = 15 - PhanChan.Length;
            for (int i = 0; i < khoangtrang; i++)
                PhanChan = "0" + PhanChan;
            NganTy = int.Parse(PhanChan.Substring(0, 3));
            Ty = int.Parse(PhanChan.Substring(3, 3));
            Trieu = int.Parse(PhanChan.Substring(6, 3));
            Ngan = int.Parse(PhanChan.Substring(9, 3).Replace("00", " "));
            Dong = int.Parse(PhanChan.Substring(12, 3));

            if (NganTy == 0 & Ty == 0 & Trieu == 0 & Ngan == 0 & Dong == 0)
            {
                BangChu = String.Format("không {0} ", DonViTien);
                I = 5;
            }
            else if (NganTy == 0 & Ty == 0 & Trieu == 0 & Ngan == 0 & Dong != 0)
            {
                BangChu = String.Format(Dong + "{0} ", DonViTien);
                I = 0;
            }
            else if (NganTy == 0 & Ty == 0 & Trieu == 0 & Ngan != 0)
            {
                BangChu = String.Format(Ngan + "{0} ", " ngàn");
                I = 0;
            }
            else if (NganTy == 0 & Ty == 0 & Trieu != 0)
            {
                if (Ngan != 0)
                {
                    BangChu = String.Format(Trieu + "," + Ngan + "{0}", " Mil");
                    I = 0;
                }
                else
                {
                    BangChu = String.Format(Trieu + "{0} ", " Mil");
                    I = 0;
                }

            }
            else if (NganTy == 0 & Ty != 0)
            {
                BangChu = String.Format(Ty + "{0} ", DonViTien);
                I = 0;
            }
            else if (NganTy != 0)
            {
                BangChu = String.Format(NganTy + "{0} ", DonViTien);
                I = 0;
            }
            else
            {
                BangChu = "";
                I = 0;
            }

            //while (I <= 5)
            //{
            //    switch (I)
            //    {
            //        case 0:
            //            SoDoi = NganTy;
            //            Ten = "ngàn tỷ";
            //            break;
            //        case 1:
            //            SoDoi = Ty;
            //            Ten = "tỷ";
            //            break;
            //        case 2:
            //            SoDoi = Trieu;
            //            Ten = "triệu";
            //            break;
            //        case 3:
            //            SoDoi = Ngan;
            //            Ten = "ngàn";
            //            break;
            //        case 4:
            //            SoDoi = Dong;
            //            Ten = DonViTien;
            //            break;
            //        case 5:
            //            SoDoi = SoLe;
            //            Ten = DonViLe;
            //            break;
            //    }
            //    if (SoDoi != 0)
            //    {
            //        Tram = (int)(SoDoi / 100);
            //        Muoi = (int)((SoDoi - Tram * 100) / 10);
            //        DonVi = (SoDoi - Tram * 100) - Muoi * 10;
            //        BangChu = BangChu.Trim() + (BangChu.Length == 0 ? "" : " ") + (Tram != 0 ? CharVND[Tram].Trim() + " trăm " : "");
            //        if (Muoi == 0 & Tram == 0 & DonVi != 0 & BangChu != "")
            //            BangChu = BangChu + "không trăm lẻ ";
            //        else if (Muoi != 0 & Tram == 0 & (DonVi == 0 || DonVi != 0) & BangChu != "")
            //            BangChu = BangChu + "không trăm ";
            //        else if (Muoi == 0 & Tram != 0 & DonVi != 0 & BangChu != "")
            //            BangChu = BangChu + "lẻ ";
            //        if (Muoi != 0 & Muoi > 0)
            //            BangChu = BangChu + ((Muoi != 0 & Muoi != 1) ? CharVND[Muoi].Trim() + " mươi " : "mười ");
            //        if (Muoi != 0 & DonVi == 5)
            //            BangChu = String.Format("{0}lăm {1} ", BangChu, Ten);
            //        else if (Muoi > 1 & DonVi == 1)
            //            BangChu = String.Format("{0}mốt {1} ", BangChu, Ten);
            //        else
            //            BangChu = BangChu + ((DonVi != 0) ? String.Format("{0} {1} ", CharVND[DonVi].Trim(), Ten) : Ten + " ");
            //    }
            //    else
            //        BangChu = BangChu + ((I == 4) ? DonViTien + " " : "");
            //    I = I + 1;
            //}
            //BangChu = BangChu[0].ToString().ToUpper() + BangChu.Substring(1);
            //SoRaChu = BangChu;
            return BangChu;
        }

        public string Dola(decimal sotien, decimal tygia)
        {
            string src = "";

            src = "$ " + FormatMoney(sotien / tygia);

            return src;
        }

        public string Encrypt(string toEncrypt)
        {
            String key = "toiyeuvietnam";
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                  toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string Decrypt(string toDecrypt)
        {
            String key = "toiyeuvietnam";
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
             toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public bool CheckApprove(int IdNew,int active)
        {
            bool fl = false;
            var check = db.CustomerConfirm.FirstOrDefault(x => x.CustomerConfirm_Active == active && x.NEWS_ID == IdNew);
            if(check != null)
            {
                fl = true;
            }   
                
            return fl;
        }
        
        public int count(int Customer)
        {       
            int fl = 0;
            var check = db.CustomerConfirm.Where(x => x.CustomerConfirm_Active == 0 && x.CUSTOMER_ID == Customer);
            if (check != null)
            {
                fl = check.ToList().Count();
            }

            return fl;
        }
    }
}
