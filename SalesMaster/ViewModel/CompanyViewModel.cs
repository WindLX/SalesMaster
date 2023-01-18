using SalesMaster.Model;
using SalesMaster.Utils;
using SalesMaster.Service;
using SalesMaster.View;
using System;
using System.Collections.ObjectModel;

namespace SalesMaster.ViewModel
{
    public class CompanyViewModel : ViewModelBase, IChangeState, IChangeLineNumber
    {
        public event Action<object, string> OnChangeState;
        public event Action<object, string> OnChangeLineNumber;

        private EditModern editModern = EditModern.None;
        private ICompanyService companyService;
        private Companies companies;
        private string editTargetCompany = "";

        private ObservableCollection<CompanyInfoViewModel> companyInfos = new ObservableCollection<CompanyInfoViewModel>();
        private bool canInput = false;
        private bool canConfirm = false;
        private bool canAdd = true;
        private string inputText = "";
        
        public RelayCommand Add { get; set; }
        public RelayCommand Confirm { get; set; }

        public ObservableCollection<CompanyInfoViewModel> CompanyInfos
        {
            get => companyInfos;
            set
            {
                companyInfos = value;
                OnPropertyChanged();
            }
        }

        public bool CanInput 
        {
            get => canInput;
            set
            {
                canInput = value;
                OnPropertyChanged();
            }
        }

        public bool CanAdd
        {
            get => canAdd;
            set
            {
                canAdd = value;
                OnPropertyChanged();
            }
        }

        public bool CanConfirm
        {
            get => canConfirm;
            set
            {
                canConfirm = value;
                OnPropertyChanged();
            }
        }

        public string InputText
        {
            get => inputText;
            set
            {
                inputText = value;
                OnPropertyChanged();
            }
        }

        public CompanyViewModel()
        {
            companyService = new JsonCompanyService();

            Add = new RelayCommand(new Action<object>(AddNewCompany));
            Confirm = new RelayCommand(new Action<object>(ConfirmAdd));

            OnChangeState += Broadcaster.Instace.Publish;
            OnChangeLineNumber += Broadcaster.Instace.Publish;

            Broadcaster.Instace.Subscribe(ChangeEditModern, "ChangeCompanyEditModern");
            Broadcaster.Instace.Subscribe(DeleteCompany, "DeleteCompany");
            Broadcaster.Instace.Subscribe((targetCompany) => editTargetCompany = (string)targetCompany, "EditCompany");
            Broadcaster.Instace.Subscribe(EditCompany, "FinishEditCompany");

            ResetView();
        }

        private void ResetView()
        {
            int i = 0;
            companies = companyService.GetCompanies();

            CompanyInfos.Clear();

            foreach (var companyName in companies.CompanyName)
            {
                CompanyInfos.Add(new CompanyInfoViewModel { CompanyIndex = i, CompanyName = companyName });
                i++;
            }

            OnChangeLineNumber?.Invoke(i, "ChangeLineNumber");
        }

        private void JudgeButtonAvaiable()
        {
            switch (editModern)
            {
                case EditModern.None:
                    CanInput = false;
                    CanAdd = true;
                    CanConfirm = false;
                    foreach (var item in companyInfos)
                    {
                        item.CanEdit = true;
                        item.CanDelete = true;
                        item.CanInput = false;
                    }
                    break;
                case EditModern.Add:
                    CanInput = true;
                    CanAdd = false;
                    CanConfirm = true;
                    foreach (var item in companyInfos)
                    {
                        item.CanEdit = false;
                        item.CanDelete = false;
                        item.CanInput = false;
                    }
                    break;
                case EditModern.Edit:
                    CanInput = false;
                    CanAdd = false;
                    CanConfirm = false;
                    foreach (var item in companyInfos)
                    {
                        item.CanEdit = false;
                        item.CanDelete = false;
                    }
                    break;
            }
        }

        private void ChangeEditModern(object editModern)
        {
            this.editModern = (EditModern)editModern;
            JudgeButtonAvaiable();
        }

