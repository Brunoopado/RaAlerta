using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace raAlerta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            SetStartup();

            if (!File.Exists(Application.StartupPath + @"\dados.txt"))
                File.WriteAllText(Application.StartupPath + @"\dados.txt",DateTime.Now.AddMonths(-6).ToString());

            //File.Delete(Application.StartupPath + @"\dados.txt");

            Console.WriteLine(File.ReadAllText(Application.StartupPath + @"\dados.txt"));

            if (DateTime.Parse(File.ReadAllText(Application.StartupPath + @"\dados.txt")) >= DateTime.Now)
                this.Close();
            else { 
                string tempoEspera = File.ReadAllText(Application.StartupPath + @"\dados.txt");

                if (tempoEspera == "NUNCA")
                {
                    return;
                }
                else
                {
                    txtMsg.Text = "Já se passaram mais de 6 meses desde a última manutenção!";
                    txtMsg2.Text = "Para manter sua máquina em perfeito estado, entre em contato para uma revisão!";
                    radioButton1.Checked = true;
                    this.StartPosition = FormStartPosition.Manual;
                    Rectangle screen = Screen.PrimaryScreen.WorkingArea;

                    for (int i = screen.Height; i > screen.Height - this.Height; i = i - 13)
                    {
                        this.Location = new Point(screen.Width - this.Width, i);
                        await Task.Delay(1);
                    }
                    this.Location = new Point(screen.Width - this.Width, screen.Height - this.Height);
                }
            }
        }
        public static void SetStartup()
        {
            try
            {
                const string appName = "RaAlerta";
                string appPath = "\"" + Application.ExecutablePath + "\"";

                using (RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (startupKey == null)
                    {
                        MessageBox.Show("Não foi possível acessar o registro do Windows.");
                        return;
                    }

                    object existingValue = startupKey.GetValue(appName);
                    if (existingValue?.ToString() == appPath)
                    {
                        return; // Já está configurado corretamente
                    }

                    startupKey.SetValue(appName, appPath, RegistryValueKind.String);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Permissões administrativas são necessárias para realizar essa operação.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnContato_Click(object sender, EventArgs e)
        {
            string url = "https://api.whatsapp.com/send/?phone=551733455001&text=Ol%C3%A1%2C+R.A+Inform%C3%A1tica%21+Gostaria+de+agendar+uma+revis%C3%A3o+para+o+meu+PC%2Fnotebook.+Obrigado%21&type=phone_number&app_absent=0";

            
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedRadioButton = this.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Text;
            string path = Application.StartupPath + @"\dados.txt";

            if (selectedRadioButton != null)
            {
                switch (selectedRadioButton)
                {
                    case "UM DIA":
                        File.WriteAllText(path, DateTime.Now.AddDays(1.0).ToString());
                        break;
                    case "UMA SEMANA":
                        File.WriteAllText(path, DateTime.Now.AddDays(7.0).ToString());
                        break;
                    case "UM MÊS":
                        File.WriteAllText(path, DateTime.Now.AddMonths(1).ToString());
                        break;
                }
            }
            else
            {
                MessageBox.Show("Defina quando quer ser lembrado!");
            }
            this.Close();
        }
    }
}