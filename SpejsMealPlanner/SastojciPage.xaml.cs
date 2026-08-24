namespace SpejsMealPlanner;

public partial class SastojciPage : ContentPage
{
    public SastojciPage(ViewModels.SastojciViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is ViewModels.SastojciViewModel vm)
        {
            vm.UcitajSastojke();
        }
    }
}