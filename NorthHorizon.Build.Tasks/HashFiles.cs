using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mustache;

namespace NorthHorizon.Build.Tasks
{
    public class HashFiles : Task
    {
        public HashFiles()
        {
            HashName = "MD5";
        }

        public string HashName { get; set; }

        [Required]
        public string TemplatePath { get; set; }

        [Required]
        public string[] FilePaths { get; set; }

        [Required]
        public string OutPath { get; set; }

        public override bool Execute()
        {
            var hash = FileHashProvider.HashFiles(HashName, FilePaths);

            var templateContent = File.ReadAllText(TemplatePath);

            var compiler = new FormatCompiler();
            
            var compiled = compiler.Compile(templateContent);

            var rendered = compiled.Render(new
            {
                Algorithm = HashName,
                Hash = new
                {
                    HexString = Format("{0:x2}", null, hash),
                    Base64String = Convert.ToBase64String(hash),
                    CommaDelim = Format("0x{0:x}", ", ", hash)
                }
            });

            var outFileInfo = new FileInfo(OutPath);

            if (outFileInfo.Directory != null && !outFileInfo.Directory.Exists)
                outFileInfo.Directory.Create();

            File.WriteAllText(outFileInfo.FullName, rendered, Encoding.UTF8);

            return true;
        }

        private static string Format(string byteFormat, string seperator, byte[] bytes)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < bytes.Length; i ++)
            {
                builder.AppendFormat(byteFormat, bytes[i]);

                if (!string.IsNullOrEmpty(seperator) && i < bytes.Length - 1)
                    builder.Append(seperator);
            }

            return builder.ToString();
        }
    }
}