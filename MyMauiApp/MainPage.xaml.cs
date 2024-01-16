namespace MyMauiApp
{
    public partial class MainPage : ContentPage
    {
        //Répondre à l’appui sur le bouton TranslateButton
        public MainPage()
        {
            InitializeComponent();
        }
        string translatedNumber;

        //La méthode OnTranslate extrait le numéro de téléphone de la propriété Text du contrôle Entry, et le transmet à la méthode ToNumber statique de la classe PhonewordTranslator 
        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = PhoneNumberText.Text;
            translatedNumber = MyMauiApp.PhonewordTranslator.ToNumber(enteredNumber);

            if (!string.IsNullOrEmpty(translatedNumber))
            {
                //code pour modifier la propriété OnTranslate du bouton Call (Appeler) afin d’inclure le numéro de téléphone une fois celui-ci converti
                CallButton.IsEnabled = true;
                CallButton.Text = "Call " + translatedNumber;
            }
            else
            {
                CallButton.IsEnabled = false;
                CallButton.Text = "Call";
            }
        }

        //Créer la méthode d’événement pour le bouton CallButton

        //méthode de gestion des événements OnCall (asynchrone)
        async void OnCall(object sender, System.EventArgs e)
        {
            if (await this.DisplayAlert(
        "Dial a Number",
        "Would you like to call " + translatedNumber + "?",
        "Yes",
        "No")) //demander à l’utilisateur, à l’aide de la méthode Page.DisplayAlert, s’il souhaite composer le numéro.
            {
                try
                {
                    if (PhoneDialer.Default.IsSupported)
                        PhoneDialer.Default.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (Exception)
                {
                    // Other error has occurred.
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }
            }
        }
    }
}