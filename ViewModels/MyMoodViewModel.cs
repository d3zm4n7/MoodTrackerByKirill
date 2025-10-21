// Файл: /ViewModels/MyMoodViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodTrackerByKirill.Models;
using MoodTrackerByKirill.Services;
using System.Collections.ObjectModel;

namespace MoodTrackerByKirill.ViewModels
{
    public partial class MyMoodViewModel : ObservableObject
    {
        private readonly DataStorageService _dataStorageService;

        // --- Свойства для новой записи (привязка к UI) ---

        [ObservableProperty]
        private int _moodScore = 5; // Значение для Slider (1-10)

        [ObservableProperty]
        private string _moodNote = string.Empty; // Значение для Editor

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Today; // Значение для DatePicker

        // --- Список для истории ---
        public ObservableCollection<MoodEntry> MoodHistory { get; set; }

        public MyMoodViewModel(DataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
            MoodHistory = new ObservableCollection<MoodEntry>();

            // Загружаем историю сразу при создании
            // Мы также сделаем команду, чтобы обновлять ее при входе на страницу
        }

        // Команда для сохранения
        [RelayCommand]
        private async Task SaveMoodAsync()
        {
            // 1. Создаем новую запись из данных UI
            var newEntry = new MoodEntry
            {
                MoodScore = this.MoodScore,
                Note = this.MoodNote,
                Date = this.SelectedDate.Date // Убедимся, что время "обнулено"
            };

            // 2. Загружаем *весь* существующий список
            var allEntries = await _dataStorageService.LoadMoodEntriesAsync();

            // 3. Проверяем, есть ли уже запись за эту дату
            var existingEntry = allEntries.FirstOrDefault(e => e.Date.Date == newEntry.Date);
            if (existingEntry != null)
            {
                // Если есть - удаляем старую
                allEntries.Remove(existingEntry);
            }

            // 4. Добавляем новую (или обновленную) запись
            allEntries.Add(newEntry);

            // 5. Сохраняем полный список обратно в JSON
            await _dataStorageService.SaveMoodEntriesAsync(allEntries);

            // 6. Показываем PopUp (Тема 4)
            await Application.Current.MainPage.DisplayAlert("Сохранено", "Ваше настроение успешно записано!", "OK");

            // 7. Обновляем историю в CarouselView
            await LoadMoodHistoryAsync();
        }

        // Команда для загрузки истории
        [RelayCommand]
        private async Task LoadMoodHistoryAsync()
        {
            var entries = await _dataStorageService.LoadMoodEntriesAsync();

            MoodHistory.Clear();

            // Сортируем, чтобы последние были первыми
            var sortedEntries = entries.OrderByDescending(e => e.Date);

            foreach (var entry in sortedEntries)
            {
                MoodHistory.Add(entry);
            }
        }
    }
}