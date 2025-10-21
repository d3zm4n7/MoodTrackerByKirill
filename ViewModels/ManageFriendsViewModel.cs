// Файл: /ViewModels/ManageFriendsViewModel.cs
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input; // <-- Добавьте этот using для [RelayCommand]
using MoodTrackerByKirill.Models;
using MoodTrackerByKirill.Services; // <-- Добавьте этот using для сервиса

namespace MoodTrackerByKirill.ViewModels
{
    public partial class ManageFriendsViewModel : ObservableObject
    {
        private readonly DataStorageService _dataStorageService;

        [ObservableProperty]
        private string _newFriendName;

        public ObservableCollection<Friend> FriendsList { get; set; }

        // Мы "внедряем" сервис хранения данных через конструктор
        public ManageFriendsViewModel(DataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
            FriendsList = new ObservableCollection<Friend>();
            NewFriendName = string.Empty;

            // Загружаем друзей при создании ViewModel
            LoadFriendsAsync();
        }

        private async void LoadFriendsAsync()
        {
            // Загружаем список из JSON-файла
            var friends = await _dataStorageService.LoadFriendsAsync();

            // Очищаем текущий список и наполняем его загруженными данными
            FriendsList.Clear();
            foreach (var friend in friends)
            {
                FriendsList.Add(friend);
            }
        }

        // [RelayCommand] - "магия", которая создает AddFriendCommand
        // Эта команда будет вызвана при нажатии кнопки.
        [RelayCommand]
        private async Task AddFriendAsync()
        {
            // Проверка, что имя не пустое
            if (string.IsNullOrWhiteSpace(NewFriendName))
            {
                // Позже мы заменим это на PopUp (Тема 4)
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Имя друга не может быть пустым.", "OK");
                return;
            }

            // 1. Создаем новую модель
            var newFriend = new Friend
            {
                Name = NewFriendName,
                AvatarIcon = "default_avatar.png" // Используем заглушку
            };

            // 2. Добавляем в ObservableCollection (UI обновится сам)
            FriendsList.Add(newFriend);

            // 3. Сохраняем ВЕСЬ список в JSON
            await _dataStorageService.SaveFriendsAsync(FriendsList.ToList());

            // 4. Очищаем поле ввода (UI обновится сам)
            NewFriendName = string.Empty;
        }

        [RelayCommand]
        private async Task DeleteFriendAsync(Friend? friendToDelete)
        {
            if (friendToDelete == null)
            {
                return; // Ничего не делаем, если друг не передан
            }

            // Показываем подтверждение (Хорошая практика!)
            bool confirmed = await Application.Current.MainPage.DisplayAlert(
                "Delete friend?",
                $"Are you sure, that you want to delete {friendToDelete.Name}?",
                "Yes, delete",
                "Cancel");

            if (!confirmed)
            {
                return; // Пользователь передумал
            }

            // 1. Удаляем из ObservableCollection (UI обновится сам)
            FriendsList.Remove(friendToDelete);

            // 2. Сохраняем обновленный список в JSON
            await _dataStorageService.SaveFriendsAsync(FriendsList.ToList());
        }
        // ^-- КОНЕЦ НОВОЙ КОМАНДЫ --^
    }
}
