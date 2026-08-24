namespace SpejsMealPlanner;

public partial class SelectMealPage : ContentPage
{
    private readonly ViewModels.SelectMealViewModel _viewModel;

    public SelectMealPage(ViewModels.SelectMealViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.UcitajMeni();
    }
}