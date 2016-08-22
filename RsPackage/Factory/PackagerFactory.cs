using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RsPackage.CommandLineArgs;
using RsPackage.Action;

namespace RsPackage.Factory
{
    public class PackagerFactory
    {
        public Packager GetPackager(PackageOptions options)
        {
            var solutionFile = options.SourceFile;

            string resourcePath;
            if (string.IsNullOrEmpty(options.ResourcePath))
                resourcePath = Path.GetDirectoryName(options.SourceFile) + Path.DirectorySeparatorChar;
            else if (!Path.IsPathRooted(options.ResourcePath))
                resourcePath = Path.GetDirectoryName(options.SourceFile) + Path.DirectorySeparatorChar + options.ResourcePath;
            else
                resourcePath = options.ResourcePath;


            string targetFile;
            if (string.IsNullOrEmpty(options.TargetFile))
                targetFile = Path.GetDirectoryName(options.SourceFile) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(options.SourceFile);
            else if (Path.IsPathRooted(options.TargetFile))
                targetFile = options.TargetFile;
            else
                targetFile = Path.GetDirectoryName(options.SourceFile) + Path.DirectorySeparatorChar + options.TargetFile;

            if (targetFile.EndsWith(Path.DirectorySeparatorChar.ToString()))
                targetFile += Path.GetFileNameWithoutExtension(options.SourceFile);

            if (string.IsNullOrEmpty(Path.GetExtension(targetFile)))
                targetFile += ".rspac";

            return new Packager(solutionFile, resourcePath, targetFile);
        }
    }
}
