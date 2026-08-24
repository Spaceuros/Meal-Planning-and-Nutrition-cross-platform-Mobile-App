namespace SpejsMealPlanner;

public partial class MealsManagerPage : ContentPage
{
    public MealsManagerPage(ViewModels.MealsManagerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is ViewModels.MealsManagerViewModel vm)
        {
            vm.UcitajObroke();
        }
    }
}