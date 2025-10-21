using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTrackerByKirill.Models
{
    public class Friend
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? AvatarIcon { get; set; } // Будем хранить имя файла (н.п. "avatar1.png")

        public Friend()
        {
            Id = Guid.NewGuid();
        }
    }
}
