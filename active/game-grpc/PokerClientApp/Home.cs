using Pocker;
using System;
using System.Windows.Forms;

namespace PokerClientApp
{
    public partial class Home : Form
    {
        private readonly ConnectService _connectService;
        public Home()
        {
            _connectService = new ConnectService();
            InitializeComponent();
        }


        private async void btnConnect_Click(object sender, EventArgs e)
        {
            var name = tbName.Text;
            if (!string.IsNullOrEmpty(name))
            {
                btnConnect.Enabled = false;
                await _connectService.ConnectAsync(name, UpdateHistory);
                tbName.Enabled = false; 
                tbRow.Enabled = true;
                tbCount.Enabled = true;
                btnSend.Enabled = true;
                btnDisconnect.Enabled = true;
            }
        }

        private void UpdateHistory(StreamResponse response)
        {
            Invoke(new Action(() =>
            {
                rtbHistory.AppendText($"{response.Username}: {response.Message}{Environment.NewLine}");
            }));

            Invoke(new Action(() =>
            {
                if(!string.IsNullOrEmpty(response.Remaining))
                    rtbRemaining.Text = $"{response.Remaining} {Environment.NewLine}";
            }));

        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            int.TryParse(tbRow.Text.Trim(), out int row);
            int.TryParse(tbCount.Text.Trim(), out int count);
            if (row > 0 && count > 0)
            {
                await _connectService.SendAsync(row, count);
                tbRow.Text = string.Empty;
                tbCount.Text = string.Empty;
            }
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            btnDisconnect.Enabled = false;
            await _connectService.DisconnectAsync();

            btnConnect.Enabled = true;
            tbName.Enabled = true;
            tbRow.Enabled = false;
            tbCount.Enabled = false;
            btnSend.Enabled = false;
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            _connectService.Dipose();
        }
    }
}
