using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.IO;
using System.Drawing;
using Microsoft.Win32;

namespace Project2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                this.img.Source = bitmapimage;
                this.img.Width = bitmap.Width;
                if (bitmap.Width > 1400)
                    this.img.Width = 1400;
                if (bitmap.Width < 100)
                    this.img.Width = 100;
                this.img.Height = bitmap.Height;
                if (bitmap.Height > 900)
                    this.img.Height = 900;
                if (bitmap.Height < 100)
                    this.img.Height = 100;
                return bitmapimage;
            }
        }

        public void ReadBitmapFromPPM(string file)
        {
            int width = 0;
            int height = 0;
            int max = 0;
            int line = 1;
            Bitmap bitmap;
            bool p6 = true;
            char ppmtype;
            var reader = new BinaryReader(new FileStream(file, FileMode.Open), Encoding.ASCII);
            if (reader.ReadChar() != 'P' || ((ppmtype = reader.ReadChar()) != '6' && ppmtype != '3'))
                return;

            if (ppmtype == '3')
                p6 = false;

            char pom;
            while (true)
            {
                pom = reader.ReadChar();
                if (pom == ' ' || pom == '\n' || pom == '\t')
                {

                }
                else if (pom == '#')
                {
                    while (true && reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        pom = reader.ReadChar();
                        if (pom == '\n')
                            break;
                    }
                }
                else
                {
                    if (line == 1)
                    {
                        string widthstr = "";
                        widthstr += pom;
                        while ((pom = reader.ReadChar()) != ' ' && pom != '\n' && pom != '#')
                        {

                            widthstr += pom;
                        }
                        width = Int32.Parse(widthstr);
                        line++;
                    }

                    else if (line == 2)
                    {
                        string heightstr = "";
                        heightstr += pom;
                        while ((pom = reader.ReadChar()) != ' ' && pom != '\n' && pom != '#' && pom != '\t')
                        {
                            heightstr += pom;
                        }
                        height = Int32.Parse(heightstr);
                        line++;
                    }

                    else if (line == 3)
                    {
                        string maxstr = "";
                        maxstr += pom;
                        while ((pom = reader.ReadChar()) != ' ' && pom != '\n' && pom != '#' && pom != '\t')
                        {
                            maxstr += pom;
                        }
                        max = Int32.Parse(maxstr);
                        break;
                    }
                }

            }

            bitmap = new Bitmap(width, height);
            int x = 0, y = 0;

            if (p6)
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {

                    if (reader.BaseStream.Position + 2 < reader.BaseStream.Length)
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte()));
                    x++;
                    if (x >= width)
                    {
                        x = 0;
                        y++;
                    }
                    if (y >= height)
                        break;
                }
            }
            else
            {
                max = max / 255;
                int x1 = -1;
                int x2 = -1;
                int x3 = -1;

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    pom = reader.ReadChar();
                    if (pom == ' ' || pom == '\n' || pom == '\t')
                    {

                    }
                    else if (pom == '#')
                    {
                        while (true && reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            pom = reader.ReadChar();
                            if (pom == '\n')
                                break;
                        }
                    }
                    else
                    {

                        string xstr = "";
                        xstr += pom;
                        while ((pom = reader.ReadChar()) != ' ' && pom != '\n' && pom != '#' && pom != '\t')
                        {
                            xstr += pom;
                        }
                        if (x1 == -1)
                            x1 = Int32.Parse(xstr);
                        else if (x2 == -1)
                            x2 = Int32.Parse(xstr);
                        else
                        {
                            x3 = Int32.Parse(xstr);

                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(x1 / max, x2 / max, x3 / max));
                            x++;
                            if (x >= width)
                            {
                                x = 0;
                                y++;
                            }
                            if (y >= height)
                                break;

                            x1 = -1;
                            x2 = -1;
                            x3 = -1;
                        }

                    }
                }
            }

            this.BitmapToImageSource(bitmap);
        }

        private void chooseFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new();

            if (fileDialog.ShowDialog() == true)
            {
                ReadBitmapFromPPM(fileDialog.FileName);
            }
        }
    }
    }
