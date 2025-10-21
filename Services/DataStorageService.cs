using MoodTrackerByKirill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoodTrackerByKirill.Services
{
    public class DataStorageService
    {
        // Путь к файлу, где будут храниться друзья
        private readonly string _friendsFilePath;

        // Путь к файлу, где будут храниться записи настроения
        private readonly string _moodEntriesFilePath;

        public DataStorageService()
        {
            // FileSystem.AppDataDirectory - это безопасная папка приложения на любом устройстве (iOS, Android)
            _friendsFilePath = Path.Combine(FileSystem.AppDataDirectory, "friends.json");
            _moodEntriesFilePath = Path.Combine(FileSystem.AppDataDirectory, "moodentries.json");
        }

        // --- Методы для Друзей (Friends) ---

        public async Task SaveFriendsAsync(List<Friend> friends)
        {
            // Сериализуем список друзей в строку JSON
            string json = JsonSerializer.Serialize(friends);
            // Асинхронно записываем строку в файл
            await File.WriteAllTextAsync(_friendsFilePath, json);
        }

        public async Task<List<Friend>> LoadFriendsAsync()
        {
            // Если файл еще не существует (первый запуск), возвращаем пустой список
            if (!File.Exists(_friendsFilePath))
            {
                return new List<Friend>();
            }

            // Читаем весь текст из файла
            string json = await File.ReadAllTextAsync(_friendsFilePath);

            // Если файл пустой, тоже возвращаем пустой список
            if (string.IsNullOrEmpty(json))
            {
                return new List<Friend>();
            }

            // Десериализуем JSON обратно в List<Friend>
            return JsonSerializer.Deserialize<List<Friend>>(json);
        }

        // --- Методы для Настроения (MoodEntries) ---

        public async Task SaveMoodEntriesAsync(List<MoodEntry> entries)
        {
            string json = JsonSerializer.Serialize(entries);
            await File.WriteAllTextAsync(_moodEntriesFilePath, json);
        }

        public async Task<List<MoodEntry>> LoadMoodEntriesAsync()
        {
            if (!File.Exists(_moodEntriesFilePath))
            {
                return new List<MoodEntry>();
            }

            string json = await File.ReadAllTextAsync(_moodEntriesFilePath);

            if (string.IsNullOrEmpty(json))
            {
                return new List<MoodEntry>();
            }

            return JsonSerializer.Deserialize<List<MoodEntry>>(json);
        }
    }
}