        private void AddNewCompany(object parameter)
        {
            OnChangeState?.Invoke(StateName.Edit, "ChangeState");
            ChangeEditModern(EditModern.Add);
        }

        private void EditCompany(object newCompany)
        {
            companyService.ChangeCompany(editTargetCompany, (string)newCompany);
            editTargetCompany = "";
            ResetView();
        }

        private void DeleteCompany(object targetCompany)
        {
            companyService.DeleteCompany((string)targetCompany);
            ResetView();
        }

        private void ConfirmAdd(object parameter)
        {
            ChangeEditModern(EditModern.None);
            if (parameter != null)
            {
                companyService.AddCompany((string)parameter);
                ResetView();
            }
            OnChangeState?.Invoke(StateName.View, "ChangeState");
            InputText = "";

            NotifyBox notifyBox = new NotifyBox { NotifyMessage = "添加单位成功" };
            notifyBox.Show();
        }
    }

    public class CompanyInfoViewModel : ViewModelBase, IChangeState
    {
        public event Action<object, string> OnChangeState;
        public event Action<object, string> OnChangeEditModern;
        public event Action<object, string> OnDelete;
        public event Action<object, string> OnEdit;
        public event Action<object, string> OnFinishEdit;

        private bool canEdit = true;
        private bool canInput = false;
        private bool canDelete = true;
        private string editImage = "/Resources/Pictures/Pen.png";
        private string companyName = "";

        public int CompanyIndex { get; set; }
        public RelayCommand Edit { get; set; }
        public RelayCommand Delete { get; set; }

        public bool CanEdit
        {
            get => canEdit;
            set
            {
                canEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanInput
        {
            get => canInput;
            set
            {
                canInput = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete
        {
            get => canDelete;
            set
            {
                canDelete = value;
                OnPropertyChanged();
            }
        }

        public string EditImage
        {
            get => editImage;
            set
            {
                editImage = value;
                OnPropertyChanged();
            }
        }

        public string CompanyName
        {
            get => companyName;
            set
            {
                companyName = value;
                OnPropertyChanged();
            }
        }

        public CompanyInfoViewModel()
        {
            OnChangeState += Broadcaster.Instace.Publish;
            OnChangeEditModern += Broadcaster.Instace.Publish;
            OnDelete += Broadcaster.Instace.Publish;
            OnEdit += Broadcaster.Instace.Publish;
            OnFinishEdit += Broadcaster.Instace.Publish;

            Edit = new RelayCommand(new Action<object>(EditCompany));
            Delete = new RelayCommand(new Action<object>(DeleteCompany));
        }

        private void EditCompany(object newCompany)
        {
            if (CanInput == false)
            {
                OnChangeState?.Invoke(StateName.Edit, "ChangeState");
                OnChangeEditModern?.Invoke(EditModern.Edit, "ChangeCompanyEditModern");
                EditImage = "/Resources/Pictures/Confirm.png";
                OnEdit?.Invoke(CompanyName, "EditCompany");
                CanEdit = true;
                CanInput = true;
            }
            else
            {
                OnChangeState?.Invoke(StateName.View, "ChangeState");
                OnChangeEditModern?.Invoke(EditModern.None, "ChangeCompanyEditModern");
                CanInput = false;
                CompanyName = (string)newCompany;
                EditImage = "/Resources/Pictures/Pen.png";
                if (newCompany != null)
                    OnFinishEdit?.Invoke(CompanyName, "FinishEditCompany");

                NotifyBox notifyBox = new NotifyBox { NotifyMessage = "变更单位成功" };
                notifyBox.Show();
            }
        }

        private void DeleteCompany(object targetCompany)
        {
            OnChangeState?.Invoke(StateName.View, "ChangeState");
            OnChangeEditModern?.Invoke(EditModern.None, "ChangeCompanyEditModern");
            OnDelete?.Invoke(targetCompany, "DeleteCompany");

            NotifyBox notifyBox = new NotifyBox { NotifyMessage = "删除单位成功" };
            notifyBox.Show();
        }
    }
}
