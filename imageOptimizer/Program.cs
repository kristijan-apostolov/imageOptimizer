using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace imageOptimizer
{
    static class  Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void UploadFile(this IWebDriver webDriver, string fileName)
        {
            webDriver.FindElement(By.ClassName("wi-drop-zone")).Click();
            var dialogHWnd = FindWindow(null, "Select file(s) to upload by localhost");
            var setFocus = SetForegroundWindow(dialogHWnd);
            if (setFocus)
            {
                //object p = System.Windows.Forms.SendKeys.SendWait("complete path of the file");
                //Thread.Sleep(500);  
                //webDriver.findElement(By.name("uploadfile"));
                //fileInput.sendKeys("C:/path/to/file.jpg");
                ////webDriver.WindowHandles.
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Please insert rootPath");

            var rootPath = Console.ReadLine();

            if(String.IsNullOrEmpty(rootPath))
            {
                rootPath = @"C:\projects\Moznosti CMS\WebCMS";
            }
            IWebDriver driver = new ChromeDriver();

            driver.Url = "https://kraken.io/web-interface";
            driver.Manage().Window.Maximize();

            RecursiveFileProcessor.ProcessDirectory(rootPath);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);


            driver.FindElement(By.ClassName("lossless")).Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Console.WriteLine("These images are larger than 1mb, please use another tools to optimise (ex. Photoshop)");
            //RecursiveFileProcessor.GetImagesPaths.ToList().ForEach(i => Console.WriteLine("" + i.ToString()));

            //group by folders

            //for each folder
            var dict = RecursiveFileProcessor.GetImagesPathsWithDirs();
            foreach (KeyValuePair<string, List<string>> entry in dict)
            {
                if(entry.Value.Count==0)
                {
                    dict.Remove(entry.Key);
                    continue;
                }
                Console.WriteLine("\n" + "Dir is:" + entry.Key + "\n");
                entry.Value.ForEach(i => Console.WriteLine("" + i.ToString()));
            }
            //driver.FindElement(By.ClassName("wi-drop-zone")).SendKeys(@"C:\projects\Moznosti CMS\WebCMS\images\logo.jpg");

            UploadFile(driver, @"C:\projects\Moznosti CMS\WebCMS\images\logo.jpg");

        }
        
    }
}
