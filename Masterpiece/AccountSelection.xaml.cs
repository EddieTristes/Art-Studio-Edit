using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Masterpiece
{
    public partial class AccountSelection : Window
    {
        public string SelectedAccount { get; private set; }
        public bool AccountsLoaded { get; private set; }

        public AccountSelection()
        {
            InitializeComponent();
            AccountsLoaded = LoadAccounts();
        }

        private bool LoadAccounts()
        {
            List<string> accounts = new List<string>();

            try
            {
                string filePath = "users.dat";

                if (File.Exists(filePath))
                {
                    string[] accountLines = File.ReadAllLines(filePath);
                    accounts.AddRange(accountLines);

                    AccountList.ItemsSource = accounts;
                    return true;
                }
                else
                {
                    MessageBox.Show($"'{filePath}' does not exist. You should log in.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading accounts: {ex.Message}");
                return false;
            }
        }

        private void SelectAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountList.SelectedItem != null)
            {
                SelectedAccount = AccountList.SelectedItem.ToString();

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select an account.");
            }
        }
    }
}