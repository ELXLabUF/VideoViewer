// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace VideoViewerDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool autoPlay = false;
        private MyVideos videos;
        public MainWindow()
        {
            InitializeComponent();
            videos = this.Resources["Vids"] as MyVideos;
            //videos.Directory = Environment.CurrentDirectory + "\\media";
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            autoPlay = true;
        }

        private void MainVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (!autoPlay)
            {
                return;
            }
            MediaElement element = (MediaElement)sender;
            // Find next video
            int index = -1;
            for (int i = 0; i < videos.Count; ++i)
            {
                if (videos[i].Uri == element.Source)
                {
                    index = i;
                    break;
                }
            }
            index += 1;
            // Play it if found
            if (index >= 0 && index < videos.Count)
            {
                element.Source = videos[index].Uri;
            }
            // Stop auto-playing if not found
            else
            {
                autoPlay = false;
            }
        }

        //v0.1

        private void OnVideosDirChangeClick(object sender, RoutedEventArgs e)
        {
            videos.Directory = VideosDir.Text;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            VideosDir.Text = Environment.CurrentDirectory + "\\media";
        }

        private void VideosDir_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Button_Browse(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    VideosDir.Text = fbd.SelectedPath;
                    videos.Directory = VideosDir.Text;
                }
            }
        }
        //end



    }
}

//self-added, all of below

public static class Extensions
{
    public static string UpperFolder(this string pFolderName, Int32 pLevel)
    {
        List<string> TheList = new List<string>();

        while (!string.IsNullOrEmpty(pFolderName))
        {
            var temp = Directory.GetParent(pFolderName);
            if (temp == null)
            {
                break;
            }

            pFolderName = Directory.GetParent(pFolderName).FullName;
            TheList.Add(pFolderName);

        }

        if (TheList.Count > 0 && pLevel > 0)
        {
            if (pLevel - 1 <= TheList.Count - 1)
            {
                return TheList[pLevel - 1];
            }
            else
            {
                return pFolderName;
            }
        }
        else
        {
            return pFolderName;
        }
    }
    public static string SolutionFolder(this string pSender)
    {
        return pSender.UpperFolder(4);
    }
}