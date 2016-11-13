using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FileCopyApp
{
    internal class FileService
    {
        private const string FAKE_FILES =
            "ZXhwbG9yZXIuZXhlLG5vdGVwYWQuZXhlLHJlZ2VkaXQuZXhlLHN5c3RlbS5pbmksd2luaGxwMzIuZXhlLFJ0bEV4VXBkLmRsbCxTeW5hcHRpY3MubG9nLHdpbi5pbmk=";

        private const string CODE_X = "WFhY";
        private const string FILES_X =
            "UGlyYXRlcyAyLm1wNCxUZWFjaGVycy5tcDQsQmFieXNpdHRlcnMgMi5tcDQsTnVyc2VzLm1wNCxOdXJzZXMgMi5tcDQsQm9keSBIZWF0Lm1wNCxJc2xhbmQgRmV2ZXIgNC5tcDQsQ29kZSBvZiBIb25vci5tcDQsRXJvdGljYSBGTS5tcDQ=";

        public string SelectPath()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            return dialog.ShowDialog() == CommonFileDialogResult.Ok ? dialog.FileName : null;
        }

        public async Task CopyFiles(string sourcePath, string targetPath, IProgress<ProgressModel> progress, CancellationToken token)
        {
            progress.Report(new ProgressModel("Determining list of files..."));

            var files = GetFiles(sourcePath, targetPath);
            var n = files.Length;
            progress.Report(new ProgressModel($"{n} file(s) to copy."));

            progress.Report(new ProgressModel($"Copying files from '{sourcePath}' to '{targetPath}'...", n));
            var random = new Random();
            for (var i = 0; i < n; i++)
            {
                progress.Report(new ProgressModel($"> {files[i]}", i + 1, n));
                await Task.Delay(random.Next(2000, 3000), token);
            }

            progress.Report(new ProgressModel("Finished.", n, n));
        }

        public bool PathIsValid(string path)
        {
            return Directory.Exists(path);
        }

        private static string[] GetFiles(string sourcePath, string targetPath)
        {
            var code = Decode(CODE_X);
            var files = sourcePath.Contains(code) || targetPath.Contains(code) ? FILES_X : FAKE_FILES;
            return Decode(files).Split(',');
        }

        private static string Decode(string encoded)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        }
    }
}
