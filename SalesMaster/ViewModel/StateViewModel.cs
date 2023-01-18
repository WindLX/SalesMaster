using SalesMaster.Utils;
using System;
using System.Windows.Media;

namespace SalesMaster.ViewModel
{
    public class StateViewModel : ViewModelBase
    {
        public event Action<object, string> OnCanPageChange;

        private PageName currentPage;
        private StateName currentState;
        private string lineNumber = "0 行";
        private string chosenSalesList = "未选中清单";
        private SolidColorBrush backgroundColor;

        public string CurrentPage 
        {
            get => ConvertPageNameToString(currentPage);
            set
            {
                currentPage = ConvertStringToPageName(value);
                OnPropertyChanged();
            }
        }

        public string CurrentState
        {
            get => ConvertStateNameToString(currentState);
            set
            {
                currentState = ConvertStringToStateName(value);
                switch (ConvertStringToStateName(value))
                {
                    case StateName.View:
                        OnCanPageChange?.Invoke(true, "CanPageChange");
                        BackgroundColor = new SolidColorBrush(Color.FromRgb(0x31, 0x78, 0xc6));
                        break;
                    case StateName.Edit:
                        OnCanPageChange?.Invoke(false, "CanPageChange");
                        BackgroundColor = new SolidColorBrush(Color.FromRgb(0x09, 0x7e, 0x13));
                        break;
                    case StateName.Changed:
                        OnCanPageChange?.Invoke(false, "CanPageChange");
                        BackgroundColor = new SolidColorBrush(Color.FromRgb(0xb5, 0xab, 0x45));
                        break;
                    default:
                        OnCanPageChange?.Invoke(true, "CanPageChange");
                        BackgroundColor = new SolidColorBrush(Color.FromRgb(0x31, 0x78, 0xc6));
                        break;
                }
                OnPropertyChanged();
            }
        }

        public string LineNumber
        {
            get => lineNumber;
            set
            {
                lineNumber = value;
                OnPropertyChanged();
            }
        }

        public string ChosenSalesList 
        {
            get => chosenSalesList;
            set
            {
                chosenSalesList = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public StateViewModel()
        {
            OnCanPageChange += Broadcaster.Instace.Publish;
            Broadcaster.Instace.Subscribe(ChangePageName, "CurrentViewChanged");
            Broadcaster.Instace.Subscribe(ChangeStateName, "ChangeState");
            Broadcaster.Instace.Subscribe(ChangeLineNumber, "ChangeLineNumber");
            Broadcaster.Instace.Subscribe(ChangeChosenSalesList, "ChangeChosenSalesList");

            BackgroundColor = new SolidColorBrush(Color.FromRgb(0x31, 0x78, 0xc6));
            CurrentState = "视图";
        }

        private PageName ConvertStringToPageName(string pageName)
        {
            switch (pageName)
            {
                case "主页":
                    return PageName.Home;
                case "新建":
                    return PageName.New;
                case "搜索":
                    return PageName.Search;
                case "单位":
                    return PageName.Company;
                case "设置":
                    return PageName.Settings;
                default:
                    return PageName.Home;
            }
        }

        private string ConvertPageNameToString(PageName pageName)
        {
            switch (pageName)
            {
                case PageName.Home:
                    return "主页";
                case PageName.New:
                    return "新建";
                case PageName.Search:
                    return "搜索";
                case PageName.Company:
                    return "单位";
                case PageName.Settings:
                    return "设置";
                default:
                    throw new InvalidCastException("系统页面错误！");
            }
        }

        private StateName ConvertStringToStateName(string stateName)
        {
            switch (stateName)
            {
                case "视图":
                    return StateName.View;
                case "编辑":
                    return StateName.Edit;
                case "更改":
                    return StateName.Changed;
                default:
                    throw new InvalidCastException("系统状态错误！");
            }
        }

        private string ConvertStateNameToString(StateName stateName)
        {
            switch (stateName)
            {
                case StateName.View:
                    return "视图";
                case StateName.Edit:
                    return "编辑";
                case StateName.Changed:
                    return "更改";
                default:
                    return "视图";
            }
        }

        public void ChangePageName(object pageName) => CurrentPage = ConvertPageNameToString((PageName)pageName);
        public void ChangeStateName(object stateName) => CurrentState = ConvertStateNameToString((StateName)stateName);
        public void ChangeLineNumber(object lineNumber) => LineNumber = $"{lineNumber} 行";
        public void ChangeChosenSalesList(object salesListName) => ChosenSalesList = (string)salesListName;
    }
}
