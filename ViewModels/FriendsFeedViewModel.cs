// Файл: /ViewModels/FriendsFeedViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodTrackerByKirill.Models;
using MoodTrackerByKirill.Services;
using System.Collections.ObjectModel;

namespace MoodTrackerByKirill.ViewModels
{
    // Это специальный маленький ViewModel, который мы будем использовать
    // ТОЛЬКО для отображения в списке. Он объединяет Friend + сгенерированное Настроение.
    public partial class FriendMoodDisplay : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private int _moodScore;

        // Мы могли бы добавить сюда иконку-смайлик, зависящую от MoodScore
        // public string MoodIcon => MoodScore > 7 ? "smile.png" : "sad.png";
    }


    public partial class FriendsFeedViewModel : ObservableObject
    {
        private readonly DataStorageService _dataStorageService;

        // Это список, который увидит пользователь
        public ObservableCollection<FriendMoodDisplay> FriendsFeed { get; set; }

        public FriendsFeedViewModel(DataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
            FriendsFeed = new ObservableCollection<FriendMoodDisplay>();
        }

        [RelayCommand]
        private async Task LoadFeedAsync()
        {
            FriendsFeed.Clear();

            // 1. Загружаем "шаблоны" друзей из JSON
            var friends = await _dataStorageService.LoadFriendsAsync();

            // 2. Тема "Kellade rakendused" (Логика на основе времени)
            foreach (var friend in friends)
            {
                // 3. Генерируем настроение
                int dailyMood = GenerateDailyMood(friend.Id);

                // 4. Создаем ViewModel для отображения
                FriendsFeed.Add(new FriendMoodDisplay
                {
                    Name = friend.Name,
                    MoodScore = dailyMood
                });
            }
        }

        /// <summary>
        /// Генерирует "стабильное" настроение для друга на текущий день.
        /// </summary>
        private int GenerateDailyMood(Guid friendId)
        {
            // Мы используем ID друга и ТЕКУЩУЮ ДАТУ как "зерно" (seed)
            // для генератора случайных чисел.
            // Это гарантирует, что у одного и того же друга
            // будет одно и то же настроение ВЕСЬ ДЕНЬ.
            int seed = friendId.GetHashCode() ^ DateTime.Today.Ticks.GetHashCode();
            Random dailyRng = new Random(seed);

            // Генерируем число от 1 до 10
            return dailyRng.Next(1, 11);
        }
    }
}