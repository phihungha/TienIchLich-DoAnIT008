using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    public class EventEditorViewModel : ViewModelBase
    {
        private MasterViewModel masterVM;
        private ICommand cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand;
            }
        }

        public EventEditorViewModel(MasterViewModel masterVM)
        {
            this.masterVM = masterVM;
            this.cancelCommand = new RelayCommand(i => masterVM.NavigateToMainView(), i => true);
        }


    }
}
