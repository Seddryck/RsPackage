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
            string solutionFile;
            if (!Path.IsPathRooted(options.SourceFile))
                solutionFile = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + Path.DirectorySeparatorChar + options.SourceFile;
            else
                solutionFile = options.SourceFile;

            string resourcePath;
            if (string.IsNullOrEmpty(options.ResourcePath))
                resourcePath = Path.GetDirectoryName(solutionFile) + Path.DirectorySeparatorChar;
            else if (!Path.IsPathRooted(options.ResourcePath))
                resourcePath = Path.GetDirectoryName(solutionFile) + Path.DirectorySeparatorChar + options.ResourcePath;
            else
                resourcePath = options.ResourcePath;


            string targetFile;
            if (string.IsNullOrEmpty(options.TargetFile))
                targetFile = Path.GetDirectoryName(solutionFile) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(solutionFile);
            else if (Path.IsPathRooted(options.TargetFile))
                targetFile = options.TargetFile;
            else
                targetFile = Path.GetDirectoryName(solutionFile) + Path.DirectorySeparatorChar + options.TargetFile;

            if (targetFile.EndsWith(Path.DirectorySeparatorChar.ToString()))
                targetFile += Path.GetFileNameWithoutExtension(solutionFile);

            if (string.IsNullOrEmpty(Path.GetExtension(targetFile)))
                targetFile += ".rspac";

            return new Packager(solutionFile, resourcePath, targetFile);
        }
    }
}
