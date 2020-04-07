using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace PhotoRecognizer
{
    class Photo
    {
        public static Image img = new Image();
        public static async void AddPhoto()
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                img.Source = ImageSource.FromFile(photo.Path);
                Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(photo.Path));
                MainActivity.imageView.SetImageURI(uri);
            }
        }
        public static async void TakePhoto()
        {
            /*if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Sample",
                    Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                });

                if (file == null)
                    return;

                img.Source = ImageSource.FromFile(file.Path);
            }*/
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                img.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(photo.Path));
                MainActivity.imageView.SetImageURI(uri);
            }
        }

    }
}