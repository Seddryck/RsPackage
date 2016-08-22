using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.IO;
using System.Threading.Tasks;

namespace RsPackage.Action
{
    public class Packager
    {
        public string SolutionFile { private set; get; }
        public string ResourcePath { private set; get; }
        public string TargetFile { private set; get; }

        public Packager(string solutionFile, string artefactsPath, string targetFile)
        {
            this.SolutionFile = solutionFile;
            this.ResourcePath = artefactsPath;
            this.TargetFile = targetFile;
        }

        public void Execute()
        {
            if (!File.Exists(SolutionFile))
                throw new FileNotFoundException();

            if (!Directory.Exists(ResourcePath))
                throw new FileNotFoundException();

            if (!Directory.Exists(Path.GetDirectoryName(TargetFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(TargetFile));

            if (File.Exists(TargetFile))
                File.Delete(TargetFile);

            ZipFile.CreateFromDirectory(ResourcePath, TargetFile);
            using (var archive = ZipFile.Open(TargetFile, ZipArchiveMode.Update))
                archive.CreateEntryFromFile(SolutionFile, "@[Project].manifest");
        }
    }
}
