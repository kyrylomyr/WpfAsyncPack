using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAsyncPack.Base;
using WpfAsyncPack.Command;

namespace FileCopyApp
{
    internal class MainViewModel : AsyncBindableBase
    {
        private readonly FileService _fileService;

        private string _sourcePath;
        private string _targetPath;
        private int _copyProgress;
        private int _maxProgress;
        private string _log;

        public MainViewModel(FileService fileService)
        {
            _fileService = fileService;

            SelectSourcePathCommand = new SyncCommand(p => { SourcePath = _fileService.SelectPath(); }, p => CopyCommand.Task.IsNotRunning);
            SelectTargetPathCommand = new SyncCommand(p => { TargetPath = _fileService.SelectPath(); }, p => CopyCommand.Task.IsNotRunning);

            CopyCommand = new ProgressiveAsyncCommand<ProgressModel>(
                async (parameter, token, progress) =>
                      {
                          Log = string.Empty;
                          try
                          {
                              await _fileService.CopyFiles(SourcePath, TargetPath, progress, token);
                          }
                          catch (TaskCanceledException)
                          {
                              progress.Report(new ProgressModel("Cancelled."));
                          }
                      },
                progress =>
                {
                    CopyProgress = progress.ProgressValue;
                    MaxProgress = progress.MaxProgress;
                    Log += $"{progress.LogMessage}{Environment.NewLine}";
                },
                parameter => _fileService.PathIsValid(SourcePath) && _fileService.PathIsValid(TargetPath));
        }

        public MainViewModel() : this(new FileService())
        {
        }

        public string SourcePath
        {
            get { return _sourcePath; }
            set { SetProperty(ref _sourcePath, value); }
        }

        public ICommand SelectSourcePathCommand { get; }

        public string TargetPath
        {
            get { return _targetPath; }
            set { SetProperty(ref _targetPath, value); }
        }

        public ICommand SelectTargetPathCommand { get; }

        public IAsyncCommand CopyCommand { get; }

        public int CopyProgress
        {
            get { return _copyProgress; }
            set { SetProperty(ref _copyProgress, value); }
        }

        public int MaxProgress
        {
            get { return _maxProgress; }
            set { SetProperty(ref _maxProgress, value); }
        }

        public string Log
        {
            get { return _log; }
            set { SetProperty(ref _log, value); }
        }
    }
}