﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BangumiX.Services;
using BangumiX.Views;
using System.Threading.Tasks;
using System.IO;
using BangumiX.Helper;

namespace BangumiX
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal, Environment.SpecialFolderOption.Create);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            Bangumi.Api.BangumiApi.Init(
                "bgm8905c514a1b94ec1",
                "b678c34dd896203627da308b6b453775",
                "BangumiGithubVersion",
                folderPath,
                folderPath,
                async (s) => await Task.FromResult(System.Text.Encoding.Default.GetBytes(s)),
                async (b) => await Task.FromResult(System.Text.Encoding.Default.GetString(b)));
            if (SettingHelper.Setting.UseBangumiData)
            {
                Bangumi.Data.BangumiData.Init(Path.Combine(folderPath, "bangumi-data"),
                    autoCheckCallback: (message) => NotificationHelper.Notify(message));
            }
            DependencyService.Register<ProgressDataStore>();
            DependencyService.Register<CollectionDataStore>();
            DependencyService.Register<CalendarDataStore>();
            if (Bangumi.Api.BangumiApi.BgmOAuth.IsLogin)
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected async override void OnSleep()
        {
            await Bangumi.Api.BangumiApi.BgmCache.WriteToFile();
        }

        protected override void OnResume()
        {
        }
    }
}
