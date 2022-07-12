namespace WpfApp1.ViewModels
{
    public class InformationViewModel
    {
        public string InformationText { get; set; }

        public InformationViewModel(string informationText)
        {
            this.InformationText = informationText;
        }
    }
}
