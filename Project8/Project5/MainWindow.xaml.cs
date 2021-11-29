using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Project5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage bitmap2;
        BitmapImage bithelp;
        int Type = 1;
        int pom = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void chooseFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new();

            if (fileDialog.ShowDialog() == true)
            {
                string ext = System.IO.Path.GetExtension(fileDialog.FileName);
                if (ext == ".jpg" || ext == ".jpeg")
                {
                    ReadJpeg(fileDialog.FileName);
                }
                else
                {
                    MessageBox.Show("                           Not correct file format                     ");
                }
            }
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
                bitmap2 = bitmapimage;
                this.img.Source = bitmapimage;
                this.img.Width = bitmap.Width*3;
                this.img.Height = bitmap.Height*3;
                return bitmapimage;
            }
        }

        public void ReadJpeg(string file)
        {
            bitmap2 = new BitmapImage();
            bitmap2.BeginInit();
            bitmap2.UriSource = new Uri(file);
            bitmap2.EndInit();
            img.Source = bitmap2;
            this.img.Width = bitmap2.Width;
            this.img.Height = bitmap2.Height;

            var bitmap = new Bitmap((int)bitmap2.Width, (int)bitmap2.Height);
            var helpbitmap = BitmapImage2Bitmap(bitmap2);

            for (int x = 0; x < (int)bitmap2.Width; x++)
            {
                for (int y = 0; y < (int)bitmap2.Height; y++)
                {
                    System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, PixelColor.G, PixelColor.B));

                }
            }

             this.BitmapToImageSource(bitmap);
              bithelp = bitmap2;
            /*

              var val = 50;
              var vall = new int[256];

              for (int x = 0; x < (int)bitmap2.Width; x++)
              {
                  for (int y = 0; y < (int)bitmap2.Height; y++)
                  {
                      System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                      vall[PixelColor.G]++;
                  }
              }

              var count = 0;
              var place = 0;
              var prc = (double)((double)(bitmap2.Width * bitmap2.Height) * val / 100);

              for (int i = 0; i < 255; i++)
              {
                  count += vall[i];
                  if (count >= prc)
                  {
                      place = i;
                      break;
                  }
              }


              for (int x = 0; x < (int)bitmap2.Width; x++)
              {
                  for (int y = 0; y < (int)bitmap2.Height; y++)
                  {
                      System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                      if (PixelColor.G >= place)
                          bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, 255, 255));
                      else
                          bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                  }
              }
              this.BitmapToImageSource(bitmap);
            */
                     
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }




        public void Bin(object sender, RoutedEventArgs e)
        {
            if (bitmap2 != null)
            {
                int [,] array = new int[,] { { 0, 1, 0 },
                                             { 1, 1, 1 },
                                             { 0, 1, 0 } };

                var count = 0;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (array[i, j] == 1)
                        {
                            count++;
                        }
                        
                    }
                }

                this.Reset();

                var name = ((Button)sender).Name;
                var bitmap = new Bitmap((int)bitmap2.Width, (int)bitmap2.Height);
                var bitmapfin = new Bitmap((int)bitmap2.Width, (int)bitmap2.Height);
                var helpbitmap = BitmapImage2Bitmap(bitmap2);

                for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                {
                    for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                    {
                        
                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, 255, 255));
                    }
                }

                        

                if (name == "bin1")
                {

                    for (int x = 1; x < (int)bitmap2.Width-1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height-1; y++)
                        {

                            for(int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    var pom = helpbitmap.GetPixel(x + i - 1, y + j - 1);
                                    if (array[i,j]==1 && helpbitmap.GetPixel(x+i-1, y + j - 1).R < 100)
                                    {
                                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                                        i = 4;
                                        j = 4;
                                    }
                                }
                            }

                        }
                    }

                    this.BitmapToImageSource(bitmap);
                }

                else if(name == "bin2")
                {

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {
                            var countpom = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    if (array[i, j] == 1 && helpbitmap.GetPixel(x + i - 1, y + j - 1).R < 100)
                                    {
                                        countpom++;
                                    }
                                }
                            }

                            if (countpom == count)
                            {
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                            }

                        }
                    }
                    this.BitmapToImageSource(bitmap);
                }
                else if (name == "bin3")
                {

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {

                            bitmapfin.SetPixel(x, y, System.Drawing.Color.FromArgb(255, 255, 255));
                        }
                    }

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {
                            var countpom = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    if (array[i, j] == 1 && helpbitmap.GetPixel(x + i - 1, y + j - 1).R < 100)
                                    {
                                        countpom++;
                                    }
                                }
                            }

                            if (countpom == count)
                            {
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                            }

                        }
                    }


                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {

                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {

                                    if (array[i, j] == 1 && bitmap.GetPixel(x + i - 1, y + j - 1).R < 100)
                                    {
                                        bitmapfin.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                                        i = 4;
                                        j = 4;
                                    }
                                }
                            }
                        }

                    }
                    
                
                    this.BitmapToImageSource(bitmapfin);

                }
                else if (name == "bin4")
                {

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {

                            bitmapfin.SetPixel(x, y, System.Drawing.Color.FromArgb(255, 255, 255));
                        }
                    }



                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {

                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {

                                    if (array[i, j] == 1 && helpbitmap.GetPixel(x + i - 1, y + j - 1).R < 100)
                                    {
                                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                                        i = 4;
                                        j = 4;
                                    }
                                }
                            }
                        }

                    }

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {
                            var countpom = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    if (array[i, j] == 1 && bitmap.GetPixel(x + i - 1, y + j - 1).R < 100)
                                    {
                                        countpom++;
                                    }
                                }
                            }

                            if (countpom == count)
                            {
                                bitmapfin.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                            }

                        }
                    }


                    this.BitmapToImageSource(bitmapfin);

                }
                else if (name == "bin5")
                {
                    int[,] arraypom = new int[,] { { 0, 0, 0 },{ 0, 0, 0 },{ 0, 0, 0 } };

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (array[i, j] == 0)
                            {
                                arraypom[i, j] = 1;
                            }

                        }
                    }

                    var countother = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (arraypom[i, j] == 1)
                            {
                                countother++;
                            }

                        }
                    }


                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {
                            var countpom = 0;
                            var countpompom = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    if (array[i, j] == 1 && helpbitmap.GetPixel(x + i - 1, y + j - 1).R < 100)
                                    {
                                        countpom++;
                                    }
                                    else if (arraypom[i, j] == 1 && helpbitmap.GetPixel(x + i - 1, y + j - 1).R > 100)
                                    {
                                        countpompom++;
                                    }
                                }
                            }

                            if (countpom == count && countpompom==countother)
                            {
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                            }

                        }
                    }
                    this.BitmapToImageSource(bitmap);
                }

            }
            
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Reset()
        {
            txt.Content = "";
            bitmap2 = bithelp;
            img.Source = bitmap2;
            this.img.Width = bitmap2.Width;
            this.img.Height = bitmap2.Height;
        }
    }
}

