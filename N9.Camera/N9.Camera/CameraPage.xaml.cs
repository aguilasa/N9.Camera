using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Media;
using Plugin.Media.Abstractions;

namespace N9.Camera
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        public CameraPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //Primeiro verificamos se o dispositivo possui uma câmera disponível usando os métodos IsCameraAvailable e IsTakePhotoSupported
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Nenhuma Câmera", ":( Nenhuma Câmera disponível.", "OK");
                return;
            }

            //A seguir definimos as opções de armazenamento como local e nome da imagem
            var armazenamento = new StoreCameraMediaOptions()
            {
                SaveToAlbum = true,
                Name = "MinhaFoto.jpg"
            };

            // Finalmente tiramos a foto e verificamos se a foto não é null para obter o resultado e armazenar na propriedade ImageSource da view ImageView
            var foto = await CrossMedia.Current.TakePhotoAsync(armazenamento);

            if (foto == null)
                return;
            imgFoto.Source = ImageSource.FromStream(() =>
            {
                var stream = foto.GetStream();
                foto.Dispose();
                return stream;
            });
        }

        // No evento Click do botão - Selecionar Imagem - usamos o método PickPhotoAsycn() para obter uma imagem suporta exibindo-a no ImageView
        private async void btnSelecionarImagem_Clicked(object sender, EventArgs e)
        {
            if (CrossMedia.Current.IsTakePhotoSupported)
            {
                var imagem = await CrossMedia.Current.PickPhotoAsync();
                if (imagem != null)
                {
                    imgFoto.Source = ImageSource.FromStream(() =>
                    {
                        var stream = imagem.GetStream();
                        imagem.Dispose();
                        return stream;
                    });
                }
            }
        }
    }
}