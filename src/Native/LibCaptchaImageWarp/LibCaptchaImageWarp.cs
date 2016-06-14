using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace YOYO.DotnetCore
{

   
    public class CaptchaImageCore
    {

        [DllImport("libcaptchaimage.so", EntryPoint = "GCaptcha")]
        public static extern void libCaptcha(string file_o, string captcha_text, int count, int width, int height, int offset, int quality, int isjpeg, int fontSize);


        [DllImport("libcaptchaimage.dll", EntryPoint = "GCaptcha")]
        public static extern void GCaptcha(string file_o, string captcha_text, int count, int width, int height, int offset, int quality, int isjpeg, int fontSize);

        public string Text { set; get; }

        public int ImageWidth { set; get; }
        public int ImageHeight { set; get; }

        public int ImageOffset { set; get; }

        public int ImageQuality { set; get; }

        public int FontSize { set; get; }


        public CaptchaImageCore(int w,int h,int fontSize)
        {
            this.ImageWidth = w;
            this.ImageHeight = h;
            this.FontSize = fontSize;
            this.ImageOffset = 40;
            this.ImageQuality = 100;
            
        }

        public MemoryStream GetStream(string fileName)
        {
            this.Save(fileName);
            MemoryStream ms = new MemoryStream();
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                fileStream.CopyTo(ms);
            }
            try
            {
                File.Delete(fileName);
            }
            catch { }
            return ms;

        }


        public void Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new NullReferenceException("file name is null");
            if (string.IsNullOrEmpty(this.Text))
                this.Text = this.GenerateCheckCode();



            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                GCaptcha(fileName, this.Text, this.Text.Length, this.ImageWidth, this.ImageHeight,
                                          this.ImageOffset, this.ImageQuality, 0, this.FontSize);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                libCaptcha(fileName, this.Text, this.Text.Length, this.ImageWidth, this.ImageHeight,
                                         this.ImageOffset, this.ImageQuality, 0, this.FontSize);
            }
        }

        public string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    //生成'0'-'9'字符
                    code = (char)('0' + (char)(number % 10));
                else
                    //生成'A'-'Z'字符
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }

            return checkCode;
            //两个字符相加等于=asicc码加
        }



    }
}
