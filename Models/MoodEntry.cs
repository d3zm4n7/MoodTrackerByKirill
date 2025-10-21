using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTrackerByKirill.Models
{
    public class MoodEntry
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int MoodScore { get; set; } // Наше значение от 1 до 10 (из Slider)
        public string Note { get; set; }

        public MoodEntry()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Today;
            MoodScore = 5; // Значение по умолчанию
            Note = string.Empty;
        }
    }
}
