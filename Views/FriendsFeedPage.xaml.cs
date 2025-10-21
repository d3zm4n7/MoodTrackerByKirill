// Файл: /Views/FriendsFeedPage.xaml.cs
using MoodTrackerByKirill.ViewModels;

namespace MoodTrackerByKirill.Views;

public partial class FriendsFeedPage : ContentPage
{
    private readonly FriendsFeedViewModel _viewModel;

    public FriendsFeedPage(FriendsFeedViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    // Каждый раз, когда пользователь открывает вкладку "Лента"
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Мы запускаем команду загрузки, чтобы лента обновилась
        // (Особенно важно, если пользователь только что добавил нового друга)
        _viewModel.LoadFeedCommand.Execute(null);
    }
}