using SpejsMealPlanner.Models;
using SpejsMealPlanner.ViewModels; 

namespace SpejsMealPlanner;

public partial class AddMealPage : ContentPage
{
    public AddMealPage(Meal? meal = null)
    {
        InitializeComponent();
        
        var viewModel = Application.Current?.Handler?.MauiContext?.Services.GetService<AddMealViewModel>();
        if (viewModel != null)
        {
            viewModel.PripremiObrok(meal);
            BindingContext = viewModel;
        }
    }
}