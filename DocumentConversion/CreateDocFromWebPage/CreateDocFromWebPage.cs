using System;
using Datalogics.PDFL;

/*
 *
 * This sample demonstrates converting a web page or a local HTML file into
 * a PDF document using the WebToPDF plug-in.
 *
 * The first optional argument can be either a URL (http://, https://, or
 * file://) or a path to a local HTML file. The sample auto-detects which
 * based on the scheme prefix and routes to Document.FromWebUrl or
 * Document.FromHtmlFile accordingly.
 *
 * Copyright (c) 2026, Datalogics, Inc. All rights reserved.
 *
 */
namespace CreateDocFromWebPage
{
    class CreateDocFromWebPage
    {
        // Returns true if s looks like a URL the conversion plug-in handles
        // natively (http/https/file).
        static bool LooksLikeUrl(string s)
        {
            return s.StartsWith("http://") || s.StartsWith("https://") || s.StartsWith("file://");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("CreateDocFromWebPage Sample:");

            using (Library lib = new Library())
            {
                Console.WriteLine("Initialized the library.");

                String sSource = "https://www.datalogics.com";
                String sOutput = "CreateDocFromWebPage-out.pdf";

                if (args.Length > 0)
                    sSource = args[0];
                if (args.Length > 1)
                    sOutput = args[1];

                // Enum-valued parameters are plain ints; the values match the
                // plug-in enums 1:1.
                WebToPDFConvertParams webParams = new WebToPDFConvertParams();
                webParams.ViewportSize = 0;                // Desktop (1280x1024)
                webParams.PageSize = 0;                    // US Letter (8.5 x 11 in)
                webParams.PageOrientation = 0;             // Portrait
                webParams.ImageCompression = 0;            // JPEG
                webParams.DownsamplingDPI = 0;             // 300 DPI
                webParams.SetMargins(0.5, 0.5, 0.5, 0.5);  // top, right, bottom, left, in inches
                webParams.PrintBackground = true;
                webParams.GenerateTaggedPDF = true;        // produce an accessible (tagged) PDF
                webParams.TimeoutSeconds = 60;             // override the 300s plug-in default

                bool isUrl = LooksLikeUrl(sSource);
                Console.WriteLine("Converting " + (isUrl ? "URL " : "HTML file ") + sSource + " ...");

                WebConvertInfo info = new WebConvertInfo();
                using (Document doc = isUrl
                    ? Document.FromWebUrl(sSource, webParams, info)
                    : Document.FromHtmlFile(sSource, webParams, info))
                {
                    Console.WriteLine("Wrote " + info.PageCount + " pages in " + info.ConversionTimeMs + " ms");
                    if (!String.IsNullOrEmpty(info.Title))
                        Console.WriteLine("Title: " + info.Title);
                    if (!String.IsNullOrEmpty(info.SourceUrl) && info.SourceUrl != sSource)
                        Console.WriteLine("Resolved URL: " + info.SourceUrl);

                    doc.Save(SaveFlags.Full, sOutput);
                    Console.WriteLine("Saved to " + sOutput);
                }
            }
        }
    }
}
