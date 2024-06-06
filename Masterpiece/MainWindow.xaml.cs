using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Configuration;
using System.Diagnostics;
using System.Management;
using System.Text.Json;
using System.Threading;
using Microsoft.Win32;
using System.Windows;
using System.Media;
using System.Linq;
using System.Text;
using Ionic.Zlib;
using System.IO;
using System;


namespace Masterpiece
{
    public partial class MainWindow : Window
    {
        static string uuid = "";
        static byte[] _keybytes_ = new byte[0];
        static byte[] _ivbytes_ = new byte[0];
        static string encodeFile = "";
        static string decodeFile = "";

        private volatile bool _shouldStop;

        private string JsonFilePath = $@"C:\Users\{Environment.UserName}\AppData\Roaming\AJ Classic\config.json";
        static AMF3SpecCLI.AMFParser AMFParser = new AMF3SpecCLI.AMFParser();

        public MainWindow()
        {
            InitializeComponent(); 
            AMFParser.Initialize();
            LoginText.Text = "Logged Out";
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeContent.Visibility = Visibility.Visible;
            EncodeContent.Visibility = Visibility.Collapsed;
            DecodeContent.Visibility = Visibility.Collapsed;
        }

        private void EncodeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeContent.Visibility = Visibility.Collapsed;
            EncodeContent.Visibility = Visibility.Visible;
            DecodeContent.Visibility = Visibility.Collapsed;
        }

