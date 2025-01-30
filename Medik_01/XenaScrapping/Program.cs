using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace WebScraperSelenium
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://xenabrowser.net/datapages/?hub=https://tcga.xenahubs.net:443";
            //string downloadDirectory = @"C:\Downloads\XENA";
            string downloadDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Downloads", "XENA");

            Directory.CreateDirectory(downloadDirectory);

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            using var chromeDriver = new ChromeDriver(chromeOptions);

            chromeDriver.Navigate().GoToUrl(url);
            Console.WriteLine($"Loaded: {url}");

            WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));

            Thread.Sleep(2000);

            wait.Until(driver => driver.FindElement(By.XPath("//ul[contains(@class, 'Datapages-module__list')]")));

            var tcgaLinks = chromeDriver.FindElements(By.XPath("//a[starts-with(@href, '?cohort=TCGA')]"))
                .Select(link => link.GetAttribute("href"))
                .ToList();

            Console.WriteLine($"Found {tcgaLinks.Count} 'TCGA' links.");

            using HttpClient client = new HttpClient();

            foreach (var relativeTcgaLink in tcgaLinks)
            {
                try
                {
                    var tcgaUrl = new Uri(new Uri(url), relativeTcgaLink).ToString();
                    Console.WriteLine($"Navigating to TCGA page: {tcgaUrl}");

                    chromeDriver.Navigate().GoToUrl(tcgaUrl);
                    Thread.Sleep(2000);

                    var illuminaLink = chromeDriver.FindElement(By.XPath("//a[contains(text(), 'IlluminaHiSeq pancan normalized')]"))
                        ?.GetAttribute("href");

                    if (!string.IsNullOrEmpty(illuminaLink))
                    {
                        var illuminaUrl = new Uri(new Uri(tcgaUrl), illuminaLink).ToString();
                        Console.WriteLine($"Navigating to IlluminaHiSeq page: {illuminaUrl}");

                        chromeDriver.Navigate().GoToUrl(illuminaUrl);

                        Thread.Sleep(2000);

                        wait.Until(driver => driver.FindElement(By.XPath("//a[contains(@href, '.gz')]")));

                        var gzLink = chromeDriver.FindElement(By.XPath("//a[contains(@href, '.gz')]"))
                            ?.GetAttribute("href");

                        if (!string.IsNullOrEmpty(gzLink))
                        {
                            string fileName = Path.GetFileName(gzLink);

                            Console.WriteLine($"Downloading: {fileName} from {gzLink}");

                            byte[] fileBytes = await client.GetByteArrayAsync(gzLink);

                            string filePath = Path.Combine(downloadDirectory, fileName);
                            await File.WriteAllBytesAsync(filePath, fileBytes);

                            Console.WriteLine($"Saved: {filePath}");
                        }
                        else
                        {
                            Console.WriteLine("No .gz file found on this page.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No 'IlluminaHiSeq pancan normalized' link found on this page.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process link {relativeTcgaLink}: {ex.Message}");
                }
            }

            Console.WriteLine("Download complete.");

            chromeDriver.Quit();
        }
    }
}
