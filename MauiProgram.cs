using Microsoft.Extensions.Logging;
using MoodTrackerByKirill.Services;
using MoodTrackerByKirill.Views;
using MoodTrackerByKirill.ViewModels;

namespace MoodTrackerByKirill
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
            builder.Services.AddSingleton<DataStorageService>();
            builder.Services.AddSingleton<MoodTrackerByKirill.Views.ManageFriendsPage>();
            builder.Services.AddSingleton<ManageFriendsViewModel>();
            builder.Services.AddSingleton<MoodTrackerByKirill.Views.MyMoodPage>();
            builder.Services.AddSingleton<MyMoodViewModel>();
            builder.Services.AddSingleton<MoodTrackerByKirill.Views.FriendsFeedPage>();
            builder.Services.AddSingleton<FriendsFeedViewModel>();
#endif

            return builder.Build();
        }
    }
}
