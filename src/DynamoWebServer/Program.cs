﻿using System;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Windows;

using Dynamo.Controls;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo;

using DynamoUtilities;

namespace DynamoWebServer
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            DynamoPathManager.Instance.InitializeCore(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            DynamoPathManager.PreloadAsmLibraries(DynamoPathManager.Instance);

            var model = DynamoModel.Start(
                new DynamoModel.StartConfiguration()
                {
                    Preferences = PreferenceSettings.Load()
                });
            model.MaxTesselationDivisions = int.Parse(ConfigurationManager.AppSettings["MaxTesselationDivisions"]);

            var webSocketServer = new WebServer(model, new WebSocket());

            webSocketServer.Start();
            //*
            var viewModel = DynamoViewModel.Start(
                new DynamoViewModel.StartConfiguration()
                {
                    DynamoModel = model
                });

            var view = new DynamoView(viewModel);
            //*/
            var app = new Application();
            app.Exit += webSocketServer.ProcessExit;
            app.Run(view);
        }
    }
}
