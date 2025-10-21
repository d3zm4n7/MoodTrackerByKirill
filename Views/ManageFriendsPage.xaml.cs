// Файл: /Views/ManageFriendsPage.xaml.cs
using MoodTrackerByKirill.ViewModels;

namespace MoodTrackerByKirill.Views;

public partial class ManageFriendsPage : ContentPage
{
    // Принимаем ViewModel через Dependency Injection
    public ManageFriendsPage(ManageFriendsViewModel viewModel)
    {
        InitializeComponent();
        // Устанавливаем ViewModel как "контекст данных" для XAML
        BindingContext = viewModel;
    }
}