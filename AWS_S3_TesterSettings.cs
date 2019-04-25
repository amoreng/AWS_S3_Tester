using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AWS_S3_Tester
{
    class AWS_S3_TesterSettings
    {
        public static string GetSetting(string key)
        {
            //returns the configuration value as string from app.config
            string value = "";
            try
            {
               value = ConfigurationManager.AppSettings[key].ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return value;

        }
    }
}
