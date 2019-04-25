using System;
using System.Collections.Generic;

namespace AWS_S3_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //this is the main entry point for the console program
                
                Console.WriteLine("Type LIST to run the AmazonS3 List test \r\n");
                Console.WriteLine("Type END to end the program \r\n");
                Console.WriteLine("Type DL to run AmazonS3 Download test \r\n");
                string line = Console.ReadLine();
                //choose test to run based on user input
                if (line == "LIST")
                {
                    AmazonS3 amazonS3 = new AmazonS3();                   
                    amazonS3.ListAllObjects();
                }     
                if(line == "DL")
                {
                    AmazonS3 amazonS3 = new AmazonS3();
                    amazonS3.DownloadAllFiles();
                }
                if (line == "END")
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
