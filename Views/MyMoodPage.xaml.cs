// Файл: /Views/MyMoodPage.xaml.cs
using MoodTrackerByKirill.ViewModels;

namespace MoodTrackerByKirill.Views;

public partial class MyMoodPage : ContentPage
{
    private readonly MyMoodViewModel _viewModel;

    // Внедряем ViewModel
    public MyMoodPage(MyMoodViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel; // Сохраняем ссылку
    }

    // Этот метод вызывается каждый раз, когда страница ПОЯВЛЯЕТСЯ на экране
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Мы вручную вызываем команду загрузки,
        // чтобы история (CarouselView) всегда была актуальной
        _viewModel.LoadMoodHistoryCommand.Execute(null);
    }
}