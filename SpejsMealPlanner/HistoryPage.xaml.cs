namespace SpejsMealPlanner;

public partial class HistoryPage : ContentPage
{
    public HistoryPage(ViewModels.HistoryPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is ViewModels.HistoryPageViewModel vm)
        {
            vm.UcitajIstoriju();
        }
    }
}