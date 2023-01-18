using SalesMaster.Utils;
using SalesMaster.Model;
using SalesMaster.View;
using SalesMaster.Service;
using System;
using System.IO;
using System.Windows.Forms;

namespace SalesMaster.ViewModel
{
    public class SettingsViewModel : ViewModelBase, IChangeState
    {
        public event Action<object, string> OnChangeState;

        private IConfigService configService;
        private Config config;

        public string ExportLocation
        {
            get => Path.GetFullPath(config.ExportLocation);
            set
            {
                config.ExportLocation = value;
                OnPropertyChanged();
            }
        }

        public bool IsAutoSaveCompany
        {
            get => config.IsAutoSaveCompany;
            set
            {
                OnChangeState?.Invoke(StateName.Changed, "ChangeState");
                config.IsAutoSaveCompany = value;
                OnPropertyChanged();
            }
        }

        public bool IsAutoUseNowDate
        {
            get => config.IsAutoUseNowDate;
            set
            {
                OnChangeState?.Invoke(StateName.Changed, "ChangeState");
                config.IsAutoUseNowDate = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ChangeExportLocation { get; set; }
        public RelayCommand SaveConfig { get; set; }
        public RelayCommand LoadDefaultConfig { get; set; }

        public SettingsViewModel()
        {
            configService = new JsonConfigService();

            ResetView();

            ChangeExportLocation = new RelayCommand(new Action<object>(OpenPathChooser));
            SaveConfig = new RelayCommand(new Action<object>((parameter) =>
            {
                configService.SetConfig(config);
                OnChangeState?.Invoke(StateName.View, "ChangeState");
                NotifyBox notifyBox = new NotifyBox { NotifyMessage = "设置已保存" };
                notifyBox.Show();
            }));
            LoadDefaultConfig = new RelayCommand(new Action<object>((parameter) => 
            { 
                configService.LoadDefaultConfig();
                ResetView();
                OnChangeState?.Invoke(StateName.View, "ChangeState");
                NotifyBox notifyBox = new NotifyBox { NotifyMessage = "已加载默认设置" };
                notifyBox.Show();
            }));

            OnChangeState += Broadcaster.Instace.Publish;
        }

        private void ResetView()
        {
            config = configService.GetConfig();
            ExportLocation = config.ExportLocation;
            IsAutoSaveCompany = config.IsAutoSaveCompany;
            IsAutoUseNowDate = config.IsAutoUseNowDate;
        }

        private void OpenPathChooser(object parameter)
        {
            OnChangeState?.Invoke(StateName.Edit, "ChangeState");
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "请选择销货清单导出文件的路径(文件夹)",
                ShowNewFolderButton = false,
            };
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.ShowDialog();
            if (folderBrowserDialog.SelectedPath == string.Empty)
            {
                return;
            }
            string epath = folderBrowserDialog.SelectedPath;
            if (epath == ExportLocation)
                OnChangeState?.Invoke(StateName.Edit, "ChangeState");
            else
                OnChangeState?.Invoke(StateName.Changed, "ChangeState");
            ExportLocation = epath;
        }
    }
}