        private void DecodeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeContent.Visibility = Visibility.Collapsed;
            EncodeContent.Visibility = Visibility.Collapsed;
            DecodeContent.Visibility = Visibility.Visible;
        }

        public static byte[] SimpleEncrypt(SymmetricAlgorithm algorithm, CipherMode cipherMode, byte[] key, byte[] iv, byte[] bytes)
        {
            algorithm.Mode = cipherMode;
            algorithm.Padding = PaddingMode.Zeros;
            algorithm.Key = key;
            algorithm.IV = iv;

            using (var encryptor = algorithm.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            }
        }

        public static byte[] SimpleDecrypt(SymmetricAlgorithm algorithm, CipherMode cipherMode, byte[] key, byte[] iv, byte[] bytes)
        {
            algorithm.Mode = cipherMode;
            algorithm.Padding = PaddingMode.Zeros;
            algorithm.Key = key;
            algorithm.IV = iv;

            using (var encryptor = algorithm.CreateDecryptor())
            {
                return encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                encodeFile = selectedFileName;
                try
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(selectedFileName));
                    EncodeImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An Exception occurred: {ex.Message} Invalid or Corrupted file format", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void BrowseDecodeButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Encoded Art Files (*.ajart;*.ajgart)|*.ajart;*.ajgart";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                decodeFile = selectedFileName;
                DecodeImage.Source = null;
            }
        }

        private void DecodeFButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(decodeFile))
            {
                MessageBox.Show("Please select an .ajart or .ajgart file before Decoding", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (uuid.Length != 36)
            {
                MessageBox.Show("Please Log In before attempting to Decode", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string file = decodeFile;
                byte[] byteData = new byte[0];
                byteData = SimpleDecrypt(new RijndaelManaged(), CipherMode.CBC, _keybytes_, _ivbytes_, File.ReadAllBytes(file));
                byte[] buffuncompress = new byte[1024];
                try
                {
                    using (Stream decompstream = new MemoryStream(byteData))
                    {
                        using (ZlibStream decompressionStream = new ZlibStream(decompstream, Ionic.Zlib.CompressionMode.Decompress))
                        {
                            var resultStream = new MemoryStream();
                            int read;

                            while ((read = decompressionStream.Read(buffuncompress, 0, buffuncompress.Length)) > 0)
                            {
                                resultStream.Write(buffuncompress, 0, read);
                            }

                            byteData = resultStream.ToArray();
                        }
                    }

                    try
                    {
                        byteData = AMFParser.deserialize(byteData);
                    }
                    catch (System.Runtime.InteropServices.SEHException ex)
                    {
                        MessageBox.Show($"An SEHException occurred: {ex.Message} Invalid or Corrupted file format", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    try
                    {
                        File.WriteAllBytes(file.Substring(0, file.IndexOf('.')) + ".png", byteData);
                        string export = (file.Substring(0, file.IndexOf('.')) + ".png");
                        BitmapImage bitmap = new BitmapImage(new Uri(export));
                        DecodeImage.Source = bitmap;
                        MessageBox.Show($"Decoded as {export}!", "Success", MessageBoxButton.OK, MessageBoxImage.None);

                        decodeFile = "";
                        file = "";
                        export = "";
                        Array.Clear(byteData, 0, byteData.Length);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"{ex.Message}", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Ionic.Zlib.ZlibException)
                {
                    MessageBox.Show("You do not the right account selected", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EncodePaintingButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(encodeFile))
            {
                MessageBox.Show("Please select an image file before Encoding", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (uuid.Length != 36)
            {
                MessageBox.Show("Please Log In before attempting to Encode", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool pixelArtwork = false;
                string file = encodeFile;
                Dictionary<string, object> srcData = new Dictionary<string, object>();

                BitmapImage bitmap = new BitmapImage(new Uri(file));
                int width = bitmap.PixelWidth;
                int height = bitmap.PixelHeight;

                if (width != 711 || height != 434)
                {
                    var result = MessageBox.Show("Image is not 711x434, and may look strange in the canvas", "", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }

                try
                {
                    byte[] byteData = AMFParser.serialize(File.ReadAllBytes(file), uuid, pixelArtwork);
                    using (Stream compstream = new MemoryStream(byteData))
                    {
                        using (ZlibStream compressionStream = new ZlibStream(compstream, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestCompression))
                        {
                            var resultStream = new MemoryStream();
                            int read;

                            while ((read = compressionStream.Read(byteData, 0, byteData.Length)) > 0)
                            {
                                resultStream.Write(byteData, 0, read);
                            }

                            byteData = resultStream.ToArray();

                            byteData = SimpleEncrypt(new RijndaelManaged(), CipherMode.CBC, _keybytes_, _ivbytes_, byteData);
                        }
                    }

                    File.WriteAllBytes(file.Substring(0, file.IndexOf('.')) + (pixelArtwork == false ? ".ajart" : ".ajgart"), byteData);
                    string export = (file.Substring(0, file.IndexOf('.')) + (pixelArtwork == false ? ".ajart" : ".ajgart"));
                    MessageBox.Show($"Encoded as {export}!", "Success", MessageBoxButton.OK, MessageBoxImage.None);
                    EncodeImage.Source = null;

                    encodeFile = "";
                    file = "";
                    export = "";
                    Array.Clear(byteData, 0, byteData.Length);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Failed to Encode... File Not Found Exception", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Ionic.Zlib.ZlibException ze)
                {
                    MessageBox.Show($"Failed Compression... {ze.Message}", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EncodePixelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(encodeFile))
            {
                MessageBox.Show("Please select an image file before Encoding", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (uuid.Length != 36)
            {
                MessageBox.Show("Please Log In before attempting to Encode", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool pixelArtwork = true;
                string file = encodeFile;
                Dictionary<string, object> srcData = new Dictionary<string, object>();

                BitmapImage bitmap = new BitmapImage(new Uri(file));
                int width = bitmap.PixelWidth;
                int height = bitmap.PixelHeight;

                if (width != 714 || height != 434)
                {
                    MessageBox.Show("Image is not 714x434, and will not fit the pixel canvas", "", MessageBoxButton.OK, MessageBoxImage.Error);

                    EncodeImage.Source = null;
                    encodeFile = "";
                    return;
                }

                try
                {
                    byte[] byteData = AMFParser.serialize(File.ReadAllBytes(file), uuid, pixelArtwork);
                    using (Stream compstream = new MemoryStream(byteData))
                    {
                        using (ZlibStream compressionStream = new ZlibStream(compstream, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestCompression))
                        {
                            var resultStream = new MemoryStream();
                            int read;

                            while ((read = compressionStream.Read(byteData, 0, byteData.Length)) > 0)
                            {
                                resultStream.Write(byteData, 0, read);
                            }

                            byteData = resultStream.ToArray();

                            byteData = SimpleEncrypt(new RijndaelManaged(), CipherMode.CBC, _keybytes_, _ivbytes_, byteData);
                        }
                    }

                    File.WriteAllBytes(file.Substring(0, file.IndexOf('.')) + (pixelArtwork == false ? ".ajart" : ".ajgart"), byteData);
                    string export = (file.Substring(0, file.IndexOf('.')) + (pixelArtwork == false ? ".ajart" : ".ajgart"));
                    MessageBox.Show($"Encoded as {export}!", "Success", MessageBoxButton.OK, MessageBoxImage.None);
                    EncodeImage.Source = null;

                    encodeFile = "";
                    file = "";
                    export = "";
                    Array.Clear(byteData, 0, byteData.Length);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Failed to Encode... File Not Found Exception", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Ionic.Zlib.ZlibException ze)
                {
                    MessageBox.Show($"Failed Compression... {ze.Message}", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            uuid = "";
            CheckAJClassicConfig();
        }

        public string CheckAJClassicConfig()
        {
            try
            {
                if (File.Exists(JsonFilePath))
                {
                    string jsonText = File.ReadAllText(JsonFilePath);
                    JsonDocument jsonDoc = JsonDocument.Parse(jsonText);
                    JsonElement root = jsonDoc.RootElement;
                    JsonElement loginElement = root.GetProperty("login");
                    if (loginElement.TryGetProperty("username", out JsonElement usernameElement) && loginElement.TryGetProperty("authToken", out JsonElement authTokenElement))
                    {
                        string username = usernameElement.GetString();
                        string authToken = authTokenElement.GetString();

                        int firstPeriodIndex = authToken.IndexOf('.');
                        int secondPeriodIndex = authToken.IndexOf('.', firstPeriodIndex + 1);
                        string middlePart = authToken.Substring(firstPeriodIndex + 1, secondPeriodIndex - firstPeriodIndex - 1);

                        while (middlePart.Length % 4 != 0)
                        {
                            middlePart += "=";
                        }

                        byte[] decodedBytes = Convert.FromBase64String(middlePart);

                        string json = Encoding.UTF8.GetString(decodedBytes);

                        JsonDocument jsonDoc2 = JsonDocument.Parse(json);

                        JsonElement root2 = jsonDoc2.RootElement;
                        uuid = root2.GetProperty("uuid").GetString();
                        LoginText.Text = $"{username}";

                        if (uuid.Length == 36)
                        {
                            string _key_ = "";
                            string _iv_ = "";

                            int _counter_ = 0;
                            while (_key_.Length < 16)
                            {
                                _key_ += uuid.ElementAt(_counter_++);
                                _iv_ += uuid.ElementAt(_counter_++);
                            }

                            _keybytes_ = Encoding.Default.GetBytes(_key_);
                            _ivbytes_ = Encoding.Default.GetBytes(_iv_);
                        }
                        return uuid;
                    }
                    else
                    {
                        SystemSounds.Exclamation.Play();
                        LoginText.Text = "[ERROR]";
                        MessageBox.Show($"Failed to fetch user, please log into AJ Classic with 'Remember Me' selected");
                        return null;
                    }
                }
                else
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show($"Failed to find AJ Classic files");
                    return null;
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show($"Error: {ex.Message}");
                return null;
            }
        }

        private byte[] GenerateKeyOrIv(string uuid, int size)
        {
            var keyOrIv = new byte[size];
            int uuidLength = uuid.Length;
            int index = 0;

            for (int i = 0; i < size; i++)
            {
                keyOrIv[i] = (byte)uuid[index];
                index = (index + 1) % uuidLength;
            }

            return keyOrIv;
        }
    }
}