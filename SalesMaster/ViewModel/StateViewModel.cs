using SalesMaster.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SalesMaster.ViewModel
{
    public class StateViewModel : ViewModelBase
    {
        private PageName currentPage;
        private StateName currentState;
        private Color stateColor;

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
                OnPropertyChanged();
            }
        }

        public StateViewModel()
        {
            NavigationViewModel.Instace.OnCurrentViewChanged += ChangePageName;
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
                    return "？？？";
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
                    return StateName.View;
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

        private Color ConvertColor(StateName stateName)
        {
            switch (stateName)
            {
                case StateName.View:
                    return new Color();
                case StateName.Edit:
                    return new Color();
                case StateName.Changed:
                    return new Color();
                default:
                    return new Color();
            }
        }

        public void ChangePageName(PageName pageName) => CurrentPage = ConvertPageNameToString(pageName);

        public void ChnageStateName(StateName stateName) => CurrentState = ConvertStateNameToString(stateName);
    }
}
