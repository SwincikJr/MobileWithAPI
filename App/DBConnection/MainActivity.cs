using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using GraphQL.Client;
using GraphQL.Common.Request;

namespace DBConnection
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            EditText usernameText = FindViewById<EditText>(Resource.Id.UsernameText);
            EditText passwordText = FindViewById<EditText>(Resource.Id.PasswordText);
            Button submitButton = FindViewById<Button>(Resource.Id.SubmitButton);
            TextView textView = FindViewById<TextView>(Resource.Id.textView3);

            submitButton.Click += async delegate
            {
                if (!string.IsNullOrWhiteSpace(usernameText.Text.ToString())
                        && !string.IsNullOrWhiteSpace(passwordText.Text.ToString())
                        && !string.IsNullOrEmpty(usernameText.Text.ToString())
                        && !string.IsNullOrEmpty(passwordText.Text.ToString()))
                {
                    HttpClient client = new HttpClient();
                    try
                    {
                        string nome = usernameText.Text.ToString();
                        string senha = passwordText.Text.ToString();
                        string uri = "http://192.168.0.13:59414/Home/Login/" + nome + "/" + senha;
                        textView.Text = "Requisição: " + uri;
                        var result = await client.GetStringAsync(uri);

                        User user = JsonConvert.DeserializeObject<User>(result.ToString());
                        if (user != null && user.UserID != null)
                        {
                            textView.Text = "Autenticação bem sucedida!";
                        }
                        else
                        {
                            textView.Text = "Usuário e/ou senha incorreto(s)";
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        textView.Text = textView.Text + "--->" + e.ToString();
                    } 
                }
                else
                {
                    textView.Text = "Usuário e senha são campos obrigatórios!";
                }
            };  
        }

        public class User
        {
            public string UserID { get; set; }
        }
    }
}

