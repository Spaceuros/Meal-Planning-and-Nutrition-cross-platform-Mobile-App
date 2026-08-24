namespace SpejsMealPlanner;

public partial class MainPage : ContentPage
{
    public MainPage(ViewModels.MainPageViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is ViewModels.MainPageViewModel vm)
        {
            vm.UcitajPodatke();
        }
    }
}