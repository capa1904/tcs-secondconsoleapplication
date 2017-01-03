using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PackageC;

namespace SecondConsoleApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            PrintSecondConsoleApplicationInfo();
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        public static string GetName()
        {
            return "SecondConsoleApplication";
        }

        private static string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string productVersion = fileVersionInfo.ProductVersion;
            return productVersion;
        }

        private static string GetSummary()
        {
            return $"{GetName()} v. {GetVersion()}";
        }

        private static void PrintSecondConsoleApplicationInfo()
        {
            Console.WriteLine($"Application information: {GetSummary()}");
            PrintInformationAboutUsedPackage();
        }

        private static void PrintInformationAboutUsedPackage()
        {
            var classOfPackageC = new ClassOfPackageC();
            var packageInfo = classOfPackageC.GetSummary();
            Console.WriteLine($"Information about used package: {packageInfo}");
        }
    }
}
